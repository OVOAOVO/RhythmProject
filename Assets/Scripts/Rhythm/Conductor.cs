using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor Instance { get; private set; }

    public enum MusicState
    {
        NotStarted,
        Playing,
        Finished
    }

    public MusicState CurrentState { get; private set; } = MusicState.NotStarted;

    [Header("Music Settings")]
    public float songBPM = 150.0f;
    //public float offset = 0.2f;
    //public const float EndTolerance = 0.01f;  // 容差值：可以用于判断songPosition >= musicSource.clip.length
    [Header("Runtime Info")]
    public float secPerBeat;
    public float songPosition;
    public int hit;
    public float dspSongTime;

    public AudioSource musicSource;

    public int aliveEnemies = 0; // 存活敌人数量

    public float durationInSeconds = 0f; // 音乐持续时间（秒）
    public float hitOffset = 0f; // 击打偏移量（秒）
    public bool IsPaused = false;
    private void Awake()
    {
        // —— 单例管理 —— 
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();

        ResetConductor();
        StartMusic();
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case MusicState.Playing:
                UpdateSongPosition();
                break;
            case MusicState.Finished:
                // 如果需要在Finished状态做些什么，可以加在这里
                break;
        }
    }

    private void UpdateSongPosition()
    {
        if (IsPaused) return;
        
        if (musicSource.isPlaying)
        {
            // offset = 0.2f; // 这里可以设置一个偏移量，单位是秒，但要注意不要导致songPosition大于musicSource.clip.length
            //songPosition = (float)(AudioSettings.dspTime - dspSongTime) - offset;
            songPosition = (float)(AudioSettings.dspTime - dspSongTime);
            hit = (int)(songPosition / secPerBeat);

            if (songPosition >= musicSource.clip.length)
            {
                durationInSeconds = musicSource.clip.length;
                FinishMusic();
            }
        }
    }

    public void ResetConductor()
    {
        secPerBeat = 60.0f / songBPM;
        songPosition = 0f;
        hit = 0;
        dspSongTime = 0f;
        CurrentState = MusicState.NotStarted;
    }

    public void StartMusic()
    {
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
        CurrentState = MusicState.Playing;
        Debug.Log("音乐开始播放");
    }

    private void FinishMusic()
    {
        CurrentState = MusicState.Finished;
        Debug.Log("音乐播放完毕");
    }

    public bool IsMusicFinished()
    {
        return CurrentState == MusicState.Finished;
    }

    public void GetBeatOffset()
    {
        float songPosition = this.songPosition;
        float secPerBeat = this.secPerBeat;

        if (secPerBeat <= 0f) return; // 防止除0错误

        float currentBeat = songPosition / secPerBeat;
        int nearestBeat = Mathf.RoundToInt(currentBeat);// 找到最近的节拍
        // 计算当前节拍的时间
        float nearestBeatTime = nearestBeat * secPerBeat;
        hitOffset = songPosition - nearestBeatTime;
    }

    public void PauseMusic()
    {
        musicSource.Pause();
        IsPaused = true;
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
        dspSongTime += (float)(AudioSettings.dspTime - dspSongTime - songPosition); // 修正时间
        IsPaused = false;
    }
}

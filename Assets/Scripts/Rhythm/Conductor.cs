using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Conductor : MonoBehaviour {

    // crotchetsperbar = 8;
    //public float bpm = 180;
    //public float crotchet;

    //public float songPosition;
    //public float deltaSongPos;
    //public float lastHit;
    //public float actualLastHit;
    //public float nextBeatTime = 0.0f;
    //public  float nextBarTime = 0.0f;

    //public float offset = 0.2f;
    //public float addOffset;
    //public static float offSetStatic = 0.4f;
    //public static bool hasOffsetAdjusted = false;
    //public int beatNumber = 0;
    //public int barNumber = 0;

    public static Conductor Instance { get; private set; }
    public float songBPM = 150.0f;

    public float secPerBeat;
    public float songPosition;
    public int hit;
    public float dspSongTime;
    public int lastHit;

    public float offset = 0.2f;

    public AudioSource musicSource;

    private bool isMusicFinished = false; 
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        secPerBeat = 60.0f / songBPM;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
        isMusicFinished = false; // 初始化
    }

    private void Update()
    {
        // 如果音乐未结束，则继续计算
        if (!isMusicFinished && musicSource.isPlaying)
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime) - offset;
            hit = (int)(songPosition / secPerBeat);
            lastHit = hit - 1;

            // 检查是否播放完毕（当前时间 >= 音频长度）
            if (songPosition >= musicSource.clip.length)
            {
                isMusicFinished = true;
                Debug.Log("音乐播放完毕，停止计算。");
                SceneManager.LoadScene("ResultMenu"); // 加载结果场景
            }
        }
    }
}

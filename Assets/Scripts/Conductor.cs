using UnityEngine;

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
    public float songBPM;

    public float secPerBeat;
    public float songPosition;
    public int songPositionInBeats;
    public float dspSongTime;

    public float offset = 0.2f;

    public AudioSource musicSource;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        secPerBeat = 60.0f / songBPM;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    private void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime) - offset;
        songPositionInBeats = (int)(songPosition / secPerBeat);
    }
}

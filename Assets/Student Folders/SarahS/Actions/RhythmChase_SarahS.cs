using UnityEngine;
using UnityEngine.Audio;

public class RhythmChase_SarahS : ActionScript
{
    private float bpm = 129f;
    private float secPerBeat;
    private float bopDuration = 0.15f;
    private float bopTimer = 0;
    private bool bopping = false;
    private float lastBeatTime = 0;
    private AudioSource music;

    public RhythmChase_SarahS(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.RhythmChaseSarahS, who, true);
        Type = Actions.RhythmChaseSarahS;
        secPerBeat = 60f / bpm;
        lastBeatTime = Time.time;
    }
    
    public override void Begin()
    {
        base.Begin();
        GameObject playerObj = GameObject.Find("MusicPlayer");
        if (playerObj != null) 
            music = playerObj.GetComponent<AudioSource>();
        lastBeatTime = Time.time;
    }

    public override void OnRun()
    {
        base.OnRun();
        if (Who.Target == null) return;
        
        float currentTime = music != null ? music.time : Time.time;
        float currentBeat = currentTime / secPerBeat;
        if (Mathf.Floor(currentBeat) > Mathf.Floor(lastBeatTime / secPerBeat))
        {
            bopping = true;
            bopTimer = bopDuration;
        }

        lastBeatTime = currentTime;

        if (bopping)
        {
            bopTimer -= Time.deltaTime;
            if (bopTimer <= 0) bopping = false;

            MoveMult = 3f;
            Who.Thing.MoveTowards(Who.Target.Thing.transform.position, Who.AttackRange);
            Who.Thing.LookAt(Who.Target, 0.5f);
        }
        else
        {
            MoveMult = 0f;
            Who.Thing.ActualMove = Vector2.zero;
        }
        
    }
}

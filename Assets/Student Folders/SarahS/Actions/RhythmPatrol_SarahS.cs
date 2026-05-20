using Unity.VisualScripting;
using UnityEngine;

public class RhythmPatrol_SarahS : ActionScript
{
    private float bpm = 129f;
    private float secPerBeat;
    private float bopDuration = 0.15f;
    private float bopTimer = 0;
    private bool bopping = false;
    private float lastBeatTime = 0;
    private AudioSource music;
    public Vector3 TargetNode;

    public RhythmPatrol_SarahS(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.RhythmPatrolSarahS, who, true);
        secPerBeat = 60f / bpm;
    }

    public override void Begin()
    {
        base.Begin();
        GameObject playerObj = GameObject.Find("MusicPlayer");
        if (playerObj != null) 
            music = playerObj.GetComponent<AudioSource>();
        lastBeatTime = Time.time;
        NewTargetNode();
    }

    public override void OnRun()
    {
        base.OnRun();
        if (Who.Target != null && Who.Thing.SeenThings.Contains(Who.Target))
        {
            Vector2 dir = Who.Target.Thing.transform.position - Who.Thing.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(Who.Thing.transform.position, dir, Who.VisionRange,
                LayerMask.GetMask("Wall"));
            if (hit.collider == null)
            {
                Who.Thing.DoAction(Actions.DefaultAction);
                return;
            }
        }

        if (Vector3.Distance(Who.Thing.transform.position, TargetNode) < 0.2f)
        {
            NewTargetNode();
        }
        
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
            MoveMult = 2f;

            float turn = Who.Thing.LookAt(TargetNode, 0.5f);
            if (turn < 5)
            {
                RaycastHit2D hit = Physics2D.Raycast(Who.Thing.transform.position, Who.Thing.Body.transform.right, 1,
                    LayerMask.GetMask("Wall"));
                if (hit.collider != null)
                {
                    NewTargetNode();
                    return;
                }
                Who.Thing.MoveTowards(TargetNode, 0);
            }
        }
        else
        {
            MoveMult = 0f;
            Who.Thing.ActualMove = Vector2.zero;
        }
    }

    void NewTargetNode()
    {
        TargetNode = Who.Thing.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
    }
}

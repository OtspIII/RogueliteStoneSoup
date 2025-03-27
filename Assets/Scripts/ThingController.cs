using System.Collections.Generic;
using UnityEngine;

public class ThingController : MonoBehaviour
{
    public Dictionary<Traits, TraitInfo> Trait = new Dictionary<Traits, TraitInfo>();
    public Dictionary<EventTypes, List<Traits>> PreListen = new Dictionary<EventTypes, List<Traits>>();
    public Dictionary<EventTypes, List<Traits>> TakeListen = new Dictionary<EventTypes, List<Traits>>();
    public List<EventInfo> EventQueue = new List<EventInfo>();
    public bool MidEvent = false;
    
    public void Awake()
    {
        OnAwake();
    }

    public virtual void OnAwake()
    {
        
    }
    
    public void Start()
    {
        OnStart();
    }

    public virtual void OnStart()
    {
        
    }

    public void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        
    }
    
    public TraitInfo AddTrait(Traits t, bool init = true)
    {
        return AddTrait(t, null, init);
    }
    
    public TraitInfo AddTrait(Traits t,EventInfo i,bool init=true)
    {
        TraitInfo r = Get(t);
        if (r != null)
        {
            r.ReUp(i);
        }
        else
        {
            r = new TraitInfo(t, this, i);
            Trait.Add(t,r);
            if(init) r.Init();
        }
        return r;
    }

    public TraitInfo Get(Traits t)
    {
        if (Trait.TryGetValue(t, out TraitInfo r)) return r;
        return null;
    }

    public void AddListen(EventTypes e, Traits t,bool pre = false)
    {
        Dictionary<EventTypes, List<Traits>> d = pre ? PreListen : TakeListen;
        if(!d.ContainsKey(e)) d.Add(e,new List<Traits>());
        if(!d[e].Contains(t)) d[e].Add(t);
    }

    public void TakeEvent(EventTypes e)
    {
        TakeEvent(new EventInfo(e));
    }
    
    public void TakeEvent(EventInfo e,bool instant=false,int safety=999)
    {
        safety--;
        if (safety <= 0)
        {
            Debug.Log("INFINITE EVENT LOOP: " + e);
            return;
        }
        if (MidEvent && !instant)
        {
            EventQueue.Add(e);
            return;
        }
        MidEvent = true;
        PreListen.TryGetValue(e.Type, out List<Traits> pre);
        if(pre != null) foreach (Traits t in pre) Get(t).PreEvent(e);

        if (e.Abort) return;
        
        TakeListen.TryGetValue(e.Type, out List<Traits> take);
        if(take != null) foreach (Traits t in take) Get(t).TakeEvent(e);
        MidEvent = false;
        if (EventQueue.Count > 0)
        {
            EventInfo next = EventQueue[0];
            EventQueue.RemoveAt(0);
            TakeEvent(next,false,safety);
        }
    }
}

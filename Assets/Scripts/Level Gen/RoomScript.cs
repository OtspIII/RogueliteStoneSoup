using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public GeoTile Geo;
    public Transform SpawnerHolder;
    public Transform DoorHolder;
    public List<SpawnPointController> Spawners;
    public Dictionary<Directions,List<DoorController>> Doors = new Dictionary<Directions, List<DoorController>>();
    public BoxCollider2D Coll;
    [SerializeReference] public List<ThingController> Contents = new List<ThingController>();
    // public List<RoomTags> Tags;
    
    public void Setup(GeoTile g)
    {
        Geo = g;
        transform.parent = God.GM.LevelHolder;
        if (SpawnerHolder == null) SpawnerHolder = transform;
        if (DoorHolder == null) DoorHolder = transform;
        for(int n = 0;n < SpawnerHolder.childCount;n++)
        {
            SpawnPointController s = SpawnerHolder.GetChild(n).GetComponent<SpawnPointController>();
            if (s != null)
            {
                Spawners.Add(s);
            }
        }
        for(int n = 0;n < DoorHolder.childCount;n++)
        {
            DoorController d = DoorHolder.GetChild(n).GetComponent<DoorController>();
            if (d != null)
            {
                if(!Doors.ContainsKey(d.Dir)) Doors.Add(d.Dir,new List<DoorController>());
                Doors[d.Dir].Add(d);
            }
        }

        foreach (Directions d in God.Dirs)
        {
            if(Geo.Links.Contains(d)) continue;
            if (Doors.TryGetValue(d, out List<DoorController> doors))
            {
                foreach (DoorController dc in doors) dc.TurnOn();
            }
        }

        if (Coll == null) God.LogWarning("ROOM SET UP WITH NO COLLIDER: " + this);
        else if (!Coll.isTrigger) God.LogWarning("ROOM SET UP WITH NON-TRIGGER COLLIDER: " + this);
        else if (Coll.gameObject.layer != LayerMask.NameToLayer("Room")) God.LogWarning("ROOM SET UP WITH NON-ROOM LAYER ON COLLIDER: " + this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ThingController o = other.gameObject.transform.parent?.GetComponent<ThingController>();
        if (o == null)return;
        o.EnterRoom(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ThingController o = other.gameObject.transform.parent?.GetComponent<ThingController>();
        if (o == null)return;
        o.ExitRoom(this);
    }

    public void SendEvent(EventInfo e)
    {
        foreach (ThingController who in Contents)
        {
            EventInfo ec = new EventInfo();
            ec.Clone(e);
            who.TakeEvent(ec);
        }
    }

    // public void Spawn()
    // {
    //     foreach(SpawnPointController s in Spawners)
    //         s.Spawn();
    // }
}

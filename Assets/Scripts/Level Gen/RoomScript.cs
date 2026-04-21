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

    public void SetupText(GeoTile g,TextRoomOption o)
    {
        ThingOption w = God.Library.GetThing(new SpawnRequest(GameTags.Wall));
        ThingOption f = God.Library.GetThing(new SpawnRequest(GameTags.Floor));
        string[] lines = o.Map.text.Split("\n");
        Vector2 size = new Vector2(0, lines.Length+1);
        foreach (string s in lines) size.x = Mathf.Max(size.x, s.Length);
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char l = lines[y][x];
                if (x == lines[y].Length - 1 && l != 'x') continue;
                string tag = God.Library.GetJSONTile(l,o.JSON);
                switch (tag) 
                {
                    case "Left": tag = g.Links.Contains(Directions.Left) ? "Floor" : "Wall"; break;
                    case "Right": tag = g.Links.Contains(Directions.Right) ? "Floor" : "Wall"; break;
                    case "Up": tag = g.Links.Contains(Directions.Down) ? "Floor" : "Wall"; break;
                    case "Down": tag = g.Links.Contains(Directions.Up) ? "Floor" : "Wall"; break;
                }

                if (tag == "Wall")
                {
                    ThingInfo wi = w.Create();
                    wi.Spawn(transform,GetPos(x,y,size)).gameObject.isStatic = true;
                    continue;
                }
                //Spawn a floor
                ThingInfo fi = f.Create();
                Vector3 where = GetPos(x, y, size, 50);
                fi.Spawn(transform,where).gameObject.isStatic = true;
                // Debug.Log("TILE: " + tag + " / " + x+"."+y);
                where.z = 0;
                if (tag == "Floor" && (int)g.Path > 3)
                    God.LB.SpawnPoints.Add(new SpawnRequest().SetPos(where));
                if (tag == "" || tag == "Floor") continue;
                if(tag == "Player") God.LB.SpawnPointsPlayer.Add(new SpawnRequest(new Tag(tag)).SetPos(where));
                else God.LB.SpawnPointsFixed.Add(new SpawnRequest(new Tag(tag)).SetPos(where));
                
            }
        }
        Setup(g);
    }

    public void SetupPrefab(GeoTile g)
    {
        Setup(g);
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
    
    public void Setup(GeoTile g)
    {
        Geo = g;
        transform.parent = God.GM.LevelHolder;
        if (SpawnerHolder == null) SpawnerHolder = transform;
        if (DoorHolder == null) DoorHolder = transform;
        
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
    public Vector3 GetPos(int x, int y, Vector2 size,float z=0)
    {
        return transform.position+new Vector3(x - ((size.x - 2) / 2), y - ((size.y - 2) / 2), z);
    }
    
}

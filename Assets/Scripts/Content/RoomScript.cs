using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public GeoTile Geo;
    public Transform SpawnerHolder;
    public Transform DoorHolder;
    public List<SpawnPointController> Spawners;
    public Dictionary<Directions,List<DoorController>> Doors = new Dictionary<Directions, List<DoorController>>();
    public List<RoomTags> Tags;
    
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
                God.GM.AddSpawn(s);
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
    }

    public void Spawn()
    {
        foreach(SpawnPointController s in Spawners)
            s.Spawn();
    }
}

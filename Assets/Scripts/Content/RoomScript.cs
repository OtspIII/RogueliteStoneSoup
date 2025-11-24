using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public GeoTile Geo;
    public Transform SpawnerHolder;
    public Transform DoorHolder;
    public List<SpawnPointController> Spawners;
    public SpawnPointController PlayerSpawn;
    public Dictionary<Directions,List<DoorController>> Doors = new Dictionary<Directions, List<DoorController>>();
    
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
                if (s.Type == Tags.Player)
                    PlayerSpawn = s;
                else
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
        // if (Geo.X == 0 && Doors.TryGetValue(Directions.Left,out List<DoorController> ldoors))
        //     foreach (DoorController d in ldoors) d.TurnOn();
        // if (Geo.Y == 0 && Doors.TryGetValue(Directions.Down,out List<DoorController> ddoors))
        //     foreach (DoorController d in ddoors) d.TurnOn();
        // if (Geo.X >= God.GM.Level.Size.x-1 && Doors.TryGetValue(Directions.Right,out List<DoorController> rdoors))
        //     foreach (DoorController d in rdoors) d.TurnOn();
        // if (Geo.Y >= God.GM.Level.Size.y-1 && Doors.TryGetValue(Directions.Up,out List<DoorController> udoors))
        //     foreach (DoorController d in udoors) d.TurnOn();
    }

    public void Spawn()
    {
        if (Geo.Path == GeoTile.GeoTileTypes.PlayerStart)
        {
            PlayerSpawn.Spawn();
            return;
        }
        if (Geo.Path == GeoTile.GeoTileTypes.Exit)
        {
            Debug.Log("TODO: ADD EXIT TO ROOM " + Geo.X + "."+Geo.Y);
        }
        foreach(SpawnPointController s in Spawners)
            s.Spawn();
    }
}

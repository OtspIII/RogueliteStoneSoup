using System.Collections.Generic;
using UnityEngine;

/*public class Level_SarahS : LevelBuilder
{
    public Level_SarahS()
    {
        Author = Authors.SarahS;
    }

    public override void Customize()
    {
        SpawnPlayer();
        Size = new Vector2Int(5, 5);
        LinkOdds = 1;
        RoomSize = new Vector2Int(17, 17);
        
    }

    public override void BuildGeoMap()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                AddGeo(new GeoTile(x, y, this));
            }
        }
    }
    
    public override void BuildMainPath()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                GeoTile current = GetGeo(x, y);
                if (current == null) continue;
                
                GeoTile right = GetGeo(x + 1, y);
                if (right != null)
                {
                    if (!current.Links.Contains(Directions.Right))
                        current.Links.Add(Directions.Right);
                    if (!right.Links.Contains(Directions.Left))
                        right.Links.Add(Directions.Left);
                    right.SetPath(GeoTile.GeoTileTypes.MainPath);
                }
                
                GeoTile up = GetGeo(x, y + 1);
                if (up != null)
                {
                    if (!current.Links.Contains(Directions.Up))
                        current.Links.Add(Directions.Up);
                    if (!up.Links.Contains(Directions.Down))
                        up.Links.Add(Directions.Down);
                    up.SetPath(GeoTile.GeoTileTypes.MainPath);
                }
            }
        }

        PlayerSpawn = GetGeo(0, 0);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
        
        Exit = GetGeo(Size.x - 1, Size.y - 1);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);
    }

    public override void SpawnThings()
    {
        if (SpawnPointsPlayer.Count == 0 && PlayerSpawn?.Room != null)
            SpawnPointsPlayer.Add(new SpawnRequest(GameTags.Player).SetPos(PlayerSpawn.Room.transform.position));
        
        ThingOption keeper = God.Library.GetThing(new SpawnRequest("RhythmKeeper"));
        if (keeper != null)
        {
            Vector3 center = AllGeo[0].Room != null ? AllGeo[0].Room.transform.position : Vector3.zero;
            SpawnPointsFixed.Add(new SpawnRequest(keeper).SetPos(center));
        }
        else
            God.LogWarning("missing RhythmKeeper");
    
        ThingOption ghost = God.Library.GetThing(new SpawnRequest("Ghosty"));
        if (ghost != null)
        {
            int ghostCount = Mathf.CeilToInt((Size.x * Size.y) * 0.5f);
            List<GeoTile> availableRooms = new List<GeoTile>(AllGeo);
            availableRooms.Remove(PlayerSpawn);
            for (int i = 0; i < ghostCount; i++)
            {
                if (availableRooms.Count == 0) break;
                GeoTile randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                availableRooms.Remove(randomRoom);
                if (randomRoom != null && randomRoom.Room != null)
                /*{
                    Vector3 roomCenter = randomRoom.Room.transform.position;
                    Vector3 offset = new Vector3(Random.Range(-3f, 3f) > 0 ? 6f : -6f,
                        Random.Range(-3f, -3f) > 0 ? 5f : -5f, 0f);
                    Vector3 spawnPos = roomCenter + offset;
                }*/
               /* SpawnPointsFixed.Add(new SpawnRequest(ghost).SetPos(randomRoom.Room.transform.position));
            }
        }
        else
            God.LogWarning("missing Ghosty");
        
        GameObject musicObj = new GameObject("SarahS_Music");
        AudioSource audio = musicObj.AddComponent<AudioSource>();
        AudioClip song = Resources.Load<AudioClip>("SarahS/02 - Heads Will Roll");
        if (song != null)
        {
            audio.clip = song;
            audio.loop = true;
            audio.spatialBlend = 0f;
            audio.Play();
        }
        else
            God.LogWarning("missing SarahS_Music");

        base.SpawnThings();
    }

    public override void FindQuotas()
    {
        if (God.Session.Level == -1) return;
        
        float totalRooms = Size.x * Size.y;
        AddQuota(GameTags.ScoreThing, totalRooms * 4);
        AddQuota(GameTags.Consumable, 1);
        FindThings();
    }
}
*/
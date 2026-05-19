using System.Collections.Generic;
using UnityEngine;

public class Level_SarahS : LevelBuilder
{
    public Level_SarahS()
    {
        Author = Authors.SarahS;
    }

    public override void Customize()
    {
        SpawnPlayer();
        Size = new Vector2Int(4, 4);
        LinkOdds = 1.5f;
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
        PlayerSpawn = GetGeo(0, 0);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
        GeoTile current = PlayerSpawn;
        for (int x = 0; x < Size.x; x++)
        {
            GeoTile next = GetGeo(x, 0);
            if (!current.Links.Contains(Directions.Right))
                current.Links.Add(Directions.Right);
            if (!next.Links.Contains(Directions.Left))
                next.Links.Add(Directions.Left);
            current = next;
            current.SetPath(GeoTile.GeoTileTypes.MainPath);
        }

        for (int y = 0; y < Size.y; y++)
        {
            GeoTile next = GetGeo(Size.x - 1, y);
            if (!current.Links.Contains(Directions.Up))
                current.Links.Add(Directions.Up);
            if (!next.Links.Contains(Directions.Down))
                next.Links.Add(Directions.Down);
            current = next;
            current.SetPath(GeoTile.GeoTileTypes.MainPath);
        }

        Exit = current;
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);
    }

    public override void SpawnThings()
    {
        GameObject musicPlayer = new GameObject("MusicPlayer");
        AudioSource audioSource = musicPlayer.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("SarahS/02 - Heads Will Roll");
        audioSource.loop = true;
        audioSource.Play();
        
        SpawnPointsPlayer.Add(new SpawnRequest(GameTags.Player).SetPos(PlayerSpawn.Room.transform.position));
        foreach (GeoTile geo in AllGeo)
        {
            if (geo == PlayerSpawn) continue;
            foreach (SpawnPointController spawn in geo.Room.Spawners)
            {
                if (!spawn.AlwaysSpawn)
                {
                    SpawnPoints.Add(new SpawnRequest("Dot").SetPos(spawn.transform.position));
                    SpawnPoints.Add(new SpawnRequest("Ghost").SetPos(spawn.transform.position));
                }
            }
        }
    }

    public override void FindQuotas()
    {
        
    }
}

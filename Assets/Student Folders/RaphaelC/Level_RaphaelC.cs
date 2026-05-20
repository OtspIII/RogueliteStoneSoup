using System.Collections.Generic;
using UnityEngine;

public class Level_RaphaelC : LevelBuilder
{
    public Level_RaphaelC()
    {
        Author = Authors.RaphaelC;
    }
    public override void Customize()
    {
        SpawnPlayer();
        Size = new Vector2Int(1, 3);
        RoomSize = new Vector2Int(15, 15);
        Boss = God.Library.GetThing(new SpawnRequest(GameTags.Boss), this, false);
    }
    public override void BuildGeoMap()
    {
        for (int x = 0; x < Size.x; x++)
        for (int y = 0; y < Size.y; y++)
        {
            AddGeo(new GeoTile(x, y, this));
        }
    }
    public override void BuildMainPath()
    {
        PlayerSpawn = GetGeo(0, 0);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);

        GeoTile bossRoom = GetGeo(0, 1);
        bossRoom.SetPath(GeoTile.GeoTileTypes.Boss);

        Exit = GetGeo(0, 2);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);

        PlayerSpawn.Links.Add(Directions.Up);
        bossRoom.Links.Add(Directions.Down);

        bossRoom.Links.Add(Directions.Up);
        Exit.Links.Add(Directions.Down);
    }

    public override void FindQuotas()
    { 
            
        Quotas.Clear();
        ToSpawn.Clear();

        if (Boss != null)
        {
            ToSpawn.Add(Boss);
        }

    }
    public override float JudgeRoom(GeoTile g, RoomOption o, bool backup = false)
    {
        o.Audit();
        if (o.Author != Authors.RaphaelC) return 0f;

        if (RoomSize != o.MapSize) return 0;

        if (g.Path == GeoTile.GeoTileTypes.PlayerStart)
        {
            return o.HasTag("Player") ? 1f : 0f;
        }

        if (g.Path == GeoTile.GeoTileTypes.Boss)
        {
            return o.HasTag("Boss") ? 1f : 0f;
        }

        if (g.Path == GeoTile.GeoTileTypes.Exit)
        {
            return o.HasTag("Exit") ? 1f : 0f;
        }
        return 1f;
    }
    public override void SpawnThings()
    {
        base.SpawnThings();

        if (Exit != null && Exit.Room != null)
        {

            var trigger = Exit.Room.GetComponent<Collider2D>();
            if (trigger != null) trigger.enabled = false;
            
        }
    }
}

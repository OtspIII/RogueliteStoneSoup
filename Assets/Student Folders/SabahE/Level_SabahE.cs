using System.Collections.Generic;
using UnityEngine;

public class Level_SabahE : LevelBuilder
{
    public Level_SabahE()
    {
        Author = Authors.SabahE;
    }

    public enum LevelStyle
    {
        FirelinkShrine,
        LongCorridor,
        OpenArena,
        Maze,
        SplitPath
    }

    public LevelStyle Style;

    public override void Customize()
    {
        SpawnPlayer();
        Size = new Vector2Int(6, 6);
        LinkOdds = 0f;
        Boss = God.Library.GetThing(new SpawnRequest(GameTags.Boss), this, false);
    }

    public override void BuildGeoMap()
    {
        AddGeo(new GeoTile(3, 1, this));
        AddGeo(new GeoTile(3, 2, this));
        AddGeo(new GeoTile(3, 3, this));
        AddGeo(new GeoTile(3, 4, this));
        AddGeo(new GeoTile(3, 5, this));
        AddGeo(new GeoTile(2, 3, this));
        AddGeo(new GeoTile(1, 3, this));
        AddGeo(new GeoTile(1, 4, this));
        AddGeo(new GeoTile(2, 4, this));
        AddGeo(new GeoTile(2, 5, this));
        AddGeo(new GeoTile(4, 3, this));
        AddGeo(new GeoTile(5, 3, this));
        AddGeo(new GeoTile(5, 4, this));
        AddGeo(new GeoTile(4, 5, this));
    }

    public override void BuildMainPath()
    {
        PlayerSpawn = GetGeo(3, 1);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);

        Exit = GetGeo(3, 5);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);

        LinkRooms(3, 1, Directions.Up);
        LinkRooms(3, 2, Directions.Up);

        LinkRooms(3, 3, Directions.Up);
        LinkRooms(3, 4, Directions.Up);

        LinkRooms(3, 3, Directions.Left);
        LinkRooms(2, 3, Directions.Left);
        LinkRooms(1, 3, Directions.Up);
        LinkRooms(1, 4, Directions.Right);
        LinkRooms(2, 4, Directions.Up);
        LinkRooms(2, 5, Directions.Right);

        LinkRooms(3, 3, Directions.Right);
        LinkRooms(4, 3, Directions.Right);
        LinkRooms(5, 3, Directions.Up);

        LinkRooms(5, 4, Directions.Left);
        LinkRooms(4, 5, Directions.Left);

        MarkMainPath(3, 2);
        MarkMainPath(3, 3);
        MarkMainPath(3, 4);

        MarkConnected(2, 3);
        MarkConnected(1, 3);
        MarkConnected(1, 4);
        MarkConnected(2, 4);
        MarkConnected(2, 5);

        MarkConnected(4, 3);
        MarkConnected(5, 3);
        MarkConnected(5, 4);
        MarkConnected(4, 5);
    }

    private void LinkRooms(int x, int y, Directions dir)
    {
        GeoTile a = GetGeo(x, y);
        if (a == null) return;

        GeoTile b = a.Neighbor(dir);
        if (b == null) return;

        if (!a.Links.Contains(dir))
            a.Links.Add(dir);

        Directions opposite = God.OppositeDir(dir);

        if (!b.Links.Contains(opposite))
            b.Links.Add(opposite);
    }

    private void MarkMainPath(int x, int y)
    {
        GeoTile g = GetGeo(x, y);
        if (g != null)
            g.SetPath(GeoTile.GeoTileTypes.MainPath);
    }

    private void MarkConnected(int x, int y)
    {
        GeoTile g = GetGeo(x, y);
        if (g != null)
            g.SetPath(GeoTile.GeoTileTypes.Connected);
    }

    public override void Build()
    {
        Style = (LevelStyle)(God.Session.Level % 5);
        base.Build();
    }

    public override void FindQuotas()
    {
        base.FindQuotas();

        if (Style == LevelStyle.FirelinkShrine)
        {
            AddQuota(GameTags.Consumable, 2);
            AddQuota(GameTags.ScoreThing, 2);
        }

        if (Style == LevelStyle.LongCorridor)
        {
            AddQuota(GameTags.NPC, 3);
            AddQuota(GameTags.Consumable, 1);
        }

        if (Style == LevelStyle.OpenArena)
        {
            AddQuota(GameTags.NPC, 5);
            AddQuota(GameTags.Weapon, 1);
        }

        if (Style == LevelStyle.Maze)
        {
            AddQuota(GameTags.ScoreThing, 5);
            AddQuota(GameTags.Consumable, 2);
        }

        if (Style == LevelStyle.SplitPath)
        {
            AddQuota(GameTags.NPC, 2);
            AddQuota(GameTags.Weapon, 1);
            AddQuota(GameTags.ScoreThing, 3);
        }
    }
}
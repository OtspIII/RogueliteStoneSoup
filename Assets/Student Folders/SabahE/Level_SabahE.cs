using System.Collections.Generic;
using UnityEngine;

public class Level_SabahE : LevelBuilder
{
    public Level_SabahE()
    {
        Author = Authors.SabahE;
    }

    public override void Customize()
    {
        SpawnPlayer();

        Size = new Vector2Int(6, 6);
        LinkOdds = 0f;

        Boss = God.Library.GetThing(new SpawnRequest(GameTags.Boss), this, false);
    }

    public override void BuildGeoMap()
    {
        AddGeo(new GeoTile(1, 0, this));
        AddGeo(new GeoTile(1, 1, this));
        AddGeo(new GeoTile(1, 2, this));
        AddGeo(new GeoTile(2, 2, this));
        AddGeo(new GeoTile(3, 2, this));
        AddGeo(new GeoTile(3, 3, this));
        AddGeo(new GeoTile(4, 3, this));
        AddGeo(new GeoTile(4, 4, this));
        AddGeo(new GeoTile(3, 4, this));

        AddGeo(new GeoTile(0, 2, this));
        AddGeo(new GeoTile(0, 3, this));
        AddGeo(new GeoTile(1, 3, this));
        AddGeo(new GeoTile(2, 3, this));
        AddGeo(new GeoTile(2, 4, this));
    }

    public override void BuildMainPath()
    {
        PlayerSpawn = GetGeo(1, 0);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);

        Exit = GetGeo(3, 4);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);

        LinkRooms(1, 0, Directions.Up);
        LinkRooms(1, 1, Directions.Up);
        LinkRooms(1, 2, Directions.Right);
        LinkRooms(2, 2, Directions.Right);
        LinkRooms(3, 2, Directions.Up);
        LinkRooms(3, 3, Directions.Right);
        LinkRooms(4, 3, Directions.Up);
        LinkRooms(4, 4, Directions.Left);

        LinkRooms(1, 2, Directions.Left);
        LinkRooms(0, 2, Directions.Up);
        LinkRooms(0, 3, Directions.Right);
        LinkRooms(1, 3, Directions.Right);
        LinkRooms(2, 3, Directions.Up);
        LinkRooms(2, 4, Directions.Right);

        MarkMainPath(1, 1);
        MarkMainPath(1, 2);
        MarkMainPath(2, 2);
        MarkMainPath(3, 2);
        MarkMainPath(3, 3);
        MarkMainPath(4, 3);
        MarkMainPath(4, 4);

        MarkConnected(0, 2);
        MarkConnected(0, 3);
        MarkConnected(1, 3);
        MarkConnected(2, 3);
        MarkConnected(2, 4);
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
}
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
        Size = new Vector2Int(5, 5);
        LinkOdds = 1;
        
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
        ThingOption keeper = God.Library.GetThing(new SpawnRequest("RhythmKeeper"));
        if (keeper != null)
        {
            Vector3 center = PlayerSpawn.Room.transform.position;
            SpawnPointsFixed.Add(new SpawnRequest(keeper).SetPos(center));
        }
        else
        {
            God.LogWarning("could not find rhythmKeeper");
            base.SpawnThings();
        }
    }
}

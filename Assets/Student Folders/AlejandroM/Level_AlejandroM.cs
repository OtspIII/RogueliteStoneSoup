using UnityEngine;

class Level_AlejandroM : LevelBuilder
{
    public Level_AlejandroM()
    {
        Author = Authors.AlejandroM;
    }

    public override void Customize()
    {
        SpawnPlayer();

        // one room
        Size = new Vector2Int(1, 1);

        LinkOdds = 0f;

        Boss = null;
    }

    public override void BuildGeoMap()
    {
        AddGeo(new GeoTile(0, 0, this));
    }

    public override void FindQuotas()
    {
        //Disables 
        Quotas.Clear();
    }

    public override void BuildMainPath()
    {
        PlayerSpawn = GetGeo(0, 0);

        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.MainPath);

        GameObject spawnerObject = new GameObject("Arena Spawner");
        spawnerObject.AddComponent<ArenaSpawner>();
    }
}
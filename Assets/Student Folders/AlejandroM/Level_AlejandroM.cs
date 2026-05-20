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

        LinkOdds = 0f; // prevents others rooms from forming

        Boss = null; // no boss
    }

    public override void BuildGeoMap()
    {
        AddGeo(new GeoTile(0, 0, this)); // adds room at (0,0)
    }

    public override void FindQuotas()
    {
        //Disables default enmies spawning allowing Arena Spawn script to control the enemy spawn
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
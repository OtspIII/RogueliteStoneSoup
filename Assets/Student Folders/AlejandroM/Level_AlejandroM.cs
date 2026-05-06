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

        Size = new Vector2Int(1, 2);

        LinkOdds = 0f;

        Boss = null;
    }

    public override void BuildGeoMap()
    {
        AddGeo(new GeoTile(0, 0, this));
        AddGeo(new GeoTile(0, 1, this));
    }

    public override void FindQuotas()
    {
        Quotas.Clear();

        
        int enemyCount = God.Session.Level;

        // Safety clamp so it does not get crazy
        enemyCount = Mathf.Clamp(enemyCount, 1, 5);

        Quotas.Add(new Tag("Hater", 1, enemyCount));

        FindThings();
    }

    public override void BuildMainPath()
    {
        // Player spawns in the room 
        PlayerSpawn = GetGeo(0, 0);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
    }
}
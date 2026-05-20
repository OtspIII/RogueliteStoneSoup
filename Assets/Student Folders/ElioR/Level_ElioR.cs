using Unity.VisualScripting;
using UnityEngine;

public class Level_ElioR : LevelBuilder
{
    public Level_ElioR()
    {
        Author = Authors.ElioR;
    }

    public override void Customize()
    {
        LinkOdds =1;
         SpawnPlayer();
        //As you go deeper the map gets bigger
        int l = God.Session.Level;
        //Width starts at 2, and every third level grows by 1
        int w = 4 + Mathf.FloorToInt(l/3);
        //Height starts at 2, and every other level grows by 1
        int h = 4 + Mathf.FloorToInt(l/2);
        //If this is a test level, make it just be 1x2 so the exit touches the start point
        if (l == -1)
        {
            w = 1;
            h = 2;
        }
        //Set the size we calculated
        Size = new Vector2Int(w, h);
        //Pick a boss. If this isn't null, it'll spawn a boss room.
        Boss = God.Library.GetThing(new SpawnRequest(GameTags.Boss),this,false); //the false means 'its okay if you dont find one'
    }
    public override void BuildGeoMap()
    {
        for(int x =0; x < Size.x; x++)
        for(int y = 0; y < Size.y; y++)
            {
                GeoTile g = new GeoTile(x,y,this);
                AddGeo(g);
            }
    }

    public override void BuildMainPath()
    {
        base.BuildMainPath();
    }

    public override void ConnectAllGeos()
    {
        base.ConnectAllGeos();
    }

    public override void PickRooms()
    {
        base.PickRooms();
    }
}

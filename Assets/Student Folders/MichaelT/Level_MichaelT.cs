using UnityEngine;

public class Level_MichaelT : LevelBuilder
{
    public Level_MichaelT()
    {
        Author = Authors.MichaelT;
    }
    
    public override void Customize()
    {
        SpawnPlayer(); 
        Size = new Vector2Int(4,4); 
        LinkOdds = 1; 
    }
    
}

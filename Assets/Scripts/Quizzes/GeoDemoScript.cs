using UnityEngine;

public class GeoDemoScript : MonoBehaviour
{
    public GeoTile Tile;
    public SpriteRenderer RightPath;
    public SpriteRenderer LeftPath;
    public SpriteRenderer UpPath;
    public SpriteRenderer DownPath;
    public SpriteRenderer Background;

    public void Setup()
    {
        switch (Tile.Path)
        {
            case GeoTile.GeoTileTypes.PlayerStart: Background.color = Color.blue; break;
            case GeoTile.GeoTileTypes.Exit: Background.color = Color.seaGreen; break;
            case GeoTile.GeoTileTypes.MainPath: Background.color = Color.lightGreen; break;
            case GeoTile.GeoTileTypes.Boss: Background.color = Color.red; break;
            case GeoTile.GeoTileTypes.Connected: Background.color = Color.white; break;
            case GeoTile.GeoTileTypes.Unreachable: Background.color = Color.gray; break;
            // case GeoTile.GeoTileTypes.Target: Background.color = Color.yellow; break;
            default: Background.color = Color.magenta; break;
        }

        if(Tile.DebugColor != "")
            switch (Tile.DebugColor)
            {
                case "A": Background.color = Color.orange; break;
                case "B": Background.color = Color.tan; break;
                case "C": Background.color = Color.pink; break;
                default: Background.color = Color.yellow; break;
            }

        foreach (Directions d in Tile.Links)
        {
            switch (d)
            {
                case Directions.Right: RightPath.gameObject.SetActive(true); break;
                case Directions.Left: LeftPath.gameObject.SetActive(true); break;
                case Directions.Up: UpPath.gameObject.SetActive(true); break;
                case Directions.Down: DownPath.gameObject.SetActive(true); break;
            }
        }
    }
}

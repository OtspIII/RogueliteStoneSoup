using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomOption", menuName = "Scriptable Objects/RoomOption")]
public class RoomOption : GameOption
{
    public RoomScript Prefab;
    public List<RoomTags> Tags;

    public virtual RoomScript Build(GeoTile g, LevelBuilder b)
    {
        RoomScript r = Instantiate(Prefab, new Vector3(g.X * God.RoomSize.x, g.Y * God.RoomSize.y, 0), Quaternion.identity);
        r.gameObject.name = "[" + g.X + "." + g.Y + "] " + r.gameObject.name;
        r.SetupPrefab(g);
        return r;
    }
}

[CreateAssetMenu(fileName = "TextRoomOption", menuName = "Scriptable Objects/TextRoomOption")]
public class TextRoomOption : RoomOption
{
    public TextAsset Map;
    public TextAsset JSON;

    public override RoomScript Build(GeoTile g, LevelBuilder b)
    {
        RoomScript r = Instantiate(God.Library.EmptyRoom, new Vector3(g.X * God.RoomSize.x, g.Y * God.RoomSize.y, 0), Quaternion.identity);
        r.gameObject.name = "[" + g.X + "." + g.Y + "] " + r.gameObject.name;
        ThingOption w = God.Library.GetThing(new SpawnRequest(GameTags.Wall));
        ThingOption f = God.Library.GetThing(new SpawnRequest(GameTags.Floor));
        string[] lines = Map.text.Split("\n");
        Vector2 size = new Vector2(0, lines.Length+1);
        foreach (string s in lines) size.x = Mathf.Max(size.x, s.Length);
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char l = lines[y][x];
                if (x == lines[y].Length - 1 && l != 'x') continue;
                switch (l) 
                {
                    case 'l': l = g.Links.Contains(Directions.Left) ? ' ' : 'x'; break;
                    case 'r': l = g.Links.Contains(Directions.Right) ? ' ' : 'x'; break;
                    case 'u': l = g.Links.Contains(Directions.Down) ? ' ' : 'x'; break;
                    case 'd': l = g.Links.Contains(Directions.Up) ? ' ' : 'x'; break;
                }
                switch (l)
                {
                    case 'x':
                    {
                        ThingInfo wi = w.Create();
                        wi.Spawn(r.transform,GetPos(x,y,size)); 
                        break;
                    }
                    default:
                    {
                        ThingInfo fi = f.Create();
                        fi.Spawn(r.transform,GetPos(x,y,size,50));
                        break;
                    }
                }
            }
        }

        r.SetupText(g);
        return r;
    }

    public Vector3 GetPos(int x, int y, Vector2 size,float z=0)
    {
        return new Vector3(x - ((size.x - 2) / 2), y - ((size.y - 2) / 2), z);
    }
}

public static class JSONReader
{
    public static RoomJSON ParseJSON(string txt)
    {
        return JsonUtility.FromJson<RoomJSON>(txt);
    }
}

public class RoomJSON
{
    public JSONPair[] Pairs;
}

public class JSONPair
{
    public string K;
    public string V;
}
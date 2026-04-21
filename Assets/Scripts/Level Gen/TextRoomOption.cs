using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextRoomOption", menuName = "Scriptable Objects/TextRoomOption")]
public class TextRoomOption : RoomOption
{
    public TextAsset Map;
    public TextAsset JSON;
    public RoomJSON Pairs;
    public Dictionary<char, string> TileTypes = new Dictionary<char, string>();

    public override RoomScript Build(GeoTile g, LevelBuilder b)
    {
        ParseJSON();
        RoomScript r = Instantiate(God.Library.EmptyRoom, new Vector3(g.X * God.RoomSize.x, g.Y * God.RoomSize.y, 0), Quaternion.identity);
        r.gameObject.name = "[" + g.X + "." + g.Y + "] " + Name;
        r.SetupText(g,this);
        return r;
    }

    

    public void ParseJSON()
    {
        if (Pairs != null && Pairs.Pairs != null && Pairs.Pairs.Length > 0) return;
        Pairs = JSONReader.ParseJSON(JSON.text);
        Debug.Log(JSON.text);
        Debug.Log(Pairs.Pairs + " / " + Pairs.Name);
        foreach (JSONPair p in Pairs.Pairs)
        {
            if (p.K.Length == 0 || TileTypes.ContainsKey(p.K[0])) continue;
            TileTypes.Add(p.K[0], p.V);
        }
    }

}

public static class JSONReader
{
    public static RoomJSON ParseJSON(string txt)
    {
        return JsonUtility.FromJson<RoomJSON>(txt);
    }
}

[System.Serializable]
public class RoomJSON
{
    public string Name;
    public JSONPair[] Pairs;
}

[System.Serializable]
public class JSONPair
{
    public string K;
    public string V;
}
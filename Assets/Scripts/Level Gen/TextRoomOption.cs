using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextRoomOption", menuName = "Scriptable Objects/TextRoomOption")]
public class TextRoomOption : RoomOption
{
    public TextAsset Map;
    public TextAsset JSON;
    [HideInInspector] public string[] MapText;


    public override RoomScript Build(GeoTile g, LevelBuilder b)
    {
        RoomScript r = Instantiate(God.Library.EmptyRoom, new Vector3(g.X * (b.RoomSize.x-1), g.Y * (b.RoomSize.y-1), 0), Quaternion.identity);
        r.gameObject.name = "[" + g.X + "." + g.Y + "] " + Name;
        r.SetupText(g,this);
        return r;
    }

    public override void Audit()
    {
        if (Audited) return;
        Audited = true;
        MapText = Map.text.Split("\n");
        MapSize = new Vector2Int(0, MapText.Length);
        foreach (string s in MapText) MapSize.x = Mathf.Max(MapSize.x, s.Length-1);
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
    public JSONPair[] Pairs;
}

[System.Serializable]
public class JSONPair
{
    public string K;
    public string V;
}
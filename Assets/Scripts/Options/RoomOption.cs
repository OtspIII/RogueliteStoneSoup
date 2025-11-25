using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomOption", menuName = "Scriptable Objects/RoomOption")]
public class RoomOption : ScriptableObject
{
    public RoomScript Prefab;
    public List<RoomTags> Tags;

    public virtual RoomScript Build(GeoTile g, LevelBuilder b)
    {
        RoomScript r = Instantiate(Prefab, new Vector3(g.X * God.RoomSize.x, g.Y * God.RoomSize.y, 0), Quaternion.identity);
        r.gameObject.name = "[" + g.X + "." + g.Y + "] " + r.gameObject.name;
        r.Setup(g);
        return r;
    }
}

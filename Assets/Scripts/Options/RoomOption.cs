using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomOption", menuName = "Scriptable Objects/RoomOption")]
public class RoomOption : GameOption
{
    public RoomScript Prefab;
    [HideInInspector] public bool Audited = false;
    [HideInInspector] public Vector2Int MapSize = new Vector2Int(11,11);

    public virtual RoomScript Build(GeoTile g, LevelBuilder b)
    {
        RoomScript r = Instantiate(Prefab, new Vector3(g.X * (b.RoomSize.x-1), g.Y * (b.RoomSize.y-1), 0), Quaternion.identity);
        r.gameObject.name = "[" + g.X + "." + g.Y + "] " + r.gameObject.name;
        r.SetupPrefab(g);
        return r;
    }

    public virtual void Audit()
    {
        if (Audited) return;
        Audited = true;
    }
}

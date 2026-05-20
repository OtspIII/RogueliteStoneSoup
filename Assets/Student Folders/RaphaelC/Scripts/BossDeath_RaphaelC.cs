using UnityEngine;

public class BossDeath_RaphaelC : Trait
{
    public BossDeath_RaphaelC()
    {
        Type = Traits.BossDeath_RaphaelC;
        AddListen(EventTypes.OnDestroy);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type == EventTypes.OnDestroy)
        {

            if (God.LB != null)
            {
                GeoTile bossRoomTile = God.LB.GetGeo(0, 1);

                if (bossRoomTile != null)
                {
                    if (bossRoomTile.Room != null)
                    {
                        UnityEngine.Object.Destroy(bossRoomTile.Room.gameObject);
                        bossRoomTile.Room = null; // Clear the memory link
                    }

                    // 2. Punch the navigation path upward
                    if (!bossRoomTile.Links.Contains(Directions.Up))
                    {
                        bossRoomTile.Links.Add(Directions.Up);
                    }

                    // 3. THE PRO MOVE: Change the tile's identity. 
                    // This forces the library to completely ignore its Boss cache!
                    bossRoomTile.SetPath(GeoTile.GeoTileTypes.PlayerStart);

                    // 4. Query the library. It now strictly looks for an open Player map.
                    RoomOption openRoomOption = God.Library.GetRoom(bossRoomTile, God.LB);

                    if (openRoomOption != null)
                    {
                        openRoomOption.Build(bossRoomTile, God.LB);
                        Debug.Log("Arena seamlessly converted into an open pathway.");
                    }
                }
            }
        }
    }
}

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
                        bossRoomTile.Room = null; 
                    }

                    if (!bossRoomTile.Links.Contains(Directions.Up))
                    {
                        bossRoomTile.Links.Add(Directions.Up);
                    }

                    bossRoomTile.SetPath(GeoTile.GeoTileTypes.PlayerStart);

                    RoomOption openRoomOption = God.Library.GetRoom(bossRoomTile, God.LB);

                    if (openRoomOption != null)
                    {
                        openRoomOption.Build(bossRoomTile, God.LB);
                    }
                }
            }
        }
    }
}

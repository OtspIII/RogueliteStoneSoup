using System.Collections.Generic;
using UnityEngine;

public class TeleportRandomRoomTrait : Trait
{
    public TeleportRandomRoomTrait()
    {
        //Type = Traits.TeleportRandomRoom;
        AddListen(EventTypes.OnUse);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnUse:
            {
                List<RoomScript> mapRooms = new List<RoomScript>(God.GM.Rooms); // Make copy of rooms (gonna remove from list later)
                ThingController userThingController = e.GetThing().Thing;
                List<RoomScript> userCurrentRooms = userThingController.CurrentRooms;

                // Remove rooms user is currently in
                foreach (RoomScript userRoom in userCurrentRooms)
                {
                    if (mapRooms.Contains(userRoom)) 
                        mapRooms.Remove(userRoom);
                }

                // Waste item and do nothing if there are no available rooms
                if (mapRooms.Count == 0)
                    break;

                RoomScript randomRoom = mapRooms.Random();
                userThingController.transform.position =
                    randomRoom.transform.position.WithZ(userThingController.transform.position.z);
                break;
            }
        }
    }
}



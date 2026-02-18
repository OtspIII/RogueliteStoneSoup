using System.Collections.Generic;
using UnityEngine;

public class TeleportRandomRoomTrait : Trait
{
    public TeleportRandomRoomTrait()
    {
        Type = Traits.TeleportRandomRoom;
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

public class DamageReflectTrait : Trait
{
    public DamageReflectTrait()
    {
        Type = Traits.DamageReflect;
        AddListen(EventTypes.Damage);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Damage:
            {
                ThingInfo attacker = e.GetThing();
                if (attacker == null)
                    return;
                ThingInfo victim = i.Who;
                if (victim == null)
                    return;
                float damageTaken = e.GetFloat();
                Debug.Log($"DamageReflectTrait: {attacker.Thing.gameObject.name} dealt {damageTaken} to {victim.Thing.gameObject.name}");
                attacker.TakeEvent(new EventInfo(EventTypes.Damage)
                    .Set(NumInfo.Default, damageTaken)
                    .Set(ThingEInfo.Default, victim) // Victim is the one reflecting
                );
                // Possible recursion with damage calling upon damage? Will worry about later.
                break;
            }
        }
    }
}
using UnityEngine;

public class CharacterSwapTrait_Wesley : Trait
{
    
    public BodyController Held;
    public ThingController Who;
    public HitboxController Hitbox;



    public CharacterSwapTrait_Wesley()
    {
        AddListen(EventTypes.OnSpawn,5);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        // Debug.Log("TAKE EVENT PLAYER: " + i.Type);
        switch (e.Type)
        {
            // case EventTypes.Setup:
            // {
            //     // Debug.Log("SETUP PLAYER");
            //     
            //     // i.Set(EnumInfo.DefaultAction, (int)Actions.Idle);
            //     
            //     break;
            // }
            case EventTypes.OnSpawn:
                {
                    God.Player = i.Who;
                    God.Cam.Target = i.Who.Thing.gameObject;
                    if (i.Who.CurrentHeld != null)
                        i.Who.TakeEvent(God.E(EventTypes.DidPickup).Set(i.Who.CurrentHeld));
                    EventInfo hpi = i.Who.Ask(EventTypes.ShownHP);
                    God.GM.SetUI("Health", hpi.GetInt() + "/" + hpi.GetInt(NumInfo.Max), 1);
                    God.GM.SetUI("Score", "Score: " + Mathf.Floor(i.GetN()), 2);
                    God.GM.UpdateInvText();
                    break;
                    
                    for (int n = 0; n < God.InvKeys.Count; n++)
                    {
                        if (Input.GetKeyDown(God.InvKeys[n]) && God.Session.PlayerInventory.Count > n)
                        {
                            God.Session.InventoryIndex = n + 1;
                            God.Player.SetHeld(God.Session.PlayerInventory[n]);
                            God.GM.UpdateInvText();
                        }

                    }
           
            
                }
        }
     }
    
}

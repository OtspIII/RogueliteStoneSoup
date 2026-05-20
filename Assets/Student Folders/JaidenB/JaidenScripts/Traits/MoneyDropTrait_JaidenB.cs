using UnityEngine;

public class MoneyDropTrait : Trait
{
    public MoneyDropTrait() 
    {
        Type = Traits.MoneyDrop;
        AddListen(EventTypes.Death);
        AddListen(EventTypes.AddScore);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) 
    {
        ThingInfo ThePlayer = God.Session.Player;

        switch (e.Type) 
        {
            case EventTypes.Death:
                {
                    Debug.Log("Increase coins to 5, didnt know how to get this working sadly");
                    //ThePlayer.AddTrait(Traits.AddScore);//e.Set(5).Set(ThePlayer);
                }
                break;


            case EventTypes.AddScore:
                {
                    e.Set(5).Set(ThePlayer);
                }
                break;
        }
    }

}

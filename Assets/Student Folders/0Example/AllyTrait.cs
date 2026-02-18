using UnityEngine;

public class AllyTrait_Misha : Trait
{
    public AllyTrait_Misha()
    {
        Type = Traits.Ally_Misha;
        AddListen(EventTypes.OnSee);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSee:
            {
                ThingInfo t = e.GetThing();
                if (t.Team == GameTeams.Enemy)
                {
                    i.Who.Target = t;
                    Debug.Log("Saw New Target: " + i.Who.Target);
                }
                break;
            }
            case EventTypes.OnTargetDie:
            {
                //look at everyone i can currently see to see if they are a valid new target
                break;
            }
        }
    }
}

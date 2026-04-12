using UnityEngine;

public class Flee_SarahS : Trait
{
    private float runRange = 5f;
    private float catchRange = 0.8f;

    public Flee_SarahS()
    {
        Type = Traits.FleeSarahS;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                ThingInfo player = God.Session.Player;
                ThingInfo me = i.Who;

                if (player != null && player.Thing != null && me.Thing != null)
                {
                    float dist = Vector2.Distance(me.Thing.transform.position, player.Thing.transform.position);
                    if (dist <= catchRange)
                    {
                        player.TakeEvent(God.E(EventTypes.AddScore).Set(1));
                        me.Destruct(player);
                    }
                    else if (dist <= runRange)
                    {
                        Vector2 fleeDir = (me.Thing.transform.position - player.Thing.transform.position).normalized;
                        me.DesiredMove = fleeDir;
                    }
                    else
                    {
                        me.DesiredMove = Vector2.zero;
                    }
                }

                break;
            }
        }
    }
}

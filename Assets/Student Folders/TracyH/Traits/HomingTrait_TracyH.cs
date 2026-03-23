using UnityEngine;

//Note:
//Default : movement speed while homing
public class HomingTrait_TracyH : Trait
{
    public HomingTrait_TracyH()
    {
        Type = Traits.Homing_TracyH;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //Move toward player
            case EventTypes.Update:
                {
                    if (i.Who == null) return;
                    if (i.Who.Thing == null) return;
                    if (God.Player == null) return;
                    if (God.Player.Thing == null) return;

                    float speed = i.GetFloat(NumInfo.Default, 5f);

                    Vector3 selfPos = i.Who.Thing.transform.position;
                    Vector3 playerPos = God.Player.Thing.transform.position;
                    Vector3 dir = (playerPos - selfPos).normalized;

                    i.Who.Thing.transform.position += dir * speed * Time.deltaTime;
                    break;
                }
        }
    }
}
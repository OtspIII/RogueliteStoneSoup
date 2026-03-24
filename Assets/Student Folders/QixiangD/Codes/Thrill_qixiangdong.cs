using UnityEngine;

public class Thrill_qixiangdong : Trait
{
    private bool Thrilled = false;
    private float thrillTimer = 0f;

    private float thrillDuration = 4f;
    private float speedBoost = 5f;

    private float originalSpeed;

    public Thrill_qixiangdong()
    {
        Type = Traits.Thrill_qixiangdong;
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.OnTargetDie);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
                {
                    originalSpeed = i.Who.CurrentSpeed;
                    break;
                }
            case EventTypes.OnTargetDie:
                {
                    Debug.Log("trigger");
                    ThingInfo killer = e.Get(ThingEInfo.Source);

                    if (killer == i.Who)
                    {
                        Debug.Log("player killed something");
                        Thrilled = true;
                        thrillTimer = thrillDuration;
                    }
                    break;
                }

            case EventTypes.Update:
                {
                    if(Thrilled)
                    {
                        thrillTimer -= Time.deltaTime;
                        i.Who.CurrentSpeed = originalSpeed + speedBoost;
                        if(thrillTimer <= 0f)
                        {
                            Thrilled = false;
                            i.Who.CurrentSpeed = originalSpeed;
                        }
                    }
                    break;
                }
        }
    }
}

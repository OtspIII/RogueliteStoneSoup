using UnityEngine;

//Note:
//Time : how long freeze lasts
//Effect : sets speed to 0 while active
public class FreezeTrait_TracyH : Trait
{
    public FreezeTrait_TracyH()
    {
        Type = Traits.Freeze_TracyH;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //Freeze effect
            case EventTypes.Update:
                {
                    i.Who.CurrentSpeed = 0f;

                    float timeLeft = i.GetFloat(NumInfo.Time, 0f);
                    timeLeft -= Time.deltaTime;
                    i.Set(NumInfo.Time, timeLeft);

                    if (timeLeft <= 0f)
                    {
                        i.Who.RemoveTrait(Type);
                    }
                    break;
                }
        }
    }

    protected override void OnRemove(TraitInfo i, EventInfo e)
    {
        float baseSpeed = 5f;

        if (i.Who.ActorTrait != null)
            baseSpeed = i.Who.ActorTrait.GetFloat(NumInfo.Speed, 5f);

        i.Who.CurrentSpeed = baseSpeed;
    }

    public override void ReUp(TraitInfo old, EventInfo n)
    {
        if (n == null) return;

        old.Set(NumInfo.Time, n.GetFloat(NumInfo.Time, old.GetFloat(NumInfo.Time, 0f)));
    }
}

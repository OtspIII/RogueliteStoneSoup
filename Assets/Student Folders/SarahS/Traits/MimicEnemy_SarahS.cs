using UnityEngine;

public class MimicEnemy_SarahS : Trait
{
    public MimicEnemy_SarahS()
    {
        Type = Traits.MimicEnemySarahS;
        AddListen(EventTypes.OnSee);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSee:
            {
                ThingInfo target = e.GetThing(ThingEInfo.Target);
                if (target == null) return;

                if (target.Team != GameTeams.Enemy) return;

                float mimicRange = i.GetFloat(NumInfo.Distance, 1f);
                if (i.Who.Thing == null || target.Thing == null) return;
                
                float distance = Vector2.Distance(i.Who.Thing.transform.position, target.Thing.transform.position);
                if (distance > mimicRange) return;

                foreach (Traits trait in target.Trait.Keys)
                {
                    if (trait == Type) continue;
                    
                    TraitInfo enemyTrait = target.Trait[trait];
                    EventInfo copyInfo = new EventInfo();
                    foreach (NumInfo num in enemyTrait.Numbers.Keys)
                        copyInfo.SetFloat(num, enemyTrait.GetFloat(num));
                    
                    i.Who.AddTrait(trait, copyInfo);
                }

                i.Who.Team = target.Team;
                i.Who.Thing.SetTeam(target.Team);
                
                Actions enemyAction = target.Ask(God.E(EventTypes.GetDefaultAction)).GetAction();
                if (enemyAction != Actions.None)
                {
                    i.Who.DoAction(enemyAction);
                }

                break;
            }
        }
    }
}

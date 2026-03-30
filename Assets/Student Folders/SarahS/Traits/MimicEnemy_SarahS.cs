using UnityEngine;

public class MimicEnemy_SarahS : Trait
{
    public MimicEnemy_SarahS()
    {
        Type = Traits.MimicEnemySarahS;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type == EventTypes.Update)
        {
            if (i.Who.Thing == null) return;

            ThingInfo currentTarget = i.GetThing(ThingEInfo.Target);
            float mimicRange = i.GetFloat(NumInfo.Distance, 3f);

            if (currentTarget == null)
            {
                ThingInfo closestEnemy = null;
                float closestDist = mimicRange;

                foreach (ThingController tc in God.GM.Things)
                {
                    if (tc.Info.Team == GameTeams.Enemy)
                    {
                        float dist = Vector2.Distance(i.Who.Thing.transform.position, tc.transform.position);
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            closestEnemy = tc.Info;
                        }
                    }
                }

                if (closestEnemy != null)
                {
                    i.SetThing(ThingEInfo.Target, closestEnemy);
                    i.Who.Team = closestEnemy.Team;
                    i.Who.Thing.SetTeam(closestEnemy.Team);

                    foreach (Traits t in closestEnemy.Trait.Keys)
                    {
                        if (t == Traits.Actor || t == Traits.Health || t == Traits.Player || t == Type) continue;
                        i.Who.AddTrait(t, closestEnemy.Trait[t]);
                        i.TraitI.Add(t);
                    }

                    Actions enemyBrain = closestEnemy.Ask(God.E(EventTypes.GetDefaultAction))
                        .Get(ActionInfo.DefaultAction);
                    if (enemyBrain != Actions.None)
                    {
                        i.Who.ActorTrait.Set(ActionInfo.DefaultAction, enemyBrain);
                        i.Who.DoAction(enemyBrain);
                    }

                    Debug.Log("mimicing: " + closestEnemy.Name);
                }
            }
            else
            {
                bool shouldRevert = false;
                if (currentTarget.Thing == null)
                {
                    shouldRevert = true;
                }
                else
                {
                    float dist = Vector2.Distance(i.Who.Thing.transform.position, currentTarget.Thing.transform.position);
                    if (dist > mimicRange) shouldRevert = true;
                }

                if (shouldRevert)
                {
                    foreach (Traits t in i.TraitI)
                    {
                        i.Who.RemoveTrait(t);
                    }
                    
                    i.TraitI.Clear();
                    
                    i.SetThing(ThingEInfo.Target, null);
                    i.Who.Team = GameTeams.Neutral;
                    i.Who.Thing.SetTeam(GameTeams.Neutral);
                    
                    i.Who.ActorTrait.Set(ActionInfo.DefaultAction, Actions.Idle);
                    i.Who.DoAction(Actions.Idle);
                    Debug.Log("npc reverting");
                }
            }
        }
    }
}

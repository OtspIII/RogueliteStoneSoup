using UnityEngine;

public class ProtectionCircle_SarahS : Trait
{
    private float circleRadius = 3.5f; 
    private bool playerIsProtected = false;

    public ProtectionCircle_SarahS()
    {
        Type = Traits.ProtectionCircleSarahS;
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.Update);
        AddListen(EventTypes.OnDestroy); 
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
            {
                i.Who.Team = GameTeams.Player;
                if (i.Who.Thing != null) i.Who.Thing.SetTeam(GameTeams.Player);
                break;
            }
            case EventTypes.Update:
            {
                if (i.Who.Thing == null || God.Player == null || God.Player.Thing == null) return;
                
                float dist = Vector2.Distance(i.Who.Thing.transform.position, God.Player.Thing.transform.position);

                if (dist < circleRadius)
                {
                    if (!playerIsProtected)
                    {
                        EventInfo protectionInfo = God.E().SetThing(ThingEInfo.Source, i.Who);
                        God.Player.AddTrait(Traits.ProtectionSpellSarahS, protectionInfo);
                        playerIsProtected = true;
                        Debug.Log("Player entered the Safe Zone");
                    }
                }
                else
                {
                    if (playerIsProtected)
                    {
                        God.Player.RemoveTrait(Traits.ProtectionSpellSarahS);
                        playerIsProtected = false;
                        Debug.Log("Player left the Safe Zone");
                    }
                }
                break;
            }
            case EventTypes.OnDestroy:
            {
                if (playerIsProtected && God.Player != null)
                {
                    God.Player.RemoveTrait(Traits.ProtectionSpellSarahS);
                }
                break;
            }
        }
    }
}
using UnityEngine;

public class SpeedPotion_AlejandroM : Trait
{
    public SpeedPotion_AlejandroM()
    {
        Type = Traits.SpeedPotion_AlejandroM;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
                {
                    float duration = i.GetFloat(NumInfo.Max, 5f);
                    i.Set(NumInfo.Default, duration);

                    var itemThing = i.Who.Thing;
                    if (itemThing == null) return;

                    var body = itemThing.Body;
                    if (body == null)
                    {
                        Debug.Log("SpeedPotion: body is null");
                        return;
                    }

                    var player = body.Who;
                    if (player == null)
                    {
                        Debug.Log("SpeedPotion: player is null");
                        return;
                    }

                    Debug.Log("SpeedPotion player = " + player.name);

                    float originalSpeed = player.Info.CurrentSpeed;
                    i.Set(NumInfo.Min, originalSpeed);

                    player.Info.CurrentSpeed = originalSpeed * 1.5f;

                    break;
                }

            case EventTypes.Update:
                {
                    float t = i.GetFloat(NumInfo.Default, 0f);
                    if (t <= 0) return;

                    t -= Time.deltaTime;
                    i.Set(NumInfo.Default, t);

                    var itemThing = i.Who.Thing;
                    if (itemThing == null) return;

                    var body = itemThing.Body;
                    if (body == null) return;

                    var player = body.Who;
                    if (player == null) return;

                    if (t <= 0)
                    {
                        float originalSpeed = i.GetFloat(NumInfo.Min, player.Info.CurrentSpeed);
                        player.Info.CurrentSpeed = originalSpeed;
                        i.Set(NumInfo.Default, 0f);

                        Debug.Log("SpeedPotion ended");
                    }

                    break;
                }
        }
    }
}
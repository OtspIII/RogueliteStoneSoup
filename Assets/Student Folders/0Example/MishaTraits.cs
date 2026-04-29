
using System.Collections;
using UnityEngine;

public class TestTrait_Misha : Trait
{
   public TestTrait_Misha()
    {
        Type = Traits.TestTrait_Misha;
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
            {
                ThingInfo bl = God.Library.GetThing("EvilBlock").Create();
                bl.Spawn(i.Who.Thing.transform.position + new Vector3(1, 0));

                ThingInfo bl2 = God.Library.GetThing("EvilBlock").Create();
                bl2.Spawn(i.Who.Thing.transform.position + new Vector3(-1, 0));
                i.Set(bl);
                i.SetThing(ThingEInfo.C, bl2);
                i.Who.Thing.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                i.Set(BoolInfo.Default, true);
                i.SetFloat(NumInfo.Default,0);
                break;
            }
            case EventTypes.Update:
            {
                float offset = i.GetFloat();
                if (i.GetBool())
                {
                    offset += Time.time * 3;
                    if (offset > 5) i.SetBool(BoolInfo.Default,false);
                }
                else
                {
                    offset -= Time.time * 3;
                    if (offset < -5) i.SetBool(BoolInfo.Default,true);
                }

                ThingInfo bl = i.GetThing();
                bl.Thing.transform.position = i.Who.Thing.transform.position + new Vector3(-1, offset);
                ThingInfo bl2 = i.GetThing(ThingEInfo.C);
                bl2.Thing.transform.position = i.Who.Thing.transform.position + new Vector3(-1, -offset);
                break;
            }
        }
    }
}
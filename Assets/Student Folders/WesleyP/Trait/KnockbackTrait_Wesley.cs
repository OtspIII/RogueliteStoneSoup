using UnityEngine;

public class KnockbackTrait_Wesley : Trait
{
    public float Knockback = 10f;

    Type = Traits.Knockback;
    AddListen(EvenTypes.Update);
    public override void TakeEvent(TraitInfo i, EvenInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Updates:
                {
                    Debug.log("Knockback");
                    break;
                }
        }

        base.TakeEvent(i, e);
        hit.TakeKnockback(Who.Thing.transform.position, Knockback);


        //Reference the AttackAction kncokback
        //ref the lunge script

        // Update is called once per frame
        void Update()
        {

        }
    }

    public LungeAction(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Lunge, who);
        Anim = "Lunge";
        MoveMult = 0;
        Knockback = 34f;
    }
}

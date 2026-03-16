using UnityEngine;

public class KnockbackTrait_Wesley : Trait
{
    public class AttackAction : UseAction
    {
        public float Knockback = 10;
    }
    public class LungeAction : AttackAction
    {
        public LungeAction(ThingInfo who, EventInfo e = null)
        {
            Setup(Actions.Lunge, who);
            Anim = "Lunge";
            MoveMult = 1;
            Knockback = 12;
        }
        public override void OnRun()
        {
            base.OnRun();
            if (Phase == 0)
            {
                MoveMult = Who.AttackRange;
                Who.Thing.MoveForwards();
            }
            else
                MoveMult = 0;
        }
    }
}


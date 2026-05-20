using UnityEngine;

public class DoMoreDamageAction_MichaelT : AttackAction
{
    public DoMoreDamageAction_MichaelT(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.DoMoreDamageAttack_MichaelT, who);
        Anim = "Swing";
        Damage = 3;
    }

    public override float GetDamage()
    {
        return base.GetDamage() * 2f;
    }
}
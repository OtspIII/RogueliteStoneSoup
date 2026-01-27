using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileOption", menuName = "Scriptable Objects/ProjectileOption")]
public class ProjectileOption : ThingOption
{
    public float Damage;
    public float Speed;
    
    public override ThingInfo Create()
    {
        ThingInfo r = base.Create();
        r.AddTrait(Traits.Projectile,God.E().Set(NumInfo.Speed,Speed)).Set(NumInfo.Amount,Damage);
        return r;
    }
}

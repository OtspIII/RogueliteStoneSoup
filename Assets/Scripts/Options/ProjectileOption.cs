using UnityEngine;

///A type of Option that auto-adds the Projectile trait
[CreateAssetMenu(fileName = "ProjectileOption", menuName = "Scriptable Objects/ProjectileOption")]
public class ProjectileOption : ThingOption
{
    //How much damage does the projectile do on an impact?
    public float Damage;
    //How fast does the projectile fly?
    public float Speed;
    
    public override ThingInfo Create()
    {
        ThingInfo r = base.Create();
        r.AddTrait(Traits.Projectile,God.E().Set(NumInfo.Speed,Speed)).Set(NumInfo.Default,Damage);
        return r;
    }
}

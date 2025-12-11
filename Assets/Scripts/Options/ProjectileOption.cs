using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileOption", menuName = "Scriptable Objects/ProjectileOption")]
public class ProjectileOption : ThingOption
{
    public float Damage;
    public float Speed;
    
    public override ThingInfo Create()
    {
        return base.Create();
    }
}

using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // public Animator Anim;
    public ThingController Who;
    public HurtboxController Hurtbox;
    public float Size = 1;
    public Actions DefaultAttack = Actions.Swing;
    public WeaponStats Stats;
    public TraitInfo WeaponTrait;
    public ThingSeed Seed;
    public SpriteRenderer SR;
    
    public void Setup(ThingController who)
    {
        Who = who;
        gameObject.name = "Weapon";
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(Size,Size,1);
        if (God.IsPlayer(Who)) Hurtbox.SetPlayer(true);
        Imprint(Who.CurrentWeapon);
    }
    
    public void SetPhase(int n)
    {
        Who.SetPhase(n);
    }
    
    public void Imprint(TraitInfo stats)
    {
        WeaponTrait = stats;
        Seed = WeaponTrait.Seed;
        SR.sprite = God.Library.GetWeaponArt(Seed.Art,SR.sprite);
    }
    
    public void Imprint(WeaponStats stats)
    {
        Stats = stats;
        SR.sprite = God.Library.GetWeaponArt(Stats.Art,SR.sprite);
        if(!string.IsNullOrEmpty(stats.DefaultAttack)) DefaultAttack = Enum.Parse<Actions>(stats.DefaultAttack);
    }
}

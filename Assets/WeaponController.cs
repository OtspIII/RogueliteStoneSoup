using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // public Animator Anim;
    public ActorController Who;
    public HurtboxController Hurtbox;
    public float Size = 1;
    public Actions DefaultAttack = Actions.Swing;
    public WeaponStats Stats;
    public SpriteRenderer SR;
    
    public void Setup(ActorController who)
    {
        Who = who;
        gameObject.name = "Weapon";
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(Size,Size,1);
        if (Who.IsPlayer) Hurtbox.SetPlayer(true);
        Imprint(Who.CurrentWeapon);
    }
    
    public void SetPhase(int n)
    {
        Who.SetPhase(n);
    }

    public void Imprint(WeaponStats stats)
    {
        Stats = stats;
        SR.sprite = God.Library.GetWeaponArt(Stats.Art,SR.sprite);
        if(!string.IsNullOrEmpty(stats.DefaultAttack)) DefaultAttack = Enum.Parse<Actions>(stats.DefaultAttack);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Animator Anim;
    public BodyController Weapon;
    public ThingController Who;
    public HitboxController Hitbox;
    public HurtboxController Hurtbox;
    public float Size = 1;
    public SpriteRenderer SR;
    public Dictionary<string,float> Anims = new Dictionary<string, float>();

    public void Setup(ThingController who,string art="",bool weapon=false)
    {
        Who = who;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(Size,Size,1);
        if (!weapon && who.CurrentWeapon != null)
        {
            Weapon = Instantiate(God.Library.WeaponBody, transform);
            // Weapon = Instantiate(God.Library.GetWeaponPrefab(who.CurrentWeapon.Seed.Body), transform);
            Weapon.Setup(Who,"",true);
        }
        if(Who.Info.Has(Traits.Player) && Hitbox != null) Hitbox.SetPlayer(true);
        if(art != "")
            SR.sprite = God.Library.GetArt(art,SR.sprite);
        if(Anim != null)
            foreach(AnimationClip c in Anim.runtimeAnimatorController.animationClips)
                Anims.Add(c.name,c.length);
    }

    public float PlayAnim(string a)
    {
        if (Anim == null) return 0;
        if (Anims.TryGetValue(a,out float r))
        {
            Anim.Play(a);
            return r * Anim.speed;
        }
        Anim.Play("Idle");
        return 0;
    }

    public void SetPhase(int n)
    {
        Who.SetPhase(n);
    }

    public void SetHitbox(bool tf)
    {
        Hitbox.Coll.enabled = tf;
    }
    
    public void SetHurtbox(bool tf)
    {
        Hurtbox.Coll.enabled = tf;
    }
}

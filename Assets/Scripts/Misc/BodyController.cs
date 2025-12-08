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
        if (God.IsPlayer(Who)) Hitbox.SetPlayer(true);
        if(art != "")
            SR.sprite = God.Library.GetArt(art,SR.sprite);
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

using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Animator Anim;
    public WeaponController Weapon;
    public ThingController Who;
    public HitboxController Hitbox;
    public float Size = 1;
    public SpriteRenderer SR;

    public void Setup(ThingController who)
    {
        Who = who;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(Size,Size,1);
        Weapon = Instantiate(God.Library.GetWeaponPrefab(who.CurrentWeapon.Seed.Body), transform);
        Weapon.Setup(Who);
        if (God.IsPlayer(Who)) Hitbox.SetPlayer(true);
        SR.sprite = God.Library.GetArt(who.Stats.Art,SR.sprite);
    }

    public void SetPhase(int n)
    {
        Who.SetPhase(n);
    }

    public void SetHitbox(bool tf)
    {
        Hitbox.Coll.enabled = tf;
    }
}

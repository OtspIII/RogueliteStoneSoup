using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Animator Anim;
    public WeaponController Weapon;
    public ActorController Who;
    public HitboxController Hitbox;

    public void Setup(ActorController who)
    {
        Who = who;
        transform.localPosition = new Vector3(0, 0, 0);
        Weapon = Instantiate(God.Library.WeaponPrefab, transform);
        Weapon.Setup(Who);
        if (Who.IsPlayer) Hitbox.SetPlayer(true);
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

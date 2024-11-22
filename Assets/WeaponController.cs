using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // public Animator Anim;
    public ActorController Who;
    public HurtboxController Hurtbox;
    
    public void Setup(ActorController who)
    {
        Who = who;
        gameObject.name = "Weapon";
        transform.localPosition = new Vector3(0, 0, 0);
        if (Who.IsPlayer) Hurtbox.SetPlayer(true);
    }
    
    public void SetPhase(int n)
    {
        Who.SetPhase(n);
    }
}

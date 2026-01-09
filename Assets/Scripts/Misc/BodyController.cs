using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Animator Anim;
    public BodyController Weapon;
    public ThingController Who;
    public HitboxController Hitbox;
    // public HurtboxController Hurtbox;
    public float Size = 1;
    public SpriteRenderer SR;
    public Dictionary<string,float> Anims = new Dictionary<string, float>();
    public string DefaultAnim = "Idle";

    public void Setup(ThingController who,ThingOption type=null,bool weapon=false)
    {
        Who = who;
        // gameObject.name = Who.Info.Name + " Body";
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(Size,Size,1);
        if (!weapon && who.CurrentWeapon != null)
        {
            // ThingOption w = God.Library.GetThing(GameTags.Weapon);
            ThingOption w = who.CurrentWeapon.Who.Type;
            Weapon = Instantiate(w.Body, transform);
            // Weapon = Instantiate(God.Library.GetWeaponPrefab(who.CurrentWeapon.Seed.Body), transform);
            Weapon.Setup(Who,w,true);
            Weapon.gameObject.name = w.Name;
            Who.WeaponBody = Weapon;
        }

        if (type != null)
        {
            if (SR != null)
            {
                if (type.Art != null)
                    SR.sprite = type.Art;
                SR.color = type.Color;
            }
        }

        if(Anim != null)
            foreach(AnimationClip c in Anim.runtimeAnimatorController.animationClips)
                Anims.Add(c.name,c.length);
        if (!Anims.ContainsKey(DefaultAnim) && Anims.Keys.Count > 0)
            DefaultAnim = Anims.Keys.ToArray()[0];
    }

    public float PlayAnim(string a)
    {
        if (Anim == null) return 0;
        if (Anims.TryGetValue(a,out float r))
        {
            Anim.Play(a);
            return r * Anim.speed;
        }
        Anim.Play(DefaultAnim);
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

    public void SetTeam(GameTeams t)
    {
        if(Hitbox != null)
            Hitbox.SetTeam(t);
    }
}

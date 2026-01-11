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
            SetWeapon(who.CurrentWeapon);
        }

        if (type != null)
        {
            if (SR != null)
            {
                Sprite art = type.GetArt(weapon);
                if (art != null)
                    SR.sprite = art;
                SR.color = type.Color;
            }
        }

        if(Anim != null)
            foreach(AnimationClip c in Anim.runtimeAnimatorController.animationClips)
                Anims.Add(c.name,c.length);
        if (!Anims.ContainsKey(DefaultAnim) && Anims.Keys.Count > 0)
            DefaultAnim = Anims.Keys.ToArray()[0];
    }

    public void SetWeapon(ThingInfo wpn)
    {
        if (Weapon != null)
        {
            Destroy(Weapon.gameObject);
        }
        // ThingOption w = God.Library.GetThing(GameTags.Weapon);
        ThingOption w = wpn.Type;
        Weapon = Instantiate(w.GetBody(true), transform);
        // Weapon = Instantiate(God.Library.GetWeaponPrefab(who.CurrentWeapon.Seed.Body), transform);
        Weapon.Setup(Who,w,true);
        Weapon.gameObject.name = w.Name;
        Who.WeaponBody = Weapon;
    }

    public float PlayAnim(string a,float speed=1)
    {
        if (Anim == null) return 0;
        Anim.speed = speed;
        if (Anims.TryGetValue(a,out float r))
        {
            Anim.Play(a);
            return r / Anim.speed;
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

    public List<ThingController> GetTouching(HitboxTypes filter=HitboxTypes.None)
    {
        List<ThingController> r = new List<ThingController>();
        if (Hitbox != null && (filter == HitboxTypes.None || Hitbox.Type == filter))
        {
            r.AddRange(Hitbox.Touching);
        }
        if(Weapon != null) r.AddRange(Weapon.GetTouching(filter));
        return r;
    }
}

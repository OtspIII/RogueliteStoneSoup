using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BodyController : MonoBehaviour
{
    public Animator Anim;
    public Transform ItemHolder;
    public BodyController Held;
    public ThingController Who;
    public HitboxController Hitbox;
    // public HurtboxController Hurtbox;
    public float Size = 1;
    public SpriteRenderer SR;
    public Dictionary<string,float> Anims = new Dictionary<string, float>();
    public string DefaultAnim = "Idle";

    public void Setup(ThingController who,ThingOption type=null,bool held=false)
    {
        Who = who;
        if (type != null) Size = type.Size;
        if (ItemHolder == null)
        {
            ItemHolder = new GameObject("Item Holder").transform;
            ItemHolder.parent = transform;
            ItemHolder.localPosition = new Vector3(Size * 0.7f, 0, 0);
        }
        // gameObject.name = Who.Info.Name + " Body";
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(Size,Size,1);
        if (!held && who.CurrentHeld != null)
        {
            SetHeld(who.CurrentHeld);
        }

        if (type != null)
        {
            if (SR != null)
            {
                Sprite art = type.GetArt(held);
                if (art != null)
                    SR.sprite = art;
                SR.color = type.Color;
            }
        }

        if (Anim != null)
        {
            foreach (AnimationClip c in Anim.runtimeAnimatorController.animationClips)
                Anims.Add(c.name, c.length);
            Anim.Rebind();
        }

        if (!Anims.ContainsKey(DefaultAnim) && Anims.Keys.Count > 0)
            DefaultAnim = Anims.Keys.ToArray()[0];
            
    }

    public void SetHeld(ThingInfo wpn)
    {
        if (Held != null)
        {
            Destroy(Held.gameObject);
        }
        // ThingOption w = God.Library.GetThing(GameTags.Weapon);
        ThingOption w = wpn.Type;
        Held = Instantiate(w.GetBody(true), ItemHolder);
        // Weapon = Instantiate(God.Library.GetWeaponPrefab(who.CurrentWeapon.Seed.Body), transform);
        Held.Setup(Who,w,true);
        Held.gameObject.name = w.Name;
        Who.HeldBody = Held;
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

    ///Called by animations, to let the Thing know what phase of the animation is currently playing
    public void SetPhase(int n)
    {
        Who.TakeEvent(God.E(EventTypes.SetPhase).Set(n));
        // Who.SetPhase(n);
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
        if(Held != null) r.AddRange(Held.GetTouching(filter));
        return r;
    }
}

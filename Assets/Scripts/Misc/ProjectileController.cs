// using System;
// using UnityEngine;
//
// public class ProjectileController : MonoBehaviour
// {
//     public Rigidbody2D RB;
//     public ProjStats Stats;
//     public ThingController Source;
//     
//     public virtual void Setup(ThingController source, ProjStats stat)
//     {
//         Source = source;
//         Stats = stat;
//         RB.linearVelocity = transform.up * Stats.Speed;
//         SetPlayer(Source.Info.Has(Traits.Player));
//     }
//
//     private void OnCollisionEnter2D(Collision2D other)
//     {
//         HitboxController a = other.collider.gameObject.GetComponent<HitboxController>();
//         if(a != null && a.Who != Source) Payload(a.Who);
//         Destroy(gameObject);
//     }
//
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         HitboxController a = other.gameObject.GetComponent<HitboxController>();
//         if (a != null && a.Who != Source) return;
//         if(a != null) Payload(a.Who);
//         Destroy(gameObject);
//     }
//
//     public void SetPlayer(bool isPlayer=true)
//     {
//         if(isPlayer) gameObject.layer = LayerMask.NameToLayer("PHurtbox");
//         else gameObject.layer = LayerMask.NameToLayer("MHurtbox");
//     }
//     
//     public virtual void Payload(ThingController a)
//     {
//         a.TakeEvent(God.E(EventTypes.Damage).Set(NumInfo.Amount,Stats.Damage));
//         // a.TakeDamage(Stats.Damage);
//     }
// }

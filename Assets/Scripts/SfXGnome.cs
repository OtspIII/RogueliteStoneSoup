using UnityEngine;

public class SfXGnome : MonoBehaviour
{
    public ParticleSystem Particles;
    public AudioSource AS;
    
    public void Setup(GnomeOption o,int amt=0)
    {
        float life = 0;
        if (o.Particles != null)
        {
            if (amt == 0) amt = o.ParticleBlast;
            Particles = Instantiate(o.Particles, transform);
            Particles.transform.localPosition = Vector3.zero;
            if(amt > 0) Particles.Emit(amt);
            life = Mathf.Max(Particles.main.duration, Particles.main.startLifetime.constantMax);
        }
        if (o.Audio != null)
        {
            AS.PlayOneShot(o.Audio);
            life = Mathf.Max(life, o.Audio.length);
        }
        if(!o.ManualDelete)
            Destroy(gameObject,life + 0.5f);
    }
}

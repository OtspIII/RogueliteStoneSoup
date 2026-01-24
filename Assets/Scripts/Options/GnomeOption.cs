using UnityEngine;

[CreateAssetMenu(fileName = "GnomeOption", menuName = "Scriptable Objects/GnomeOption")]
public class GnomeOption : GameOption
{
    public ParticleSystem Particles;
    public int ParticleBlast = 0;
    public AudioClip Audio;
    public bool ManualDelete = false;

    public SfXGnome Spawn(Transform where, int amt=0)
    {
        SfXGnome r = Instantiate(God.Library.GnomePrefab,where);
        r.Setup(this,amt);
        return r;
    }
    
    public SfXGnome Spawn(Vector2 pos, int amt=0)
    {
        SfXGnome r = Instantiate(God.Library.GnomePrefab,pos,Quaternion.identity);
        r.Setup(this,amt);
        return r;
    }
    
    public SfXGnome Spawn(ThingInfo where, int amt=0,bool sticky=false)
    {
        if (sticky) return Spawn(where.Thing.transform, amt);
        return Spawn(where.Thing.transform.position, amt);
    }
}

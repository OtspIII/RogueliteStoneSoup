using UnityEngine;

[CreateAssetMenu(fileName = "GnomeOption", menuName = "Scriptable Objects/GnomeOption")]
public class GnomeOption : ScriptableObject
{
    public ParticleSystem Particles;
    public int ParticleBlast = 0;
    public AudioClip Audio;

    public SfXGnome Spawn(Vector2 pos) //INCOMPLETE!!!
    {
        SfXGnome r = Instantiate(God.Library.GnomePrefab,pos,Quaternion.identity);
        r.Setup(this);
        return r;
    }
}

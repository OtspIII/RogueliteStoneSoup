using UnityEngine;

public class UpgradeTrait : Trait
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //if sword, increase range
    //if axe, increase spd and size
    //if Bow, increase shootspeed?
    //if wand, refresh its use+ projectiles home on targets
    //if staff, 
    public UpgradeTrait()
    {
            
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //Type = Traits.UpgradeTrait; //this is causing bugs for some reason

            //triggers whenever you obtain a new item:
        }
    }
}

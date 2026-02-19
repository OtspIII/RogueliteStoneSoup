using UnityEngine;

public class StatusResist : Trait
{
    public StatusResist()
    {
        Type = Traits.StatusResist;
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {

        switch (e.Type) 
        {
        
        }
    }
}

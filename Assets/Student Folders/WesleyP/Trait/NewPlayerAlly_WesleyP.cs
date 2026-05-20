using System.Collections.Generic;
using UnityEngine;

public class NewPlayerAlly_WesleyP : Trait
{
    public NewPlayerAlly_WesleyP()
    {
        Type = Traits.Hostile;
        AddListen(EventTypes.OnSee);
        AddListen(EventTypes.OnTargetDie);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
           
        }
    }

   
   
}
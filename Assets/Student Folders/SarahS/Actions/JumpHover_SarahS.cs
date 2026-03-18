using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class JumpHover_SarahS : ActionScript
{
   public JumpHover_SarahS(ThingInfo who, EventInfo e = null)
   {
      Setup(Actions.JumpHover_SarahS,who,true);
      MoveMult = 1;
   }
}

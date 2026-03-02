using UnityEngine;

public class DefendAction : ActionScript
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public DefendAction(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.DefendAction_AdamD, who);
    }
}

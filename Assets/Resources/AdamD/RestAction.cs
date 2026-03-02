using UnityEngine;

public class RestAction : ActionScript
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RestAction(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.RestAction_AdamD, who);
    }
}

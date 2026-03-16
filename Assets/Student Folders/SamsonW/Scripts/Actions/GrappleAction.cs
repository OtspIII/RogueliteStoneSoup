using System.Collections;
using UnityEngine;

public class GrappleAction : ActionScript
{
    private Vector3 targetPosition;
    
    public GrappleAction(ThingInfo who,EventInfo e=null)
    {
       // Setup(Actions.Grapple,who,true);
        Duration = e.GetFloat(NumInfo.Time, 1f);
        Priority = e.GetInt(NumInfo.Priority); // Assign priority to make this action noncancellable
        targetPosition = e.GetVector(VectorInfo.Position);
    }

    public override IEnumerator Script()
    {
        Vector3 startPosition = Who.Thing.transform.position;
        for (float t = 0f; t < Duration; t += Time.fixedDeltaTime)
        {
            float parameter = t / Duration;
            float easedParameter = 1 - Mathf.Pow(1 - parameter, 3); // Eased out cubic
            Who.Thing.transform.position = Vector3.Lerp(startPosition, targetPosition, easedParameter);
            yield return new WaitForFixedUpdate();
        }
        Who.Thing.transform.position = targetPosition;
        End();
    }
}
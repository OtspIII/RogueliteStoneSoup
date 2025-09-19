using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Directions Dir;

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
    
    public void TurnOn()
    {
        gameObject.SetActive(true);
    }
}

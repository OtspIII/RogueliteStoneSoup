using UnityEngine;

public class RoomScript : MonoBehaviour
{
    
    
    public void Setup(GameManager gm)
    {
        transform.parent = gm.LevelHolder;
    }
}

using System;
using UnityEngine;

public class GameLibrary : MonoBehaviour
{
    public ActorController ActorPrefab;
    public BodyController HumanoidBodyPrefab;
    public BodyController LungerBodyPrefab;
    public WeaponController SwordPrefab;
    public WeaponController TramplePrefab;

    private void Awake()
    {
        God.Library = this;
    }

    public BodyController GetBody(string which)
    {
        switch (which)
        {
            case "Humanoid": return HumanoidBodyPrefab;
            case "Lunger": return LungerBodyPrefab;
        }
        Debug.Log("INVALID BODY NAME: " + which);
        return HumanoidBodyPrefab;
    }
    
    public WeaponController GetWeapon(string which)
    {
        switch (which)
        {
            case "Sword": return SwordPrefab;
            case "Trample": return TramplePrefab;
        }
        Debug.Log("INVALID WEAPON NAME: " + which);
        return SwordPrefab;
    }
}

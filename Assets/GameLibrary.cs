using System;
using UnityEngine;

public class GameLibrary : MonoBehaviour
{
    public ActorController ActorPrefab;
    public BodyController BodyPrefab;
    public WeaponController WeaponPrefab;

    private void Awake()
    {
        God.Library = this;
    }
}

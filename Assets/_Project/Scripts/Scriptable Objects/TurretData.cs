﻿using UnityEngine;

namespace Scripts.Scriptable_Objects
{
    [CreateAssetMenu]
    public class TurretData : ScriptableObject
    {
        public GameObject Ammo;
        public float Range;
        public float FireRate;
        public float FieldOfView;
        public AiStates AiState;


        public bool Rotates;

        public GameObject SpawnAmmo(Transform transform)
        {
            return Instantiate(Ammo, transform);
        }
    }


    public enum AiStates { FIRST, NEAREST, FURTHEST, WEAKEST, STRONGEST };
}
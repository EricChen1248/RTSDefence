using UnityEngine;

namespace Scripts.Scriptable_Objects
{
    [CreateAssetMenu]
    public class TurretData : ScriptableObject
    {
        public AiStates AiState;
        public GameObject Ammo;
        public float FieldOfView;
        public float FireRate;
        public float Range;


        public bool Rotates;

        public GameObject SpawnAmmo(Transform transform)
        {
            return Instantiate(Ammo, transform);
        }
    }


    public enum AiStates
    {
        FIRST,
        NEAREST,
        FURTHEST,
        WEAKEST,
        STRONGEST
    }
}
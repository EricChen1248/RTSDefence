using UnityEngine;

namespace Scripts.Scriptable_Objects
{
    [CreateAssetMenu]
    public class EnemyData : ScriptableObject
    {
        public int Health;
        public float Radius;
        [Tooltip("Default is 3.5")]
        public float MovementSpeed = 3.5f;
        public LayerMask TargetLayers;
        public float ReloadTime;
        public bool FindGroup;

        public EnemyType[] Types;
    }

    public enum EnemyType
    {
        Scout,
        FireResistant,
        BlastResistant,
        SuicideBomber,
    }
}

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
        public int Damage = 10;
        public LayerMask TargetLayers;
        public float ReloadTime;
        public bool FindGroup;
        public int Points = 10;

        public EnemyType[] Types;
    }

    public enum EnemyType
    {
        Scout,
        FireResistant,
        BlastResistant,
        SuicideBomber,
        HardHitting,
        Fast,
        Tank,
    }
}

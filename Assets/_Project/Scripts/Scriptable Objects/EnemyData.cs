using UnityEngine;

namespace Scripts.Scriptable_Objects
{
    [CreateAssetMenu]
    public class EnemyData : ScriptableObject
    {
        public int Damage = 10;
        public bool FindGroup;
        public int Health;

        [Tooltip("Default is 3.5")] public float MovementSpeed = 3.5f;

        public int Points = 10;
        public float Radius;
        public float ReloadTime;
        public LayerMask TargetLayers;

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
        Tank
    }
}
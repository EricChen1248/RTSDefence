using UnityEngine;

namespace Scripts.Scriptable_Objects
{
    public class EnemyData : ScriptableObject
    {
        public int Health;
        public float Radius;
        public LayerMask TargetLayers;
        public float ReloadTime;
        public bool FindGroup;
    }
}

using System;
using Scripts.Resources;
using Scripts.Buildable_Components;
using UnityEngine;

namespace Scripts.Scriptable_Objects
{
    [CreateAssetMenu]
    public class BuildData : ScriptableObject
    {
        public string Name;
        public GameObject GhostModel;

        public int Health;

        public Vector3 Offset;
        public Vector3 Size;
        public Vector3 ColliderOffset;
        public bool CanRotateHorizontal;
        public bool CanRotateVertical;

        public int Points;

        public int BuildTime;
        public RecipeItem[] Recipe;

        public DefenceType[] Types;
    }

    [Serializable]
    public struct RecipeItem
    {
        public ResourceTypes Resource;
        public int Amount;
    }
}
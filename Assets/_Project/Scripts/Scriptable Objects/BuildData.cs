using System;
using Scripts.Buildable_Components;
using Scripts.Resources;
using UnityEngine;

namespace Scripts.Scriptable_Objects
{
    [CreateAssetMenu]
    public class BuildData : ScriptableObject
    {
        public int BuildTime;
        public bool CanRotateHorizontal;
        public bool CanRotateVertical;
        public Vector3 ColliderOffset;
        public GameObject GhostModel;

        public int Health;
        public string Name;

        public Vector3 Offset;

        public int Points;
        public RecipeItem[] Recipe;
        public Vector3 Size;

        public DefenceType[] Types;
    }

    [Serializable]
    public struct RecipeItem
    {
        public ResourceTypes Resource;
        public int Amount;
    }
}
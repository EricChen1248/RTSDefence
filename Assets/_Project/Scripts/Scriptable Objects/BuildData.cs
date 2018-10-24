﻿using System;
using Scripts.Resources;
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
        public bool CanRotateHorizontal;
        public bool CanRotateVertical;

        public int BuildTime;
        public RecipeItem[] Recipe;
    }

    [Serializable]
    public struct RecipeItem
    {
        public ResourceTypes Resource;
        public int Amount;
    }
}
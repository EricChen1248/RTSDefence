using System.Collections.Generic;
using Entity_Components;
using UnityEngine;
using UnityEngine.AI;

namespace Buildable_Components
{
    [RequireComponent(typeof(NavMeshObstacle))]
    [RequireComponent(typeof(HealthComponent))]
    public class Buildable : MonoBehaviour
    {
        public string Name { get; protected set; }

        /// <summary>
        /// A ghost model with no components on it to show user where the object will be placed
        /// </summary>
        public GameObject GhostModel;
    
        // TODO: need to set recipe
        public Dictionary<object, int> RecipeDictionary { get; protected set; }

        // Use this for initialization
        protected virtual void Start ()
        {
            Name = "BaseBuildable";
        }
    }
}
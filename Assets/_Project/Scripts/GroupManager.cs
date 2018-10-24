using System.Collections.Generic;
using Scripts.Controllers;
using UnityEngine;

namespace Scripts
{
    [DefaultExecutionOrder(-99)]
    public class GroupManager : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
            CoreController.Instance.GroupsGameObject = gameObject;
        }

        public IEnumerable<Transform> Children()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                print("Apply for Group No." + i);
                yield return transform.GetChild(i);
            }
        }
    }
}

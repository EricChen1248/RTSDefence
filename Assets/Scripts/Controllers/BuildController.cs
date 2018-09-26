using System;
using System.Runtime.Serialization;
using Buildable_Components;
using UnityEngine;

namespace Controllers
{
    public class BuildController : MonoBehaviour
    {
        public GameObject CurrentGameObject;
        public GameObject CurrentGhostModel;
        public bool IsBuilding { get; private set; }

        // Use this for initialization
        private void Start ()
        {
            enabled = false;
        }
	
        // Update is called once per frame
        private void Update ()
        {
            if (IsBuilding)
            {
                // If right clicked
                if (Input.GetMouseButtonDown(1))
                {
                    DeselectBuild();
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    Build();
                }
            } 
            else
            {
                throw new BuildControllerException("Build controller is active when it shouldn't be");
            }
        }

        public void EnableBuildMode(GameObject selectedGameObject)
        {
            CurrentGameObject = selectedGameObject;
            GetComponent<MouseController>().enabled = false;

            enabled = true;
        }

        private void DeselectBuild()
        {
            GetComponent<MouseController>().enabled = true;
            IsBuilding = false;

            CurrentGameObject = null;
            CurrentGhostModel = null;

            enabled = false;
        }

        private void CreateGhostModel()
        {
            Instantiate(CurrentGameObject.GetComponent<Buildable>().GhostModel);
        }

        private void Build()
        {
            Instantiate(CurrentGameObject);
            DeselectBuild();
        }
        

    }

    [Serializable]
    public class BuildControllerException : Exception
    {
        public BuildControllerException()
        {
        }

        public BuildControllerException(string message) : base(message)
        {
        }

        public BuildControllerException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BuildControllerException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

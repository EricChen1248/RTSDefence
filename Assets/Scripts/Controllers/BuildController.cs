using System;
using System.Runtime.Serialization;
using Buildable_Components;
using Navigation;
using UnityEngine;

namespace Controllers
{
    public class BuildController : MonoBehaviour
    {
        public Camera MainCamera;
        public GameObject CurrentGameObject;
        public GameObject CurrentGhostModel;
        public bool IsBuilding { get; private set; }

        // Use this for initialization
        private void Start ()
        {
            MainCamera = Camera.main;
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
                else
                {
                    Vector3 mouseLocation;
                    RaycastHelper.TryMouseRaycast(out mouseLocation,
                        RaycastHelper.LayerMaskDictionary["Buildable Surface"]);
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
            Destroy(CurrentGhostModel);
            CurrentGhostModel = null;

            enabled = false;
        }

        private void CreateGhostModel()
        {
            CurrentGhostModel = Instantiate(CurrentGameObject.GetComponent<Buildable>().Data.GhostModel);
        }

        private void Build()
        {
            Instantiate(CurrentGameObject);
            // TODO : Get recipe and remove resources
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

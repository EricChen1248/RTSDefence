using System;
using System.Runtime.Serialization;
using Buildable_Components;
using Navigation;
using Scriptable_Objects;
using UnityEngine;

namespace Controllers
{
    public class BuildController : MonoBehaviour
    {
        public Camera MainCamera;
        public GameObject CurrentGameObject;
        public GameObject CurrentGhostModel;
        private BuildData _currentData;

        // Use this for initialization
        private void Start ()
        {
            MainCamera = Camera.main;
            enabled = false;
        }
	
        // Update is called once per frame
        private void Update ()
        {
            var screenRect = new Rect(0, 0, Screen.width, Screen.height);
            if (!screenRect.Contains(Input.mousePosition))
                return;

            // If right clicked
            if (Input.GetMouseButtonDown(1))
            {
                DeselectBuild();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (CurrentGhostModel.GetComponent<GhostModelScript>().CanBuild())
                {
                    Build();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (_currentData.CanRotateHorizontal)
                {
                    CurrentGhostModel.transform.Rotate(Vector3.up, 90);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_currentData.CanRotateVertical)
                {
                    CurrentGhostModel.transform.Rotate(Vector3.left, 90);
                }
            }
            else
            {
                Vector3 mouseLocation;
                if (RaycastHelper.TryMouseRaycastToGrid(out mouseLocation, RaycastHelper.LayerMaskDictionary["Buildable Surface"]))
                {
                    
                    CurrentGhostModel.transform.position = mouseLocation - new Vector3(0.5f, -0.5f, 0.5f) - CurrentGhostModel.transform.rotation * _currentData.Offset;
                }
            }
        }

        public void EnableBuildMode(GameObject selectedGameObject)
        {
            CurrentGameObject = selectedGameObject;
            _currentData = CurrentGameObject.GetComponent<Buildable>().Data;

            CoreController.MouseController.enabled = false;
            enabled = true;

            CreateGhostModel();
        }

        private void DeselectBuild()
        {
            GetComponent<MouseController>().enabled = true;

            CurrentGameObject = null;
            Destroy(CurrentGhostModel);
            CurrentGhostModel = null;

            enabled = false;
        }

        private void CreateGhostModel()
        {
            CurrentGhostModel = Instantiate(_currentData.GhostModel);
        }

        private void Build()
        {
            var go = Instantiate(CurrentGameObject);
            go.transform.position = CurrentGhostModel.transform.position;
            go.transform.rotation = CurrentGhostModel.transform.rotation;

            // TODO : Get recipe and remove resources
            DeselectBuild();
        }
        
    }
}

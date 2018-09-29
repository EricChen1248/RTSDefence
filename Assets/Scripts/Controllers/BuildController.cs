using System;
using Buildable_Components;
using Navigation;
using Scriptable_Objects;
using UnityEngine;

namespace Controllers
{
    public class BuildController : MonoBehaviour
    {
        private GameObject _currentGameObject;
        private GameObject _currentGhostModel;
        private BuildData _currentData;

        // Use this for initialization
        private void Start ()
        {
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
                if (_currentGhostModel.GetComponent<GhostModelScript>().CanBuild())
                {
                    Build();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (_currentData.CanRotateHorizontal)
                {
                    _currentGhostModel.transform.Rotate(Vector3.up, 90);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_currentData.CanRotateVertical)
                {
                    _currentGhostModel.transform.Rotate(Vector3.left, 90);
                }
            }
            else
            {
                Vector3 mouseLocation;
                if (RaycastHelper.TryMouseRaycastToGrid(out mouseLocation, RaycastHelper.LayerMaskDictionary["Buildable Surface"]))
                {
                    _currentGhostModel.transform.position = mouseLocation - new Vector3(0, -0.5f, 0) - _currentGhostModel.transform.rotation * _currentData.Offset;
                }
            }
        }

        public void EnableBuildMode(GameObject selectedGameObject)
        {
            _currentGameObject = selectedGameObject;
            _currentData = _currentGameObject.GetComponent<Buildable>().Data;

            CoreController.MouseController.enabled = false;
            enabled = true;

            CreateGhostModel();
        }

        private void DeselectBuild()
        {
            GetComponent<MouseController>().enabled = true;

            _currentGameObject = null;
            Destroy(_currentGhostModel);
            _currentGhostModel = null;

            enabled = false;
        }

        private void CreateGhostModel()
        {
            _currentGhostModel = Instantiate(_currentData.GhostModel);
        }

        private void Build()
        {
            var go = Instantiate(_currentGameObject);
            go.transform.position = _currentGhostModel.transform.position;
            go.transform.rotation = _currentGhostModel.transform.rotation;

            // TODO : Get recipe and remove resources
            DeselectBuild();
        }
        
    }
}

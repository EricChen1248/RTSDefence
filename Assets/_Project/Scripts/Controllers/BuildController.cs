using System.Collections.Generic;
using Scripts.Buildable_Components;
using Scripts.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using BuildData = Scripts.Scriptable_Objects.BuildData;

namespace Scripts.Controllers
{
    public class BuildController : MonoBehaviour
    {
        #region Cheat Variables

        public bool NoGhost;

        #endregion

        #region Public Variables

        public BuildData[] ActiveDefences;
        public BuildData[] ResourceCollectors;
        public BuildData[] StaticDefences;
        public BuildData[] Traps;
        
        public Dictionary<ScriptableObject, Dictionary<int, GameObject>> BuiltObjects;

        #endregion

        #region Private Variables

        private GameObject _currentGameObject;
        private GameObject _currentGhostModel;
        private BuildData _currentData;

        #endregion

        #region Unity Callbacks
        
        // Use this for initialization
        private void Start ()
        {
            enabled = false;
            InitializeBuildDictionary();

        }

        // Update is called once per frame
        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

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
                var script = _currentGhostModel.GetComponent<GhostModelScript>();
                if (!script.CanBuild()) return;
                var ghost = _currentGhostModel;
                if (NoGhost)
                {
                    Build(_currentGhostModel);
                }
                else
                {
                    script.Activate();
                }

                _currentGhostModel = null;
                CreateGhostModel();
                _currentGhostModel.transform.rotation = ghost.transform.rotation;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateGhostModel(vertical: false);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateGhostModel(vertical: true);
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

        #endregion

        private void RotateGhostModel(bool vertical)
        {
            if (vertical)
            {
                if (!_currentData.CanRotateVertical) return;

                _currentGhostModel.transform.Rotate(Vector3.left, 90);
                return;
            }

            if (!_currentData.CanRotateHorizontal) return;

            _currentGhostModel.transform.Rotate(Vector3.up, 90);
        }


        public void EnableBuildMode(GameObject selectedGameObject)
        {
            _currentGameObject = selectedGameObject;
            _currentData = _currentGameObject.GetComponent<Buildable>().Data;

            CoreController.MouseController.enabled = false;
            enabled = true;

            CreateGhostModel();
        }

        private void InitializeBuildDictionary()
        {
            BuiltObjects = new Dictionary<ScriptableObject, Dictionary<int, GameObject>>();

            ActiveDefences = UnityEngine.Resources.LoadAll<BuildData>("Prefabs/Buildables/Data/Active Defence");
            ResourceCollectors = UnityEngine.Resources.LoadAll<BuildData>("Prefabs/Buildables/Data/Resource Collector");
            StaticDefences = UnityEngine.Resources.LoadAll<BuildData>("Prefabs/Buildables/Data/Static Defence");
            Traps = UnityEngine.Resources.LoadAll<BuildData>("Prefabs/Buildables/Data/Traps");

            foreach (var activeDefence in ActiveDefences)
            {
                BuiltObjects[activeDefence] = new Dictionary<int, GameObject>();
            }

            foreach (var resourceCollector in ResourceCollectors)
            {
                BuiltObjects[resourceCollector] = new Dictionary<int, GameObject>();
            }

            foreach (var staticDefence in StaticDefences)
            {
                BuiltObjects[staticDefence] = new Dictionary<int, GameObject>();
            }

            foreach (var trap in Traps)
            {
                BuiltObjects[trap] = new Dictionary<int, GameObject>();
            }
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
            if (_currentGhostModel != null) Destroy(_currentGhostModel);
            _currentGhostModel = Instantiate(_currentData.GhostModel);
            var script = _currentGhostModel.GetComponent<GhostModelScript>();
            script.AssignData(_currentData, _currentGameObject);
        }

        public void Build(GameObject ghostModel)
        {
            var go = Instantiate(ghostModel.GetComponent<GhostModelScript>().OriginalGameObject);
            go.transform.position = ghostModel.transform.position;
            go.transform.rotation = ghostModel.transform.rotation;

            // Add newly built object to the dictionary list
            if (!BuiltObjects.ContainsKey(_currentData))
            {
                BuiltObjects[_currentData] = new Dictionary<int, GameObject>();
            }
            BuiltObjects[_currentData][go.GetInstanceID()] = go;

            Destroy(ghostModel);

            NavigationBaker.Instance.Rebuild();
        }

        public void Destroy(Buildable building)
        {
            BuiltObjects[building.Data].Remove(building.ID);
        }
        
    }
}

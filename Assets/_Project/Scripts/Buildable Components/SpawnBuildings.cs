using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Controllers;
using Scripts.Entity_Components.Friendlies;
using Scripts.GUI;
using Scripts.Resources;
using UnityEngine;

namespace Scripts.Buildable_Components
{
    public class SpawnBuildings : Buildable
    {
        private readonly Dictionary<SpawnObject, int> _spawnCount = new Dictionary<SpawnObject, int>();

        private readonly Queue<SpawnObject> _spawnList = new Queue<SpawnObject>();
        private bool _spawning;

        [SerializeField] public SpawnObject[] Objects;

        public Vector3 SpawnPosition;

        public override void Start()
        {
            base.Start();
            foreach (var obj in Objects) _spawnCount[obj] = 0;
        }

        public void OnMouseDown()
        {
            CoreController.MouseController.SetFocus(this);
        }

        public override void Focus()
        {
            base.Focus();
            HasFocus = true;
            if (CoreController.MouseController.FocusedItem.Count == 1) UpdateGui();
        }

        public override void LostFocus()
        {
            base.LostFocus();
            HasFocus = false;
            ObjectMenuGroupComponent.Instance.Hide();
        }

        private IEnumerator StartSpawn()
        {
            _spawning = true;
            var progress = GetComponentInChildren<CircularProgress>();
            while (_spawnList.Count > 0)
            {
                for (var i = 0; i < 120; i++)
                {
                    progress.UpdateProgress(i / 120f);
                    yield return new WaitForFixedUpdate();
                }

                var spawn = _spawnList.Dequeue();
                var spawnedGo = Instantiate(spawn.Object);
                spawnedGo.transform.position = transform.position + SpawnPosition;
                ResourceController.AddResource(ResourceTypes.People, 1);
                _spawnCount[spawn]--;

                UpdateGui();
            }

            progress.UpdateProgress(0);
            _spawning = false;
        }

        private void UpdateGui()
        {
            if (!HasFocus) return;
            var omg = ObjectMenuGroupComponent.Instance;
            for (var i = 0; i < Objects.Length; i++)
            {
                var obj = Objects[i];
                var s = obj.Name + (_spawnCount[obj] > 0 ? " " + _spawnCount[obj] : "");
                omg.SetButton(i + 1, s, () =>
                {
                    var p = obj.Object.GetComponent<PlayerComponent>();
                    if (ResourceController.ResourceCount[ResourceTypes.Gold] < p.ResourceCount) return;
                    ResourceController.AddResource(ResourceTypes.Gold, -p.ResourceCount);
                    _spawnList.Enqueue(obj);
                    _spawnCount[obj]++;
                    UpdateGui();
                    if (!_spawning) StartCoroutine(StartSpawn());
                });
                omg.SetButtonImage(i + 1, obj.Texture);
            }
        }
    }

    [Serializable]
    public struct SpawnObject
    {
        public string Name;
        public GameObject Object;
        public Texture Texture;
    }
}
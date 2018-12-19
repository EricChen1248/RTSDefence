using UnityEngine;
using System.Collections;
using Scripts.GUI;
using System.Collections.Generic;
using Scripts.Interface;
using Scripts.Controllers;
using System;

namespace Scripts.Buildable_Components
{
    public class SpawnBuildings : Buildable, IClickable
    {
        [SerializeField]
        public SpawnObject[] Objects;
        public Vector3 SpawnPosition;

        private readonly Queue<SpawnObject> SpawnList = new Queue<SpawnObject>();
        private readonly Dictionary<SpawnObject, int> SpawnCount = new Dictionary<SpawnObject, int>();
        private bool spawning;

        public override void Start()
        {
            base.Start();
            foreach (var obj in Objects)
            {
                SpawnCount[obj] = 0;
            }
        }

        public void OnMouseDown()
        {
            CoreController.MouseController.SetFocus(this);
        }

        public override void Focus()
        {
            base.Focus();
            HasFocus = true;
            if (CoreController.MouseController.FocusedItem.Count == 1)
                UpdateGui();
        }

        public override void LostFocus()
        {
            base.LostFocus();
            HasFocus = false;
            ObjectMenuGroupComponent.Instance.Hide();
        }

        private IEnumerator StartSpawn()
        {
            spawning = true;
            var progress = GetComponentInChildren<CircularProgress>();
            while (SpawnList.Count > 0)
            {
                for (var i = 0; i < 120; i++)
                {
                    progress.UpdateProgress(i / 120f);
                    yield return new WaitForFixedUpdate();
                }

                var spawn = SpawnList.Dequeue();
                var spawnedGo = Instantiate(spawn.Object);
                spawnedGo.transform.position = transform.position + SpawnPosition;

                SpawnCount[spawn]--;

                UpdateGui();
            }

            progress.UpdateProgress(0);
            spawning = false;
        }

        private void UpdateGui()
        {
            if (!HasFocus) return;
            var omg = ObjectMenuGroupComponent.Instance;
            for (int i = 0; i < Objects.Length; i++)
            {
                var obj = Objects[i];
                var s = obj.Name + (SpawnCount[obj] > 0 ? " " + SpawnCount[obj] : "");
                omg.SetButton(i + 1, s, () =>
                {
                    SpawnList.Enqueue(obj);
                    SpawnCount[obj]++;
                    UpdateGui();
                    if (!spawning) StartCoroutine(StartSpawn());
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
using UnityEngine;
using System.Collections;
using Scripts.GUI;
using System.Collections.Generic;
using Scripts.Interface;
using Scripts.Controllers;
using System;
using UnityEngine.EventSystems;
using Scripts.Entity_Components.Friendlies;

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

        public bool HasFocus { get; private set; }
        public void Focus()
        {
            HasFocus = true;
            ObjectMenuGroupComponent.Instance.Show();
            UpdateGUI();

            StartCoroutine(DetectLoseFocus());
        }

        public void LostFocus()
        {
            HasFocus = false;
            ObjectMenuGroupComponent.Instance.Hide();
        }

        public void RightClick()
        {
        }

        private IEnumerator StartSpawn()
        {
            spawning = true;
            var progress = GetComponentInChildren<CircularProgress>();
            while (SpawnList.Count > 0)
            {
                for (int i = 0; i < 120; i++)
                {
                    progress.UpdateProgress(i / 120f);
                    yield return new WaitForFixedUpdate();
                }

                var spawn = SpawnList.Dequeue();
                var spawnedGO = Instantiate(spawn.Object);
                spawnedGO.transform.position = transform.position + SpawnPosition;
                SpawnCount[spawn]--;

                UpdateGUI();
            }

            progress.UpdateProgress(0);
            spawning = false;
        }

        private void UpdateGUI()
        {
            if (!HasFocus) return;
            var omg = ObjectMenuGroupComponent.Instance;
            for (int i = 0; i < Objects.Length; i++)
            {
                var obj = Objects[i];
                var s = obj.Name + (SpawnCount[obj] > 0 ? " " + SpawnCount[obj] : "");
                omg.SetButton(i, s, () =>
                {
                    SpawnList.Enqueue(obj);
                    SpawnCount[obj]++;
                    UpdateGUI();
                    if (!spawning) StartCoroutine(StartSpawn());
                });
            }
        }

        private IEnumerator DetectLoseFocus()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {

                        RaycastHit hit = new RaycastHit();
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.gameObject != gameObject)
                            {
                                CoreController.MouseController.SetFocus(null);
                                yield break;
                            }
                        }
                    }
                }
                yield return null;
            }
        }
    }
    [Serializable]
    public struct SpawnObject
    {
        public string Name;
        public GameObject Object;
        public Sprite Sprite;
    }
}
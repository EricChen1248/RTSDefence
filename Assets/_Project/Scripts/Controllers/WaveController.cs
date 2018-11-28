using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Buildable_Components;
using Scripts.Entity_Components;
using Scripts.Entity_Components.Ais;

namespace Scripts.Controllers
{
    public class WaveController : MonoBehaviour
    {
        private int _score;

        public int CurrentWave { get; private set; }
        public Dictionary<DefenceType, int> DefenceInfo;
        public Dictionary<int, int> WaveTime = new Dictionary<int, int>
        {
            [1] = 100,
            [2] = 100,
            [3] = 100,
            [4] = 100,
            [5] = 100,
            [6] = 100,
            [7] = 200
        };

        public void EndWave()
        {
            StartCoroutine(WaitForWave());
        }

        public IEnumerator WaitForWave()
        {
            yield return new WaitForSeconds(WaveTime[CurrentWave]);

            StartNewWave();
        }

        public void AddScore(int score)
        {
            _score += score;
        }

        public void StartNewWave()
        {
            // Start Collecting Points

            // TODO : Set target of wave
            var target = new GameObject();


            // TODO : Instantiate enemies;
            var enemies = new List<GameObject>();
            for (int i = 0; i < 500; i++)
            {
                enemies.Add(Instantiate(new GameObject()));
            }

            // Wait for an update to start controlling enemy.

            // Start assigning target
            var hash = new HashSet<Transform>();
            foreach (var enemy in enemies)
            {
                var group = enemy.GetComponent<GroupFinder>().Group;
                if (hash.Contains(group)) continue;

                hash.Add(group);


                var ai = group.GetComponent<GroupAiBase>();
                ai.TargetTo(target, true);

            }
        }
    }

}
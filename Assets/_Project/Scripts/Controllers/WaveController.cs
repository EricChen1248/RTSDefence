using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Buildable_Components;

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
        }
    }

}
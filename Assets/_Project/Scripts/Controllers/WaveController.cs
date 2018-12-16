using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Buildable_Components;
using Scripts.Entity_Components;
using Scripts.Entity_Components.Ais;
using Scripts.Scriptable_Objects;
using UnityEngine.UI;

namespace Scripts.Controllers
{
    public class WaveController : MonoBehaviour
    {
        public static WaveController Instance;
        public Text Indicator;
        private int _score = 1000;

        public GameObject[] Enemies;

        public int CurrentWave { get; private set; }
        public Dictionary<DefenceType, int> DefenceInfo;
        public Dictionary<EnemyType, GameObject> EnemyTypes;

        public HashSet<GameObject> enemies;

        public void Start()
        {
            Instance = this;
            CurrentWave = 1;
            StartCoroutine(WaitForWave());
        }

        public IEnumerator WaitForWave()
        {
            var time = (int) (20 / Mathf.Log10(CurrentWave * 10));
            for (int i = time - 1; i >= 0; i--)
            {
                Indicator.text = $"Next Wave: {i / 60:D2}:{i % 60:D2}";
                yield return new WaitForSeconds(1);
            }
            StartCoroutine(StartNewWave());
        }

        public void AddScore(int score)
        {
            _score += score;
        }

        public IEnumerator StartNewWave()
        {
            print($"Starting Wave {CurrentWave}, with score {_score}");
            Indicator.text = $"Wave {CurrentWave}";
            // Start Collecting Points

            // TODO : Set target of wave
            var target = CoreController.Instance.CoreGameObject;

            var currentScore = _score;
            var currentMaxIndex = 1;
            var maxPoints = Enemies[currentMaxIndex].GetComponent<EnemyComponent>().Data.Points;
            
            enemies = new HashSet<GameObject>();

            while(currentMaxIndex > 0)
            {
                if (maxPoints > currentScore)
                {
                    currentMaxIndex--;
                    continue;
                }

                var enemyIndex = Random.Range(1, currentMaxIndex + 1);
                var enemy = Instantiate(Enemies[enemyIndex]);
                enemies.Add(enemy);
                var pos = Random.insideUnitSphere;
                pos.y = 0;
                enemy.transform.position = pos.normalized * 125f;
                currentScore -= enemy.GetComponent<EnemyComponent>().Data.Points;
            }


            // Wait for an update to start controlling enemy.
            yield return new WaitForFixedUpdate();

            // Start assigning target
            foreach (var enemy in enemies)
            {
                var ai = enemy.GetComponent<AiBase>();

                ai.Target = target;
                ai.FindTarget();
            }
            _score += currentScore;
            print($"attacking with enemies: {enemies.Count}");
            yield return new WaitUntil(() => enemies.Count == 0);
            CurrentWave++;
            StartCoroutine(WaitForWave());
        }

        public int Weighting(DefenceType type)
        {
            switch (type)
            {
                case DefenceType.Regular:
                    return 10;
                case DefenceType.Fire:
                    return 30;
                case DefenceType.Bomb:
                    break;
                case DefenceType.Ice:
                    break;
                case DefenceType.Fast:
                    break;
                case DefenceType.Slow:
                    return 20;
                case DefenceType.Wall:
                    return 1;
                default:
                    return 0;
            }
            return 0;
        }

        public void GameOver()
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<AiBase>().LeaveMap();
            }
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using Scripts.Buildable_Components;
using Scripts.Entity_Components;
using Scripts.Entity_Components.Ais;
using Scripts.Scriptable_Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Controllers
{
    public class WaveController : MonoBehaviour
    {
        public static WaveController Instance;
        public int StartingWave = 1;
        private int _score;
        public Dictionary<DefenceType, int> DefenceInfo;

        public HashSet<GameObject> Enemies;

        public GameObject[] EnemiesLookup;
        public GameObject Boss;
        public Dictionary<EnemyType, GameObject> EnemyTypes;

        public Text Indicator;
        public GameObject GameOverGo;

        public int CurrentWave { get; private set; }

        public void Start()
        {
            Instance = this;
            CurrentWave = StartingWave;
            StartCoroutine(WaitForWave());
        }

        public IEnumerator WaitForWave()
        {
            yield return new WaitForEndOfFrame();
            var time = (int) ((CurrentWave == 1 ? 120 : 0) + 60 / Mathf.Log10(CurrentWave * 10));
            for (var i = time - 1; i >= 0; i--)
            {
                Indicator.text = $"Next Wave: {i / 60:D2}:{i % 60:D2}";
                yield return new WaitForSeconds(1);
            }

            _score += 1000 * CurrentWave;
            StartCoroutine(Random.Range(0, 3) == 0 ? StartScouting() : StartNewWave());
        }

        public void AddScore(int score)
        {
            _score += score;
        }

        private IEnumerator StartScouting()
        {
            Enemies = new HashSet<GameObject>();
            for (var i = 0; i < CurrentWave * 5; i++)
            {
                var scout = Instantiate(EnemiesLookup[0]);
                var pos = Random.insideUnitSphere;
                pos.y = 0;
                scout.transform.position = pos.normalized * 120f;
                Enemies.Add(scout);
            }

            yield return new WaitForEndOfFrame();
            foreach (var enemy in Enemies)
            {
                enemy.GetComponent<ScoutAi>().FindTarget();
            }

            Indicator.text = "Scouting Phase";
            yield return new WaitUntil(() => Enemies.Count == 0);
            StartCoroutine(StartNewWave());
        }

        private IEnumerator StartNewWave()
        {
            Enemies = new HashSet<GameObject>();

            AudioController.Instance.State = AudioController.AudioState.Fight;
            Indicator.text = $"Wave {CurrentWave}";
            // Start Collecting Points

            // TODO : Set target of wave
            var target = CoreController.Instance.CoreGameObject;
            Boss.GetComponent<BossAi>().Target = target;
            if (CurrentWave % 10 == 0)
            {
                for (var i = 0; i < CurrentWave / 10; i++)
                {
                    var boss = Instantiate(Boss);
                    var pos = Random.insideUnitSphere;
                    pos.y = 0;
                    boss.transform.position = pos.normalized * 50f;
                    Enemies.Add(Boss);

                }
                // Wait for an update to start controlling enemy.
                yield return new WaitForFixedUpdate();

                // Start assigning target
                foreach (var enemy in Enemies)
                {
                    var ai = enemy.GetComponent<BossAi>();
                    ai.TargetTo(target, true);

                }
            }
            else
            {
                var currentScore = _score;
                var currentMaxIndex = CurrentWave > 4 ? 4 : CurrentWave;
                var maxPoints = EnemiesLookup[currentMaxIndex].GetComponent<EnemyComponent>().Data.Points;

                while (currentMaxIndex > 0)
                {
                    if (maxPoints > currentScore)
                    {
                        currentMaxIndex--;
                        continue;
                    }

                    var enemyIndex = Random.Range(1, currentMaxIndex + 1);
                    var enemy = Instantiate(EnemiesLookup[enemyIndex]);
                    Enemies.Add(enemy);
                    var pos = Random.insideUnitSphere;
                    pos.y = 0;
                    enemy.transform.position = pos.normalized * 120f;
                    currentScore -= enemy.GetComponent<EnemyComponent>().Data.Points;
                }


                // Wait for an update to start controlling enemy.
                yield return new WaitForEndOfFrame();

                // Start assigning target
                foreach (var enemy in Enemies)
                {
                    var ai = enemy.GetComponent<AiBase>();

                    ai.Target = target;
                    ai.FindTarget();
                }

                _score += currentScore;
            }

            print($"attacking with enemies: {Enemies.Count}");
            yield return new WaitUntil(() => Enemies.Count == 0);
            CurrentWave++;
            StartCoroutine(WaitForWave());
        }

        public void GameOver()
        {
            AudioController.Instance.State = AudioController.AudioState.Lose;
            GameOverGo.SetActive(true);
            foreach (var enemy in Enemies) enemy.GetComponent<AiBase>().LeaveMap();
        }
    }
}
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Scripts.Entity_Components.Misc;
using UnityEngine;

namespace Scripts.Entity_Components.Status
{
    public class ThunderComponent : MonoBehaviour
    {
        public int Damage = 20;
        public int Range = 5;
        public int NumberOfOtherVictim = 2;

        public void Start () {
            var rnd = new System.Random();
            GetComponent<HealthComponent>()?.Damage(Damage);
            foreach(var health in GetRangeHealth(transform, Range).Where(x => transform != x.transform).OrderBy(x => rnd.Next()).Take(NumberOfOtherVictim)){
                health.Damage(Damage);
            }
            Destroy(this);
        }

        public static List<HealthComponent> GetRangeHealth(Transform t, int Range){
            var l = new List<HealthComponent>();
            Vector3 v = t.position;
            foreach(var health in FindObjectsOfType<HealthComponent>()){
                if(Vector3.Distance(v, health.transform.position) <= Range){
                    l.Add(health);
                }
            }
            return l;
        }
    }
}

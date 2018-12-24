using System.Collections;
using System.Collections.Generic;
using Scripts.Entity_Components.Misc;
using UnityEngine;

namespace Scripts.Entity_Components.Status
{
    public class ThunderComponent : MonoBehaviour
    {
        public bool Immediate = true;
        public int Duration;
        public int Damage = 20;
        public int Range = 5;

        public void Start () {
            if(Immediate){
                foreach(var health in GetRangeHealth(transform, Range)){
                    health.Damage(Damage);
                }
                Destroy(this);
            }else{
                throw new System.ArgumentOutOfRangeException();
            }
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

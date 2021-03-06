﻿using System.Collections;
using Scripts.Entity_Components.Misc;
using UnityEngine;

namespace Scripts.Entity_Components.Status
{
    [RequireComponent(typeof(HealthComponent))]
    public class HealComponent : MonoBehaviour
    {
        public int Duration;
        public int Heal;


        // Use this for initialization
        private void Start()
        {
        }

        private IEnumerator HealhRoutine()
        {
            var health = GetComponent<HealthComponent>();
            var i = 0;
            while (i < Duration)
            {
                i++;
                yield return new WaitForSeconds(1);
                health.Damage(-Heal);
            }

            Destroy(this);
        }
    }
}
﻿using System.Collections;
using UnityEngine;

namespace Scripts.Entity_Components.Status
{
    [RequireComponent(typeof(HealthComponent))]
    public class BurnComponent : MonoBehaviour
    {

        public int Damage = 2;

        public int Duration = 5;
        // Use this for initialization
        private void Start ()
        {
            StartCoroutine(BurnDamage());
        }

        private IEnumerator BurnDamage()
        {
            var health = GetComponent<HealthComponent>();
            var i = 0;
            while (i < Duration)
            {
                i++;
                yield return new WaitForSeconds(1);
                health.Damage(Damage);
            }

            Destroy(this);
        }
    }
}
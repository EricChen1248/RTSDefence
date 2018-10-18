using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity_Components{
	public class AutoRecover : MonoBehaviour {
		public int RecoverHealth = 0;
		private HealthComponent _health;
		// Use this for initialization
		void Start () {
			_health = GetComponent<HealthComponent>();
			_health.OnDeath += (HealthComponent h) => {
				h.Health = RecoverHealth;
			};
		}
	}
}
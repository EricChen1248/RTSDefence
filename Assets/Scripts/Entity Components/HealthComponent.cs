using UnityEngine;

namespace Entity_Components
{
    public class HealthComponent : MonoBehaviour
    {

        public int Health = 100;
        public void Damage(int damage)
        {
            Health -= damage;
        }
    }
}

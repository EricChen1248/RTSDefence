using System.Linq;
using Scripts.Entity_Components.Attacks;
using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
using UnityEngine;

namespace Scripts.Entity_Components.Status
{
    public class ThunderComponent : MonoBehaviour
    {
        public int NumberOfOtherVictim = 2;
        public int Range = 5;

        public void Spread()
        {
            var colliders = Physics.OverlapSphere(transform.position, Range, RaycastHelper.LayerMaskDictionary["Enemies"]);
            
            if (colliders.Length <= 0) return;
            for (var i = 0; i < NumberOfOtherVictim; i++)
            {
                var go = Instantiate(gameObject);
                go.transform.position = transform.position;
                go.GetComponent<ThunderComponent>().NumberOfOtherVictim = NumberOfOtherVictim - 1;

                var bullet = go.GetComponent<BulletScript>();
                
                bullet.Target = colliders[Random.Range(0, colliders.Length)].transform;
                bullet.Layer = RaycastHelper.LayerMaskDictionary["Enemies"];
                bullet.Fire();
            }

            Destroy(this);
        }
    }
}
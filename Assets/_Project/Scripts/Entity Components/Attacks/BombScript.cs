using System.Collections;
using System.Collections.Generic;
using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
using UnityEngine;

namespace Scripts.Entity_Components.Attacks
{
    public class BombScript : MonoBehaviour
    {
        public int Damage = 20;
        public int Range = 5;

        public void Start () {
        }
        public void FixedUpdate(){
            if(transform.position.y <= 0){
                var colliders = Physics.OverlapSphere(transform.position, Range, RaycastHelper.LayerMaskDictionary["Enemies"]);

                foreach(var collider in colliders)
                {
                    var health = collider.GetComponent<HealthComponent>();
                    health?.Damage(Damage);
                }
                Destroy(this);
            }
        }
    }
}

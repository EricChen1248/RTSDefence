using System.Linq;
using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
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
            var colliders = Physics.OverlapSphere(transform.position, Range, RaycastHelper.LayerMaskDictionary["Enemies"]);
            var choose =
                from c in colliders
                where transform != c.transform && c.GetComponent<HealthComponent>() != null
                select c.GetComponent<HealthComponent>();
            foreach(var health in choose.OrderBy(x => rnd.Next()).Take(NumberOfOtherVictim)){
                health.Damage(Damage);
            }
            Destroy(this);
        }
    }
}

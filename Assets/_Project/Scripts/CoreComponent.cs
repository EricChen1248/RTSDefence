using Scripts.Controllers;
using Scripts.Entity_Components.Misc;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts
{
    [DefaultExecutionOrder(-99)]
    public class CoreComponent : MonoBehaviour {

        // Use this for initialization
        private void Start ()
        {
            CoreController.Instance.CoreGameObject = gameObject;
            GetComponent<HealthComponent>().OnDeath += (e) =>
            {
                WaveController.Instance.GameOver();
                gameObject.AddComponent<DestroyComponent>();
                GetComponent<NavMeshObstacle>().enabled = false;
            };

        }
	
    }
}

using UnityEngine;

namespace Controllers
{
    public class CoreController : MonoBehaviour {
        public static CoreController Instance { get; private set; }
        public static MouseController MouseController { get; private set; }

        public void Awake()
        {
            Instance = this;
            MouseController = GetComponent<MouseController>();
        }


    }
}

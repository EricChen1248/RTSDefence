using UnityEngine;

namespace Controllers
{
    [DefaultExecutionOrder(-100)]
    public class CoreController : MonoBehaviour {
        public static CoreController Instance { get; private set; }
        public static BuildController BuildController { get; private set; }
        public static MouseController MouseController { get; private set; }
        public static CameraController CameraController { get; set; }

        public GameObject CoreGameObject;

        public void Start()
        {
            Instance = this;
            BuildController = GetComponent<BuildController>();
            MouseController = GetComponent<MouseController>();
        }


    }
}

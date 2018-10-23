using UnityEngine;

namespace Scripts.Controllers
{
    [DefaultExecutionOrder(-100)]
    public class CoreController : MonoBehaviour {
        public static CoreController Instance { get; private set; }
        public static BuildController BuildController { get; private set; }
        public static MouseController MouseController { get; private set; }
        public static CameraController CameraController { get; set; }

        public GameObject CoreGameObject;
        public GameObject GroupsGameObject;

        public void Start()
        {
            Instance = this;
            BuildController = GetComponent<BuildController>();
            MouseController = GetComponent<MouseController>();
        }


    }
}

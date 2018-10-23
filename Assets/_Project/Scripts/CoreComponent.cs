using Scripts.Controllers;
using UnityEngine;

namespace Scripts
{
    [DefaultExecutionOrder(-99)]
    public class CoreComponent : MonoBehaviour {

        // Use this for initialization
        private void Start ()
        {
            CoreController.Instance.CoreGameObject = gameObject;
        }
	
    }
}

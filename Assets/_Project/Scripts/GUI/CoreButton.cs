using Scripts.Controllers;
using UnityEngine;

namespace Scripts.GUI
{
    public class CoreButton : MonoBehaviour
    {
        public void Click()
        {
            CoreController.CameraController.MoveTo(CoreController.Instance.CoreGameObject.transform.position);
        }
    }
}
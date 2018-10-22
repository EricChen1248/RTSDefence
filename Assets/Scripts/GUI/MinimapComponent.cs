using Controllers;
using UnityEngine;

namespace GUI
{
    [DefaultExecutionOrder(1)]
    public class MinimapComponent : MonoBehaviour
    {
        private RectTransform _rect;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void OnClick()
        {
            var mPos = Input.mousePosition;
            var relX = mPos.x - _rect.position.x + _rect.sizeDelta.x / 2 ;
            var relY = mPos.y - _rect.position.y + _rect.sizeDelta.y / 2 - 6;
            print(relX);
            print(relY);
            CoreController.CameraController.MoveCam(relX / 10, relY / 10);
        }
    }
}

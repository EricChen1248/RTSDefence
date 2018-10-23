using System;
using Controllers;
using UnityEngine;

namespace GUI
{
    [DefaultExecutionOrder(1)]
    public class MinimapComponent : MonoBehaviour, ICanvasRaycastFilter
    {
        private RectTransform _rect;
        private Vector2 _screenPoint;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();

            _screenPoint = new Vector2(Screen.width - _rect.sizeDelta.x / 2, Screen.height - _rect.sizeDelta.y / 2);
        }

        public void OnClick()
        {
            var mPos = Input.mousePosition;
            var relX = mPos.x - _rect.position.x + _rect.sizeDelta.x / 2 ;
            var relY = mPos.y - _rect.position.y + _rect.sizeDelta.y / 2 - 6;
            CoreController.CameraController.MoveCam(relX / 10, relY / 10);
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return (sp - _screenPoint).sqrMagnitude < 10000;
        }
    }
}

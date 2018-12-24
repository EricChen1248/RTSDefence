using Scripts.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Controllers
{
    [DefaultExecutionOrder(0)]
    public class UnitSelectionController : MonoBehaviour
    {
        private bool _isMoving;
        private bool _isSelecting;

        private Camera _mainCam;
        private Vector3 _mousePosition;
        public Color BorderColor;
        public Color RectangleColor;
        public IClickable SingleSelect;

        public void Start()
        {
            _mainCam = Camera.main;
        }

        // Update is called once per frame
        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                _isSelecting = true;
                _isMoving = false;
                _mousePosition = Input.mousePosition;
                CoreController.MouseController.SetFocus(SingleSelect);
                SingleSelect = null;
            }

            if (_isSelecting && !_isMoving && (_mousePosition - Input.mousePosition).sqrMagnitude > 16f)
            {
                _isMoving = true;
                CoreController.MouseController.Clear();
            }

            if (!Input.GetMouseButtonUp(0)) return;
            if (_isSelecting && _isMoving)
                foreach (var m in FindObjectsOfType<MonoBehaviour>())
                {
                    var clickable = m as IClickable;
                    if (clickable != null && IsInSelectionBox(m.transform))
                        CoreController.MouseController.AddFocus(clickable);
                }

            _isSelecting = false;
        }

        public void OnGUI()
        {
            if (!_isSelecting) return;
            var rect = RectUtil.GetScreenRect(_mousePosition, Input.mousePosition);
            RectUtil.DrawScreenRect(rect, RectangleColor);
            RectUtil.DrawScreenRectBorder(rect, 2, BorderColor);
        }

        public bool IsInSelectionBox(Transform t)
        {
            if (!_isSelecting) return false;

            var viewportBound = RectUtil.GetViewportBound(_mainCam, _mousePosition, Input.mousePosition);
            return viewportBound.Contains(_mainCam.WorldToViewportPoint(t.position));
        }
    }
}
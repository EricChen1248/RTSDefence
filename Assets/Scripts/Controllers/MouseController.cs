using Interface;
using Navigation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class MouseController : MonoBehaviour
    {
        private IClickable _focusedItem;

        public void SetFocus(IClickable click)
        {
            _focusedItem?.LostFocus();
            _focusedItem = click;
        }

        private void Update()
        {
            // Right click
            if (!Input.GetMouseButtonDown(1)) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Vector3 clickPos;
            if (!RaycastHelper.TryMouseRaycastToGrid(out clickPos,
                RaycastHelper.LayerMaskDictionary["Walkable Surface"])) return;

            _focusedItem?.RightClick(clickPos);
        }
    }
}

using Buildable_Components;
using Interface;
using Navigation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class MouseController : MonoBehaviour
    {
        public IPlayerControllable _focusedItem { get; private set; }

        public void SetFocus(IPlayerControllable click)
        {
            _focusedItem?.LostFocus();
            _focusedItem = click;
        }

        private void Update()
        {
            // Right click
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
            }

            if (Input.GetMouseButtonDown(1))
            {

                Vector3 clickPos;
                if (!RaycastHelper.TryMouseRaycastToGrid(out clickPos,
                    RaycastHelper.LayerMaskDictionary["Walkable Surface"])) return;

                _focusedItem?.RightClick(clickPos);
                return;
            }

            // Left click
            if (Input.GetMouseButtonDown(0))
            {
                GameObject go;
                if (RaycastHelper.RaycastGameObject(out go, RaycastHelper.LayerMaskDictionary["Ghost Models"]))
                {
                    go.GetComponent<GhostModelScript>().Clicked();
                }
            }
            
        }
    }
}

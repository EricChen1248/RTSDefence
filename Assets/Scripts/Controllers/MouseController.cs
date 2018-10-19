using Buildable_Components;
using Entity_Components.Friendlies;
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
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
            }

            // Right click
            if (Input.GetMouseButtonDown(1))
            {

                Vector3 clickPos;
                if (!RaycastHelper.TryMouseRaycastToGrid(out clickPos,
                    RaycastHelper.LayerMaskDictionary["Walkable Surface"])) return;

                _focusedItem?.RightClick(clickPos);
                return;
            }
            HandleLeftClicks();

        }

        #region Left Clicks

        private void HandleLeftClicks()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            if (GhostModelLeftClick()) return;
            if (PlayerLeftClick()) return;
        }

        private static bool GhostModelLeftClick()
        {
            GameObject go;
            if (!RaycastHelper.RaycastGameObject(out go, 1 << LayerMask.NameToLayer("GhostModel"))) return false;
            go.GetComponent<GhostModelScript>().Clicked();
            return true;
        }

        private bool PlayerLeftClick()
        {
            GameObject player;
            if (!RaycastHelper.RaycastGameObject(out player, 1 << LayerMask.NameToLayer("Player"))) return false;
            SetFocus(player.GetComponent<PlayerComponent>());
            return true;
        }

        #endregion
    }
}

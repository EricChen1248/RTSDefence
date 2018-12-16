using Scripts.Buildable_Components;
using Scripts.Entity_Components.Friendlies;
using Scripts.Interface;
using Scripts.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Controllers
{
    public class MouseController : MonoBehaviour
    {
        public IClickable FocusedItem { get; private set; }

        public void SetFocus(IClickable click)
        {
            if (FocusedItem == click) return;
            FocusedItem?.LostFocus();
            FocusedItem = click;
            FocusedItem?.Focus();
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
            }

            // Right click
            if (Input.GetMouseButtonDown(1))
            {
                FocusedItem?.RightClick();
                return;
            }

            //HandleLeftClicks();

        }

        #region Left Clicks

        private void HandleLeftClicks()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            if (FocusedItem != null)
            {
                GameObject go;
                RaycastHelper.RaycastGameObject(out go, 1);
                if (go == null || go.GetComponent<IClickable>() != FocusedItem)
                {
                    FocusedItem.LostFocus();
                    FocusedItem = null;
                }
            }
        }
                
        #endregion
    }
}

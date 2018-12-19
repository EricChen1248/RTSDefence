using Scripts.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Scripts.Navigation;

namespace Scripts.Controllers
{
    public class MouseController : MonoBehaviour
    {
        public HashSet<IClickable> FocusedItem { get; private set; }

        public void Start()
        {
            FocusedItem = new HashSet<IClickable>();
        }

        public void SetFocus(IClickable click)
        {
            if (FocusedItem.Contains(click)){
                // Note: We cannot Clear() -> AddFocus(click)
                // because that will make the new PathDrawer in PlayerComponent fail to start.
                // That is, we do not call click.LostFocus() in this method.
                foreach (var item in FocusedItem)
                {
                    if(item != click){
                        item?.LostFocus();
                    }
                }
                FocusedItem.Clear();
                FocusedItem.Add(click);
                return;
            }
            Clear();
            if (click == null) return;
            FocusedItem.Add(click);
            click.Focus();
        }

        public void AddFocus(IClickable click){
            if (click == null || FocusedItem.Contains(click)) return;
            FocusedItem.Add(click);
            click.Focus();
        }
        public void RemoveFocus(IClickable click){
            if (click == null || !FocusedItem.Contains(click)) return;
            click.LostFocus();
            FocusedItem.Remove(click);
        }
        public void Clear(){
            foreach (var item in FocusedItem)
            {
                item?.LostFocus();
            }
            FocusedItem.Clear();
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
                Vector3 clickPos;
                if (!RaycastHelper.TryMouseRaycastToGrid(out clickPos,
                    RaycastHelper.LayerMaskDictionary["Walkable Surface"])) return;

                var center = Vector3.zero;
                foreach (var item in FocusedItem)
                {
                    center += (item as MonoBehaviour).transform.position / FocusedItem.Count;
                }
                clickPos -= center;
                foreach (var item in FocusedItem)
                {
                    var pos = (item as MonoBehaviour).transform.position + clickPos;
                    item?.RightClick(pos);
                }
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    MenuController.Instance.MenuLowered();
                }
            }

        }

        #region Left Clicks

        #endregion
    }
}

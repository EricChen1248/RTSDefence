using Scripts.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
            if (FocusedItem.Contains(click)) return;
            foreach (var item in FocusedItem)
            {
                item?.LostFocus();
            }
            FocusedItem.Clear();
            if (click == null) return;
            FocusedItem.Add(click);
            click.Focus();
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
                foreach (var item in FocusedItem)
                {
                    item?.RightClick();
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

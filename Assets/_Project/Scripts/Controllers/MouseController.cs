﻿using System.Collections.Generic;
using Scripts.GUI;
using Scripts.Interface;
using Scripts.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Controllers
{
    public class MouseController : MonoBehaviour
    {
        public HashSet<IClickable> FocusedItem { get; private set; }
        public HashSet<string> FocusedTypes { get; private set; }

        public void Start()
        {
            FocusedItem = new HashSet<IClickable>();
            FocusedTypes = new HashSet<string>();
        }

        public void SetFocus(IClickable click)
        {
            if (FocusedItem.Contains(click))
            {
                // Note: We cannot Clear() -> AddFocus(click)
                // because that will make the new PathDrawer in PlayerComponent fail to start.
                // That is, we do not call click.LostFocus() in this method.
                foreach (var item in FocusedItem)
                    if (item != click)
                        item?.LostFocus();
                FocusedItem.Clear();
                FocusedItem.Add(click);
                return;
            }

            Clear();
            if (click == null) return;
            FocusedItem.Add(click);
            click.Focus();
        }

        public void AddFocus(IClickable click)
        {
            if (click == null || FocusedItem.Contains(click)) return;
            FocusedItem.Add(click);
            FocusedTypes.Add(click.Type);
            click.Focus();
        }

        public void Clear()
        {
            foreach (var item in FocusedItem) item?.LostFocus();

            FocusedItem.Clear();
            FocusedTypes.Clear();
            ObjectMenuGroupComponent.Instance.ResetButtons();
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

            // Right click
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 clickPos;
                if (!RaycastHelper.TryMouseRaycastToGrid(out clickPos,
                    RaycastHelper.LayerMaskDictionary["Walkable Surface"])) return;

                var center = Vector3.zero;
                foreach (var item in FocusedItem)
                    center += ((MonoBehaviour) item).transform.position / FocusedItem.Count;
                clickPos -= center;
                foreach (var item in FocusedItem)
                {
                    var pos = ((MonoBehaviour) item).transform.position + clickPos;
                    item?.RightClick(pos);
                }

                return;
            }

            if (!Input.GetMouseButtonDown(0)) return;
            if (!EventSystem.current.IsPointerOverGameObject())
                MenuController.Instance.MenuLowered();
        }

        #region Left Clicks

        #endregion
    }
}
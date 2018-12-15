using Scripts.Buildable_Components;
using Scripts.Entity_Components.Friendlies;
using Scripts.Interface;
using Scripts.Navigation;
using Scripts.Resources;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Scripts.Controllers
{
    public class MouseController : MonoBehaviour
    {
        public HashSet<IClickable> FocusedItem { get; private set; }

        public void Start(){
        	FocusedItem = new HashSet<IClickable>();
        }

        public void SetFocus(IClickable click)
        {
        	//Set single clickable?
        	//Unclick all objects and leave a set containing only 1 object
        	/*
            if (FocusedItem == click) return;
            FocusedItem?.LostFocus();
            FocusedItem = click;
            FocusedItem.Focus();
            */
            if(FocusedItem.Contains(click)) return;
            foreach(var item in FocusedItem){
            	item?.LostFocus();
            }
            FocusedItem.Clear();
            FocusedItem.Add(click);
            click.Focus();
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

                foreach(var item in FocusedItem){
                	item?.RightClick(clickPos);
                }
                return;
            }
            HandleLeftClicks();

        }

        #region Left Clicks

        private void HandleLeftClicks()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            //if (GhostModelLeftClick()) return;
            //if (ResourceLeftClick()) return;
            //if (PlayerLeftClick()) return;
        }

        private static bool GhostModelLeftClick()
        {
            GameObject go;
            if (!RaycastHelper.RaycastGameObject(out go, 1 << LayerMask.NameToLayer("GhostModel"))) return false;
            go.GetComponent<GhostModelScript>().Clicked();
            return true;
        }

        private static bool ResourceLeftClick()
        {
            GameObject go;
            if (!RaycastHelper.RaycastGameObject(out go, 1 << LayerMask.NameToLayer("Resource Collection"))) return false;
            print("Click Resources");
            go.GetComponent<NodeManager>().Clicked();
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

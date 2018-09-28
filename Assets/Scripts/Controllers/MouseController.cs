using Interface;
using Navigation;
using UnityEngine;

namespace Controllers
{
    public class MouseController : MonoBehaviour
    {
        public IClickable FocusedItem { get; private set; }

        public void SetFocus(IClickable click)
        {
            FocusedItem?.LostFocus();
            FocusedItem = click;
        }

        private void Update()
        {
            // Right click
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 clickPos;
                if (RaycastHelper.TryMouseRaycastToGrid(out clickPos, RaycastHelper.LayerMaskDictionary["Walkable Surface"]))
                {
                    FocusedItem?.RightClick(clickPos);
                    print(clickPos);
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using Scripts.Interface;

namespace Scripts.Controllers
{
    [DefaultExecutionOrder(0)]
	public class UnitSelectionController : MonoBehaviour {
		private bool isSelecting = false;
		private bool isMoving = false;
		private Vector3 mousePosition;
		public Color RectangleColor;
		public Color BorderColor;
		public IClickable SingleSelect;
		
		// Update is called once per frame
		public void Update () {
			if(Input.GetMouseButtonDown(0)){
				if (EventSystem.current.IsPointerOverGameObject()) return;
				isSelecting = true;
				isMoving = false;
				mousePosition = Input.mousePosition;
				CoreController.MouseController.SetFocus(SingleSelect);
				SingleSelect = null;
			}
			if(isSelecting && !isMoving && mousePosition != Input.mousePosition){
				isMoving = true;
				CoreController.MouseController.Clear();
			}
			if(Input.GetMouseButtonUp(0)){
				if(isSelecting && isMoving){
					foreach(var m in FindObjectsOfType<MonoBehaviour>()){
						if(m is IClickable && IsInSelectionBox(m.transform)){
							CoreController.MouseController.AddFocus(m as IClickable);
						}
					}
				}
				isSelecting = false;
			}
		}
		public void OnGUI(){
			if(isSelecting){
				var rect = RectUtil.GetScreenRect(mousePosition, Input.mousePosition);
				RectUtil.DrawScreenRect(rect, RectangleColor);
				RectUtil.DrawScreenRectBorder(rect, 2, BorderColor);
			}
		}
		public bool IsInSelectionBox(GameObject obj){
			return IsInSelectionBox(obj.transform);
		}
		public bool IsInSelectionBox(Transform t){
			if(!isSelecting){
				return false;
			}
			var camera = Camera.main;
			var viewportBound = RectUtil.GetViewportBound(camera, mousePosition, Input.mousePosition);
			return viewportBound.Contains(camera.WorldToViewportPoint(t.position));
		}
	}
}
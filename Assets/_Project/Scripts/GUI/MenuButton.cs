using Scripts.Controllers;
using UnityEngine;

namespace Scripts.GUI
{
	public class MenuButton : MonoBehaviour {
		public GameObject GameObject;
		
		public void Select()
		{
			InterfaceController.Instance.ClickMenu(this);
			// Todo menu button click;
		}

		public void Deselect()
		{

		}
		
	}	

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Controllers;

namespace GUI
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

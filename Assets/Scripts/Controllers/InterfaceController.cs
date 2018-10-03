using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GUI;

namespace Controllers
{
	public class InterfaceController : MonoBehaviour {

		public static InterfaceController Instance;

		private MenuButton _activeMenu;

		// Use this for initialization
		void Start () {
			Instance = this;	
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void ClickMenu(MenuButton menu)
		{
			if (_activeMenu != null)
			{
				_activeMenu.Deselect();
			}
			_activeMenu = menu;
		}

	}
}

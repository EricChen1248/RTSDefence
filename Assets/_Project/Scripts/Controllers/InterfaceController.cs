using Scripts.GUI;
using UnityEngine;

namespace Scripts.Controllers
{
	public class InterfaceController : MonoBehaviour {

		public static InterfaceController Instance;

		private MenuButton _activeMenu;

		// Use this for initialization
	    private void Start ()
		{
			Instance = this;	
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

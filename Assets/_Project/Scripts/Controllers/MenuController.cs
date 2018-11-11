using Scripts.GUI;
using UnityEngine;

namespace Scripts.Controllers
{
    public class MenuController : MonoBehaviour {

        public static MenuController Instance { get; private set; }
        private MenuButton _currentMenu;

        private void Start ()
        {
            Instance = this;
        }

        public void MenuClicked(MenuButton currentMenuButton)
        {
            if (_currentMenu != null) _currentMenu.Lower();
            _currentMenu = currentMenuButton;
        }

        public void MenuLowered()
        {
            _currentMenu.Lower();
            _currentMenu = null;
        }

    }
}

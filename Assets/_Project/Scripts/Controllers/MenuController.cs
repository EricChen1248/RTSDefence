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
            if (_currentMenu != currentMenuButton) _currentMenu?.Animate();
            _currentMenu = currentMenuButton;
            _currentMenu.Animate();
            ObjectMenuGroupComponent.Instance.Hide();
        }

        public void MenuLowered()
        {
            if (_currentMenu != null) _currentMenu.Animate();
            _currentMenu = null;
        }

    }
}

using UnityEngine;

namespace Scripts.GUI
{
    public class FullScreenButton : MonoBehaviour {

        public void Click()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace Scripts.GUI
{
    public class ClickOnQuit : MonoBehaviour
    {
        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
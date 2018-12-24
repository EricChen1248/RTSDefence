using UnityEngine;

namespace Scripts.DebugScripts
{
    public class ChangeSpeedButton : MonoBehaviour
    {
        public bool Fast;

        public void Click()
        {
            Time.timeScale = Fast ? 1 : 5;
            Fast = !Fast;
        }
    }
}
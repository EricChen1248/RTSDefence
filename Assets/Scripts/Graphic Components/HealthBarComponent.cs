using UnityEngine;

namespace Graphic_Components
{
    public class HealthBarComponent : MonoBehaviour
    {
        public GameObject CurrentHealthBar;

        public void ReportProgress(float i)
        {
            var scale = CurrentHealthBar.transform.localScale;
            CurrentHealthBar.transform.localScale = new Vector3(0.9f * i, scale.y, scale.z);
            CurrentHealthBar.transform.localPosition = new Vector3(-0.9f * (1f - i) / 2, 0, 0);
        }
    }
}

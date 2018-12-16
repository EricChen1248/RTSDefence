using Scripts.Entity_Components.Misc;
using UnityEngine;

namespace Scripts.Graphic_Components
{
    [DefaultExecutionOrder(-1)]
    public class HealthBarComponent : MonoBehaviour
    {
        public GameObject CurrentHealthBar;

        public void Start()
        {
            gameObject.SetActive(false);
        }

        public void ReportProgress(float i)
        {
            var scale = CurrentHealthBar.transform.localScale;
            CurrentHealthBar.transform.localScale = new Vector3(0.9f * i, scale.y, scale.z);
            CurrentHealthBar.transform.localPosition = new Vector3(-0.9f * (1f - i) / 2, 0, 0);
            gameObject.SetActive(i < 1);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.IO;
using UnityEngine;

namespace Graphic_Components
{
    public class HealthBarComponent : MonoBehaviour
    {
        public GameObject CurrentHealthBar;
        private Camera _camera;

        private float _previous = 1;
        // Use this for initialization
        void Start()
        {
            _camera = Camera.main;
            StartCoroutine(UpdateRotation());
        }

        IEnumerator UpdateRotation()
        {
            while (true)
            {
                transform.rotation = _camera.transform.rotation;
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void ReportProgress(float i)
        {
            var scale = CurrentHealthBar.transform.localScale;
            CurrentHealthBar.transform.localScale = new Vector3(0.9f * i, scale.y, scale.z);
            CurrentHealthBar.transform.localPosition = new Vector3(-0.9f * (1f - i) / 2, 0, 0);

        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.GUI
{
    public class FPSScript : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
            StartCoroutine(PrintFPS());
        }

        private IEnumerator PrintFPS()
        {
            var text = GetComponent<Text>();

            while (true)
            {
                text.text = $"{1 / Time.deltaTime}";
                yield return new WaitForSeconds(1);
            }
        }
    }
}
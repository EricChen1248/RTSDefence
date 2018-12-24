using UnityEngine;
using UnityEngine.UI;

namespace Scripts.GUI
{
    public class CircularProgress : MonoBehaviour
    {
        public Image loading;

        public void UpdateProgress(float percentage)
        {
            loading.fillAmount = percentage;
        }
    }
}
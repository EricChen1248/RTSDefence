using UnityEngine;
using UnityEngine.UI;

namespace Scripts.GUI
{
    public class ResourcePanelComponent : MonoBehaviour
    {
        public Text Text;

        public void AssignImage(Sprite sprite)
        {
            GetComponentInChildren<Image>().sprite = sprite;
        }

        public void UpdateText(int count)
        {
            Text.text = count.ToString();
        }
    }
}
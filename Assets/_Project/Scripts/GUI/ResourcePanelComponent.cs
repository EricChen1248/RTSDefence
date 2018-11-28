using UnityEngine;
using UnityEngine.UI;

namespace Scripts.GUI
{
    public class ResourcePanelComponent : MonoBehaviour
    {
        public void AssignImage(Sprite sprite)
        {
            print("getting image comp");
            GetComponentInChildren<Image>().sprite = sprite;
            print("assigned image");
        }

        public Text Text;
        public void UpdateText(int count)
        {
            Text.text = count.ToString();
        }
    }
}

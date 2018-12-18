using System.Collections.Generic;
using Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.GUI
{
    public class ObjectMenuGroupComponent : MonoBehaviour
    {
        public static ObjectMenuGroupComponent Instance;
        public delegate void ClickEvent();
        public GameObject ButtonPrefab;
        public List<Button> Buttons;


        public void Start()
        {
            Instance = this;
            Buttons = new List<Button>();
            Hide();
        }

        public void ResetButtons()
        {
            foreach (var button in Buttons)
            {
                button.gameObject.SetActive(false);
            }
        }

        public void SetButton(int index, string text, ClickEvent clickEvent)
        {
            while (index >= Buttons.Count)
            {
                var go = Instantiate(ButtonPrefab, transform);
                Buttons.Add(go.GetComponent<Button>());
            }

            Buttons[index].GetComponentInChildren<Text>().text = text;
            Buttons[index].onClick.RemoveAllListeners();
            Buttons[index].onClick.AddListener(delegate { clickEvent(); });
            Buttons[index].gameObject.SetActive(true);
        }

        public void SetButtonImage(int index, Texture tex)
        {
            Buttons[index].GetComponentInChildren<RawImage>().texture = tex;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            MenuController.Instance.MenuLowered();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

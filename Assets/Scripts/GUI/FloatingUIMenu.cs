using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class FloatingUIMenu : MonoBehaviour
    {
        public GameObject[] Buttons = new GameObject[3];

        public delegate void ClickEvent();
	
        // TODO : update to buttons to use sprites
        public void AssignButton(List<string> texts, List<ClickEvent> events)
        {
            switch (texts.Count)
            {
                case 1:
                    Buttons[1].GetComponentInChildren<Text>().text = texts[0];
                    Buttons[1].GetComponent<Button>().onClick.AddListener(delegate { events[0](); });
                    Buttons[1].SetActive(true);
                    break;
                case 2:
                    Buttons[0].GetComponentInChildren<Text>().text = texts[0];
                    Buttons[2].GetComponentInChildren<Text>().text = texts[1];
                    Buttons[0].GetComponent<Button>().onClick.AddListener(delegate { events[0](); });
                    Buttons[2].GetComponent<Button>().onClick.AddListener(delegate { events[1](); });
                    Buttons[0].SetActive(true);
                    Buttons[2].SetActive(true);
                    break;
                case 3:
                    for (var i = 0; i < 3; i++)
                    {
                        Buttons[i].GetComponentInChildren<Text>().text = texts[i];
                        var i1 = i;
                        Buttons[i].GetComponent<Button>().onClick.AddListener(delegate { events[i1](); });

                        Buttons[i].SetActive(true);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            for (var i = 0; i < 3; i++)
            {
                Buttons[i].SetActive(false);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.GUI
{
    public class SelectOnInput : MonoBehaviour
    {
        private bool buttonSelected;
        public EventSystem eventSystem;
        public GameObject selectedObject;

        private void Update()
        {
            if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
            {
                eventSystem.SetSelectedGameObject(selectedObject);
                buttonSelected = true;
            }
        }

        private void OnDisable()
        {
            buttonSelected = false;
        }
    }
}
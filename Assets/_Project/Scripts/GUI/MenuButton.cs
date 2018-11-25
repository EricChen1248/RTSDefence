using System;
using System.Collections;
using Scripts.Controllers;
using UnityEngine;

namespace Scripts.GUI
{
    public class MenuButton : MonoBehaviour
    {
        public GameObject Menu;

        public float UpY;
        private float _downY;

        private bool _isUp;

        private IEnumerator _heightRoutine;

        private void Start()
        {
            try
            {
                _downY = Menu.GetComponent<RectTransform>().anchoredPosition.y;
            }
            catch (NullReferenceException)
            {
            }
        }

        public void ChangeMenu()
        {
            MenuController.Instance.MenuClicked(this);
        }

        public void Animate()
        {
            if (_heightRoutine != null) StopCoroutine(_heightRoutine);
            _heightRoutine = _isUp ? Lower() : Raise();
            StartCoroutine(_heightRoutine);
        }

        public IEnumerator Raise()
        {
            const int time = 20;
            _isUp = true;
            var rect = Menu.GetComponent<RectTransform>();
            var change = (UpY - rect.anchoredPosition.y) / time;
            for (var i = 0; i < time; i++)
            {
                rect.anchoredPosition += Vector2.up * change;
                yield return new WaitForFixedUpdate();
            }
        }

        public IEnumerator Lower()
        {
            const int time = 20;
            _isUp = false;
            var rect = Menu.GetComponent<RectTransform>();
            var change = (rect.anchoredPosition.y - _downY) / time;
            for (var i = 0; i < time; i++)
            {
                rect.anchoredPosition += Vector2.down * change;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

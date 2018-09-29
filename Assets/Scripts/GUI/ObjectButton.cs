using Controllers;
using UnityEngine;

namespace GUI
{
    public class ObjectButton : MonoBehaviour {

        public GameObject GameObject;

        public void ClickEvent()
        {
            var builder = CoreController.BuildController;
            builder.EnableBuildMode(GameObject);
        }
    }
}

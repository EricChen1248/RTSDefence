using Scripts.Controllers;
using UnityEngine;

namespace Scripts.GUI
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

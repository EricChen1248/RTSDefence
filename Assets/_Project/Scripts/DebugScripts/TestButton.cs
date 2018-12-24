using Scripts.Controllers;
using Scripts.Entity_Components.Ais;
using UnityEngine;

namespace Scripts.DebugScripts
{
    //write a method in TestButton
    //and make ClickEvent call the method
    //Free to add properties, but for compatibility, not to remove properties or methods
    public class TestButton : MonoBehaviour
    {
        public Vector3 V;

        public void ClickEvent()
        {
            //call one method
            LeadAllGroup();
        }

        public void LeadAllGroup()
        {
            foreach (var group in CoreController.Instance.GroupsGameObject.GetComponent<GroupManager>().Children())
                group.GetComponent<GroupAiBase>().LeadMemberTo(V);
        }

        //Add some methods if you want...
    }
}
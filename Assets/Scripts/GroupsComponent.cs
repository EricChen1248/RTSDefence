using Controllers;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class GroupsComponent : MonoBehaviour {

	// Use this for initialization
    private void Start ()
	{
	    CoreController.Instance.GroupsGameObject = gameObject;
	}
	
}

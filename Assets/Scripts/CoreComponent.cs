using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class CoreComponent : MonoBehaviour {

	// Use this for initialization
    private void Start ()
	{
	    CoreController.Instance.CoreGameObject = gameObject;
	}
	
}

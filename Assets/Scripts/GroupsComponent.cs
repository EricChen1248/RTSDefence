﻿using Controllers;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DefaultExecutionOrder(-99)]
public class GroupsComponent : MonoBehaviour {

	// Use this for initialization
    private void Start ()
	{
	    CoreController.Instance.GroupsGameObject = gameObject;
	}
	
	public IEnumerable<Transform> Children(){
		for(int i = 0;i < transform.childCount;i++){
			print("Apply for Group No." + i.ToString());
			yield return transform.GetChild(i);
		}
	}
}

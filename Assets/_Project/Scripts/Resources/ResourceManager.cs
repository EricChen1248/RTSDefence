using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class ResourceManager : MonoBehaviour {

    public float rock;
    public float maxRock;
    public float wood;
    public float maxWood;
    public float gold;
    public float maxGold;
    public float coal;
    public float maxCoal;

    public Text rockDesc;
    public Text treeDesc;
    public Text goldDesc;
    public Text coalDesc;
	// Use this for initialization
	void Start () {
		
	}
	
	public void UpdateGUI () 
    {
        rockDesc.text = "" + rock + "/" + maxRock;
	}
}

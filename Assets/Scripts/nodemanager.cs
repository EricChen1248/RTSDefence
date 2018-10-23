using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodemanager : MonoBehaviour {

    public enum ResourceTypes{Rock}
    public ResourceTypes resourceType;

    public float harvestTime; //一次花多少時間採集
    public float availableResource; //有多少的量可採

    public int gatherers;

	// Use this for initialization
	void Start () {
        StartCoroutine(ResourceTick());
	}
	
	// Update is called once per frame
	void Update () {
        if(availableResource <= 0){
            Destroy(gameObject);
        }
	}

    public void ResourceGather(){
        if(gatherers != 0)
        {
            availableResource -= gatherers;
        }
    }



    IEnumerator ResourceTick(){
        while(true){
            yield return new WaitForSeconds(1);
            ResourceGather();
        }
    }
}

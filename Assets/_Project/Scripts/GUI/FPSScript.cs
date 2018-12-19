using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(PrintFPS());
	}
	
    private IEnumerator PrintFPS()
    {
        var text = GetComponent<Text>();

        while(true)
        {
            text.text = $"{1/Time.deltaTime }";
            yield return new WaitForSeconds(1);
        }
    }

}

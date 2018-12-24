using System.Collections;
using UnityEngine;

public class SmokeScript : MonoBehaviour {

	// Use this for initialization
	public void Start ()
    {
        StartCoroutine(Wait());
	}

	public IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}

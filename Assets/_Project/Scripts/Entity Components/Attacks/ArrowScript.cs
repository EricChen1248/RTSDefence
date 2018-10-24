using System.Collections;
using System.Collections.Generic;
using Scripts.Helpers;
using Scripts.Entity_Components;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    public Transform Target;

    private IEnumerator _currentCoroutine;
	// Use this for initialization
	private void Start ()
    {
        Fire(Vector3.zero, Target.position);
	}
	
    public void Fire(Vector3 startPos, Vector3 endPos)
    {
        _currentCoroutine = FireRoutine(startPos, endPos);

        StartCoroutine(_currentCoroutine);
    }
    

    private IEnumerator FireRoutine(Vector3 startPos, Vector3 endPos)
    {
        var distance = Vector3.Distance(startPos, endPos);

        var midPoint = (startPos + endPos) / 2;

        var xDir = midPoint - startPos;
        var yDir = Vector3.up * distance / 2;

        var times = distance * 60;
        
        for (var i = 0; i <= times; i++)
        {
            var radians = (i / times) * Mathf.PI;
            transform.position = midPoint + Mathf.Sin(radians) * xDir + Mathf.Cos(radians) * yDir;
            yield return new WaitForFixedUpdate();
        }

        // Explosion

        Pool.ReturnToPool("Arrow", gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var health = other.GetComponent<HealthComponent>();
            health.Damage(10);
        }

        StopCoroutine(_currentCoroutine);
        _currentCoroutine = null;
        Pool.ReturnToPool("Arrow", gameObject);
    }


}

using System.Collections;
using System.Collections.Generic;
using Scripts.Helpers;
using Scripts.Entity_Components;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    public Transform Target;
    public float height = 20f;
    public float acceleration = 0.01f;
    private IEnumerator _currentCoroutine;
	// Use this for initialization
	private void Start ()
    {
        Fire(transform.position, Target.position);
	}
	
    public void Fire(Vector3 startPos, Vector3 endPos)
    {
        _currentCoroutine = FireRoutine(startPos, endPos);

        StartCoroutine(_currentCoroutine);
    }
    
    private IEnumerator FireRoutine(Vector3 startPos, Vector3 endPos)
    {
        var max_height = 0f;

        if (startPos.y >= endPos.y)
        {
            max_height = startPos.y + height;
        }
        else
        {
            max_height = endPos.y + height;
        }

        var time = Mathf.Sqrt(2 * (max_height - startPos.y) / acceleration) + Mathf.Sqrt(2 * (max_height - endPos.y) / acceleration);

        var velocity_v = acceleration * Mathf.Sqrt(2 * (max_height - startPos.y) / acceleration);

        var velocity_h = new Vector3(endPos.x - startPos.x , 0 , endPos.z - startPos.z) / time;

        for (var i = 0f; i <= Mathf.Ceil(time) + 5; i++)
        {
            velocity_v = velocity_v - acceleration;
            transform.rotation = Quaternion.LookRotation(new Vector3(velocity_h.x,velocity_v,velocity_h.z));
            transform.position = transform.position + velocity_h + velocity_v * Vector3.up;
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
            health.Damage(50);
        }

        StopCoroutine(_currentCoroutine);
        _currentCoroutine = null;
        Pool.ReturnToPool("Arrow", gameObject);
    }


}

using Scripts.Navigation;
using System.Collections;
using UnityEngine;

public class DestroyComponent : MonoBehaviour
{
    public float size;
    public void Start()
    {
        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        var collider = GetComponent<Collider>();
        
        var start = transform.position;
        var end = start + Vector3.down * size;
        for (int i = 0; i < 30; i++)
        {
            var change = start + Random.insideUnitSphere * 0.5f;
            change.y = Vector3.Slerp(start, end, i / 30f).y;
            transform.position = change;
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);

        NavigationBaker.Instance.Rebuild();
    }

}

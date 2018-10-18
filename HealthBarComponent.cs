using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class HealthBarComponent : MonoBehaviour
{
    public GameObject CurrentHealthBar;
    private Camera _camera;

	// Use this for initialization
	void Start () {
        _camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.rotation = _camera.transform.rotation;
	}

    private void ReportProgress(double i)
    {
        CurrentHealthBar.transform.localScale *= new Vector3(i / CurrentHealthBar.transform.localScale.x,1,1);
    }
}

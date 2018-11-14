using System.Collections;
using System.Collections.Generic;
using Scripts.Controllers;
using UnityEngine;

public class CoreButton : MonoBehaviour {

	public void Click()
	{
		CoreController.CameraController.MoveTo(CoreController.Instance.CoreGameObject.transform.position);
	}
}

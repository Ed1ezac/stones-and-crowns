using System;
using System.Collections.Generic;
using UnityEngine;

public class InputDetector : InputDetectionInterface
{
	private Camera mCamera;

	public InputDetector (Camera camera)
	{
		this.mCamera = camera;
	}

	public Transform mouseClickToTransform ()
	{
		RaycastHit hit;
		Ray ray = mCamera.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			return hit.transform;
		} else {
			throw new NullReferenceException ("Transform is null.");
		}
	}

	public void detectDrag ()
	{

	}
}

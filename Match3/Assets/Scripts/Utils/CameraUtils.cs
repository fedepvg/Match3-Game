using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtils 
{
	public static Bounds OrthographicBounds()
	{
		Camera camera = Camera.main;

        float screenAspect = camera.aspect;
		float cameraHeight = camera.orthographicSize * 2;

		Bounds bounds = new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		
		return bounds;
	}
}

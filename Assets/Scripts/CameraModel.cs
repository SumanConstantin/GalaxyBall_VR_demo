using UnityEngine;
using System.Collections;

public class CameraModel {
	public GameObject camHolder;

	public CameraModel()
	{
		Init();
	}

	private void Init()
	{
		camHolder = GameObject.Find("CameraHolder");
	}

	public void UpdateRotation(float horizontalMoveValue)
	{
		camHolder.transform.Rotate(Vector3.up, horizontalMoveValue/4);
	}

	public void Destroy()
	{
		MonoBehaviour.Destroy(camHolder);
	}
}

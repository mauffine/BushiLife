using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(DynamicGrid))]
public class CameraCollection : MonoBehaviour
{
	public GameObject cameraPrefab;
	private DynamicGrid cameraGrid;

	List<CameraController> GetCameras()
	{
        var result = new List<CameraController>(GetComponentsInChildren<CameraController>());

		return result;
	}

	int GetSize()
	{
		return this.GetCameras().Count;
	}

	// Use this for initialization
	void Start()
	{
		cameraGrid = GetComponent<DynamicGrid>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void Validate()
	{
		this.cameraGrid.size = this.GetSize();
		for (int index = 0; index < this.cameraGrid.size; index++)
		{
			var cameras = this.GetCameras();

			cameras[index].SetRect(this.cameraGrid.GetRect(index));
		}
	}

	public void Add(GameObject player, int playerNumber)
	{
		var newCamera = GameObject.Instantiate(cameraPrefab);

		newCamera.GetComponentInChildren<CameraController>().Init(player.transform, playerNumber);

		newCamera.transform.parent = this.transform;
        player.GetComponent<Player>().SetCamera(newCamera.GetComponentInChildren<Camera>());
		try
		{
			this.Validate();
		}
		catch (KeyNotFoundException error)
		{
			Debug.LogWarning("Invalid Splitscreen Split! for " + GetSize().ToString() + " cameras");
		}
	}
}

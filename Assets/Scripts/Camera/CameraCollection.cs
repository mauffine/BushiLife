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
		var result = new List<CameraController>(transform.childCount);
		
		for (int index = 0; index < transform.childCount; index++)
		{
			var child = transform.GetChild(index);
			result.Add(child.GetComponentInChildren<CameraController>());
		}

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
		this.Add(GameObject.Find("Player").transform, 1);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void Validate()
	{
		Debug.Log("Validate");
		Debug.Break();
		this.cameraGrid.size = this.GetSize();
		for (int index = 0; index < this.cameraGrid.size; index++)
		{
			var cameras = this.GetCameras();

			cameras[index].SetRect(this.cameraGrid.GetRect(index));
		}
	}

	void Add(Transform player, int playerNumber)
	{
		Debug.Log("Add");
		Debug.Break();
		var newCamera = GameObject.Instantiate(cameraPrefab);

		newCamera.GetComponentInChildren<CameraController>().Init(player, playerNumber);

		newCamera.transform.parent = this.transform;

		this.Validate();
	}
}

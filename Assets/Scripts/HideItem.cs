using UnityEngine;
using System.Collections;

public class HideItem : MonoBehaviour {

	private Camera cam;
	public RectTransform objRect;
	public GameObject objToHide;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}

	// Update is called once per frame
	void Update () {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

		float fUiUnitToWorldUnit = Screen.height / (cam.orthographicSize * 2);

		Bounds buttonBounds = new Bounds(new Vector3(objRect.position.x, objRect.position.y, objRect.position.z),
			new Vector3(objRect.sizeDelta.x/fUiUnitToWorldUnit, objRect.sizeDelta.y/fUiUnitToWorldUnit, 1));

		if (GeometryUtility.TestPlanesAABB (planes, buttonBounds))
			objToHide.SetActive (true);
		else
			objToHide.SetActive (false);
	}
}

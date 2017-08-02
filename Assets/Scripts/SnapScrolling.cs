using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour {

	//items count in array
	public int itemCount;
	public GameObject itemPref;

	//Dynamic scaling when scrolling 
	public float minItemScale;
	public float maxItemScale;

	public int itemOffset;
	public float snapSpeed;

	public float scaleOffset;
	public float scaleSpeed;
	public ScrollRect scrollRect;

	[Range (0f,1f)]
	public float itemOpacity;
	private Color curItemColor;

	//Array of instantiated items
	private GameObject[] itemArray;

	private Vector2[] itemPos;
	private Vector2[] itemScale;

	//contetnt controll variables
	private RectTransform contentRect;
	private Vector2 contentVector;

	//get the current Item ID from array
	private int curItemID;
	//are we scrolling?
	private bool isScrolling;


	void Start () {
		contentRect = GetComponent <RectTransform> ();
		itemArray = new GameObject[itemCount];
		itemPos = new Vector2[itemCount];
		itemScale = new Vector2[itemCount];
		curItemColor = itemPref.transform.GetChild (0).GetComponent <Image> ().color;

		//fill the array with items
		for (int i = 0; i < itemCount; i++) {
			itemArray[i] = Instantiate (itemPref, transform, false);
			if (i == 0)
				continue;
			//set the position of instantiated items
			itemArray [i].transform.localPosition = new Vector2 (itemArray[i-1].transform.localPosition.x+itemPref.GetComponent <RectTransform>().sizeDelta.x + itemOffset, itemArray[i].transform.localPosition.y);
			itemPos [i] = -itemArray [i].transform.localPosition;
		}
	}

	void FixedUpdate () {
		if (contentRect.anchoredPosition.x >= itemPos [0].x && !isScrolling || contentRect.anchoredPosition.x <= itemPos [itemPos.Length - 1].x) {
			scrollRect.inertia = false;
		}
		//find the center to snap our selected item
		float nearestItemPos = float.MaxValue;
		for (int i = 0; i < itemCount; i++) {
			float dist = Mathf.Abs (contentRect.anchoredPosition.x - itemPos [i].x);
			if (dist < nearestItemPos) {
				nearestItemPos = dist;
				curItemID = i;

			} 

			//set Item image opacity
			Color tmpColor = itemArray [i].transform.GetChild (0).GetComponent <Image> ().color;
			tmpColor.a = itemOpacity;
			itemArray [curItemID].transform.GetChild (0).GetComponent <Image> ().color = curItemColor;
			itemArray [i].transform.GetChild (0).GetComponent <Image> ().color = tmpColor;

			float scale = Mathf.Clamp (1 / (dist / itemOffset) * scaleOffset, minItemScale, maxItemScale);
			itemScale [i].x = Mathf.SmoothStep (itemArray [i].transform.localScale.x, scale, scaleSpeed * Time.deltaTime);
			itemScale [i].y = Mathf.SmoothStep (itemArray [i].transform.localScale.y, scale, scaleSpeed * Time.deltaTime);
			itemArray [i].transform.localScale = itemScale[i];
		}

		float scrollVelocity = Mathf.Abs (scrollRect.velocity.x);
		if (scrollVelocity < 400 && !isScrolling)
			scrollRect.inertia = false;
		if (isScrolling || scrollVelocity > 400)
			return;
		contentVector.x = Mathf.SmoothStep (contentRect.anchoredPosition.x, itemPos [curItemID].x, snapSpeed * Time.deltaTime);
		contentRect.anchoredPosition = contentVector;
	}

	public void Scrolling(bool scrolling){
		isScrolling = scrolling;
		if (scrolling)
			scrollRect.inertia = true;
	}
}

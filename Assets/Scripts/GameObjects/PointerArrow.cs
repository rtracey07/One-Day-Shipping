using UnityEngine;
using System.Collections;

public class PointerArrow : MonoBehaviour {

	private Location destination;
	private Material arrowMaterial;
	private bool hasPackage = false;

	public Color getPackageColor = Color.blue;
	public Color deliverPackageColor = Color.red;

	// Use this for initialization
	void Start () {
		if(LevelManager.Instance != null && LevelManager.Instance.currentDestination != null)
			destination = LevelManager.Instance.currentDestination;

		arrowMaterial = this.GetComponent<MeshRenderer> ().sharedMaterial;
		arrowMaterial.color = getPackageColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (destination != null) {

			if (LevelManager.Instance != null && LevelManager.Instance.currentDestination != null) {
				if (!LevelManager.Instance.currentDestination.Equals (destination))
					destination = LevelManager.Instance.currentDestination;
			}

			this.transform.right = -Vector3.Normalize (destination.transform.position - this.transform.position);

			if (hasPackage != GameManager.Instance.hasPackage) {
				hasPackage = GameManager.Instance.hasPackage;
				StartCoroutine (SwapColor (3));
			}
		}
	}

	IEnumerator SwapColor(float swapTime)
	{
		float time = 0.0f;

		do {
			if (hasPackage)
				arrowMaterial.color = Color.Lerp (getPackageColor, deliverPackageColor, time / swapTime);
			else
				arrowMaterial.color = Color.Lerp (getPackageColor, deliverPackageColor, (1 - time / swapTime));

			time += GameClockManager.Instance.time;
			yield return null;	
		} while(time <= swapTime);
	}
}

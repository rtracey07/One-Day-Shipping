using UnityEngine;
using System.Collections;

/// <summary>
/// Arrow above the player's head.
/// </summary>
public class PointerArrow : MonoBehaviour {

	//private variables:
	private Location destination;
	private Material arrowMaterial;
	private bool hasPackage = false;

	//references to the arrow's color:
	public Color getPackageColor = Color.blue;
	public Color deliverPackageColor = Color.red;

	/// <summary>
	/// Start this instance.
	/// Sets the current destination to point at.
	/// Sets the color of the arrow.
	/// </summary>
	void Start () {
		if(LevelManager.Instance != null && LevelManager.Instance.currentDestination != null)
			destination = LevelManager.Instance.currentDestination;

		arrowMaterial = this.GetComponent<MeshRenderer> ().sharedMaterial;
		arrowMaterial.color = getPackageColor;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
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
		} else {
			if(LevelManager.Instance != null && LevelManager.Instance.currentDestination != null)
				destination = LevelManager.Instance.currentDestination;
		}
	}

	/// <summary>
	/// Gradually changes the color over time in between package pickup and dropoff states.
	/// </summary>
	/// <returns>The color.</returns>
	/// <param name="swapTime">Swap time.</param>
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

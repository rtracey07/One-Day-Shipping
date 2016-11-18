using UnityEngine;
using System.Collections;

public class PointerArrow : MonoBehaviour {

	private Location destination;

	// Use this for initialization
	void Start () {
		if(LevelManager.Instance != null && LevelManager.Instance.currentDestination != null)
			destination = LevelManager.Instance.currentDestination;
	}
	
	// Update is called once per frame
	void Update () {
		if (destination != null) {

			if (LevelManager.Instance != null && LevelManager.Instance.currentDestination != null) {
				if (!LevelManager.Instance.currentDestination.Equals (destination))
					destination = LevelManager.Instance.currentDestination;
			}

			this.transform.up = Vector3.Normalize (destination.transform.position - this.transform.position);
		}
	}
}

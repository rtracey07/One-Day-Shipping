using UnityEngine;
using System.Collections;

public class CarPath : MonoBehaviour {

	private Pathway path;									// the path
	public float rotationOffset = 0.05f;			// how far ahead to look to orient on path
	public float speed = 50.0f;
	private float speedMod = 1.0f;

	private float currentLook =0.25f;										// where the car is looking
	private float percentsPerSecond = 0.1f; 								// %1 of the path moved per second
	private bool swapPositions = false;
	private CarPathManager m_Manager;
	private Vector3 cameraSpacePos;

	void Start()
	{
		currentLook = CurrentPathPercent + rotationOffset;
		percentsPerSecond = 0.02f;//speed * 0.0004f;

		m_Manager = GetComponentInParent<CarPathManager> ();
	}

	public float CurrentPathPercent {
		get;
		set;
	}
		

	void FixedUpdate ()
	{
		if (path == null || !path.isActive) {
			path = m_Manager.GetAreaPath ();
		}

		if (path != null) {
			// if the we're at the end, restart
			if (CurrentPathPercent >= 0.99f) {
				CurrentPathPercent = 0.0f;
				// TODO - this needs some lerping adjustment
				currentLook = rotationOffset;
			}

			// move along the percentage of the path by time
			CurrentPathPercent += percentsPerSecond * speedMod * Time.deltaTime;
			currentLook += percentsPerSecond * speedMod * Time.deltaTime;
			Vector3 look = iTween.PointOnPath (path.pathway.ToArray(), currentLook);
			iTween.PutOnPath (gameObject, path.pathway.ToArray(), CurrentPathPercent);
			transform.LookAt (look);
		}
	}

	void OnDrawGizmos()
	{
		//Visual. Not used in movement
		iTween.DrawPath(path.pathway.ToArray());

		if (m_Manager.debug) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (this.transform.position, 3);
		}
	}
}

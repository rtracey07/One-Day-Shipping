using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarPathManager : MonoBehaviour {


	[SerializeField]
	public List<Pathway> areas;						//List of car areas.
	private List<Pathway> activeAreas;				//Currently active areas.

	public Color inactivePathColor = Color.white;	//Gizmo color for inactive paths.
	public Color activePathColor = Color.red;		//Gizmo color for active paths.
	public float activeDistance;					//Distance for path activation.

	public bool debug = false;						//Toggle Gizmos ON/OFF
	private Transform player;						//Player location.

	void Start()
	{
		player = GameObject.Find ("Player").transform;
		activeAreas = new List<Pathway> (areas.Count);
		LevelManager.Instance.SpawnCars ();
	}

	/* Get Active Paths on Update */
	void Update () {

		if (areas != null && player != null) {
			
			for (int i = 0; i < areas.Count; i++) {

				if (Vector3.Distance (areas [i].center, player.position) <= activeDistance) {
					if (!areas[i].isActive) {
						activeAreas.Add (areas [i]);
						areas [i].isActive = true;
					}
				} else {
					if (areas[i].isActive) {
						activeAreas.Remove(areas [i]);
						areas [i].isActive = false;
					}
				}
			}
		}
	}

	/* Pass back an area to a car. */
	public Pathway GetAreaPath()
	{
		if (activeAreas.Count > 0) {
			return activeAreas[Random.Range(0, activeAreas.Count-1)];
		}

		return null;
	}

	/* For Debugging: Draw GIzmos in scene. */
	void OnDrawGizmos()
	{
		if (debug == true) {
			for (int i = 0; i < areas.Count; i++) {
				if (areas[i] != null && areas[i].isActive)
					Gizmos.color = activePathColor;
				else
					Gizmos.color = inactivePathColor;
				if (areas [i] != null) {
					iTween.DrawPath (areas [i].pathway);
					Gizmos.DrawWireSphere (areas [i].center, activeDistance);
				}	

			}
		}
	}
		
	/* Deactivate paths on destroy. */
	void OnDestroy()
	{
		foreach (Pathway p in areas) {
			p.isActive = false;
		}
	}
}

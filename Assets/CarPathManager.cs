using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarPathManager : MonoBehaviour {
		
	[SerializeField]
	public List<Pathway> areas;			
	private List<Pathway> activeAreas;

	public Color inactivePathColor = Color.white;
	public Color activePathColor = Color.red;
	public float activeDistance;

	public bool debug = false;
	private Transform player;

	void Awake()
	{
		player = GameObject.Find ("Player").transform;
		activeAreas = new List<Pathway> (areas.Count);
	}

	// Update is called once per frame
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

	public Vector3[] GetAreaPath()
	{
		if (activeAreas.Count > 0) {
			return activeAreas[Random.Range(0, activeAreas.Count-1)].pathway.ToArray();
		}

		return null;
	}

	void OnDrawGizmos()
	{
		if (debug == true) {
			for (int i = 0; i < areas.Count; i++) {
				if (areas[i].isActive)
					Gizmos.color = activePathColor;
				else
					Gizmos.color = inactivePathColor;
					
				iTween.DrawPath (areas [i].pathway.ToArray ());
				Gizmos.DrawWireSphere (areas [i].center, activeDistance);
			}
		}
	}

	void OnDestroy()
	{
		foreach (Pathway p in areas) {
			p.isActive = false;
		}
	}
}

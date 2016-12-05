using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatPostmanManager : MonoBehaviour {

	[SerializeField]
	public List<StatPostmanLocations> areas;	
	private List<StatPostmanLocations> activeAreas;
	private List<PostmanStationaryAI> statPosts;

	public Color inactivePathColor = Color.white;
	public Color activePathColor = Color.red;
	public float activeDistance;

	public bool debug = false;
	private Transform player;

	void Start() {
		player = GameObject.Find ("Player").transform;
		activeAreas = new List<StatPostmanLocations> (areas.Count);
		LevelManager.Instance.SpawnStatPostman ();

		statPosts = new List<PostmanStationaryAI> ();
		GameObject[] StatPostmanGameObjects = GameObject.FindGameObjectsWithTag ("Postman");
		foreach (GameObject d in StatPostmanGameObjects) {
			statPosts.Add (d.GetComponent<PostmanStationaryAI>());
		}
	}

	// Update is called once per frame
	void Update () {

		if (areas != null && player != null) {
			for (int i = 0; i < areas.Count; i++) {
				if (Vector3.Distance (areas [i].center, player.position) <= activeDistance) {
					if (!areas[i].isActive) {
						activeAreas.Add (areas [i]);
						areas [i].isActive = true;
						SetStatPostmanPositions (areas [i]);
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

	//	public Pathway GetAreaPath()
	//	{
	//		if (activeAreas.Count > 0) {
	//			return activeAreas[Random.Range(0, activeAreas.Count-1)];
	//		}
	//
	//		return null;
	//	}

	void SetStatPostmanPositions(StatPostmanLocations area){
		// don't want to clone more statPosts than positions
		int activeStatPostmanCount = area.locations.Count;
		int inActivesStatPostmanCount = statPosts.Count - activeStatPostmanCount;
		//Debug.Log ("activeStatPostmanCount: " + activeStatPostmanCount + " inActivesStatPostmanCount: " + inActivesStatPostmanCount);
		if (activeStatPostmanCount == null && inActivesStatPostmanCount == null) {
			return;
		}
		for (int i = 0; i < activeStatPostmanCount; i++) {
			if (i >= statPosts.Count)
				break;
			statPosts [i].gameObject.SetActive (true);
			//Debug.Log ("activating statPost " + i);

			statPosts [i].Center = area.locations [i];
		}

		for (int i = activeStatPostmanCount; i < statPosts.Count; i++) {
			statPosts [i].gameObject.SetActive (false);
			//Debug.Log ("de-activating statPost " + i);
		}
	}

	void OnDrawGizmos()
	{
		if (debug == true) {
			for (int i = 0; i < areas.Count; i++) {
				if (areas[i].isActive)
					Gizmos.color = activePathColor;
				else
					Gizmos.color = inactivePathColor;

				//iTween.DrawPath (areas [i].pathway.ToArray ());
				Gizmos.DrawWireSphere (areas [i].center, activeDistance);
			}
		}
	}

	void OnDestroy()
	{
		foreach (StatPostmanLocations p in areas) {
			p.isActive = false;
		}
	}
}
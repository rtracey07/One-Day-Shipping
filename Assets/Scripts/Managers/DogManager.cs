using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DogManager : MonoBehaviour {

	[SerializeField]
	public List<DogSpawnLocations> areas;	
	private List<DogSpawnLocations> activeAreas;
	private List<DogAI> dogs;

	public Color inactivePathColor = Color.white;
	public Color activePathColor = Color.red;
	public float activeDistance;

	public bool debug = false;
	private Transform player;

	void Start() {
		player = GameObject.Find ("Player").transform;
		activeAreas = new List<DogSpawnLocations> (areas.Count);
		LevelManager.Instance.SpawnDogs ();

		dogs = new List<DogAI> ();
		GameObject[] dogGameObjects = GameObject.FindGameObjectsWithTag ("Dog");
		foreach (GameObject d in dogGameObjects) {
			dogs.Add (d.GetComponent<DogAI>());
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
						SetDogPositions (areas [i]);
						Debug.Log ("areas " + i);
					}
				} else {
					if (areas[i].isActive) {
						activeAreas.Remove(areas [i]);
						areas [i].isActive = false;
						Debug.Log ("areas " + i);
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

	void SetDogPositions(DogSpawnLocations area){
		// don't want to clone more dogs than positions
		int activeDogCount = area.locations.Count;
		int inActivesDogCount = dogs.Count - activeDogCount;
		//Debug.Log ("activeDogCount: " + activeDogCount + " inActivesDogCount: " + inActivesDogCount);
		if (activeDogCount == null && inActivesDogCount == null) {
			return;
		}
		for (int i = 0; i < activeDogCount; i++) {
			dogs [i].gameObject.SetActive (true);
			//Debug.Log ("activating dog " + i);
			dogs [i].Center = area.locations [i];
		}

		for (int i = activeDogCount; i < dogs.Count; i++) {
			dogs [i].gameObject.SetActive (false);
			//Debug.Log ("de-activating dog " + i);
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
		foreach (DogSpawnLocations p in areas) {
			p.isActive = false;
		}
	}
}

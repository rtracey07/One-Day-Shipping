using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DogManager : MonoBehaviour {

	// spawn location lists and dog script component
	[SerializeField]
	public List<DogSpawnLocations> areas;	
	private List<DogSpawnLocations> activeAreas;
	private List<DogAI> dogs;

	// for drawing in debug
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

		// if the player is in the area, activate the dogs in that area
		if (areas != null && player != null) {
			for (int i = 0; i < areas.Count; i++) {
				if (Vector3.Distance (areas [i].center, player.position) <= activeDistance) {
					if (!areas[i].isActive) {
						activeAreas.Add (areas [i]);
						areas [i].isActive = true;
						SetDogPositions (areas [i]);
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
		
	/// <summary>
	/// Set the dogs. Make sure to only set as many dogs as are required by the level
	/// </summary>
	/// <param name="area">Area.</param>
	void SetDogPositions(DogSpawnLocations area){
		// don't want to clone more dogs than positions
		int activeDogCount = area.locations.Count;
		int inActivesDogCount = dogs.Count - activeDogCount;
		//Debug.Log ("activeDogCount: " + activeDogCount + " inActivesDogCount: " + inActivesDogCount);
		if (activeDogCount == null && inActivesDogCount == null) {
			return;
		}
		for (int i = 0; i < activeDogCount; i++) {
			if (i >= dogs.Count)
				break;
			dogs [i].gameObject.SetActive (true);
			dogs [i].Center = area.locations [i];
		}

		for (int i = activeDogCount; i < dogs.Count; i++) {
			dogs [i].gameObject.SetActive (false);
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/DogSpawnLocations")]
public class DogSpawnLocations : ScriptableObject {
	public string name;							// spawn area name
	public bool isActive;						// is the player in this area
	public Vector3 center;						// the center of the area
	public List<Vector3> locations;				// where to spawn dogs
}

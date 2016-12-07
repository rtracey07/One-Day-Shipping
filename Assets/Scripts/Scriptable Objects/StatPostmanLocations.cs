using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/StatPostmanLocations")]
public class StatPostmanLocations : ScriptableObject {
	public string name;					// name
	public bool isActive;				// is the player in this area
	public Vector3 center;				// center of area
	public List<Vector3> locations;		// where to spawn postmen
}
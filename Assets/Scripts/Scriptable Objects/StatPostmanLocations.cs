using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/StatPostmanLocations")]
public class StatPostmanLocations : ScriptableObject {
	public string name;
	public bool isActive;
	public Vector3 center;
	public List<Vector3> locations;
}
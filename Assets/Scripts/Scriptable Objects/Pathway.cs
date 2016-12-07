using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/Pathway")]
public class Pathway : ScriptableObject {

	public string pathName;				// name
	public bool isActive;				// is the player in this area
	public Vector3 center;				// path center
	public Vector3[] pathway;			// the pathway points

}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/Pathway")]
public class Pathway : ScriptableObject {

	public string pathName;
	public bool isActive;
	public Vector3 center;
	public Vector3[] pathway;

}


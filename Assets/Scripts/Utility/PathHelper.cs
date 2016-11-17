using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[ExecuteInEditMode]
public class PathHelper : MonoBehaviour {

	public Pathway pathway;

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere (this.transform.position, 1);

		if (pathway != null)
			iTween.DrawPath (pathway.pathway.ToArray ());
	}
}

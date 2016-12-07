using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode]
public class PathHelper : MonoBehaviour {

	public Pathway pathway;	//Pathway to visualize.

	/* Draw Selected Pathway in Edit mode. */
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere (this.transform.position, 1);

		if (pathway != null)
			iTween.DrawPath (pathway.pathway);
	}
}

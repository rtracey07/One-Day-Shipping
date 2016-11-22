using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarPathManager : MonoBehaviour {

	[System.Serializable]
	public class AreaBoundary
	{
		public string name;
		public Color pathColor;
		public Vector3 center;
		public float radius;
		public List<Pathway> paths;
	};
		
	[SerializeField]
	public List<AreaBoundary> areas;			

	private AreaBoundary curr;

	[HideInInspector]
	public bool pathChange = false;
	public bool debug = false;
	private Transform player;

	void Awake()
	{
		player = GameObject.Find ("Player").transform;

		if (areas != null && areas.Count > 0)
			curr = areas [0];
		else
			Debug.Log ("Empty Areas in Car Path Manager");
	}

	// Update is called once per frame
	void Update () {
		AreaBoundary next = null;

		if (areas != null && player != null) {
			for (int i = 0; i < areas.Count; i++) {
				if (Vector3.Distance (areas [i].center, player.position) <= areas [i].radius) {
					next = areas [i];
					break;
				}
			}
		}

		if (next != null && next != curr) {
			curr = next;
			pathChange = true;
		} else {
			pathChange = false;
		}
	}

	public Vector3[] GetCurrentAreaPath()
	{
		if (curr != null && curr.paths.Count > 0) {
			return curr.paths [Random.Range (0, curr.paths.Count - 1)].pathway.ToArray();
		}

		return null;
	}

	void OnDrawGizmos()
	{
		if (debug == true) {
			for (int i = 0; i < areas.Count; i++) {
				Gizmos.color = areas [i].pathColor;
				for (int j = 0; j < areas [i].paths.Count; j++) {
					iTween.DrawPath (areas [i].paths [j].pathway.ToArray());
				}
			}
		}
	}
}

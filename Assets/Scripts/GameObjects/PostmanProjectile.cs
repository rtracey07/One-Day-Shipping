using UnityEngine;
using System.Collections;

public class PostmanProjectile : MonoBehaviour {
	private float damageStrength;
	private GameObject pack;

	public float DamageStrength{
		set{ damageStrength = value; }
		get{ return damageStrength; }
	}

	public GameObject Pack {
		set{ pack = value; }
		get{ return pack; }
	}
	/// <summary>
	/// Checks for collisions with player
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter(Collision collision) {
		
		if (collision.rigidbody != null && collision.rigidbody.name.Equals ("Player")) {
			//Debug.Log (DamageStrength);
			//Debug.Log (Pack.name);
			if (Pack != null) {
				Debug.Log ("Package Hit");
				Pack.GetComponent<Package> ().DamagePackage (DamageStrength);
				GameManager.Instance.stats.postmenHit++;
			}
		}
			

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

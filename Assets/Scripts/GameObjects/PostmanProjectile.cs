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

	void Start(){
		LevelManager.Instance.StartCoroutine (LastFiveSeconds());
	}

	IEnumerator LastFiveSeconds(){
		yield return new WaitForSeconds(5.0f);
		Destroy (this.gameObject);
	}

	/// <summary>
	/// Checks for collisions with player
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter(Collision collision) {
		
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Package") {
			if (Pack != null) {
				Pack.GetComponent<Package> ().DamagePackage (DamageStrength);
				GameManager.Instance.stats.postmenHit++;
			}
		} else if(collision.gameObject.tag == "Ground")
			Destroy (this.gameObject);	
	}


}

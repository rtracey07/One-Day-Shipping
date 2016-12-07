using UnityEngine;
using System.Collections;

/// <summary>
/// Postman projectile class
/// </summary>
public class PostmanProjectile : MonoBehaviour {

	//private variables:
	private float damageStrength;
	private GameObject pack;

	/// <summary>
	/// Gets or sets the damage strength.
	/// </summary>
	/// <value>The damage strength.</value>
	public float DamageStrength{
		set{ damageStrength = value; }
		get{ return damageStrength; }
	}

	/// <summary>
	/// Gets or sets the package.
	/// </summary>
	/// <value>The pack.</value>
	public GameObject Pack {
		set{ pack = value; }
		get{ return pack; }
	}

	/// <summary>
	/// Start this instance.
	/// Starts a new thread which deactivates the projectile 5 seconds after it was created.
	/// </summary>
	void Start(){
		LevelManager.Instance.StartCoroutine (LastFiveSeconds());
	}

	/// <summary>
	/// Destroy the projectile after 5 seconds.
	/// </summary>
	/// <returns>The five seconds.</returns>
	IEnumerator LastFiveSeconds(){
		yield return new WaitForSeconds(5.0f);
		Destroy (this.gameObject);
	}

	/// <summary>
	/// Checks for collisions with player or ground
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter(Collision collision) {
		
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Package") {
			if (Pack != null) {
				Pack.GetComponent<Package> ().DamagePackage (DamageStrength);
				GameManager.Instance.stats.postmenHit++;
			}
		} else if (collision.gameObject.tag == "Ground") {
			StopCoroutine ("LastFiveSeconds");
			Destroy (this.gameObject);	
		}
			
	}


}

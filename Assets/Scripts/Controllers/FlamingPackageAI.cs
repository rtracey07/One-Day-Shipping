using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Flaming package AI.
/// </summary>
public class FlamingPackageAI : MonoBehaviour {

	private List<GameObject> flamingPackages;

	public AudioClip fallSound, collisionSound;
	public AudioClip damageSound1, damageSound2, damageSound3;

	public int poolsize;

	public float damageStrength;

	private GameObject pack;

	/// <summary>
	/// At instantiation
	/// </summary>
	void Start(){
		
		//play sound when object gets created:
		if (fallSound != null){
			AudioSource.PlayClipAtPoint (fallSound, this.gameObject.transform.position);
		}
		StartCoroutine (DestroyAfterTenSeconds ());

	}

	/// <summary>
	/// Raises the collision enter event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnCollisionEnter(Collision other){
		//play collision sound:
		AudioManager.Instance.PlaySoundEffect(collisionSound, 0.05f);

		if (other.gameObject.tag == "Player") {
			//check if he has a package and damage it approprietly:
			//play random damageSound:
			switch(Random.Range(1,3)){
			case 1:
				AudioManager.Instance.PlaySoundEffect (damageSound1);
				break;
			case 2:
				AudioManager.Instance.PlaySoundEffect (damageSound2);
				break;
			case 3:
				AudioManager.Instance.PlaySoundEffect (damageSound3);
				break;
			}

			//get the package and do damage:
			pack = GameObject.Find ("Package");
			if (pack != null) {
				Debug.Log ("Package Hit");
				pack.GetComponent<Package> ().DamagePackage (damageStrength);

			}
			//stop coroutine on collision:
			StopCoroutine ("DestroyAfterTenSeconds");
			Destroy (this.gameObject);

		}
			
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
		if (GameClockManager.Instance.freeze) {
			gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		} else {
			gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		}
	}

	/// <summary>
	/// Destroy package 10 seconds after spawn.
	/// </summary>
	/// <returns>The after five seconds.</returns>
	public IEnumerator DestroyAfterTenSeconds(){
		yield return new WaitForSeconds (10.0f);
		if (this.gameObject != null) {
			Destroy (this.gameObject);
		}

	}

}

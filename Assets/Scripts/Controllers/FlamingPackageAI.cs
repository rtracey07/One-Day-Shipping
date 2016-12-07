using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlamingPackageAI : MonoBehaviour {

	private List<GameObject> flamingPackages;

	public AudioClip fallSound, collisionSound;
	public AudioClip damageSound1, damageSound2, damageSound3;

	public int poolsize;

	public float damageStrength;

	private GameObject pack;

	void Start(){
		
		//play sound when object gets created:
		if (fallSound != null){
			AudioSource.PlayClipAtPoint (fallSound, this.gameObject.transform.position);
		}

	}

	void OnCollisionEnter(Collision other){
		//play collision sound:
		AudioManager.Instance.PlaySoundEffect(collisionSound, 0.05f);

		if (other.gameObject.tag == "Player") {
			//check if he has a package and damage it approprietly:
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

			//get the package and do damage
			pack = GameObject.Find ("Package");
			if (pack != null) {
				Debug.Log ("Package Hit");
				pack.GetComponent<Package> ().DamagePackage (damageStrength);

				//GameManager.Instance.stats.
			}

			gameObject.SetActive(false);

		}


		//destroy package no matter what:
		//gameObject.SetActive(false);
	}

	void Update(){
		if (GameClockManager.Instance.freeze) {
			gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		} else {
			gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		}
	}

}

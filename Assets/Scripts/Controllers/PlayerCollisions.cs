using UnityEngine;
using System.Collections;

public class PlayerCollisions : MonoBehaviour {

	public AudioClip treeCollision;


	void OnCollisionEnter(Collider other){
	
		switch (other.gameObject.tag) {

		case "tree":
			AudioManager.Instance.PlaySoundEffect (treeCollision);
			break;

		}
	
	}
}

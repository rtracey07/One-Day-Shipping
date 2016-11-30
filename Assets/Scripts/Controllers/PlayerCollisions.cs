using UnityEngine;
using System.Collections;

public class PlayerCollisions : MonoBehaviour {

	public AudioClip treeCollision;

	void OnTriggerEnter(Collider other){

		switch (other.gameObject.tag) {
		case "Tree":
			AudioManager.Instance.PlaySoundEffect (treeCollision);
			break;
		//add other cases here
		}

	}
}

using UnityEngine;
using System.Collections;

public class RespawnController : MonoBehaviour {

	Transform start;
	public AudioClip respawnSound;

	void Start(){
		start = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Water"){
			AudioSource.PlayClipAtPoint (respawnSound, GameManager.Instance.mainCamera.transform.position);
			transform.position = start.position;
		}
	}
}

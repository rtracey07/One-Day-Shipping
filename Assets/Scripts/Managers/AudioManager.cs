using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	private static AudioManager _Instance;
	public static AudioManager Instance {
		get { 
			return _Instance;
		}
	}
		
	void Awake(){
		if (_Instance == null)
			_Instance = this;
		else {
			DestroyImmediate (this);
		}

	}

	public void PlaySoundEffect(AudioClip clip) {
		AudioSource.PlayClipAtPoint(clip, GameManager.Instance.mainCamera.transform.position);
	}
		
}

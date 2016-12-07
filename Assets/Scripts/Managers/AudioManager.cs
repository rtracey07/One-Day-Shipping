using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	//Singleton Pattern for manager.
	private static AudioManager _Instance;
	public static AudioManager Instance {
		get { 
			return _Instance;
		}
	}

	/* Persist and Assure singleton pattern. */
	void Awake(){
		if (_Instance == null)
			_Instance = this;
		else {
			DestroyImmediate (this);
		}
	}


	/* Play Background Audio. */
	public void playSoundtrack(){
		AudioSource backgroundAudio = GameObject.FindGameObjectWithTag("backgroundSoundtrack").GetComponent<AudioSource>();
		backgroundAudio.clip = LevelManager.Instance.GetBackgroundMusic();
		backgroundAudio.Play ();
	}
		
	/* Play a Sfx at the camera position. */
	public void PlaySoundEffect(AudioClip clip) {
		AudioSource.PlayClipAtPoint(clip, GameManager.Instance.mainCamera.transform.position);
	}

	/* Play a Sfx at the camera position with volume. */
	public void PlaySoundEffect(AudioClip clip, float volume) {
		AudioSource.PlayClipAtPoint (clip, GameManager.Instance.mainCamera.transform.position, volume);
	}
		
}

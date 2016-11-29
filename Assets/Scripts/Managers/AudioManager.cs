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


	public void playSoundtrack(){
		AudioSource backgroundAudio = GameObject.FindGameObjectWithTag("backgroundSoundtrack").GetComponent<AudioSource>();
		backgroundAudio.clip = LevelManager.Instance.GetBackgroundMusic();
		backgroundAudio.Play ();
	}
		
	public void PlaySoundEffect(AudioClip clip) {
		AudioSource.PlayClipAtPoint(clip, GameManager.Instance.mainCamera.transform.position);
	}

	public void PlaySoundEffect(AudioClip clip, float volume) {
		AudioSource.PlayClipAtPoint (clip, GameManager.Instance.mainCamera.transform.position, volume);
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {

	private Slider volumeControl;

	// Use this for initialization
	void Start () {
		AudioManager.Instance.playSoundtrack ();
		volumeControl = (Slider)GameObject.FindGameObjectWithTag ("AudioSlider").GetComponent<Slider>();
	}

	void Update(){
		if (volumeControl != null) {
			AudioListener.volume = volumeControl.value;
		}
	}
	

}

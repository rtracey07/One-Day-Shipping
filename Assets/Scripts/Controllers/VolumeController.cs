using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {

	private Slider volumeControl;

	// Use this for initialization
	void Start () {
		GameObject audio = GameObject.FindGameObjectWithTag ("AudioSlider");
		if (audio != null) {
			volumeControl = audio.GetComponent<Slider> ();
		}
	}

	void Update(){
		
		AudioListener.volume = volumeControl.value;
	}


}
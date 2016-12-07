using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {

	private Slider volumeControl;

	// Use this for initialization
	void Start () {
		SetAudioSlider ();
	}

	void Update(){
		if (Input.GetKey (KeyCode.Escape)) {
			SetAudioSlider ();
		}
		if (volumeControl != null) {
			AudioListener.volume = volumeControl.value;
		}
	}

	/// <summary>
	/// simple volume control slider for UI
	/// </summary>
	void SetAudioSlider(){
		GameObject obj = GameObject.FindGameObjectWithTag ("AudioSlider");

		if (obj != null) {
			volumeControl = (Slider)obj.GetComponent<Slider> ();
		}
	}


}
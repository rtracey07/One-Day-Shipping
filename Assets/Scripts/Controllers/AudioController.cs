﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {

	private Slider volumeControl;

	// Use this for initialization
	void Start () {
		AudioManager.Instance.playSoundtrack ();
		SetAudioSlider ();
	}

	void Update(){
		SetAudioSlider ();
		if (volumeControl != null) {
			AudioListener.volume = volumeControl.value;
		}
	}

	/// <summary>
	/// sets the volume slider for audio control
	/// </summary>
	void SetAudioSlider(){
		GameObject obj = GameObject.FindGameObjectWithTag ("AudioSlider");

		if (obj != null) {
			volumeControl = (Slider)obj.GetComponent<Slider> ();
		} 
	}
	

}

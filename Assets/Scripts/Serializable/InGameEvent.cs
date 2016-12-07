using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// In game event class which holds references to information needed for dialogue boxes.
/// Serializable to include option to set values in Inspector.
/// </summary>
[Serializable]
public class InGameEvent {

	//references to the diaalogue box values:
	public string eventName;
	public AudioClip sound;
	public float soundVolume;
	public Sprite avatar;
	public String[] dialogue;
	public float timeBeforeDisplaying;
	public bool isSkippable;
	public bool pauseGame;
	public bool requiresConfirmation;
	public float duration;
}

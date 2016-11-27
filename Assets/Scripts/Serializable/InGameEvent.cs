using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class InGameEvent {

	public AudioClip sound;
	public Sprite avatar;
	public String[] dialogue;
	public float timeBeforeDisplaying;
	public bool isSkippable;
	public bool pauseGame;
	public bool requiresConfirmation;
	public float duration;
}

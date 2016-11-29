﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class InGameEvent {

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

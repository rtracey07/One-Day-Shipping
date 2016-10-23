using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class InGameEvent {

	public AudioClip sound;
	public String dialogue;
	public bool isSkippable;
	public float duration;
}

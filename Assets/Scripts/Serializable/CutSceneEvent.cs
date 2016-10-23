using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CutSceneEvent{

	public AudioClip sound;
	public String dialogue;
	public bool isSkippable;
	public float duration;
}

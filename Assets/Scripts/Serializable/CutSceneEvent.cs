using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Cut scene event class extends InGameEvent.
/// Serializable to include option to set values in Inspector.
/// </summary>
[Serializable]
public class CutSceneEvent : InGameEvent {

	//references to the background cutscene image and left avatar:
	public Sprite background;
	public Sprite avatarL;

}

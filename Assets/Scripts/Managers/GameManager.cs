using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	private static GameManager _Instance;
	public static GameManager Instance {  get { return _Instance; } }

	[HideInInspector]
	public bool hasPackage = false;

	[HideInInspector]
	public bool continueClicked = false;

	[HideInInspector]
	public bool dogAttack = false;

	[HideInInspector]
	public bool timeUp = false;

	public Camera mainCamera;

	public Stats stats;

	void Awake()
	{
		if (_Instance == null) {
			_Instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else {
			DestroyImmediate (this);
		}
	}

	public void Continue()
	{
		continueClicked = true;
	}

	public void FindCamera()
	{
		GameObject cam = GameObject.Find ("Main Camera");

		if (cam != null)
			mainCamera = cam.GetComponent<Camera> ();
	}
}

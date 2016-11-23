using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	private static GameManager _Instance;
	public static GameManager Instance {  get { return _Instance; } }

	public bool hasPackage = false;
	public Camera mainCamera;

	public Stats stats;

	void Awake()
	{
		if (_Instance == null)
			_Instance = this;
		else
			Debug.LogError ("Multiple Game Managers in Scene.");

		DontDestroyOnLoad (gameObject);

		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameClockManager : MonoBehaviour {

	private static GameClockManager _Instance;
	public static  GameClockManager Instance { get{ return _Instance; } }

	public float time;

	public bool freeze;

	void Awake()
	{
		if (_Instance == null) 
		{
			_Instance = this;
		}

		if (_Instance != this) 
		{
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		freeze = false;
	}

	void Update()
	{
		if (!freeze) {
			time = Time.deltaTime;
		} else {
			time = 0.0f;
		}
	}

}

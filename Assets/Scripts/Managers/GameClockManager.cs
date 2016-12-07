using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Freeze Time. */
public class GameClockManager : MonoBehaviour {

	private static GameClockManager _Instance;
	public static  GameClockManager Instance { get{ return _Instance; } }

	public float time;
	public float fixedTime;

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

	void FixedUpdate()
	{
		if (!freeze) {
			fixedTime = Time.fixedDeltaTime;
		} else {
			fixedTime = 0.0f;
		}
	}

}

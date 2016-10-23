using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameClockManager : MonoBehaviour {

	public static GameClockManager instance;

	public float time;

	public bool freeze;

	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}

		if (instance != this) 
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
		}
	}

}

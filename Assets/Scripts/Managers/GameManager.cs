using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public Stats stats;

	void Awake()
	{
		if (instance == null) 
			instance = this;

		if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

}

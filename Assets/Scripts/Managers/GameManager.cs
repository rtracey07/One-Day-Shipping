using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public Stats stats;

	void Initialize()
	{
		if (instance != null) 
			DestroyImmediate (instance);

		instance = this;
	}

}

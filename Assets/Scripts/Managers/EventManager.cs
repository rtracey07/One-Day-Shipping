using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	public static EventManager instance;

	public List<CutScene> cutScenes;

	void Awake()
	{
		if (instance == null) 
			instance = this;

		if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}
}

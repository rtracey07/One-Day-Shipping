using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

	private static GameManager _Instance;
	public static GameManager Instance {  get { return _Instance; } }

	[Serializable]
	public class Mission
	{
		public Level level;
		public CutScene cutScene;
	}

	public List<Mission> levels;
	public int currLevelIndex = 0; 

	[HideInInspector]
	public bool hasPackage = false;

	[HideInInspector]
	public bool destroyed = false;

	[HideInInspector]
	public bool continueClicked = false;

	[HideInInspector]
	public bool skipClicked = false;

	[HideInInspector]
	public bool dogAttack = false;

	[HideInInspector]
	public bool postmanAttack = false;

	[HideInInspector]
	public bool timeUp = false;

	public Camera mainCamera;

	public Stats stats;

	private bool hasBeenUpdated = false;

	void Awake()
	{
		if (_Instance == null) {
			_Instance = this;
			DontDestroyOnLoad (this);
			FindCamera ();
		}
		else {
			DestroyImmediate (this.gameObject);
		}
	}

	public void Continue()
	{
		continueClicked = true;
	}

	public void Skip()
	{
		continueClicked = true;
		skipClicked = true;
	}

	public void FindCamera()
	{
		GameObject cam = GameObject.Find ("Main Camera");

		if (cam != null)
			mainCamera = cam.GetComponent<Camera> ();
	}

	public Level GetLevelInfo()
	{
		return levels [currLevelIndex].level;
	}

	public CutScene GetCutSceneInfo()
	{
		return levels [currLevelIndex].cutScene;
	}

	public void UpdateLevelInfo()
	{
		if (!hasBeenUpdated) {
			currLevelIndex++;
			if (currLevelIndex >= levels.Count)
				currLevelIndex = 0;
			hasBeenUpdated = true;
		}
	}

	public void Reset()
	{
		hasPackage = false;
		destroyed = false;
		continueClicked = false;
		skipClicked = false;
		dogAttack = false;
		postmanAttack = false;
		timeUp = false;
		hasBeenUpdated = false;

		stats.carsHit = 0;
		stats.dogsHit = 0;
		stats.packagesDelivered = 0;
		stats.packagesDestroyed = 0;
		stats.postmenHit = 0;
		stats.score = 0;

		LevelManager.Instance.m_ParcelCount.text = "";
	}
}

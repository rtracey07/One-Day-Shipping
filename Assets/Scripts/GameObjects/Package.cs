﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Package : MonoBehaviour {

	// the sliders for the UI
	[SerializeField] private Slider damageSlider;
	[SerializeField] private float totalHealth = 100.0f;
	private float health;

	//access the health if necessary
	public float Health {
		set{ health = value; }
		get{ return health; }
	}

	public void DamagePackage(float hp){
		Health = Health - hp;
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("start");
		damageSlider.gameObject.SetActive (true);
	}

	void OnEnable()
	{
		Debug.Log ("enabled");
		health = totalHealth;
		damageSlider.gameObject.SetActive (true);
	}

	void OnDisable(){
		Debug.Log ("disabled");

		if(damageSlider != null)
			damageSlider.gameObject.SetActive (false);
	}

	void Update()
	{
		damageSlider.value = Health;
	}

	public void DisablePackage()
	{
		
	}
}

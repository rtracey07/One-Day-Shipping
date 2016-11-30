using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PackageHealthController : MonoBehaviour {

	public Slider packageHealth;
	public float currentHealth;
	public float deltaHealth; //change in health

	// Use this for initialization
	void Start () {
		currentHealth = 100.0f;
		packageHealth.value = currentHealth;
	}
	
	// Update is called once per frame
	void Update () {
		//packageHealth.value = Mathf.Lerp (currentHealth, currentHealth - deltaHealth,);
	}
}

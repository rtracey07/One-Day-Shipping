using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Package : MonoBehaviour {

	// the sliders for the UI
	[SerializeField] private Slider damageSlider;
	[SerializeField] private float totalHealth = 100.0f;
	private float health;
	public AudioClip pickupSound;
	public AudioClip deliverSound;

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
		AudioManager.Instance.PlaySoundEffect (pickupSound);
	}

	void OnDisable(){
		Debug.Log ("disabled");
		damageSlider.gameObject.SetActive (false);
		AudioManager.Instance.PlaySoundEffect (deliverSound);
	}

	void Update()
	{
		damageSlider.value = Health;
	}

	public void DisablePackage()
	{
		
	}
}

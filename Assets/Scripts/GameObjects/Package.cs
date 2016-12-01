using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Package : MonoBehaviour {

	// the sliders for the UI
	[SerializeField] private Slider damageSlider;
	[SerializeField] private float totalHealth = 100.0f;
	private float health;
	public AudioClip pickupSound, deliverSound, destroyPackageSound;

	//access the health if necessary
	public float Health {
		set{ health = value; }
		get{ return health; }
	}

	public void DamagePackage(float hp){
		Health = Health - hp;
		if (Health <= 0) {
			DisablePackage ();
		}
	}

	// Use this for initialization
	void Start () {
		damageSlider.gameObject.SetActive (true); 
	}

	void OnEnable()
	{
		health = totalHealth;
		damageSlider.gameObject.SetActive (true);
		AudioManager.Instance.PlaySoundEffect (pickupSound);
	}

	void OnDisable(){
		if(damageSlider != null)
			damageSlider.gameObject.SetActive (false);
		AudioManager.Instance.PlaySoundEffect (deliverSound);
	}

	void Update()
	{
		damageSlider.value = Health;
	}

	public void DisablePackage()
	{
		AudioManager.Instance.PlaySoundEffect (destroyPackageSound);
		GameManager.Instance.hasPackage = false;
		damageSlider.gameObject.SetActive (false);

	}
}

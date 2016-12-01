using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Package : MonoBehaviour {

	// the sliders for the UI
	[SerializeField] private Slider damageSlider;
	[SerializeField] private float totalHealth = 100.0f;
	private float health;
	public AudioClip pickupSound, deliverSound, destroyPackageSound, packageDamage1, packageDamage2, packageDamage3;

	//access the health if necessary
	public float Health {
		set{ health = value; }
		get{ return health; }
	}

	public void DamagePackage(float hp){
		Health = Health - hp;
		if (GameManager.Instance.hasPackage)
			PlayPackageDamageSound ();
		if (Health <= 0) {
			GameManager.Instance.destroyed = true;
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
		GameManager.Instance.destroyed = false;
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

	public void PlayPackageDamageSound(){
		int random = Random.Range (1, 4);
		switch (random) {
		case 1:
			AudioManager.Instance.PlaySoundEffect (packageDamage1);
			break;
		case 2:
			AudioManager.Instance.PlaySoundEffect (packageDamage2);
			break;
		case 3:
			AudioManager.Instance.PlaySoundEffect (packageDamage3);
			break;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Package : MonoBehaviour {

	// the sliders for the UI
	[SerializeField] private Slider damageSlider;
	[SerializeField] private float totalHealth = 100.0f;
	private float health;

	//references to audioclips
	public AudioClip destroyPackageSound, packageDamage1, packageDamage2, packageDamage3;

	/// <summary>
	/// Gets or sets the health.
	/// </summary>
	/// <value>The health.</value>
	public float Health {
		set{ health = value; }
		get{ return health; }
	}

	/// <summary>
	/// Damages the package based on float input.
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void DamagePackage(float hp){

		if (GameManager.Instance.hasPackage) {
			PlayPackageDamageSound ();
			Health = Health - hp;

			if (Health <= 0) {
				GameManager.Instance.stats.packagesDestroyed++;
				GameManager.Instance.destroyed = true;
				DisablePackage ();
			}
		}
	}

	/// <summary>
	/// Start this instance.
	/// Activates the package.
	/// </summary>
	void Start () {
		damageSlider.gameObject.SetActive (true); 
	}

	/// <summary>
	/// Raises the enable event.
	/// Resets health to full and activates the UI slider.
	/// </summary>
	void OnEnable()
	{
		health = totalHealth;
		damageSlider.gameObject.SetActive (true);
		GameManager.Instance.destroyed = false; //package is no longer destroyed
	}

	/// <summary>
	/// Raises the disable event.
	/// Deactivates the UI slider.
	/// </summary>
	void OnDisable(){
		if(damageSlider != null)
			damageSlider.gameObject.SetActive (false);
	}

	/// <summary>
	/// Update this instance.
	/// Sets the UI slider value to the package health
	/// </summary>
	void Update()
	{
		damageSlider.value = Health;
	}

	/// <summary>
	/// Disables the package.
	/// </summary>
	public void DisablePackage()
	{
		AudioManager.Instance.PlaySoundEffect (destroyPackageSound);
		GameManager.Instance.hasPackage = false;
		damageSlider.gameObject.SetActive (false);
	}

	/// <summary>
	/// Picks a random package damage sound to play.
	/// </summary>
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

using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Stats class which holds all values associated with calculating the player scores.
/// Serializable to include option to set values in Inspector (for debugging purposes)
/// </summary>
[Serializable]
public class Stats {

	//reference to the score stats:
	public int score;
	public int packagesDelivered;
	public int packagesDestroyed;
	public int carsHit;
	public int dogsHit;
	public int postmenHit;

}

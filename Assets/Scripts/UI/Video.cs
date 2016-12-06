using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * The video that plays in the controls menu
 */
public class Video : MonoBehaviour {

    /** 
     * Use this for initialization
     */     
    void OnEnable() {
		((MovieTexture)GetComponent<RawImage> ().texture).loop = true;
		((MovieTexture)GetComponent<RawImage> ().texture).Play ();
	}
}

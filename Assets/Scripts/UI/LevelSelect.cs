using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * The MonoBehaviour script for the level select
 */
public class LevelSelect : MonoBehaviour {
    
    /**
     * The method called when the user clicks the level select button
     */
    public void OnClick()
    {
        StartCoroutine(LoadScene());
    }
    
    /**
     * Loads the level select scene
     */
    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LevelSelect");
        yield return null;
    }
}

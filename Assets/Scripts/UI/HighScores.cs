using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * The MonoBehaviour script for the high score button 
 */
public class HighScores : MonoBehaviour {
    
    /**
     * The method called when the user clicks the high score button
     */
    public void OnClick()
    {
        StartCoroutine(LoadScene());
    }
    
    /**
     * Loads the high score menu
     */
    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("HighScores");
        yield return null;
    }
}

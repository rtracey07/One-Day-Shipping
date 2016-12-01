using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HighScores : MonoBehaviour {

    public void OnClick()
    {
        StartCoroutine(LoadScene());
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("HighScores");
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSplash : MonoBehaviour
{
    public float timeToWait = 0.5f;

    public string sceneToLoad;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitThenLoad());
    }

    private IEnumerator WaitThenLoad()
    {
        yield return new WaitForSeconds(timeToWait);

        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        
        yield return new WaitForSeconds(0.5f);

        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

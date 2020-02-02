using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Josh;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadGame());

    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0.5f);

        List<Scene> toUnload = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var sc = SceneManager.GetSceneAt(i);
            if (sc != gameObject.scene)
            {
                toUnload.Add(sc);
            }
        }

        var ll = toUnload.Select(scene => SceneManager.UnloadSceneAsync(scene)).ToList();

        while (ll.Any(x => !x.isDone))
        {
            yield return null;
        }

        var loadTask = SceneManager.LoadSceneAsync("TreePLayground", LoadSceneMode.Additive);

        while (!loadTask.isDone)
        {
            yield return null;
        }

        var gameScene = SceneManager.GetSceneByName("TreePLayground");
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TreePLayground"));
        
        // TODO: DO GENERATION SHIT HERE!!!

        var core = gameScene.GetRootGameObjects().First(x => x.name == "Core");
        var worldGen = core.GetComponent<WorldGen>();

        /*
        while (!worldGen.isReady)
        {
            yield return null;
        }
        */
        // TODO: DO GENERATION SHIT HERE!!!
        
        yield return new WaitForSeconds(0.5f);

        SceneManager.UnloadSceneAsync("LoadGame");
    }
}

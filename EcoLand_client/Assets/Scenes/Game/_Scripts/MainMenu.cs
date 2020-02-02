using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button PlayButton;
    public Button CreditsButton;
        
    // Start is called before the first frame update
    void Start()
    {
        if (PlayButton == null)
            return;
        
        if(CreditsButton == null)
            return;

        PlayButton.onClick.AddListener(PlayButtonClick);
        CreditsButton.onClick.AddListener(CreditsButtonClick);
    }

    void PlayButtonClick()
    {
        DisableButtons();

        SceneManager.LoadScene("LoadGame", LoadSceneMode.Additive);
    }

    void CreditsButtonClick()
    {
        DisableButtons();
    }

    void DisableButtons()
    {
        PlayButton.interactable = false;
        CreditsButton.interactable = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

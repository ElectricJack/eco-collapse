using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button PlayButton;
    public Button CreditsButton;

    public Button ExitGameButton;

    public GameObject CreditScreen;
        
    // Start is called before the first frame update
    void Start()
    {
        if (PlayButton == null)
            return;
        
        if(CreditsButton == null)
            return;

        PlayButton.onClick.AddListener(PlayButtonClick);
        CreditsButton.onClick.AddListener(CreditsButtonClick);
        
        ExitGameButton.onClick.AddListener(()=>Application.Quit());
    }

    void PlayButtonClick()
    {
        DisableButtons();

        SceneManager.LoadScene("LoadGame", LoadSceneMode.Additive);
    }

    void CreditsButtonClick()
    {
        CreditScreen.SetActive(true);
    }

    void DisableButtons()
    {
        PlayButton.interactable = false;
        CreditsButton.interactable = false;
    }
}

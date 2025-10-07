using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour 
{
    public void PlayButton()
    {
        InputSystem.DisableAllEnabledActions();
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void ExitButton()
    {
        Debug.Log("Игра закрылась");
        Application.Quit();
    }
    
}

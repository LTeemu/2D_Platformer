using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void toMainMenu()
    {

        SceneManager.LoadScene("MainMenuScene");
    }

    public void toSecondScene()
    {

        SceneManager.LoadScene(1);
    }
}

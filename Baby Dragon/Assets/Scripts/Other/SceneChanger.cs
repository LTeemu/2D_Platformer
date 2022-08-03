using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    private int loadNextScene;
    private int currentScene;
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        loadNextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void Start()
    {
        Debug.Log("Current Scene is " + currentScene);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("BUMP!");
            SceneManager.LoadScene(loadNextScene);
        }
    }
}

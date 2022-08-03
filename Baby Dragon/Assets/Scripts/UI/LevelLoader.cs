using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private AudioManager audioManager;
    [SerializeField] Canvas canvas;

    private void Start()
    {
        canvas.enabled = true;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void LoadNextLevel(){        
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex){
        //Play animation
        transition.SetTrigger("Start");
        audioManager.Stop("Theme");
        audioManager.Play("Fanfare");
        //Wait
        yield return new WaitForSeconds(transitionTime);
        //Load Scene
        SceneManager.LoadSceneAsync(levelIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().invulnerable = true;
            LoadNextLevel();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    AudioSource[] audioSources;

    //public static bool isPaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        
    }

    // 소리도 전부 정지하고 싶다. 괴물 소리가 정지가 안된다. 
    // 이제는 bgm이 정지가 안된다. 
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        //isPaused = true;

        audioSources = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audioSources.Length;i++)
        {
            audioSources[i].Pause();
        }

        Demon[] demons = FindObjectsOfType<Demon>();
        for (int i = 0; i < demons.Length; i++)
        {
            AudioSource audioSource = demons[i].gameObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Pause();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //isPaused = false;


        audioSources = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Play();
        }


        Demon[] demons = FindObjectsOfType<Demon>();
        for (int i = 0; i < demons.Length; i++)
        {
            AudioSource audioSource = demons[i].gameObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //isPaused = false;
        DemonSpawner.deadAlienNum = 0;

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void closeWindow()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //isPaused = false;

        audioSources = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Play();
        }


        Demon[] demons = FindObjectsOfType<Demon>();
        for (int i = 0; i < demons.Length; i++)
        {
            AudioSource audioSource = demons[i].gameObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void selectScene()
    {
        SceneManager.LoadScene(2);
    }
}

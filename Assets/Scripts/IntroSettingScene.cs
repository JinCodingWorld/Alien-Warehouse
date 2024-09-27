using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSettingScene : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip selectSound;
    public void sceneTransition(int snum)
    {
        SceneManager.LoadScene(snum);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void selectSmallGun()
    {
        AsunaGun.gunNum = 0;
    }

    public void selectBigGun()
    {
        AsunaGun.gunNum = 1;
    }

    public void clickAudio()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void selectAudio()
    {
        audioSource.PlayOneShot(selectSound);
    }
}

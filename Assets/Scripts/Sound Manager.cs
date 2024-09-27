using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 SoundManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<SoundManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static SoundManager m_instance; // 싱글톤이 할당될 static 변수
    public Slider sfxSlider;

    public AudioSource audioBackground;
    public AudioSource audioSourceRandom;
    public AudioSource gunSound;
    
    public void SetBGMVolume(float volume)
    {
        audioBackground.volume = volume;
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;

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

    public void resumeGame()
    {
        Time.timeScale = 1f;

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
    public void SetSFXVolume(float volume)
    {
        audioSourceRandom.volume = volume;
        gunSound.volume = volume;
    }

    public void OnSfx()
    {
        audioSourceRandom.Play();
    }

    public void OffSfx()
    {
        audioSourceRandom.Pause();
    }

}

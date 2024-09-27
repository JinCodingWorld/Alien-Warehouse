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
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ SoundManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<SoundManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static SoundManager m_instance; // �̱����� �Ҵ�� static ����
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

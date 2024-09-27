using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    public AsunaGun gun; // ����� ��
    //public Transform gunPivot; // �� ��ġ�� ������
    public ParticleSystem ammoEffect;
    public GameObject bombImage;
    public GameObject bomb;
    public GameObject bombReady;
    public GameObject bombSetIndicate;
    public GameObject door;
    public GameObject fakedoor;
    public GameObject doorShape;
    public GameObject spaceShip;
    public GameObject attatchBombMission;
    public GameObject warningScreen;
    public GameObject bombTimer;
    public GameObject victoryWindow;

    private AudioSource playerAudio;
    public AudioSource gameBGM;
    public AudioClip ammoSound;
    public AudioClip bombPickSound;
    public AudioClip bombSetSound;
    public AudioClip alarmSound;
    public AudioClip doorOpenSound;
    public AudioClip victorySound;
    public AudioClip footStep;

    private PlayerInput playerInput; // �÷��̾��� �Է�
    private Animator playerAnimator; // �ִϸ����� ������Ʈ

    public Slider ammoChar; // ĳ���� �� ����
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
        DemonSpawner.deadAlienNum = 0;
    }
    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }
    private void Update()
    {

        if (playerInput.fire)
        {
            gun.Fire();
            playerAnimator.SetTrigger("Fire");
            // playerInput.fire false�� ����� ��� ������?
            playerInput.fire = false;
        }
        else if (playerInput.reload)
        {
            if (gun.Reload())
            {
                playerAnimator.SetTrigger("Reload");
                
                playerInput.reload = false;
            }
        }
        
        if(playerInput.jump)
        {
            playerAnimator.SetTrigger("Jump");
        }

        UpdateUI();
    }
    private void UpdateUI()
    {
        if (gun != null && UIController.instance != null)
        {
            // UI �Ŵ����� ź�� �ؽ�Ʈ�� źâ�� ź��� ���� ��ü ź���� ǥ��
            UIController.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
            ammoChar.maxValue = gun.gunData[AsunaGun.gunNum].magCapacity;
            ammoChar.value = gun.magAmmo;
        }
    }

    public void OffAttatchBombMission()
    {
         attatchBombMission.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ammo")
        {
            Debug.Log("�Ѿ� ����");
            ammoEffect.Play();
            playerAudio.PlayOneShot(ammoSound);
        }
        else if(other.tag == "Bomb")
        {
            Debug.Log("��ź ����");
            bomb.SetActive(false);
            bombImage.SetActive(true);
            bombSetIndicate.SetActive(true);
            // ��ź ��ġ �̼�â Ȱ��ȭ
            attatchBombMission.SetActive(true);
            Invoke("OffAttatchBombMission", 4f);

            playerAudio.PlayOneShot(bombPickSound);
        }
        if(other.tag == "BombReady")
        {
            Debug.Log("��ź ��ġ");
            bombReady.GetComponent<MeshRenderer>().enabled = true;

            warningScreen.SetActive(true);
            // ī��Ʈ �ٿ� ����
            bombTimer.SetActive(true);
            GameManager.instance.bombInstall = true;
            // ��� �Ҹ� �����̴� ������ ���缭 �ٿ��ֱ� ����?
            playerAudio.volume = SoundManager.instance.audioSourceRandom.volume;
            playerAudio.PlayOneShot(alarmSound);

            bombReady.SetActive(true);
            bombImage.SetActive(false);
            doorShape.SetActive(false);
            fakedoor.SetActive(true);
            playerAudio.PlayOneShot(bombSetSound);
        }
        if(other.tag == "Door")
        {
            Debug.Log("�� ����");
            fakedoor.SetActive(false);
            // �ִϸ��̼� ���
            door.SetActive(true);
            spaceShip.SetActive(true);

            playerAudio.Stop();
            playerAudio.PlayOneShot(doorOpenSound);
            // Ż�� ������ ����
        }
        // Ż�� ����! UI ������
        if(other.tag == "Success")
        {
            victoryWindow.SetActive(true);
            Time.timeScale = 0f;
            gameBGM.Stop();
            playerAudio.PlayOneShot(victorySound);

            // ���� �Ҹ��� �� ����� �Ѵ�. (�غ��ô�.)
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

        Invoke("ammoEffectStop", 2f);
    }

    public void ammoEffectStop()
    {
        ammoEffect.Stop();
    }

    public void footStepSound()
    {
        playerAudio.PlayOneShot(footStep);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    public AsunaGun gun; // 사용할 총
    //public Transform gunPivot; // 총 배치의 기준점
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

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    public Slider ammoChar; // 캐릭터 총 상태
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
            // playerInput.fire false로 만드는 방법 없을까?
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
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
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
            Debug.Log("총알 먹음");
            ammoEffect.Play();
            playerAudio.PlayOneShot(ammoSound);
        }
        else if(other.tag == "Bomb")
        {
            Debug.Log("폭탄 주음");
            bomb.SetActive(false);
            bombImage.SetActive(true);
            bombSetIndicate.SetActive(true);
            // 폭탄 설치 미션창 활성화
            attatchBombMission.SetActive(true);
            Invoke("OffAttatchBombMission", 4f);

            playerAudio.PlayOneShot(bombPickSound);
        }
        if(other.tag == "BombReady")
        {
            Debug.Log("폭탄 설치");
            bombReady.GetComponent<MeshRenderer>().enabled = true;

            warningScreen.SetActive(true);
            // 카운트 다운 시작
            bombTimer.SetActive(true);
            GameManager.instance.bombInstall = true;
            // 경고 소리 슬라이더 볼륨에 맞춰서 줄여주기 가능?
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
            Debug.Log("문 닿음");
            fakedoor.SetActive(false);
            // 애니메이션 재생
            door.SetActive(true);
            spaceShip.SetActive(true);

            playerAudio.Stop();
            playerAudio.PlayOneShot(doorOpenSound);
            // 탈출 성공한 거임
        }
        // 탈출 성공! UI 띄위기
        if(other.tag == "Success")
        {
            victoryWindow.SetActive(true);
            Time.timeScale = 0f;
            gameBGM.Stop();
            playerAudio.PlayOneShot(victorySound);

            // 괴물 소리도 다 멈춰야 한다. (해봅시다.)
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

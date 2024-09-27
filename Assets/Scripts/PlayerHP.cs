using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHP : LivingThings
{
    public UnityEngine.UI.Slider healthSlider; // 체력을 표시할 UI 슬라이더
    public UnityEngine.UI.Image hpBar;

    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip moneyPickSound;
    public AudioClip healthPackSound;
    public AudioClip bombExplodeSound;

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMoveControl playerMoveControl;
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    public ParticleSystem greenHealthBoxEffect;
    public ParticleSystem moneyEffect;
    public ParticleSystem lifeMinusEffect;
    public ParticleSystem explosionEffect;

    // 배경음악 
    public GameObject bgmObject;

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerMoveControl = GetComponent<PlayerMoveControl>();
        playerShooter = GetComponent<PlayerShooter>();

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        hpBar.fillAmount = startingHealth;

        playerMoveControl.enabled = true;
        playerShooter.enabled = true;
    }

    public override void RestoreHealth(float newHealth)
    {
        // 체력 최대 한계 100임.
        base.RestoreHealth(newHealth);
        if(health >= 100)
        {
            health = 100;
        }
        // 체력바 갱신
        hpBar.fillAmount = health;
        UIController.instance.UpdateHPText(health);
        healthSlider.value = health;
        
        greenHealthBoxEffect.Play();
        playerAudioPlayer.PlayOneShot(healthPackSound);

        Invoke("StopEffect", 2f);
    }

    public void StopEffect()
    {
        greenHealthBoxEffect.Stop();
    }

    public override void OnInjury(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        //base.OnDamage(damage, hitPoint, hitDirection);

        if (health <= 0 && life != 0)
        {
            life--;
            RestoreHealth(startingHealth);
            lifeMinusEffect.Play();

            UIController.instance.UpdateLifeText(life);
            Debug.Log("라이프 하나 감소/ 현재 라이프 : " + life);

            Invoke("stopLifeMinus", 2f);
        }

        health -= damage;
        Debug.Log("공격을 받았다." + health);
        if (life == 0 && !dead)
        {
            Die();
        }

        healthSlider.value = health;
        hpBar.fillAmount = health / startingHealth;
        UIController.instance.UpdateHPText(health);
    }

    public void stopLifeMinus()
    {
        lifeMinusEffect.Stop();
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        hpBar.enabled = false;
        //healthSlider.gameObject.SetActive(false);

        if(GameManager.instance.timeCnt <= 0)
        {
            playerAudioPlayer.PlayOneShot(bombExplodeSound);
        }
        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMoveControl.enabled = false;
        playerShooter.enabled = false;

        bgmObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if (!dead)
        {
            IItem item = other.GetComponent<IItem>();
            if (item != null)
            {
                item.Use(gameObject);
                //playerAudioPlayer.PlayOneShot(itemPickupClip);
            }

            if(other.tag == "Money")
            {
                moneyEffect.Play();
                playerAudioPlayer.PlayOneShot(moneyPickSound);
            }
        }
    }

}

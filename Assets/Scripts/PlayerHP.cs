using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHP : LivingThings
{
    public UnityEngine.UI.Slider healthSlider; // ü���� ǥ���� UI �����̴�
    public UnityEngine.UI.Image hpBar;

    public AudioClip deathClip; // ��� �Ҹ�
    public AudioClip hitClip; // �ǰ� �Ҹ�
    public AudioClip moneyPickSound;
    public AudioClip healthPackSound;
    public AudioClip bombExplodeSound;

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����

    private PlayerMoveControl playerMoveControl;
    private PlayerShooter playerShooter; // �÷��̾� ���� ������Ʈ

    public ParticleSystem greenHealthBoxEffect;
    public ParticleSystem moneyEffect;
    public ParticleSystem lifeMinusEffect;
    public ParticleSystem explosionEffect;

    // ������� 
    public GameObject bgmObject;

    private void Awake()
    {
        // ����� ������Ʈ�� ��������
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
        // ü�� �ִ� �Ѱ� 100��.
        base.RestoreHealth(newHealth);
        if(health >= 100)
        {
            health = 100;
        }
        // ü�¹� ����
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

        // LivingEntity�� OnDamage() ����(������ ����)
        //base.OnDamage(damage, hitPoint, hitDirection);

        if (health <= 0 && life != 0)
        {
            life--;
            RestoreHealth(startingHealth);
            lifeMinusEffect.Play();

            UIController.instance.UpdateLifeText(life);
            Debug.Log("������ �ϳ� ����/ ���� ������ : " + life);

            Invoke("stopLifeMinus", 2f);
        }

        health -= damage;
        Debug.Log("������ �޾Ҵ�." + health);
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

    // ��� ó��
    public override void Die()
    {
        // LivingEntity�� Die() ����(��� ����)
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
        //�����۰� �浹�� ��� �ش� �������� ����ϴ� ó��
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

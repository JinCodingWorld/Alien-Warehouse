using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingThings : MonoBehaviour, IInjurious
{
    public float startingHealth = 100f; 
    public float health { get; protected set; }
    public bool dead;

    public event Action onDeath;
    public int life;

    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable()
    {
        dead = false;
        Debug.Log("Livingthings dead = false");
        health = startingHealth;

        // 최초 라이프 수
        life = 3;
    }

    // 데미지를 입는 기능
    public virtual void OnInjury(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
            Debug.Log("좀비 죽음");
        }
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return;
        }
        health += newHealth;
    }

    // 주사 맞으면 목숨 하나 살아남
    public virtual void RestoreLife(int newLife)
    {
        if (dead)
        {
            return;
        }
        Debug.Log("목숨 + 1");
        life++;
        UIController.instance.UpdateLifeText(life);

    }

    // 사망 처리
    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }
}

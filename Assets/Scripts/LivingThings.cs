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

    // ����ü�� Ȱ��ȭ�ɶ� ���¸� ����
    protected virtual void OnEnable()
    {
        dead = false;
        Debug.Log("Livingthings dead = false");
        health = startingHealth;

        // ���� ������ ��
        life = 3;
    }

    // �������� �Դ� ���
    public virtual void OnInjury(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
            Debug.Log("���� ����");
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

    // �ֻ� ������ ��� �ϳ� ��Ƴ�
    public virtual void RestoreLife(int newLife)
    {
        if (dead)
        {
            return;
        }
        Debug.Log("��� + 1");
        life++;
        UIController.instance.UpdateLifeText(life);

    }

    // ��� ó��
    public virtual void Die()
    {
        // onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        if (onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBox : MonoBehaviour, IItem
{
    public float health = 30; // ü���� ȸ���� ��ġ
    public void Use(GameObject target)
    {
        // ���޹��� ���� ������Ʈ�κ��� LivingEntity ������Ʈ �������� �õ�
        LivingThings life = target.GetComponent<LivingThings>();

        // LivingEntity������Ʈ�� �ִٸ�
        if (life != null)
        {
            // ü�� ȸ�� ����
            life.RestoreHealth(health);

        }

        // ���Ǿ����Ƿ�, �ڽ��� �ı�
        Destroy(gameObject);
    }
}

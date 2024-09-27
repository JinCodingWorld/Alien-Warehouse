using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPack : MonoBehaviour, IItem
{
    public int ammo = 30; // ������ �Ѿ� ��
    public void Use(GameObject target)
    {
        // ���� ���� ���� ������Ʈ�κ��� PlayerShooter ������Ʈ�� �������� �õ�
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        // PlayerShooter ������Ʈ�� ������, �� ������Ʈ�� �����ϸ�
        if (playerShooter != null && playerShooter.gun != null)
        {
            Debug.Log("BulletPack ����");
            // ���� ���� źȯ ���� ammo ��ŭ ���Ѵ�
            playerShooter.gun.ammoRemain += ammo;


        }

        // ���Ǿ����Ƿ�, �ڽ��� �ı�
        Destroy(gameObject);
    }
}

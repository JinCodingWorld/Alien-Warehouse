using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Asuna", fileName = "Gun Data")]
public class AsunaGunData : ScriptableObject
{
    public AudioClip shotClip; // �߻� �Ҹ�
    public AudioClip reloadClip; // ������ �Ҹ�

    public float damage; // ���ݷ�

    public int startAmmoRemain; // ó���� �־��� ��ü ź��
    public int magCapacity; // źâ �뷮

    public float timeBetFire; // �Ѿ� �߻� ����
    public float reloadTime; // ������ �ҿ� �ð�
}

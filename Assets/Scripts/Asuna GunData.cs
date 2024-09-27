using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Asuna", fileName = "Gun Data")]
public class AsunaGunData : ScriptableObject
{
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    public float damage; // 공격력

    public int startAmmoRemain; // 처음에 주어질 전체 탄약
    public int magCapacity; // 탄창 용량

    public float timeBetFire; // 총알 발사 간격
    public float reloadTime; // 재장전 소요 시간
}

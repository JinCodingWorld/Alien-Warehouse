using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.IO.Enumeration.FileSystemEnumerable<TResult>;

public class AsunaGun : MonoBehaviour
{   
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄알집이 빔
        Reloading // 재장전 중
    }
    public State state { get; private set; } 

    public Transform fireTransform; // 발사 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과
    public ParticleSystem bulletHitEffect;

    private LineRenderer bulletLineRenderer; 

    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public AsunaGunData[] gunData; // 총의 현재 데이터
    public static int gunNum = 0;

    public GameObject smallGun;
    public GameObject bigGun;

    private float fireDistance = 50f; 

    public int ammoRemain; // 남은 전체 탄알
    public int magAmmo; //현재 탄알집에 남아 있는 탄알

    private float lastFireTime; 

    public Transform followCamera;

    private void Awake()
    {
        if (gunNum == 0)
        {
            smallGun.SetActive(true);
            bigGun.SetActive(false);
        }
        else if (gunNum == 1)
        {
            smallGun.SetActive(false);
            bigGun.SetActive(true);
        }
        fireTransform = GameObject.Find("Fire Position").transform;

        for (int i = 0; i < gunData.Length; i++)
        {
            magAmmo = gunData[gunNum].magCapacity;
        }

        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        // 총 상태 초기화
        ammoRemain = gunData[gunNum].startAmmoRemain;
        magAmmo = gunData[gunNum].magCapacity;

        // 총 1번 선택 시 정지하는 현상 발생한다. (왜 그럴까? 0번 선택하면 안 그럼)
        Time.timeScale = 1f;

        state = State.Ready;
        lastFireTime = 0;
    }
    public void Fire()
    {
        Debug.Log("건 스크립트 Fire");
        if (state == State.Ready && Time.time >= lastFireTime + gunData[gunNum].timeBetFire)
        {
            lastFireTime = Time.time;

            
            Shot();
        }
    }

    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(followCamera.position, followCamera.forward, out hit))
        {
            IInjurious target = hit.collider.GetComponent<IInjurious>();

            if (target != null)
            {
                target.OnInjury(gunData[gunNum].damage, hit.point, hit.normal);
            }
            else
            {
                hitPosition = hit.point;

                bulletHitEffect.transform.position = hitPosition;
                bulletHitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
                bulletHitEffect.Play();
            }
        }
        //else
        //{
        //    //hitPosition = followCamera.position + followCamera.forward * fireDistance;

        //    //bulletHitEffect.transform.position = hitPosition;
        //    //bulletHitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
        //    //bulletHitEffect.Play();
        //}

        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }

    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 왜 안나가는가?
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        // 소리가 겹쳐진다!
        gunAudioPlayer.PlayOneShot(gunData[gunNum].shotClip);

        // 맞은 장소에서 파티클 이펙트

        if (gunNum == 1)
        {
            Vector3 hitPosition1 = followCamera.position + followCamera.forward * fireDistance;

            bulletLineRenderer.SetPosition(0, fireTransform.position);
            bulletLineRenderer.SetPosition(1, hitPosition1);
            bulletLineRenderer.enabled = true;
        }

        yield return new WaitForSeconds(0.5f);
        // 라인 렌더러 궤적이 이상하다.
        bulletLineRenderer.enabled = false;

        // 라인 렌더러를 활성화하여 탄알 궤적을 그림
        //bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        //yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
        //bulletLineRenderer.enabled = false;
    }

    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData[gunNum].magCapacity)
        {

            return false;
        }
        Debug.Log("총 스크립트 리로드");
        StartCoroutine(ReloadRoutine());
        return true;
    }
    private IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;

        gunAudioPlayer.PlayOneShot(gunData[gunNum].reloadClip);

        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(gunData[gunNum].reloadTime);

        int ammoToFill = gunData[gunNum].magCapacity - magAmmo;
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;

    }

    public void gunEffect()
    {
        muzzleFlashEffect.Play();
    }
}

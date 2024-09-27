using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.IO.Enumeration.FileSystemEnumerable<TResult>;

public class AsunaGun : MonoBehaviour
{   
    public enum State
    {
        Ready, // �߻� �غ��
        Empty, // ź������ ��
        Reloading // ������ ��
    }
    public State state { get; private set; } 

    public Transform fireTransform; // �߻� ��ġ

    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��
    public ParticleSystem shellEjectEffect; // ź�� ���� ȿ��
    public ParticleSystem bulletHitEffect;

    private LineRenderer bulletLineRenderer; 

    private AudioSource gunAudioPlayer; // �� �Ҹ� �����

    public AsunaGunData[] gunData; // ���� ���� ������
    public static int gunNum = 0;

    public GameObject smallGun;
    public GameObject bigGun;

    private float fireDistance = 50f; 

    public int ammoRemain; // ���� ��ü ź��
    public int magAmmo; //���� ź������ ���� �ִ� ź��

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
        // �� ���� �ʱ�ȭ
        ammoRemain = gunData[gunNum].startAmmoRemain;
        magAmmo = gunData[gunNum].magCapacity;

        // �� 1�� ���� �� �����ϴ� ���� �߻��Ѵ�. (�� �׷���? 0�� �����ϸ� �� �׷�)
        Time.timeScale = 1f;

        state = State.Ready;
        lastFireTime = 0;
    }
    public void Fire()
    {
        Debug.Log("�� ��ũ��Ʈ Fire");
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
        // �� �ȳ����°�?
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        // �Ҹ��� ��������!
        gunAudioPlayer.PlayOneShot(gunData[gunNum].shotClip);

        // ���� ��ҿ��� ��ƼŬ ����Ʈ

        if (gunNum == 1)
        {
            Vector3 hitPosition1 = followCamera.position + followCamera.forward * fireDistance;

            bulletLineRenderer.SetPosition(0, fireTransform.position);
            bulletLineRenderer.SetPosition(1, hitPosition1);
            bulletLineRenderer.enabled = true;
        }

        yield return new WaitForSeconds(0.5f);
        // ���� ������ ������ �̻��ϴ�.
        bulletLineRenderer.enabled = false;

        // ���� �������� Ȱ��ȭ�Ͽ� ź�� ������ �׸�
        //bulletLineRenderer.enabled = true;

        // 0.03�� ���� ��� ó���� ���
        //yield return new WaitForSeconds(0.03f);

        // ���� �������� ��Ȱ��ȭ�Ͽ� ź�� ������ ����
        //bulletLineRenderer.enabled = false;
    }

    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData[gunNum].magCapacity)
        {

            return false;
        }
        Debug.Log("�� ��ũ��Ʈ ���ε�");
        StartCoroutine(ReloadRoutine());
        return true;
    }
    private IEnumerator ReloadRoutine()
    {
        // ���� ���¸� ������ �� ���·� ��ȯ
        state = State.Reloading;

        gunAudioPlayer.PlayOneShot(gunData[gunNum].reloadClip);

        // ������ �ҿ� �ð� ��ŭ ó�� ����
        yield return new WaitForSeconds(gunData[gunNum].reloadTime);

        int ammoToFill = gunData[gunNum].magCapacity - magAmmo;
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        // ���� ���� ���¸� �߻� �غ�� ���·� ����
        state = State.Ready;

    }

    public void gunEffect()
    {
        muzzleFlashEffect.Play();
    }
}

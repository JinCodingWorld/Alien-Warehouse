using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Demon : LivingThings
{
    public LayerMask whatIsTarget;

    private LivingThings targetEntity;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;

    public ParticleSystem hitEffect; // �ǰ� �� ����� ��ƼŬ ȿ��
    public AudioClip deathSound; // ��� �� ����� �Ҹ�
    public AudioClip hitSound; // �ǰ� �� ����� �Ҹ�

    private Animator DevilAnimator; // �ִϸ����� ������Ʈ
    private AudioSource DevilAudioPlayer; // ����� �ҽ� ������Ʈ

    public float damage = 30f; // ���ݷ�
    public float timeBetAttack = 1f; // ���� ����
    private float lastAttackTime; // ������ ���� ����

    private bool hasTarget
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            // �׷��� �ʴٸ� false
            return false;
        }
    }

    private bool punch = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        DevilAnimator = GetComponent<Animator>();
        DevilAudioPlayer = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Setup(DemonData demonData)
    {
        startingHealth = demonData.health;
        health = demonData.health;

        damage = demonData.damage;

        navMeshAgent.speed = demonData.speed;

    }
    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        DevilAnimator.SetBool("HasTarget", hasTarget);

    }
    private void FixedUpdate()
    {
        DevilAnimator.SetBool("Punch", false);
    }


    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
                
            if (hasTarget)
            { 

                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                navMeshAgent.isStopped = true;

                // �� �� ���� ����
                Collider[] colliders = Physics.OverlapSphere(transform.position, 30f, whatIsTarget);
                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingThings livingEntity = colliders[i].GetComponent<LivingThings>();

                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }

            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnInjury(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            
            hitEffect.Play();
            DevilAudioPlayer.Stop();
            DevilAudioPlayer.PlayOneShot(hitSound);
        }

        base.OnInjury(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        DevilAnimator.SetTrigger("Die");
        DevilAudioPlayer.Stop();
        DevilAudioPlayer.PlayOneShot(deathSound);

        DemonSpawner.deadAlienNum++;

        // ���� ���Ƿ� ���� ����.
        Invoke("eraseDevilColliders", 3f);

        
    }

    public void eraseDevilColliders()
    {
        Collider[] DevilColliders = GetComponents<Collider>();
        for (int i = 0; i < DevilColliders.Length; i++)
        {
            DevilColliders[i].enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingThings attackTarget = other.GetComponent<LivingThings>();

            if (attackTarget != null && attackTarget == targetEntity)
            {
                Debug.Log("�÷��̾�� �ε�����.");

                lastAttackTime = Time.time;
                punch = true;

                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                attackTarget.OnInjury(damage, hitPoint, hitNormal);

                Debug.Log("�������� �÷��̾�� �ش�.");

                DevilAnimator.SetBool("Punch", punch);
            }
        }
    }

}

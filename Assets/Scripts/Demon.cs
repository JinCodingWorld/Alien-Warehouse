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

    public ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과
    public AudioClip deathSound; // 사망 시 재생할 소리
    public AudioClip hitSound; // 피격 시 재생할 소리

    private Animator DevilAnimator; // 애니메이터 컴포넌트
    private AudioSource DevilAudioPlayer; // 오디오 소스 컴포넌트

    public float damage = 30f; // 공격력
    public float timeBetAttack = 1f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            // 그렇지 않다면 false
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

                // 이 원 범위 조절
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

        // 내가 임의로 정한 거임.
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
                Debug.Log("플레이어와 부딪혔다.");

                lastAttackTime = Time.time;
                punch = true;

                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                attackTarget.OnInjury(damage, hitPoint, hitNormal);

                Debug.Log("데미지를 플레이어에게 준다.");

                DevilAnimator.SetBool("Punch", punch);
            }
        }
    }

}

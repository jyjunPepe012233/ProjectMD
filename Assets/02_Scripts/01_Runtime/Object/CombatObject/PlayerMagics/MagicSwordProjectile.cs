using System;
using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.Utils;
using UnityEngine;

namespace MinD.Runtime.Object.Magics
{
public class MagicSwordProjectile : MonoBehaviour
{
    // public MagicSwordMain mainScript;
    /*물리 기반으로 바꾸기*/ /*현 마지막*/
    
    [Space(10)]
    [SerializeField] private ParticleSystem flightFx;
    [SerializeField] private ParticleSystem explosionFx;
    [Space(10)]
    [SerializeField] private DamageCollider explosionDamageCollider;
    
    private Rigidbody rigidbody;
    private Collider collider;

    [SerializeField] private Vector3 startPosotion;
    [SerializeField] private Vector3 readyPosition;
    
    private bool isExploded;

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public IEnumerator ShootCoroutine(BaseEntity target)
    {
        float elapsedTime = 0f;
        
        if (target != null) // 적 감지 시 추척하여 발사
        {
            while (true)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(target.transform.position),10);
                rigidbody.velocity = transform.forward * 10;
                                
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= 5)
                {
                    StartCoroutine(Explode());
                }
            }
        }
        else // 적 감지 실패 시 그냥 발사
        {
            while (true)
            {
                // /* 왜 타겟을 썻는가, 혹시 그대는 병신인가 */
                transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(transform.forward * 2 - transform.up), 10);
                // /* 너는 병신이야. 아니 진짜 아ㅏㅏㅏㅏㅏㅏㅏ */
                /* 클라이언트 쪽 날아감 수요일 작업 복구해야함 */
                rigidbody.velocity = transform.forward * 10;
                                
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= 5)
                {
                    StartCoroutine(Explode());
                }
            }
        }
    }
    
    public IEnumerator SetSwordPosition(BaseEntity owner, BaseEntity target, Vector3 position)
    {
        float elapsedTime = 0;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            
            transform.position = Vector3.Lerp(transform.position, owner.transform.position + position,0.65f);
            
            if (elapsedTime >= 0.8f)
            {
                StartCoroutine(ShootCoroutine(target));
            }
            
            
        }
        
    }


    public IEnumerator Explode()
    {
        explosionDamageCollider.gameObject.SetActive(true);
        
        flightFx.Stop();
        explosionFx.Play();

        rigidbody.velocity = Vector3.zero;
        
        isExploded = true;
        Destroy(gameObject,explosionFx.main.duration);

        yield break;
    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     if (!isExploded)
    //     {
    //         StartCoroutine(Explode());
    //     }
    // }
}
}
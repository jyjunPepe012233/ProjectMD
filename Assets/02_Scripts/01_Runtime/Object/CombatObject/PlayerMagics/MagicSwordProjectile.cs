using System;
using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.Utils;
using MinD.Runtime.System;
using UnityEngine;

namespace MinD.Runtime.Object.Magics
{
public class MagicSwordProjectile : MonoBehaviour
{
    
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

    public void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public IEnumerator ShootCoroutine(BaseEntity target)
    {
        float elapsedTime = 0f;
        collider.isTrigger = true;

        collider.enabled = true;
        
        if (target != null) // 적 감지 시 추척하여 발사
        {
            while (true)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(target.transform.position),10);
                rigidbody.velocity = transform.forward * 10;
                                
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= 5)
                {
                    yield return StartCoroutine(Explode());
                }
            }
        }
        else // 적 감지 실패 시 그냥 발사
        {
            while (true)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(transform.forward * 2 - transform.up), 10);
                rigidbody.velocity = transform.forward * 10;
                
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= 5)
                {
                    yield return StartCoroutine(Explode());
                }
            }
        }
        
    }
    
    /* 버그진행 순서
     * 애니메 이밴트
     * set Position -> re anime --------------------------- set Position << 판독결과 얘 문제 아님 ( 거짓된 정보입니다. )
     * re anime.evnet -> 에러
     * NullReferenceException: Object reference not set to an instance of an object
       MinD.Runtime.Entity.PlayerCombatHandler.OnSuccessfullyCast () (at Assets/02_Scripts/01_Runtime/Entity/Player/PlayerCombatHandler.cs:215)
     */
    
    public IEnumerator SetSwordPosition(BaseEntity owner, BaseEntity target, Vector3 position)
    {
        // /*보간이 무작위로 적용됨*/  /* 사라지기 전에 재사용 시 보간 적용 - 거짓으로 판명 */ /* 재실행 이슈 고쳐지면 해결되지 않을까, 재실행 후 보간적용 */
        
        Debug.Log("start SetPosition");
        
        PhysicUtility.IgnoreCollisionUtil(owner, collider);
        
        float elapsedTime = 0;
        flightFx.Play();

        while (true)
        {
            elapsedTime += Time.deltaTime;
            
            transform.position = Vector3.Lerp(transform.position, owner.transform.position + position + new Vector3(0,2.7f,0),0.25f);
            yield return null;
            
            if (elapsedTime >= 1.4f)
            {
                yield break; //StartCoroutine(ShootCoroutine(target));
            }
        
        }
        
        yield break;
    }


    public IEnumerator Explode()
    {
        if (!isExploded)
        {
            isExploded = true;
            explosionDamageCollider.gameObject.SetActive(true);

            flightFx.Stop();
            explosionFx.Play();

            rigidbody.velocity = Vector3.zero;
            
            Destroy(gameObject, explosionFx.main.duration);

            yield break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isExploded)
        {
            StartCoroutine(Explode());
        }
    }
}
}
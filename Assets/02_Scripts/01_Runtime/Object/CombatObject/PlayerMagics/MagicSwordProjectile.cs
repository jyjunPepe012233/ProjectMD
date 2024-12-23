using System;
using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.Utils;
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
    
    private Player owner;
    
    [SerializeField] private Vector3 startPosotion;
    [SerializeField] private Vector3 readyPosition;
    
    private bool isExploded;

    public void OnEnable()
    {
        // flightFx.Play();
        // flightFx.enabled = true;
        
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public IEnumerator ShootCoroutine( BaseEntity target )
    {
        float elapsedTime = 0f;
        float speed = 15;
        
        flightFx.Play();
            
        rigidbody.isKinematic = false;
        
        // PhysicUtility.IgnoreCollisionUtil(owner, collider); /* 기능안함 */
        
        /* 콜라이더들 활성화 */
        collider.isTrigger = true;
        collider.enabled = true; // use Colllier
        
        if (target != null) // 적 감지 시 추척하여 발사
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation((target.transform.position + new Vector3(0,target.transform.lossyScale.y * 1.2f,0) ) - transform.position), 360);
            
            
            while (true)
            {
                elapsedTime += Time.deltaTime;
                
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                        Quaternion.LookRotation(target.transform.position + new Vector3(0, /*1.5f*/ target.transform.lossyScale.y * 1.2f ,0) - transform.position), 360 * Time.deltaTime * 2);
                rigidbody.velocity = transform.forward * speed;
                
                if (elapsedTime >= 5)
                {
                    yield return StartCoroutine(Explode());
                    continue;
                }

                yield return null;

            }
        }
        else // 적 감지 실패 시 그냥 발사
        {
            while (true)
            {
                elapsedTime += Time.deltaTime;
                
                rigidbody.velocity = transform.forward * speed;
                
                if (elapsedTime >= 3f)
                {
                    yield return StartCoroutine(Explode());
                    /*  이거 안됌, 왜?
                     StartCoroutine(Explode()); 
                     yield break;
                     */
                    continue;
                }

                yield return null;

            }
        }
        
    }
    
    
    public IEnumerator SetSwordPosition(Player _owner, Vector3 position)
    {
        
        owner = _owner;
        
        // float high = 2.1f;
        float high = 5f;
        float lerpSpace = 0.5f;
        
        float elapsedTime = 0;
        
        while (true) // 타겟 있을 시 카메라 따라가기, 평소에는 플레이어 시야
        {
            elapsedTime += Time.deltaTime;

            if (owner.isLockOn)  // is LookOn
            {
                Vector3 lookTarget = owner.transform.position - owner.combat.target.transform.position;

                // 카메라 기준
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(lookTarget.z * position.y, 0, lookTarget.x * position.x).normalized * 0.8f // 좌,우 방향벡터
                    + new Vector3(0, position.x * position.y, 0) // 아래 방향벡터
                    + new Vector3(0, high, 0) + owner.transform.position // 상수 (사실아님)
                    , lerpSpace);
                
                transform.rotation = Quaternion.LookRotation( owner.combat.target.transform.position - transform.position);
                
            }
            else
            {
                // 플레이어 기준
                transform.position = Vector3.Lerp(transform.position,
                owner.transform.right * position.x * 1.6f // 좌,우 방향벡터
                + owner.transform.up * position.y * position.z // 아래 방향벡터
                + new Vector3(0, high, 0) + owner.transform.position // 상수 (사실아님)
                , lerpSpace);
                transform.rotation = owner.transform.rotation;
            }
            
            if (elapsedTime >= 1.2f)
            {
                StartCoroutine(ShootCoroutine(owner.combat.target));
                yield break;
            }
            
            yield return null;

        }
        
    }


    public IEnumerator Explode()
    {
        Debug.Log("ex");
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
        Debug.Log("MagicSword: OnTriggerEnter: " + other.name);
    
    if (!isExploded && other != owner )
    {
        StartCoroutine(Explode());
    }
    }
}
}
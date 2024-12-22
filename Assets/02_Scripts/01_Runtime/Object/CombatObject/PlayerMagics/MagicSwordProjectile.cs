using System;
using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.System;
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
    
    private BaseEntity owner;
    
    [SerializeField] private Vector3 startPosotion;
    [SerializeField] private Vector3 readyPosition;
    
    private bool isExploded;

    public void OnEnable()
    {
        // flightFx.Play();
        
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public IEnumerator ShootCoroutine( BaseEntity target )
    {
        float elapsedTime = 0f;
        float speed = 15;
            
        rigidbody.isKinematic = false;
        
        // PhysicUtility.IgnoreCollisionUtil(owner, collider); /* 기능안함 */
        
        /* 콜라이더들 활성화 */
        // collider.isTrigger = true;
        // collider.enabled = true; // use Colllier
        
        if (target != null) // 적 감지 시 추척하여 발사
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation((target.transform.position + new Vector3(0,target.transform.lossyScale.y * 1.2f,0) ) - transform.position), 360);
            
            while (true)
            {
                elapsedTime += Time.deltaTime;
                
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                        Quaternion.LookRotation((target.transform.position + new Vector3(0,target.transform.lossyScale.y * 1.2f,0) ) - transform.position), 60 * Time.deltaTime);
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
    
    
    public IEnumerator SetSwordPosition(BaseEntity _owner, BaseEntity target, Vector3 position)
    {
        
        owner = _owner;
        PlayerCombatHandler combat = owner.GetComponent<PlayerCombatHandler>();
        Player player = owner.GetComponent<Player>();

        float high = 2.1f;
        float lerpSpace = 0.5f;
        
        float elapsedTime = 0;

        while (true) // 타겟 있을 시 카메라 따라가기, 평소에는 플레이어 시야
        {
            elapsedTime += Time.deltaTime;
            
            if (player.isLockOn) // is LookOn
            {
                // 카메라 기준
                transform.position = Vector3.Lerp(transform.position,
                    owner.transform.position + owner.transform.right * position.x +
                    owner.transform.up * position.y +
                    new Vector3(0, high, 0), lerpSpace);
                transform.rotation = Quaternion.LookRotation(combat.target.transform.position - transform.position);
            }
            else
            {
                // 플레이어 기준
                transform.position = Vector3.Lerp(transform.position,
                    owner.transform.position + owner.transform.right * position.x +
                    owner.transform.up * position.y +
                    new Vector3(0, high, 0), lerpSpace);
                transform.rotation = owner.transform.rotation;
            }
            
            if (elapsedTime >= 1.2f)
            {
                StartCoroutine(ShootCoroutine(target));
                yield break;
            }
            
            yield return null;

        }
        
    }


    public IEnumerator Explode()
    {
        
        if (!isExploded)
        {
            isExploded = true;
            explosionDamageCollider.gameObject.SetActive(true);

            // flightFx.Stop();
            // explosionFx.Play();

            rigidbody.velocity = Vector3.zero;
            
            Destroy(gameObject, explosionFx.main.duration);

            yield break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("MagicSword: OnTriggerEnter: " + other);
    //     
    //     if (!isExploded && other != owner )
    //     {
    //         StartCoroutine(Explode());
    //     }
    }
}
}
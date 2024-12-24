using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Quaternion = System.Numerics.Quaternion;

namespace MinD.Runtime.Object.Magics
{

public class LazerProjectile : MonoBehaviour // LazerProjectile
{
    private const float flightTime = 3;
    
    [SerializeField] private GameObject useFx;
    [SerializeField] private ParticleSystem damageFx;
    
    [Header("[ Status ]")] [SerializeField]
    private float lazerSpeed = 30;

    private Player castPlayer;
    private Vector3 targetDirx;

    private float _flightTime;

    [SerializeField] private Rigidbody rigidbody;

    /* ToDo :: 이팩트 수정시키기 */
    
    public void SetPlayer(Player player)
    {
        castPlayer = player;
    }

    public void ShootCommonMagic(Vector3 targetDirx)
    {
        this.targetDirx = targetDirx;
        Debug.Log(targetDirx);
        transform.rotation = UnityEngine.Quaternion.LookRotation(targetDirx - transform.position);
    }

    public void ShootCommonMagic()
    {
        targetDirx = castPlayer.transform.forward;
    }

    public void Update()
    {
        rigidbody.velocity = transform.forward * lazerSpeed;

        _flightTime += Time.deltaTime;
        if (flightTime < _flightTime)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Debug.Log("ex");
        useFx.SetActive(false);
        damageFx.Play();

        rigidbody.isKinematic = true;
        Destroy(gameObject,1);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        Explode();
    }
}
}

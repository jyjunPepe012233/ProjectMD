using System;
using System.Collections.Generic;
using MinD.Runtime.Object.Magics;
using UnityEngine;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Magic Sword")]
public class MagicSword : Magic
{
    
    
    [SerializeField] private GameObject magicSword;

    private List<GameObject> projectiles; // 발사체 저장 리스트
    private List<MagicSwordProjectile> swordProjectiles; // 발사체들 MagicSwordProjectile

    public static readonly Vector3[] projectilePositions = new Vector3[3]
    {
        new Vector3(-1, 4, 0).normalized * 2.5f,
        new Vector3(0, 2.7f, 0),
        new Vector3(1, 4, 0).normalized * 2.5f
    };
    

    public override void OnUse()
    {
        projectiles = new List<GameObject>();
        swordProjectiles = new List<MagicSwordProjectile>();
        
        Debug.Log("Use MagicSword");
        for (int i = 0; i < 3; i++)
        {
            projectiles.Add(Instantiate(magicSword, castPlayer.transform.position + projectilePositions[i], castPlayer.transform.rotation));
            swordProjectiles.Add(projectiles[i].GetComponent<MagicSwordProjectile>());
            Debug.Log("create MagicSword");
        
            // MagicSwordProjectile sword = swordProjectiles[i];
            // sword.StartCoroutine(sword.SetSwordPosition(castPlayer,projectilePositions[i]));
            // Debug.Log("set MagicSword");
        }
        
        Debug.Log($"projectiles.Count: {projectiles.Count}");

        
        // for (int i = 0; i < projectiles.Count; i++)
        // {
        //     MagicSwordProjectile sword = projectiles[i].GetComponent<MagicSwordProjectile>();
        //     sword.StartCoroutine(sword.SetSwordPosition(castPlayer,projectilePositions[i]));
        // }
        
        
        // /*딜레이 만들기*/
        //
        // for (int i = 0; i < projectiles.Count; i++)
        // {
        //     MagicSwordProjectile sword = swordProjectiles[i];
        //     sword.StartCoroutine(sword.ShootCoroutine(castPlayer.combat.target));
        // }
        
    }

    public override void Tick()
    {
        
    }

    public override void OnReleaseInput()
    {
        
    }

    public override void OnCancel()
    {
        for (int i = 0; i < 3; i++)
        {
            MagicSwordProjectile sword = swordProjectiles[i];
            sword.StartCoroutine(sword.Explode());
        }
    }

    public override void OnExit()
    {
        
    }
}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public PlayerManager playerManager;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    public void Attack()
    {
        playerManager.Die();
    }
}

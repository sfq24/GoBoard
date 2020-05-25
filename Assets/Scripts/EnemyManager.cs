using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
public class EnemyManager : TurnManager
{
    EnemySensor m_enemySensor;
    EnemyMover m_enemyMover;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        m_enemyMover = GetComponent<EnemyMover>();
        m_enemySensor = GetComponent<EnemySensor>();
    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        m_enemySensor.SenseNode();

        //attach player

        //move

        yield return new WaitForSeconds(0.5f);
        m_enemyMover.Stand();


    }
}

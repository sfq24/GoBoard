using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : Mover
{
    public float standTime = 1f;
    protected override void Awake()
    {
        base.Awake();
        rotation = true;
    }

    protected override void Start()
    {
        base.Start();
        //StartCoroutine(EnemyMove());
    }

    IEnumerator EnemyMove()
    {
        yield return null;

    }

    public void Stand()
    {
        StartCoroutine(StandRoutine());
    }

    IEnumerator StandRoutine()
    {
        yield return new WaitForSeconds(standTime);
        base.FinishMoveEvent.Invoke();
    }

}

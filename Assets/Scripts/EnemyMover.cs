using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Patrol,
    Stationary,
    Spin
}

public class EnemyMover : Mover
{
    public Vector3 dirctionToMove = new Vector3(0, 0, Board.spacing);
    public MovementType movementType = MovementType.Stationary;

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
    public void MoveOneTurn()
    {
        switch (movementType)
        {
            case MovementType.Patrol:
                Patrol();
                break;
            case MovementType.Stationary:
                Stand();
                break;
            case MovementType.Spin:
                Spin();
                break;
        }
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

    public void Patrol()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        Vector3 startPos = new Vector3(CurrentNode.Coordinate.x, 0f, CurrentNode.Coordinate.y);
        Vector3 newDest = startPos + transform.TransformVector(dirctionToMove);
        Vector3 nextDest = startPos + transform.TransformVector(dirctionToMove * 2);

        Move(newDest, 0f);

        while (isMoving)
        {
            yield return null;
        }

        if(board != null)
        {
            Node newNode = board.TargetNode(newDest);
            Node nextNode = board.TargetNode(nextDest);

            if(nextNode == null || !newNode.NeighborNodes.Contains(nextNode))
            {
                destination = startPos;
                FaceDestination();
                yield return new WaitForSeconds(rotateTime);

            }

        }
        base.FinishMoveEvent.Invoke();
    }

    public void Spin()
    {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        Vector3 dir = new Vector3(0, 0, Board.spacing);
        destination = transform.TransformVector(dir * -1) + transform.position;

        FaceDestination();

        yield return new WaitForSeconds(rotateTime);
        base.FinishMoveEvent.Invoke();
    }
}

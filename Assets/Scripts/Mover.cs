using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public Vector3 destination;
    public bool isMoving = false;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

    public float moveSpeed = 1.5f;
    public float delay = 0f;
    private Board board;
    protected virtual void Awake()
    {
        board = FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void Move(Vector3 destinationPos, float delayTime = 0.25f)
    {
        if (board.targetNode(destinationPos) != null && board.PlayerNode.LinkedNodes.Contains(board.targetNode(destinationPos)))
        {
            StartCoroutine(MoveRoutine(destinationPos, delayTime));
        }
    }
    IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        isMoving = true;
        destination = destinationPos;

        yield return new WaitForSeconds(delayTime);
        iTween.MoveTo(gameObject, iTween.Hash("x", destinationPos.x, "y", destinationPos.y, "z", destinationPos.z, "delay", delay, "easetype", easeType, "speed", moveSpeed));

        //wait for itween movement
        while (Vector3.Distance(destinationPos, transform.position) > 0.01f)   //check if almost moved to destination
        {
            yield return null;
        }

        iTween.Stop(gameObject);
        transform.position = destinationPos;
        isMoving = false;

        board.UpdatePlayerNode();
    }

    public void MoveLeft()
    {
        Vector3 newPosition = transform.position + new Vector3(-Board.spacing, 0, 0);
        Move(newPosition, 0);
    }
    public void MoveRight()
    {
        Vector3 newPosition = transform.position + new Vector3(Board.spacing, 0, 0);
        Move(newPosition, 0);
    }

    public void MoveUp()
    {
        Vector3 newPosition = transform.position + new Vector3(0, 0, Board.spacing);
        Move(newPosition, 0);
    }

    public void MoveDown()
    {
        Vector3 newPosition = transform.position + new Vector3(0, 0, -Board.spacing);
        Move(newPosition, 0);
    }
}

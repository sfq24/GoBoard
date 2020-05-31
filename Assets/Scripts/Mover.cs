using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class Mover : MonoBehaviour
{

    public Vector3 destination;
    public bool isMoving = false;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

    public float moveSpeed = 1.5f;
    public float delay = 0f;
    public float rotateTime = 0.5f;
    public bool rotation = false;
    protected Board board;
    public UnityEvent FinishMoveEvent;

    private Node currentNode;
    public Node CurrentNode { get => currentNode; }

    protected virtual void Awake()
    {
        board = FindObjectOfType<Board>().GetComponent<Board>();
    }

    protected virtual void Start()
    {
        UpdateCurrentNode();
    }

    private void UpdateCurrentNode()
    {
        if(board != null)
        {
            currentNode = board.TargetNode(transform.position);
        }
    }
    public void Move(Vector3 destinationPos, float delayTime = 0.25f)
    {
        if (isMoving)
        {
            return;
        }
        if (board.TargetNode(destinationPos) != null && CurrentNode!=null)
        {
            if (CurrentNode.LinkedNodes.Contains(board.TargetNode(destinationPos)))
            {

                StartCoroutine(MoveRoutine(destinationPos, delayTime));
            }
        }
        else
        {
            Debug.Log("board or Current node is null");
        }
    }
    IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        isMoving = true;
        destination = destinationPos;

        if (rotation)
        {
            FaceDestination();
            yield return new WaitForSeconds(0.25f);
        }
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
        UpdateCurrentNode();

        FinishMoveEvent.Invoke();
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

    protected void FaceDestination()
    {
        Vector3 dir = destination - transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);

        float newY = rot.eulerAngles.y;

        iTween.RotateTo(gameObject, iTween.Hash("y", newY, "delay", 0, "time", rotateTime, "easetype", easeType));
    }
}

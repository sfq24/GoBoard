using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public Vector3 destination;
    public bool isMoving = false;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

    public float moveSpeed = 1.5f;
    public float delay = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Move(new Vector3(2f, 0, 0), 1f);
        Move(new Vector3(4f, 0, 0), 3f);
        Move(new Vector3(4f, 0, 2f), 5f);
        Move(new Vector3(4f, 0, 4f), 7f);
    }

    public void Move(Vector3 destinationPos, float delayTime = 0.25f)
    {
        StartCoroutine(MoveRoutine(destinationPos, delayTime));
    }

    IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        isMoving = true;
        destination = destinationPos;

        yield return new WaitForSeconds(delayTime);
        iTween.MoveTo(gameObject, iTween.Hash("x", destinationPos.x, "y", destinationPos.y, "z", destinationPos.z, "delay", delay, "easetype", easeType, "speed", moveSpeed));

        //wait for itween movement
        while (Vector3.Distance(destinationPos,transform.position) > 0.01f)   //check if almost moved to destination
        {
            yield return null;
        }

        iTween.Stop(gameObject);
        transform.position = destinationPos;
        isMoving = false;
    }

}

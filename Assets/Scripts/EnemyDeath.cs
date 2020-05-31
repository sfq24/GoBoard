using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemyDeath : MonoBehaviour
{
    public Vector3 offscreenOffset = new Vector3(0, 10, 0);
    Board board;

    public float deathDelay = 0f;
    public float offscreenDelay = 1f;

    public float moveTime = 0.5f;

    private void Awake()
    {
        board = FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void MoveOffScreen(Vector3 target)
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "x",target.x,
            "y",target.y,
            "z",target.z,
            "delay",0f,
            "easetype",iTween.EaseType.easeInOutQuint,
            "time",moveTime));
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(deathDelay);
        Vector3 offscreenPos = transform.position + offscreenOffset;

        MoveOffScreen(offscreenPos);

        yield return new WaitForSeconds(moveTime + 0.2f);

        if(board.CapturePositions.Count != 0 && board.CurrentCapturePostion <= board.CapturePositions.Count)
        {
            Vector3 capturePos = board.CapturePositions[board.CurrentCapturePostion].position;
            //move to the upside of capturePos
            transform.position = capturePos + offscreenOffset;

            MoveOffScreen(capturePos);
            yield return new WaitForSeconds(moveTime);

            board.CurrentCapturePostion++;
            board.CurrentCapturePostion = Mathf.Clamp(board.CurrentCapturePostion, 0, board.CapturePositions.Count - 1);
        }
    }
}

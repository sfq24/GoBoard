using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyManager : TurnManager
{
    private EnemySensor m_enemySensor;
    private EnemyMover m_enemyMover;
    private EnemyAttack m_enemyAttack;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        m_enemyMover = GetComponent<EnemyMover>();
        m_enemySensor = GetComponent<EnemySensor>();
        m_enemyAttack = GetComponent<EnemyAttack>();
    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    private IEnumerator PlayTurnRoutine()
    {
        if (m_GameManager != null && !m_GameManager.IsGameOver)
        {
            m_enemySensor.SenseNode();

            yield return new WaitForSeconds(0.0f);
            if (m_enemySensor.FindPlayer)
            {
                m_GameManager.LoseLevel();
                Vector3 playPos = new Vector3(m_board.PlayerNode.Coordinate.x, 0f, m_board.PlayerNode.Coordinate.y);
                m_enemyMover.Move(playPos, 0f);

                while (m_enemyMover.isMoving)
                {
                    yield return null;
                }
                //attach player
                m_enemyAttack.Attack();
            }
            else
            {
                //move
                m_enemyMover.MoveOneTurn();
            }
        }
    }
}
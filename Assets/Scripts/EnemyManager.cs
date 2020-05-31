using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(EnemyDeath))]
public class EnemyManager : TurnManager
{
    private EnemySensor m_enemySensor;
    private EnemyMover m_enemyMover;
    private EnemyAttack m_enemyAttack;
    private EnemyDeath m_enemyDeath;
    public bool IsDead { get; set; } = false;
    public UnityEvent deathEvent;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        m_enemyMover = GetComponent<EnemyMover>();
        m_enemySensor = GetComponent<EnemySensor>();
        m_enemyAttack = GetComponent<EnemyAttack>();
        m_enemyDeath = GetComponent<EnemyDeath>();
    }

    public void PlayTurn()
    {
        if (IsDead)
        {
            FinishTurn();        //Is listening to FinishTurnEvent. Usually Invoked at end of movement
            return;
        }
        StartCoroutine(PlayTurnRoutine());
    }

    private IEnumerator PlayTurnRoutine()
    {
        if (m_GameManager != null && !m_GameManager.IsGameOver)
        {
            m_enemySensor.SenseNode(m_enemyMover.CurrentNode);

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

    public void Die()
    {
        if (IsDead)
        {
            return;
        }
        IsDead = true;
        if(deathEvent != null)
        {
            deathEvent.Invoke();
        }
    }
}
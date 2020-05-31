using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerDeath))]
public class PlayerManager : TurnManager
{
    public PlayerMover PlayerMover;
    public PlayerInput PlayerInput;

    public UnityEvent PlayerDieEvent;

    protected override void Awake()
    {
        base.Awake();
        PlayerInput = GetComponent<PlayerInput>();
        PlayerMover = GetComponent<PlayerMover>();
        PlayerInput.inputEnabled = true;
    }

    private void Update()
    {
        if (PlayerMover.isMoving || m_GameManager.CurrentTurn != Turn.Player)
        {
            return;
        }
        PlayerInput.GetKeyInput();
        //Debug.Log("H: " + PlayerInput.H + "V: " + PlayerInput.V);
        if (PlayerInput.H == 0)
        {
            if (PlayerInput.V > 0)
            {
                PlayerMover.MoveUp();
            }
            else if (PlayerInput.V < 0)
            {
                PlayerMover.MoveDown();
            }
        }
        else if (PlayerInput.V == 0)
        {
            if (PlayerInput.H > 0)
            {
                PlayerMover.MoveRight();
            }
            else if (PlayerInput.H < 0)
            {
                PlayerMover.MoveLeft();
            }
        }
    }

    public void Die()
    {
        PlayerDieEvent.Invoke();
    }

    private void CaptureEnemies()
    {
        List<EnemyManager> enemies = m_board.FindEnemyAtNode(m_board.PlayerNode);

        if (enemies.Count > 0)
        {
            foreach (EnemyManager enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.Die();
                }
            }
        }
    }

    public override void FinishTurn()
    {
        CaptureEnemies();
        base.FinishTurn();
    }
}
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMover))]
public class PlayerManager : TurnManager
{
    public PlayerMover PlayerMover;
    public PlayerInput PlayerInput;

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
}
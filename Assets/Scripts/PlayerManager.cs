using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMover))]
public class PlayerManager : MonoBehaviour
{
    public PlayerMover PlayerMover;
    public PlayerInput PlayerInput;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        PlayerMover = GetComponent<PlayerMover>();
        PlayerInput.inputEnabled = true;
    }

    private void Update()
    {
        if (PlayerMover.isMoving)
        {
            return;
        }
        PlayerInput.GetKeyInput();
        Debug.Log("H: " + PlayerInput.H + "V: " + PlayerInput.V);
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
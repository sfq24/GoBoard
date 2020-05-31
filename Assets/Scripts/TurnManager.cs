using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    protected GameManager m_GameManager;
    protected Board m_board;
    public bool IsTurnComplete { get; set; } = false;

    protected virtual void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
    }

    public virtual void FinishTurn()
    {
        IsTurnComplete = true;

        if(m_GameManager != null)
        {
            m_GameManager.UpdateTurn();
        }
    }
}

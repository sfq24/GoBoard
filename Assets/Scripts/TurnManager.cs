using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    protected GameManager m_GameManager;

    public bool IsTurnComplete { get; set; } = false;

    protected virtual void Awake()
    {
        m_GameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();

    }

    public void FinishTurn()
    {
        IsTurnComplete = true;

        if(m_GameManager != null)
        {
            m_GameManager.UpdateTurn();
        }
    }
}

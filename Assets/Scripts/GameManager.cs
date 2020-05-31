using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Turn
{
    Player,
    Enermy
}

public class GameManager : MonoBehaviour
{
    private Board m_board;
    private PlayerManager m_playerManager;

    private bool m_hasGameStarted = false;
    public bool HasGameStarted { get => m_hasGameStarted; set => m_hasGameStarted = value; }

    private bool m_isGamePlaying = false;
    public bool IsGamePlaying { get => m_isGamePlaying; set => m_isGamePlaying = value; }

    private bool m_isGameOver = false;
    public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }
    public bool HasLevelFinished { get => m_hasLevelFinished; set => m_hasLevelFinished = value; }

    private bool m_hasLevelFinished = false;

    public float delay = 3f;

    private List<EnemyManager> enemyManagers;
    public Turn CurrentTurn { get; set; } = Turn.Player;

    public UnityEvent setUpEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;
    public UnityEvent loseLevelEvent;

    private void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
        m_playerManager = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        enemyManagers = (FindObjectsOfType<EnemyManager>() as EnemyManager[]).ToList();
    }

    private void Start()
    {
        if (m_board != null && m_playerManager != null)
        {
            StartCoroutine(RunGameLoop());
        }
    }

    private IEnumerator RunGameLoop()
    {
        yield return StartCoroutine(StartLevelRoutine());
        yield return StartCoroutine(PlayLevelRoutine());
        yield return StartCoroutine(EndLevelRoutine());
    }

    private IEnumerator StartLevelRoutine()
    {
        Debug.Log("Setup Level");
        if (setUpEvent != null)
        {
            setUpEvent.Invoke();
        }

        Debug.Log("Start Level");
        m_playerManager.PlayerInput.inputEnabled = false;

        while (!m_hasGameStarted)
        {
            //HasLevelStarted = true;
            yield return null;
        }

        if (startLevelEvent != null) { startLevelEvent.Invoke(); }
    }

    private IEnumerator PlayLevelRoutine()
    {
        Debug.Log("Play Level");
        m_isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        m_playerManager.PlayerInput.inputEnabled = true;

        if (playLevelEvent != null) { playLevelEvent.Invoke(); }

        while (!m_isGameOver)
        {
            yield return null;
            //check condition win or lose
            m_isGameOver = IsWinner();
        }
        Debug.Log("Win!");
    }

    public void LoseLevel()
    {
        StartCoroutine(LoseLevelRoutine());
    }

    private IEnumerator LoseLevelRoutine()
    {
        m_isGameOver = true;

        yield return new WaitForSeconds(1.5f);

        if (loseLevelEvent != null)
        {
            loseLevelEvent.Invoke();
        }
        yield return new WaitForSeconds(2f);

        Debug.Log("LOSE! ================================");

        RestartLevel();
    }

    private IEnumerator EndLevelRoutine()
    {
        Debug.Log("End Level");
        m_playerManager.PlayerInput.inputEnabled = false;
        if (endLevelEvent != null) { endLevelEvent.Invoke(); }

        while (!m_hasLevelFinished)
        {
            //player press button

            yield return null;
        }

        RestartLevel();
    }

    private void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel()
    {
        m_hasGameStarted = true;
    }

    private bool IsWinner()
    {
        if (m_board.PlayerNode != null)
        {
            return m_board.PlayerNode == m_board.GoalNode;
        }
        return false;
    }

    private void PlayPlayerTurn()
    {
        m_playerManager.IsTurnComplete = false;
        CurrentTurn = Turn.Player;
    }

    private void PlayEnemyTurn()
    {
        CurrentTurn = Turn.Enermy;
        foreach (EnemyManager enemy in enemyManagers)
        {
            if (enemy != null && !enemy.IsDead)
            {
                enemy.IsTurnComplete = false;

                //play Enemy turn
                enemy.PlayTurn();
            }
        }
    }

    private bool EnemyFinishedTurn()
    {
        foreach (EnemyManager enemy in enemyManagers)
        {
            if (enemy.IsDead) { continue; }
            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }
        return true;
    }

    bool AreEnemiesAllDead()
    {
        foreach (EnemyManager enemy in enemyManagers)
        {
            if (!enemy.IsDead)
            {
                return false;
            }
        }
        return true;
    }
    public void UpdateTurn()
    {
        if (m_playerManager != null && CurrentTurn == Turn.Player)
        {
            if (m_playerManager.IsTurnComplete && !AreEnemiesAllDead())
            {
                PlayEnemyTurn();
            }
        }
        else if (CurrentTurn == Turn.Enermy)
        {
            if (EnemyFinishedTurn())
            {
                PlayPlayerTurn();
            }
        }
    }
}
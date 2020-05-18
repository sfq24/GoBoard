using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

    public UnityEvent setUpEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;

    private void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
        m_playerManager = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
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
        if(setUpEvent != null)
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

    bool IsWinner()
    {
        if(m_board.PlayerNode != null)
        {
            return m_board.PlayerNode == m_board.GoalNode;
        }
        return false;
    }
}
﻿using System;
using System.Collections;
using UnityEngine;
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


    public float delay = 1f;
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
        Debug.Log("Start Level");
        m_playerManager.PlayerInput.inputEnabled = false;

        while (!m_hasGameStarted)
        {

            //HasLevelStarted = true;
            yield return null;
        }
    }

    private IEnumerator PlayLevelRoutine()
    {
        Debug.Log("Play Level");
        m_isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        m_playerManager.PlayerInput.inputEnabled = true;
        while (!m_isGameOver)
        {
            //check condition win or lose
            yield return null;
        }
    }

    private IEnumerator EndLevelRoutine()
    {
        Debug.Log("End Level");
        m_playerManager.PlayerInput.inputEnabled = false;

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
}
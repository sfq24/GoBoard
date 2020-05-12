using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private GameObject geometry;

    private Vector2 m_coordinate;
    public Vector2 Coordinate { get { return Util.Vector2Round(m_coordinate); } }

    public float scaleTime = 0.3f;

    public iTween.EaseType easeType = iTween.EaseType.easeInExpo;

    public bool autoRun;

    public float delay = 1f;
    private bool initialized = false;
    private Board board;

    private List<Node> m_neighborNodes = new List<Node>();
    public List<Node> NeighborNodes { get { return m_neighborNodes; } }

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        m_coordinate = new Vector2(transform.position.x, transform.position.z);
    }

    private void Start()
    {
        if (geometry != null)
        {
            geometry.transform.localScale = Vector3.zero;
        }
        if (autoRun)
        {
            InitNode();
        }
        if (board != null)
        {
            m_neighborNodes = FindNeighborNodes();
        }
    }

    private void ShowGeometry()
    {
        if (geometry != null)
        {
            iTween.ScaleTo(geometry, iTween.Hash("time", scaleTime, "scale", Vector3.one, "easetype", easeType, "delay", delay));
        }
    }

    private List<Node> FindNeighborNodes()
    {
        List<Node> nNodes = new List<Node>();
        foreach (var dir in Board.directions)
        {
            Node node = board.AllNode.Find(n => n.Coordinate == Coordinate + dir);
            if (node != null && !nNodes.Contains(node))
            {
                nNodes.Add(node);
            }
        }

        return nNodes;
    }

    public void InitNode()
    {
        if (!initialized)
        {
            ShowGeometry();
            InitNeighbors();
            initialized = true;
        }

    }

    private void InitNeighbors()
    {
        StartCoroutine(ShowNeighbors());
    }

    IEnumerator ShowNeighbors()
    {
        yield return new WaitForSeconds(delay);
        foreach(var neighbor in NeighborNodes)
        {
            neighbor.InitNode();
        }
    }
}
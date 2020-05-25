using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static float spacing = 2f;

    public static readonly Vector2[] directions =
    {
        new Vector2(spacing,0f),
        new Vector2(-spacing, 0f),
        new Vector2(0f, spacing),
        new Vector2(0f,-spacing)
    };

    private List<Node> m_allNodes = new List<Node>();
    public List<Node> AllNode { get { return m_allNodes; } }
    private Node m_playerNode;
    public Node PlayerNode { get { return m_playerNode; } }

    public Node GoalNode { get => m_goalNode; set => m_goalNode = value; }

    private Node m_goalNode;

    public GameObject goalPrefab;

    private PlayerMover playerMover;

    public float drawGoalTime = 2f;
    public float drawGoalDelay = 2f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutQuad;

    private void Awake()
    {
        GetAllNodes();
        m_goalNode = FindGoalNode();
    }

    Node FindGoalNode()
    {
        if(goalPrefab != null)
        {
            return m_allNodes.Find(n => n.isLevelGoal);
        }
        return null;
    }

    private void Start()
    {
        UpdatePlayerNode();

    }

    private void GetAllNodes()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        m_allNodes = new List<Node>(nodes);
    }

    public Node TargetNode(Vector3 targetPos)
    {
        Vector2 cood = new Vector2(targetPos.x, targetPos.z);
        return m_allNodes.Find(n => n.Coordinate == cood);
    }

    private Node FindPlayerNode()
    {
        playerMover = FindObjectOfType<PlayerMover>();
        if (playerMover != null && playerMover.isMoving == false)
        {
            return TargetNode(playerMover.transform.position);
        }
        return null;
    }

    public void UpdatePlayerNode()
    {
        if (FindPlayerNode() != null)
        {
            m_playerNode = FindPlayerNode();
        }
    }

    private void OnDrawGizmos()
    {
        if (m_playerNode != null)
        {
            Gizmos.color = new Color(0, 1, 1, 0.5f);
            Gizmos.DrawSphere(m_playerNode.transform.position, 0.5f);
        }
    }

    public void DrawGoal()
    {
        if(goalPrefab != null && m_goalNode != null)
        {
            GameObject goalobj = Instantiate(goalPrefab, m_goalNode.transform.position, Quaternion.identity);
            iTween.ScaleFrom(goalobj, iTween.Hash("time", drawGoalTime, "delay", drawGoalDelay, "scale", Vector3.zero, "easetype", easeType));
        }
    }

    public void InitBoard()
    {
        if (m_playerNode != null)
        {
            m_playerNode.InitNode();
        }
    }
}
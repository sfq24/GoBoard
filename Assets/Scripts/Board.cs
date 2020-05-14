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

    private PlayerMover playerMover;

    private void Awake()
    {
        GetAllNodes();
    }

    private void Start()
    {
        UpdatePlayerNode();
        if(m_playerNode != null)
        {
            m_playerNode.InitNode();
        }
    }

    private void GetAllNodes()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        m_allNodes = new List<Node>(nodes);
    }

    public Node targetNode(Vector3 targetPos)
    {
        Vector2 cood = new Vector2(targetPos.x, targetPos.z);
        return m_allNodes.Find(n => n.Coordinate == cood);
    }

    private Node FindPlayerNode()
    {
        playerMover = FindObjectOfType<PlayerMover>();
        if (playerMover != null && playerMover.isMoving == false)
        {
            return targetNode(playerMover.transform.position);
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
}
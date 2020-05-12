using System.Collections;
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
    public List<Node>  AllNode { get { return m_allNodes; } }
    private void Awake()
    {
        GetAllNodes();
    }
    private void GetAllNodes()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        m_allNodes = new List<Node>(nodes);
    }
}

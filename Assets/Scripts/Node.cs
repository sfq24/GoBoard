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
    public Link linkPrefab;

    public float delay = 1f;
    private bool initialized = false;
    private Board board;
    public LayerMask blockLayer;
    public bool isLevelGoal = false;

    private List<Node> m_neighborNodes = new List<Node>();
    public List<Node> NeighborNodes { get { return m_neighborNodes; } }

    private List<Node> m_linkedNodes = new List<Node>();
    public List<Node> LinkedNodes { get { return m_linkedNodes; } }

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

    private IEnumerator ShowNeighbors()
    {
        yield return new WaitForSeconds(delay);
        foreach (var neighbor in NeighborNodes)
        {
            if (!m_linkedNodes.Contains(neighbor))
            {
                if (FindBlock(neighbor) == null)
                {
                    DrawNeighborLink(neighbor);
                    neighbor.InitNode();
                }
            }
        }
    }

    private void DrawNeighborLink(Node target)
    {
        if (linkPrefab != null)
        {
            Link linkObj = Instantiate(linkPrefab, transform.position, Quaternion.identity);
            linkObj.transform.parent = transform;
            if (linkObj != null)
            {
                linkObj.DrawLink(transform.position, target.transform.position);
                if (!m_linkedNodes.Contains(target))
                {
                    m_linkedNodes.Add(target);
                }
                if (!target.LinkedNodes.Contains(this))
                {
                    target.LinkedNodes.Add(this);
                }
            }
        }
    }

    private Block FindBlock(Node target)
    {
        Vector3 dir = target.transform.position - transform.position;

        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, dir, out raycastHit, Board.spacing + 0.1f, blockLayer))
        {
            Debug.Log("Find block from " + this.name + " to : " + target.name);
            return raycastHit.collider.GetComponent<Block>();
        }
        return null;
    }
}
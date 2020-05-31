 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class EnemySensor : MonoBehaviour
{
    Vector3 SensingPostion = new Vector3(0, 0, 2f);
    Board m_board;
    bool m_findPlayer = false;
    public bool FindPlayer { get => m_findPlayer; }
    Node nodeToSearch;
    private void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();

    }

    public void SenseNode(Node currNode)
    {
        Vector3 worldspacePostion = transform.TransformVector(SensingPostion) + transform.position;
        if(m_board != null)
        {
            nodeToSearch = m_board.TargetNode(worldspacePostion);

            if (!currNode.LinkedNodes.Contains(nodeToSearch))
            {
                m_findPlayer = false;
                return;
            }

            if(nodeToSearch == m_board.PlayerNode)
            {
                m_findPlayer = true;
            }
        }
    }
}

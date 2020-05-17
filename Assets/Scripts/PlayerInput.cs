using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float m_h;
    private float m_v;
    public float H { get { return m_h; } }
    public float V { get { return m_v; } }

    public bool inputEnabled { get; set; }

    public void GetKeyInput()
    {
        if (inputEnabled)
        {
            m_h = Input.GetAxisRaw("Horizontal");
            m_v = Input.GetAxisRaw("Vertical");
        }
        else
        {
            m_h = 0;
            m_v = 0;
        }

    }
}

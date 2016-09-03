using UnityEngine;
using System.Collections;

public class CameraPanner : MonoBehaviour
{
    public float PanningSpeed;

    private Vector3 m_Motion;
    private bool m_Smooth;
    private bool m_AddedMotion;

    void Start()
    {
        m_Motion = Vector3.zero;
        m_Smooth = false;
        m_AddedMotion = false;
    }

    void Update()
    {
        transform.position += m_Motion;

        if (m_Smooth)
        {
            m_Motion *= 0.9f;
        }
        else if (!m_AddedMotion)
        {
            m_Motion = Vector3.zero;
        }
        m_AddedMotion = false;
    }

    public void AddSmoothMotion(Vector2 delta)
    {
        m_AddedMotion = true;
        m_Smooth = true;
        Vector3 motion = transform.TransformDirection(new Vector3(-delta.x, -delta.y, 0.0f));
        motion.y = 0.0f;
        motion *= Time.deltaTime * PanningSpeed;
        m_Motion = Vector3.Lerp(m_Motion, m_Motion + motion, 16.0f * Time.deltaTime);
    }

    public void SetMotion(Vector3 motion)
    {
        m_AddedMotion = true;
        m_Smooth = false;
        motion = transform.TransformDirection(new Vector3(-motion.x, -motion.y, 0.0f));
        motion.y = 0.0f;
        motion *= Time.deltaTime * PanningSpeed;
        m_Motion = motion;
    }
}

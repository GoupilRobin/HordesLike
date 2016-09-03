using UnityEngine;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    public float screenPercentPanning = 0.05f;
    public float mousePanningSpeedMultiplier = 350.0f;
    public float keyboardPanningSpeedMultiplier = 350.0f;

    private Touch m_LastTouch;
    private bool m_IsPanning;
    private CameraPanner m_CameraPanner;

    void Start()
    {
        m_LastTouch = default(Touch);
        m_IsPanning = false;
        m_CameraPanner = GetComponent<CameraPanner>();
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            UpdateTouchPanning();
        }
        else
        {
            UpdateMousePanning();
            UpdateKeyboardPanning();
        }
    }

    private void UpdateTouchPanning()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (m_LastTouch.fingerId == default(Touch).fingerId || touch.fingerId != m_LastTouch.fingerId)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    m_IsPanning = true;
                }
            }
            if (m_IsPanning && touch.deltaPosition.sqrMagnitude > 0.01f)
            {
                m_CameraPanner.AddSmoothMotion(touch.deltaPosition);
            }
            m_LastTouch = touch;
        }
        else
        {
            m_IsPanning = false;
        }
    }

    private void UpdateMousePanning()
    {
        Vector3 normMousePosition = new Vector3();
        normMousePosition.x = Mathf.Clamp01(Input.mousePosition.x / Screen.width) - 0.5f;
        normMousePosition.y = Mathf.Clamp01(Input.mousePosition.y / Screen.height) - 0.5f;

        Vector3 absMousePosition = new Vector3(0.5f - Mathf.Abs(normMousePosition.x), 0.5f - Mathf.Abs(normMousePosition.y), 0.0f);
        if (absMousePosition.x < screenPercentPanning || absMousePosition.y < screenPercentPanning)
        {
            Vector2 panningMotion = new Vector2();
            panningMotion.x = Mathf.Clamp01(1.0f - (absMousePosition.x / screenPercentPanning)) * Mathf.Sign(-normMousePosition.x);
            panningMotion.y = Mathf.Clamp01(1.0f - (absMousePosition.y / screenPercentPanning)) * Mathf.Sign(-normMousePosition.y);
            panningMotion.x = panningMotion.x * panningMotion.x * Mathf.Sign(panningMotion.x);
            panningMotion.y = panningMotion.y * panningMotion.y * Mathf.Sign(panningMotion.y);
            m_CameraPanner.SetMotion(panningMotion.normalized * mousePanningSpeedMultiplier);
        }
    }

    private void UpdateKeyboardPanning()
    {
        Vector2 panningMotion = new Vector2(-Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));
        if (panningMotion.sqrMagnitude > 0.01f)
        {
            m_CameraPanner.SetMotion(panningMotion.normalized * keyboardPanningSpeedMultiplier);
        }
    }
}

using UnityEngine;
using System.Collections;

public class FireLight : MonoBehaviour
{
    public Vector3 MovementAmplitude;
    public float MovementLerpSeed;
    public float IntensityAmplitude;
    public float IntensityLerpSeed;

    private Vector3 m_OriginLocalPosition;
    private float m_OriginIntensity;
    private Light m_Light;

    void Start()
    {
        m_Light = GetComponent<Light>();
        m_OriginLocalPosition = transform.localPosition;
        if (m_Light != null)
        {
            m_OriginIntensity = m_Light.intensity;
        }
    }

    void Update()
    {
        Vector3 targetPos = Vector3.Scale(Random.insideUnitSphere, MovementAmplitude);
        targetPos.x /= transform.lossyScale.x;
        targetPos.y /= transform.lossyScale.y;
        targetPos.z /= transform.lossyScale.z;
        transform.localPosition = Vector3.Lerp(transform.localPosition, m_OriginLocalPosition + targetPos, MovementLerpSeed * Time.deltaTime);

        if (m_Light != null)
        {
            float targetIntensity = m_OriginIntensity + ((Random.value - 0.5f) * IntensityAmplitude);
            m_Light.intensity = Mathf.Lerp(m_Light.intensity, targetIntensity, IntensityLerpSeed * Time.deltaTime);
        }
    }
}

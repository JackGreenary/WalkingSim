using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float power = 0.7f;
    public float duration = 1.0f;
    public Transform camera;
    public float slowDownAmount = 1.0f;
    public bool shouldShake = false;

    private Vector3 m_StartPosition;
    private float m_InitialDuration;

    void Start()
    {
        camera = Camera.main.transform;
        m_StartPosition = camera.localPosition;
        m_InitialDuration = duration;
    }

    void Update()
    {
        if (shouldShake)
        {
            if(duration > 0)
            {
                camera.localPosition = m_StartPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                shouldShake = false;
                duration = m_InitialDuration;
                camera.localPosition = m_StartPosition;
            }
        }
    }
}

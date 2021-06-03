using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomassController : MonoBehaviour
{
    public float m_Start10SecondsAfterStart;
    public bool m_StartAfterStart;

    public bool startSonicEvent;
    public float sonicEventDuration;
    public float spawnSphereIntervalDefault;
    public GameObject spherePrefab;
    public bool m_SonicEventInProg;

    private float m_StartSonicEventAfter;
    private float m_TimeTillSpawnSphere;
    private bool m_EndSonicEvent;

    void Update()
    {
        if (m_StartAfterStart && m_Start10SecondsAfterStart <= 0)
        {
            StartSonicEvent();
            m_StartAfterStart = false;
        }
        else if (m_StartAfterStart)
        {
            m_Start10SecondsAfterStart -= Time.deltaTime;
        }

        if (startSonicEvent)
        {
            StartSonicEvent();
        }

        if (m_SonicEventInProg)
        {
            if (m_StartSonicEventAfter <= 0)
            {
                if (sonicEventDuration <= 0)
                {
                    m_EndSonicEvent = true;
                }
                else
                {
                    sonicEventDuration -= Time.deltaTime;
                }

                if (m_TimeTillSpawnSphere <= 0)
                {
                    // Spawn sphere at epicenter
                    GameObject spawnedSphere = Instantiate(spherePrefab, transform.position, transform.rotation);

                    FindObjectOfType<AudioController>().Play("Low Rumble");
                    FindObjectOfType<AudioController>().Play("Suspense");

                    // Reset spawn interval
                    m_TimeTillSpawnSphere = spawnSphereIntervalDefault;
                }
                else
                {
                    m_TimeTillSpawnSphere -= Time.deltaTime;
                }
            }
            else
            {
                m_StartSonicEventAfter -= Time.deltaTime;
            }
        }
    }

    public void StartSonicEvent()
    {
        startSonicEvent = false;
        m_EndSonicEvent = false;
        m_SonicEventInProg = true;
        FindObjectOfType<AudioController>().Play("Monster Voice");
        m_StartSonicEventAfter = 5f;
    }

    public void MuffleSFX()
    {
        // Muffle SFX when earplugs are in
        FindObjectOfType<AudioController>().MuffleSound(true);
    }
}

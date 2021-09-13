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
    private bool m_GrosslyDecrease;
    private bool m_GrosslyPulsating;
    private Vector3 m_LocalScaleBeforeGross;

    private void FixedUpdate()
    {
        if (m_GrosslyPulsating)
        {
            if (m_GrosslyDecrease)
            {
                gameObject.transform.localScale -= new Vector3(5, 5, 5);

                // When biomass has reached a certain size end pulsate decrease
                if (gameObject.transform.localScale.x >= m_LocalScaleBeforeGross.x - 15)
                {
                    m_GrosslyDecrease = false;
                }
            }
            else
            {
                gameObject.transform.localScale += new Vector3(5, 5, 5);

                // After the biomass has reverted back to normal size then end pulsating
                if (gameObject.transform.localScale.x <= m_LocalScaleBeforeGross.x)
                {
                    m_GrosslyDecrease = false;
                    m_GrosslyPulsating = false;
                    gameObject.transform.localScale = m_LocalScaleBeforeGross;
                }
            }
        }
    }

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

                    // Trigger the biomass to grossly pulsate
                    m_GrosslyPulsating = true;
                    m_GrosslyDecrease = true;
                    m_LocalScaleBeforeGross = gameObject.transform.localScale;

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

    public void StopSonicEvent()
    {
        // Must delete any spawned spheres when stopping sonic event
        GameObject[] sphereEmissions = GameObject.FindGameObjectsWithTag("BiosphereEmission");
        if (sphereEmissions.Length > 0)
        {
            foreach (GameObject item in sphereEmissions)
            {
                Destroy(item);
            }
        }

        FindObjectOfType<AudioController>().Stop("Low Rumble");
        FindObjectOfType<AudioController>().Stop("Suspense");

        startSonicEvent = false;
        m_EndSonicEvent = true;
        m_SonicEventInProg = false;
    }

    public void MuffleSFX()
    {
        // Muffle SFX when earplugs are in
        FindObjectOfType<AudioController>().MuffleSound(true);
    }

    public void UnMuffleSFX()
    {
        // Muffle SFX when earplugs are in
        FindObjectOfType<AudioController>().MuffleSound(false);
    }
}

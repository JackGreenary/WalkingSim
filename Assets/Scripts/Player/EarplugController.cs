using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarplugController : MonoBehaviour
{
    public BiomassController biomassManager;
    public HUDController hudController;
    public float m_earplugBtnHoldTimeTotal;

    private bool m_earplugsAreIn;
    private float m_earplugBtnHoldTime;

    void Start()
    {
        m_earplugBtnHoldTime = m_earplugBtnHoldTimeTotal;
        m_earplugsAreIn = false;
        hudController = FindObjectOfType<HUDController>();
    }

    void Update()
    {
        if (biomassManager.m_SonicEventInProg && !m_earplugsAreIn)
        {
            if(hudController.promptTextObj.text == "")
            {
                // Show button prompt
                hudController.ShowButtonPrompt("Hold E to plug ears");
            }

            // When player holds e key they will insert the earplugs
            if (Input.GetKey(KeyCode.E))
            {
                m_earplugBtnHoldTime -= Time.deltaTime;

                // Update HUD progress bar
                float percentage = 100 - (m_earplugBtnHoldTime / m_earplugBtnHoldTimeTotal * 100);
                hudController.UpdateButtonHoldProgBar(percentage);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                m_earplugBtnHoldTime = m_earplugBtnHoldTimeTotal;
                hudController.UpdateButtonHoldProgBar(0);
            }
            if (m_earplugBtnHoldTime <= 0)
            {
                m_earplugBtnHoldTime = m_earplugBtnHoldTimeTotal;
                m_earplugsAreIn = true;
                InsertEarplugs();

                // Hide button prompt
                hudController.ShowButtonPrompt("");
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            m_earplugBtnHoldTime = m_earplugBtnHoldTimeTotal;
            hudController.UpdateButtonHoldProgBar(0);
        }
    }

    public void InsertEarplugs()
    {
        biomassManager.MuffleSFX();
    }
}

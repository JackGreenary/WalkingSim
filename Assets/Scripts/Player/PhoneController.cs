using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public GameController gameController;
    public HUDController hudController;
    public AudioController audioManager;
    public GameObject phoneObj;
    public Transform onScreenPos;
    public Transform offScreenPos;
    public bool phoneShowing;
    public bool phoneEnabled;
    public float phoneBtnHoldTimeTotal;
    public int conversationType;
    public bool memoryPlaying;

    private bool m_ShowPhone;
    private bool m_HidePhone;
    private float m_SpeedMod;

    private bool m_MemoryWaiting;
    private bool m_PhoneRinging;
    private float m_PhoneRingTime;
    private float m_PhoneBtnHoldTime;
    private float m_DialogueDelay;
    private bool m_ConversationStarted;

    void Start()
    {
        phoneObj.transform.position = offScreenPos.position;
        phoneShowing = false;
        m_ConversationStarted = false;
        m_PhoneBtnHoldTime = phoneBtnHoldTimeTotal;
        m_DialogueDelay = .5f;
        m_PhoneRinging = false;
        hudController = FindObjectOfType<HUDController>();
        audioManager = FindObjectOfType<AudioController>();
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        // If memory is ready to be activated
        if (m_MemoryWaiting)
        {
            if (hudController.promptTextObj.text == "")
            {
                // Show button prompt
                hudController.ShowButtonPrompt("Hold E to remember");
            }

            // When player holds e key they will bring up the phone
            if (Input.GetKey(KeyCode.E))
            {
                m_PhoneBtnHoldTime -= Time.deltaTime;

                // Update HUD progress bar
                float percentage = 100 - (m_PhoneBtnHoldTime / phoneBtnHoldTimeTotal * 100);
                hudController.UpdateButtonHoldProgBar(percentage);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                m_PhoneBtnHoldTime = phoneBtnHoldTimeTotal;
                hudController.UpdateButtonHoldProgBar(0);
            }
            if (m_PhoneBtnHoldTime <= 0)
            {
                // MEMORY SOUNDS HERE
                m_SpeedMod = 1;
                m_PhoneBtnHoldTime = phoneBtnHoldTimeTotal;
                memoryPlaying = true;
                m_MemoryWaiting = false;

                // Hide button prompt
                hudController.ShowButtonPrompt("");
            }
        }
        if (memoryPlaying)
        {
            // When phone showing
            // Delay then run dialogue
            if (m_DialogueDelay <= 0 && !m_ConversationStarted)
            {
                DialogueController.Instance.StartNextConversation();
                DialogueController.Instance.SetNextConversation();
                m_ConversationStarted = true;
                m_DialogueDelay = .5f;
            }
            // Otherwise if conversation is not started, keep on with delay
            else if (!m_ConversationStarted)
            {
                m_DialogueDelay -= Time.deltaTime;
            }
        }

        // When phone is enabled (ringing or being rung)
        if (m_PhoneRinging)
        {
            if (hudController.promptTextObj.text == "")
            {
                // Show button prompt
                hudController.ShowButtonPrompt("Hold E to answer phone");
            }

            // When player holds e key they will bring up the phone
            if (Input.GetKey(KeyCode.E))
            {
                m_PhoneBtnHoldTime -= Time.deltaTime;

                // Update HUD progress bar
                float percentage = 100 - (m_PhoneBtnHoldTime / phoneBtnHoldTimeTotal * 100);
                hudController.UpdateButtonHoldProgBar(percentage);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                m_PhoneBtnHoldTime = phoneBtnHoldTimeTotal;
                hudController.UpdateButtonHoldProgBar(0);
            }
            if (m_PhoneBtnHoldTime <= 0)
            {
                audioManager.Stop("Ringtone");
                audioManager.Play("Pickup Call");
                m_PhoneRinging = false;
                m_SpeedMod = 1;
                m_ShowPhone = true;
                m_HidePhone = false;
                m_PhoneBtnHoldTime = phoneBtnHoldTimeTotal;

                // Hide button prompt
                hudController.ShowButtonPrompt("");
            }
        }
        else if (m_ShowPhone)
        {
            ShowHidePhone(true);
        }
        else if (!m_HidePhone && phoneShowing)
        {
            // When phone showing
            // Delay then run dialogue
            if (m_DialogueDelay <= 0 && !m_ConversationStarted)
            {
                DialogueController.Instance.StartNextConversation();
                DialogueController.Instance.SetNextConversation();
                m_ConversationStarted = true;
                m_DialogueDelay = .5f;
            }
            // Otherwise if conversation is not started, keep on with delay
            else if (!m_ConversationStarted)
            {
                m_DialogueDelay -= Time.deltaTime;
            }
        }
        // If we want to hide phone then run the method to do so
        if (m_HidePhone)
        {
            ShowHidePhone(false);
            audioManager.Play("Hangup Call");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            m_PhoneBtnHoldTime = phoneBtnHoldTimeTotal;
            hudController.UpdateButtonHoldProgBar(0);
        }
    }

    private void ShowHidePhone(bool show)
    {
        Transform tramsformTarget = (show ? onScreenPos : offScreenPos);

        if (Vector3.Distance(phoneObj.transform.position, tramsformTarget.transform.position) > Vector3.kEpsilon)
        {
            // If not in position yet then keep on moving
            m_SpeedMod = m_SpeedMod * 1.1f;
            phoneObj.transform.position = Vector3.MoveTowards(phoneObj.transform.position, tramsformTarget.transform.position, 1);
        }
        else
        {
            if (show)
            {
                m_ShowPhone = false;
            }
            else
            {
                m_HidePhone = false;
            }
            phoneShowing = (show ? true : false);
        }
    }
    public void OnConversationEnd(Transform actor)
    {
        m_ShowPhone = false;
        m_HidePhone = true;
        m_ConversationStarted = false;
        gameController.CompleteCurrentEvent();
    }

    public void RingPhone(int convoType)
    {
        conversationType = convoType;
        if (convoType == 0)
        {
            if (!phoneShowing)
            {
                // TODO outbound/inbound calls
                // Different sound will play and different image will be shown on phone screen

                // When phone is "rung" from other class it will trigger the next dialogue
                m_PhoneRinging = true;
                audioManager.Play("Ringtone");
            }
        }
        else
        {
            m_MemoryWaiting = true;
        }
    }
}

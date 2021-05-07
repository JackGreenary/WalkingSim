using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public GameObject phoneObj;
    public Transform onScreenPos;
    public Transform offScreenPos;
    public bool phoneShowing;
    public bool phoneEnabled;

    private bool showPhone;
    private bool hidePhone;
    private float speedMod;

    private bool phoneRinging;
    private float phoneRingTime;
    private float phoneBtnHoldTime;
    private float dialogueDelay;
    private bool conversationStarted;

    void Start()
    {
        phoneObj.transform.position = offScreenPos.position;
        phoneShowing = false;
        conversationStarted = false;
        phoneBtnHoldTime = .5f;
        dialogueDelay = .5f;
        phoneRinging = false;
    }

    void Update()
    {
        // When phone is enabled (ringing or being rung)
        if (phoneRinging)
        {
            // When player holds e key they will bring up the phone
            if (Input.GetKey(KeyCode.E))
            {
                phoneBtnHoldTime -= Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                phoneBtnHoldTime = .5f;
            }
            if (phoneBtnHoldTime <= 0)
            {
                FindObjectOfType<AudioManager>().Stop("Ringtone");
                FindObjectOfType<AudioManager>().Play("Pickup Call");
                phoneRinging = false;
                speedMod = 1;
                showPhone = true;
                hidePhone = false;
                phoneBtnHoldTime = 1.5f;
            }
        }
        else if (showPhone)
        {
            ShowHidePhone(true);
        }
        else if (!hidePhone && phoneShowing)
        {
            // When phone showing
            // Delay then run dialogue
            if (dialogueDelay <= 0 && !conversationStarted)
            {
                DialogueController.Instance.StartNextConversation();
                DialogueController.Instance.SetNextConversation();
                conversationStarted = true;
                dialogueDelay = .5f;
            }
            // Otherwise if conversation is not started, keep on with delay
            else if (!conversationStarted)
            {
                dialogueDelay -= Time.deltaTime;
            }
        }
        // If we want to hide phone then run the method to do so
        if (hidePhone)
        {
            ShowHidePhone(false);
            FindObjectOfType<AudioManager>().Play("Hangup Call");
        }
    }

    private void ShowHidePhone(bool show)
    {
        Transform tramsformTarget = (show ? onScreenPos : offScreenPos);

        if (Vector3.Distance(phoneObj.transform.position, tramsformTarget.transform.position) > Vector3.kEpsilon)
        {
            // If not in position yet then keep on moving
            speedMod = speedMod * 1.1f;
            phoneObj.transform.position = Vector3.MoveTowards(phoneObj.transform.position, tramsformTarget.transform.position, 1);
        }
        else
        {
            if (show)
            {
                showPhone = false;
            }
            else
            {
                hidePhone = false;
            }
            phoneShowing = (show ? true : false);
        }
    }
    public void OnConversationEnd(Transform actor)
    {
        showPhone = false;
        hidePhone = true;
        conversationStarted = false;
    }

    public void RingPhone()
    {
        if (!phoneShowing)
        {
            // TODO outbound/inbound calls
            // Different sound will play and different image will be shown on phone screen

            // When phone is "rung" from other class it will trigger the next dialogue
            phoneRinging = true;
            FindObjectOfType<AudioManager>().Play("Ringtone");
        }
    }
}

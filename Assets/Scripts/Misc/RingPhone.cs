using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPhone : MonoBehaviour
{
    public PhoneController phoneController;
    public bool ringPhoneEnabled;
    public bool ringPhoneDone;
    public int conversationType;

    private void Start()
    {
        ringPhoneEnabled = false;
        ringPhoneDone = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (ringPhoneEnabled && !ringPhoneDone)
        {
            ringPhoneDone = true;
            phoneController.RingPhone(conversationType);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (ringPhoneEnabled && !ringPhoneDone)
        {
            ringPhoneDone = true;
            phoneController.RingPhone(conversationType);
        }
    }
}

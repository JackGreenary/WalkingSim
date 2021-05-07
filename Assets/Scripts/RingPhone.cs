using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPhone : MonoBehaviour
{
    public PhoneController phoneController;

    void OnTriggerEnter(Collider other)
    {
        phoneController.RingPhone();
    }
}

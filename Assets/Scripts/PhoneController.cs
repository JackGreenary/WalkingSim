using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public GameObject phoneObj;
    public Transform onScreenPos;
    public Transform offScreenPos;
    public bool phoneShowing;

    private bool showPhone;
    private bool hidePhone;
    private float speedMod;

    private float phoneBtnHoldTime;

    void Start()
    {
        phoneObj.transform.position = offScreenPos.position;
        phoneShowing = false;
        phoneBtnHoldTime = 2;
    }

    void Update()
    {
        if (!phoneShowing)
        {
            if (Input.GetKey(KeyCode.E))
            {
                phoneBtnHoldTime -= Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                phoneBtnHoldTime = 2;
            }
            if (phoneBtnHoldTime <= 0)
            {
                speedMod = 1;
                showPhone = true;
                hidePhone = false;
            }
            if (showPhone)
            {
                ShowHidePhone(true);
            }
        }
        else
        {
            if (hidePhone)
            {
                ShowHidePhone(false);
            }
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
            showPhone = (show ? false : true);
            hidePhone = (show ? true : false);
            phoneShowing = (show ? true : false);
        }
    }
}

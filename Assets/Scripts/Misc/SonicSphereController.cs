using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicSphereController : MonoBehaviour
{
    private Vector3 currentScaleSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentScaleSpeed = new Vector3(50,50,50);
    }

    private void Update()
    {
        transform.localScale = transform.localScale + currentScaleSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<AudioController>().Play("Impact");
            other.GetComponentInChildren<CameraShaker>().shouldShake = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool hovered;
    public bool mouseHeld;
    public float holdTimeDefault;
    public string hoverText;
    public Material outlineMaterial;
    public Material filledMaterial;
    public bool interactionDone;
    public int interactType;

    private float holdTime;

    void Start()
    {
        holdTime = holdTimeDefault;
        interactionDone = false;
        GetComponent<MeshRenderer>().material = outlineMaterial;
    }

    void Update()
    {
        if (hovered && !interactionDone)
        {
            if (Input.GetMouseButton(0))
            {
                holdTime -= Time.deltaTime;
                mouseHeld = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                holdTime = holdTimeDefault;
                mouseHeld = false;
            }
            if (holdTime <= 0)
            {
                interactionDone = true;
                switch (interactType)
                {
                    // Deploying
                    case 0:
                        GetComponent<MeshRenderer>().material = filledMaterial;
                        break;
                    // Picking up
                    case 1:
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
                FindObjectOfType<AudioManager>().Play("Item Placed");
            }
        }
    }

    public float GetPercent()
    {
        return 100 - (holdTime / holdTimeDefault * 100);
    }

    public bool ResetCursor()
    {
        return interactionDone ? true : false;
    }
}

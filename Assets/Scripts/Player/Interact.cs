using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class Interact : FirstPersonLook
{
    [Header("Reticule Management")]
    public Image reticule;
    public Sprite reticuleNormal;
    public Sprite reticuleInteract;

    public TextMeshProUGUI hoverText;
    public ProgressBar progressPie;

    private Interactable hoveredInteractable;

    void Start()
    {
        progressPie.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if looking at interactable object
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.5f) && hit.collider.gameObject.CompareTag("Interactable"))
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            reticule.sprite = reticuleInteract;
            hoveredInteractable = interactable;

            if (interactable.ResetCursor())
            {
                ResetReticule();
            }

            if (interactable != null)
            {
                if (!interactable.interactionDone)
                {
                    hoverText.text = interactable.hoverText;
                    interactable.hovered = true;

                    if (interactable.mouseHeld)
                    {
                        progressPie.gameObject.SetActive(true);
                        progressPie.currentPercent = interactable.GetPercent();
                    }
                }
            }
        }
        else
        {
            ResetReticule();
        }
    }

    void ResetReticule()
    {
        hoverText.text = "";
        reticule.sprite = reticuleNormal;
        if (hoveredInteractable != null)
        {
            hoveredInteractable.hovered = false;
            progressPie.gameObject.SetActive(false);
        }
    }
}

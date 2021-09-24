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
    public HUDController hudController;
    public int readableId;

    public GameController gameController;
    public bool active;

    private float holdTime;

    void Start()
    {
        holdTime = holdTimeDefault;
        interactionDone = false;
        //GetComponent<MeshRenderer>().material = outlineMaterial;
        hudController = FindObjectOfType<HUDController>();
    }

    void Update()
    {
        if (active && hovered && !interactionDone)
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
                    case 0: // Deploying
                        GetComponent<MeshRenderer>().material = filledMaterial;
                        break;
                    case 1: // Picking up
                        interactionDone = false;
                        gameController.CompleteCurrentEvent();
                        // TODO make better
                        FindObjectOfType<AudioController>().Play("Item Placed");
                        Destroy(gameObject);
                        break;
                    case 2: // Read
                        if (!GetComponent<Collectible>().found)
                        {
                            if(hudController == null)
                            {
                                hudController = FindObjectOfType<HUDController>();
                            }
                            hudController.ShowReadable(readableId);
                            interactionDone = false;
                            GetComponent<Collectible>().CollectibleFound();
                            FindObjectOfType<AudioController>().Play("Paper Rustle");
                        }
                        break;
                    case 3: // Continue
                        gameController.NextLocation();
                        //gameController.ActivateContinue();
                        break;
                    default:
                        break;
                }
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool startAtBeginning;

    public WarpManager warpManager;
    public PhoneController phoneController;
    public EarplugController earplugController;
    public BiomassController biomassController;
    public GameObject biomass;
    public GameObject player;
    public HUDController hudController;

    public List<SubScene> scenes;

    public List<SubScene> subScenes;
    public SubScene currentSubScene;
    public SubSceneEvent currentSubSceneEvent;

    public GameObject currentDestination;
    public UnityEngine.AI.NavMeshPath path;

    public TextMeshProUGUI objectiveText;

    public Camera playerCam;
    public Camera menuCam;

    public GameObject menuPnl;

    private int currentSubSceneIndex;
    private int currentSubSceneEventIndex;

    private float m_TimerSinceLastObjective;
    private bool m_Playing;

    private bool m_WaitingOnCollectibles;

    // Start is called before the first frame update
    void Start()
    {
        phoneController = FindObjectOfType<PhoneController>();
        earplugController = FindObjectOfType<EarplugController>();
        biomassController = FindObjectOfType<BiomassController>();
        hudController = FindObjectOfType<HUDController>();
        path = new UnityEngine.AI.NavMeshPath();

        //subScenes = new List<SubScene>()
        //{
        //    new SubScene()
        //    {
        //        sceneName = "House Exterior",
        //        // House exterior
        //        subSceneEvents = new List<SubSceneEvent>()
        //        {
        //            new SubSceneEvent()
        //            {
        //                eventType = 0,
        //                ringPhoneEvent = GameObject.Find("2.1Phone")
        //            },
        //            new SubSceneEvent()
        //            {
        //                eventType = 2,
        //                earplugEvent = earplugController
        //            },
        //            new SubSceneEvent()
        //            {
        //                eventType = 1,
        //                interactableEvent = GameObject.Find("2.3Cont")
        //            }
        //        }
        //    },
        //    new SubScene()
        //    {
        //        sceneName = "Woods",
        //        // Road to church
        //        subSceneEvents = new List<SubSceneEvent>()
        //        {
        //            new SubSceneEvent()
        //        {
        //            eventType = 0,
        //            ringPhoneEvent = GameObject.Find("3.1Phone")
        //        },
        //        new SubSceneEvent()
        //        {
        //            eventType = 1,
        //            interactableEvent = GameObject.Find("3.2Cont")
        //        }
        //        }
        //    },
        //    new SubScene()
        //    {
        //        sceneName = "Church",
        //        // Church
        //        subSceneEvents = new List<SubSceneEvent>()
        //        {
        //            new SubSceneEvent()
        //        {
        //            eventType = 0,
        //            ringPhoneEvent = GameObject.Find("4.1Phone")
        //        },
        //        new SubSceneEvent()
        //        {
        //            eventType = 2,
        //            earplugEvent = earplugController
        //        },
        //        new SubSceneEvent()
        //        {
        //            eventType = 1,
        //            interactableEvent = GameObject.Find("4.3Cont")
        //        }
        //        }
        //    },
        //    new SubScene()
        //    {
        //        sceneName = "Library",
        //        // Library
        //        subSceneEvents = new List<SubSceneEvent>()
        //        {
        //            new SubSceneEvent()
        //            {
        //                eventType = 0,
        //                ringPhoneEvent = GameObject.Find("5.1Phone")
        //            },
        //            new SubSceneEvent()
        //            {
        //                eventType = 1,
        //                interactableEvent = GameObject.Find("5.2Cont")
        //            }
        //        }
        //    },
        //    new SubScene()
        //    {
        //        sceneName = "Dock",
        //        // Dock
        //        subSceneEvents = new List<SubSceneEvent>(){
        //            new SubSceneEvent()
        //            {
        //                eventType = 0,
        //                ringPhoneEvent = GameObject.Find("6.1Phone")
        //            },
        //            new SubSceneEvent()
        //            {
        //                eventType = 2,
        //                earplugEvent = earplugController
        //            },
        //            new SubSceneEvent()
        //            {
        //                eventType = 1,
        //                interactableEvent = GameObject.Find("6.3Cont")
        //            }
        //        }
        //    },
        //    new SubScene()
        //    {
        //        sceneName = "Hill",
        //        // Road to lab
        //        subSceneEvents = new List<SubSceneEvent>(){
        //            new SubSceneEvent()
        //            {
        //                eventType = 0,
        //                ringPhoneEvent = GameObject.Find("7.1Phone")
        //            },
        //            new SubSceneEvent()
        //            {
        //                eventType = 1,
        //                interactableEvent = GameObject.Find("7.2Cont")
        //            }
        //        }
        //    },
        //    new SubScene()
        //    {
        //        sceneName = "Lab",
        //        // Lab
        //        subSceneEvents = new List<SubSceneEvent>()
        //        { 
        //            new SubSceneEvent()
        //            {
        //                eventType = 1,
        //                interactableEvent = GameObject.Find("8.1Cont")
        //            } 
        //        }
        //    }
        //};

        // Activate menu camera on start
        playerCam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        menuCam = GameObject.Find("MenuCamera").GetComponent<Camera>();

        playerCam.enabled = false;
        menuCam.enabled = true;

        // Deactivate controls on startup
        player.GetComponent<FirstPersonMovement>().controlsEnabled = false;
        m_Playing = false;

        // Unlock the cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Disable the HUD
        hudController.m_HideHud = true;
        hudController.ShowHideHUD();

        ShowMenu(true);
        FindObjectOfType<AudioController>().PlaybackgroundMusic("Menu Music");
    }

    public void ShowMenu(bool show)
    {
        menuPnl.SetActive(show);
    }

    public void StartGame()
    {
        // Stop menu background music
        FindObjectOfType<AudioController>().Stop("Menu Music");

        // Start background music
        FindObjectOfType<AudioController>().PlaybackgroundMusic("Moody Music");

        // Switch to player camera
        playerCam.enabled = true;
        menuCam.enabled = false;

        // Activate controls 
        player.GetComponent<FirstPersonMovement>().controlsEnabled = true;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Show the HUD
        hudController.m_HideHud = false;
        hudController.ShowHideHUD();

        // Hide the menu
        ShowMenu(false);

        // Teleport character to start location
        warpManager.WarpToTargetPoint(0);

        // Set subscene to first
        currentSubSceneIndex = 0;
        currentSubScene = scenes[currentSubSceneIndex];

        // Set subscene event to first
        currentSubSceneEventIndex = 0;
        currentSubSceneEvent = currentSubScene.subSceneEvents[currentSubSceneEventIndex];
        warpManager.warpText.text = currentSubScene.sceneName;
        hudController.UpdateCollectedCount(currentSubScene.collection.collectedCount, currentSubScene.collection.collectibles.Count);

        RingPhone ringPhone;
        currentSubSceneEvent.TryGetComponent(out ringPhone);

        if (ringPhone != null)
        {
            currentSubSceneEvent.GetComponent<RingPhone>().ringPhoneEnabled = true;
        }

        m_Playing = true;
        m_WaitingOnCollectibles = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonMouseOver()
    {
        FindObjectOfType<AudioController>().Play("Button Click");
    }

    private void Update()
    {
        if (m_Playing)
        {
            switch (currentSubSceneEvent.eventType)
            {
                case 0: // If current event is phone call then on end

                    break;
                default:
                    break;
            }

            if (m_TimerSinceLastObjective > 0)
            {
                m_TimerSinceLastObjective -= Time.deltaTime;
            }
        }
        if (currentSubScene != null && currentSubSceneEventIndex == (currentSubScene.subSceneEvents.Count - 1) && currentSubScene.collection.collectionComplete)
        {
            m_WaitingOnCollectibles = false;
            ActivateContinue();
        }
        else
        {
            m_WaitingOnCollectibles = true;
        }
    }

    public void CompleteCurrentEvent()
    {
        // Cooldown timer must be at 0
        if (m_TimerSinceLastObjective <= 0)
        {
            if (currentSubSceneEventIndex == (currentSubScene.subSceneEvents.Count - 1) && m_WaitingOnCollectibles)
            {
                currentSubSceneEvent.interactableEvent.SetActive(true);
                currentSubSceneEvent.interactableEvent.GetComponent<Interactable>().active = true;
                currentDestination = currentSubSceneEvent.interactableEvent;
            }
            // If any more events in scene
            if (currentSubSceneEventIndex < (currentSubScene.subSceneEvents.Count - 1))
            {
                currentSubSceneEventIndex++;
                currentSubSceneEvent = currentSubScene.subSceneEvents[currentSubSceneEventIndex];
            }
            else // All events in scene played, move to next and reset event index
            {
                currentSubSceneEventIndex = 0;
                currentSubSceneIndex++;
                currentSubScene = scenes[currentSubSceneIndex];
                currentSubSceneEvent = currentSubScene.subSceneEvents[currentSubSceneEventIndex];

                // When traveling to a new subscene stop any biomass events
                biomassController.StopSonicEvent();
                earplugController.TakeOutEarplugs();
            }
            switch (currentSubSceneEvent.eventType)
            {
                case 0: // For phone calls activate the trigger area
                    currentSubSceneEvent.ringPhoneEvent.SetActive(true);
                    currentDestination = currentSubSceneEvent.ringPhoneEvent;
                    currentSubSceneEvent.ringPhoneEvent.GetComponent<RingPhone>().ringPhoneEnabled = true;
                    break;
                case 1: // For interactables activate them
                        // If this is the last event and continue do not activate it until all collectibles have been found
                    if (currentSubSceneEventIndex == (currentSubScene.subSceneEvents.Count - 1) && currentSubScene.collection.collectionComplete || currentSubSceneEventIndex != (currentSubScene.subSceneEvents.Count - 1))
                    {
                        currentSubSceneEvent.interactableEvent.SetActive(true);
                        currentSubSceneEvent.interactableEvent.GetComponent<Interactable>().active = true;
                        currentDestination = currentSubSceneEvent.interactableEvent;
                    }
                    break;
                case 2: // For ear plug events trigger the emission
                    currentSubSceneEvent.earplugEvent.biomassManager.startSonicEvent = true;
                    currentDestination = biomass;
                    currentSubSceneEvent.earplugEvent.earplugsAreIn = false;
                    break;
                default:
                    break;
            }
            m_WaitingOnCollectibles = true;
            m_TimerSinceLastObjective = 1;
            hudController.target = currentDestination;
        }
    }

    public void ActivateContinue()
    {
        currentSubSceneEvent.interactableEvent.SetActive(true);
        currentSubSceneEvent.interactableEvent.GetComponent<Interactable>().active = true;
        currentDestination = currentSubSceneEvent.interactableEvent;
        //biomassController.StopSonicEvent();
    }

    public void NextLocation()
    {
        if (currentSubScene.collection.collectionComplete)
        {
            biomassController.StopSonicEvent();
            warpManager.warpText.text = scenes[currentSubSceneIndex + 1].sceneName;
            warpManager.warpToNextPoint = true;

            currentSubSceneEventIndex = 0;
            currentSubSceneIndex++;
            currentSubScene = scenes[currentSubSceneIndex];
            currentSubSceneEvent = currentSubScene.subSceneEvents[currentSubSceneEventIndex];

            RingPhone ringPhone;
            currentSubSceneEvent.TryGetComponent(out ringPhone);

            if (ringPhone != null)
            {
                currentSubSceneEvent.GetComponent<RingPhone>().ringPhoneEnabled = true;
            }

            hudController.UpdateCollectedCount(currentSubScene.collection.collectedCount, currentSubScene.collection.collectibles.Count);
        }

        biomass.transform.localScale += new Vector3(45, 45, 45);
        biomass.transform.position = new Vector3(biomass.transform.position.x, biomass.transform.position.y + 25, biomass.transform.position.z);
    }
}

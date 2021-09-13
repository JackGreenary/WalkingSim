using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI goalTextObj;
    public TextMeshProUGUI promptTextObj;
    public GameObject goalPnl;
    public GameObject readablePnl;
    public TextMeshProUGUI readableTxt;
    public List<Readable> readables;

    // Collection variables
    public TextMeshProUGUI collectionCountTxt;

    public FirstPersonLook firstPersonLook;

    // Waypoint
    public GameObject waypoint;
    public GameObject target;
    private bool m_ShowWaypoint;

    public Transform onScreenPos;
    public Transform offScreenPos;

    public ProgressBar buttonHoldProgBar;
    public bool goalShowing;

    public bool m_HideHud;

    void Start()
    {
        goalPnl.transform.position = offScreenPos.position;
        goalShowing = false;
        //goalTextObj.text = SetGoalText(0);
        buttonHoldProgBar.gameObject.SetActive(false);
        readablePnl.SetActive(false);
        waypoint.SetActive(false);
        //collectionCountTxt.text = "";
    }

    void Update()
    {
        if (m_ShowWaypoint && waypoint != null && target != null)
        {
            waypoint.SetActive(true);

            // Giving limits to the icon so it sticks on the screen
            // Below calculations witht the assumption that the icon anchor point is in the middle
            // Minimum X position: half of the icon width
            float minX = waypoint.GetComponent<Image>().GetPixelAdjustedRect().width / 2;
            // Maximum X position: screen width - half of the icon width
            float maxX = Screen.width - minX;

            // Minimum Y position: half of the height
            float minY = waypoint.GetComponent<Image>().GetPixelAdjustedRect().height / 2;
            // Maximum Y position: screen height - half of the icon height
            float maxY = Screen.height - minY;

            // Temporary variable to store the converted position from 3D world point to 2D screen point
            Vector2 pos = Camera.main.WorldToScreenPoint(target.GetComponent<Renderer>().bounds.center);

            // Check if the target is behind us, to only show the icon once the target is in front
            if (Vector3.Dot((target.GetComponent<Renderer>().bounds.center - Camera.main.transform.position).normalized, Camera.main.transform.forward) < 0)
            {
                // Check if the target is on the left side of the screen
                if (pos.x < Screen.width / 2)
                {
                    // Place it on the right (Since it's behind the player, it's the opposite)
                    pos.x = maxX;
                }
                else
                {
                    // Place it on the left side
                    pos.x = minX;
                }
            }

            // Limit the X and Y positions
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            // Update the marker's position
            waypoint.transform.position = pos;

            //waypoint.transform.position = Camera.main.WorldToScreenPoint(c);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_ShowWaypoint = true;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            m_ShowWaypoint = false;
            waypoint.SetActive(false);
        }
    }

    public void ShowHideHUD()
    {
        if (m_HideHud)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void UpdateButtonHoldProgBar(float percentage)
    {
        buttonHoldProgBar.currentPercent = percentage;
        if (buttonHoldProgBar.currentPercent == 0)
        {
            buttonHoldProgBar.gameObject.SetActive(false);
        }
        else
        {
            buttonHoldProgBar.gameObject.SetActive(true);
        }
    }

    public void ShowButtonPrompt(string promptText)
    {
        promptTextObj.text = promptText;
    }

    public void ShowReadable(int id)
    {
        readablePnl.SetActive(true);
        readableTxt.text = readables[id].text;
        Cursor.lockState = CursorLockMode.Confined;
        firstPersonLook.disableLook = true;
    }

    public void HideReadable()
    {
        readablePnl.SetActive(false);
        readableTxt.text = "";
        Cursor.lockState = CursorLockMode.Locked;
        firstPersonLook.disableLook = false;
    }

    public void UpdateCollectedCount(int count, int total)
    {
        collectionCountTxt.text = count.ToString() + "/" + total.ToString();
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    public List<GameObject> warpPoints;
    public GameObject player;
    public int warpIndex = -1;
    public AudioController audioManager;

    public GameObject warpPnl;
    public TextMeshProUGUI warpText;
    private float showWarpPnlFor = 0;
    private bool showingWarpPnl;

    public bool warpToNextPoint;
    public bool warpToPrevPoint;

    private float coolDown = 1;

    void Start()
    {
        //showingWarpPnl = true;
        warpPnl.SetActive(false);
        audioManager = FindObjectOfType<AudioController>();
        warpIndex = 0;
        warpToNextPoint = false;
        warpToPrevPoint = false;
    }

    private void Update()
    {
        if (coolDown <= 0)
        {
            if (warpToNextPoint)
                WarpToNextPoint();
            if (warpToPrevPoint)
                WarpToPrevPoint();
            if (Input.GetKey(KeyCode.RightArrow))
            {
                coolDown = 1;
                WarpToNextPoint();
                audioManager.Play("Warp");
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                coolDown = 1;
                WarpToPrevPoint();
                audioManager.Play("Warp");
            }
        }
        else
        {
            coolDown -= Time.deltaTime;
        }

        if(showingWarpPnl && showWarpPnlFor <= 0)
        {
            // Unpause game and hide warp panel
            //Time.timeScale = 1;
            showWarpPnlFor = 3;
            showingWarpPnl = false;
            warpPnl.SetActive(false);
            player.GetComponent<FirstPersonMovement>().controlsEnabled = true;
        }
        else
        {
            showWarpPnlFor -= Time.deltaTime;
        }
    }

    public void WarpToTargetPoint(int targetIndex)
    {
        if (warpPoints[targetIndex] != null)
        {
            player.transform.position = warpPoints[targetIndex].transform.position;
            player.transform.rotation = warpPoints[targetIndex].transform.rotation;
        }
        else
        {
            Debug.LogError("Warp Index is null");
        }

        // Pause game and show warp panel for 3 seconds then unpause
        //Time.timeScale = 0;
        showWarpPnlFor = 3;
        showingWarpPnl = true;
        warpPnl.SetActive(true);
        player.GetComponent<FirstPersonMovement>().controlsEnabled = false;
    }

    private void WarpToNextPoint()
    {
        warpToNextPoint = false;
        warpIndex++;
        if (warpPoints[warpIndex] != null)
        {
            player.transform.position = warpPoints[warpIndex].transform.position;
            player.transform.rotation = warpPoints[warpIndex].transform.rotation;
        }
        else
        {
            Debug.LogError("Warp Index is null");
        }

        // Pause game and show warp panel for 3 seconds then unpause
        //Time.timeScale = 0;
        showWarpPnlFor = 3;
        showingWarpPnl = true;
        warpPnl.SetActive(true);
        player.GetComponent<FirstPersonMovement>().controlsEnabled = false;
    }

    private void WarpToPrevPoint()
    {
        warpToPrevPoint = false;
        warpIndex--;
        if (warpPoints[warpIndex] != null)
        {
            player.transform.position = warpPoints[warpIndex].transform.position;
            player.transform.rotation = warpPoints[warpIndex].transform.rotation;
        }
        else
        {
            Debug.LogError("Warp Index is null");
        }
    }
}

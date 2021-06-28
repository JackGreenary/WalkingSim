using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    public List<GameObject> warpPoints;
    public GameObject player;
    public int warpIndex = -1;
    public AudioController audioManager;

    private float coolDown = 1;

    void Start()
    {
        audioManager = FindObjectOfType<AudioController>();
        WarpToNextPoint();
    }

    private void Update()
    {
        if (coolDown <= 0)
        {
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
    }

    private void WarpToNextPoint()
    {
        warpIndex++;
        if (warpPoints[warpIndex] != null)
        {
            player.transform.position = warpPoints[warpIndex].transform.position;
        }
        else
        {
            Debug.LogError("Warp Index is null");
        }
    }

    private void WarpToPrevPoint()
    {
        warpIndex--;
        if (warpPoints[warpIndex] != null)
        {
            player.transform.position = warpPoints[warpIndex].transform.position;
        }
        else
        {
            Debug.LogError("Warp Index is null");
        }
    }
}

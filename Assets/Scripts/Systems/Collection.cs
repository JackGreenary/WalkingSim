using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public List<GameObject> collectibles;
    public bool collectionComplete;
    public HUDController hudController;
    public int collectedCount;

    private void Start()
    {
        hudController = FindObjectOfType<HUDController>();
    }

    public bool CheckIfAllCollectiblesFound()
    {
        // This method is called everytime a new collectible is found
        // Call HUDController to update the count
        collectedCount++;
        if(hudController == null)
        {
            hudController = FindObjectOfType<HUDController>();
        }
        hudController.UpdateCollectedCount(collectedCount, collectibles.Count);
        // If all collectibles found then return true otherwise return false
        foreach (GameObject collectible in collectibles)
        {            
            if (!collectible.GetComponent<Collectible>().found)
            {
                collectionComplete = false;
                return false;
            }
        }
        collectionComplete = true;
        return true;
    }
}

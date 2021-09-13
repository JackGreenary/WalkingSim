using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool found;
    public Collection collection;

    public void CollectibleFound()
    {
        found = true;
        collection.CheckIfAllCollectiblesFound();
    }
}
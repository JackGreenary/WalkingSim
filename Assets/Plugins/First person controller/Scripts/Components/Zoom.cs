using UnityEngine;

public class Zoom : MonoBehaviour
{
    public float sensitivity = 1;
    Camera camera;
    [HideInInspector]
    public float defaultFOV;
    [Tooltip("Effectively the min FOV that we can reach while zooming with this camera.")]
    public float maxZoom = 15;
    [HideInInspector]
    public float zoomAmount;

    public bool zoomed = false;
    public int minfov;
    public int maxfov;


    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Start()
    {
        defaultFOV = camera.fieldOfView;
        maxfov = 55;
        minfov = 25;
    }

    void Update()
    {
        if (Input.GetKeyDown("mouse 1"))
            zoomed = !zoomed;

        if (Input.GetKeyUp("mouse 1"))
            zoomed = !zoomed;

        if (!zoomed && Camera.main.fieldOfView < maxfov)
            Camera.main.fieldOfView++;
        if (zoomed && Camera.main.fieldOfView > minfov)
            Camera.main.fieldOfView--;

        //zoomAmount += Input.mouseScrollDelta.y * sensitivity * .05f;
        //zoomAmount = Mathf.Clamp01(zoomAmount);
        //camera.fieldOfView = Mathf.Lerp(defaultFOV, maxZoom, zoomAmount);
    }
}

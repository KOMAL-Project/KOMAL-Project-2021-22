using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private ManageInputs inputManager;
    [SerializeField] private GameObject tileMap;

    [Tooltip("The minimum distance away from the ground the player can be for the camera to start zooming out.")]
    [SerializeField] private float startZoom;
    [Tooltip("The maximum distance away from the ground the player can be for the camera to stop zooming out.")]
    [SerializeField] private float stopZoom;

    [Tooltip("The minimum scaling of the camera view.")]
    [SerializeField] private float minZoom;
    [Tooltip("The maximum scaling of the camera view.")]
    [SerializeField] private float maxZoom;
    [Tooltip("The speed at which the camera Lerps toward its target zoom value.")]
    [SerializeField] private float zoomSpeed;

    private float targetZoom;
    private CompositeCollider2D groundCollider;
    private Camera cam;
    private Rigidbody rb;

    void Start()
    {
        cam = GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        groundCollider = tileMap.GetComponent<CompositeCollider2D>();

        targetZoom = minZoom;
    }

    void Update()
    {
        // TODO: Seems like a Lerp would do better here
        rb.velocity = 2 * (player.transform.position - transform.position + 2 * new Vector3(inputManager.GetJoystick().x, inputManager.GetJoystick().y, -10));

        float groundDistance = Vector3.Distance(groundCollider.ClosestPoint(player.transform.position), player.transform.position);

        if (groundDistance >= startZoom) 
        {
            targetZoom = scaleRangeFromUnitInterval(minZoom, maxZoom, normalizeRangeToUnitInterval(startZoom, stopZoom, groundDistance));
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }

    // Normalized a given value in the range [min-max] to the range of [0-1]
    private float normalizeRangeToUnitInterval(float min, float max, float valueInRange) 
    {
        if (valueInRange <= min) 
        {
            return 0;
        }
        if (valueInRange >= max) 
        {
            return 1;
        }
        return (valueInRange - min) / (max - min);
    }

    // Scales a value between [0-1] up to a range between [min-max]
    private float scaleRangeFromUnitInterval(float min, float max, float valueInUnitInterval) 
    {
        if (valueInUnitInterval <= 0) 
        {
            return min;
        }
        if (valueInUnitInterval >= 1) 
        {
            return max;
        }
        return valueInUnitInterval * (max - min) + min;
    }
}

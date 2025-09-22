using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointPointer : MonoBehaviour
{
    /// <summary>
    /// The game manager script for controlling whether the checkpoint pointer should function.
    /// </summary>
    [field: SerializeField]
    public GameManagerBase GameManagerScript
    { get; private set; }

    /// <summary>
    /// Reference to the currently active checkpoint in the scene for the player to move towards.
    /// </summary>
    [field: SerializeField]
    public Gate CheckpointToLocate
    { get; private set; }

    /// <summary>
    /// Reference to the player controller and by extension it's gameobject.
    /// </summary>
    [field: SerializeField]
    public AircraftController PlayerController
    { get; private set; }

    /// <summary>
    /// Reference to the point game object itself.
    /// </summary>
    [field: SerializeField]
    public GameObject Pointer
    { get; private set; }

    [field: SerializeField]
    public Vector3 PositionOffset
    { get; private set; }

    public Canvas UICanvasWithin
    { get; private set; }

    private void Awake()
    {
        if (GameManagerScript == null)
        {
            GameManagerScript = GameObject.FindObjectOfType<GameManagerBase>();

            if (GameManagerScript == null)
            {
                Debug.LogWarning("Unable to locate game manager script.");
            }
        }

        if (PlayerController == null)
        {
            PlayerController = GameObject.FindObjectOfType<AircraftController>();

            if (PlayerController == null)
            {
                Debug.LogWarning("Unable to locate game manager script.");
            }
        }

        if (Pointer == null)
        {
            Debug.LogWarning("Unable to locate Pointer gameobject.");
        }

        if (UICanvasWithin == null)
        {
            UICanvasWithin = Pointer.transform.root.GetComponentInChildren<Canvas>();

            if (UICanvasWithin == null)
            {
                Debug.LogWarning("Unable to locate game manager script.");
            }
        }
    }

        // Start is called before the first frame update
        void Start()
    {
        LocateNearestCheckpoint(PlayerController.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LateUpdate()
    {
        PointTowardsCheckpoint();
    }

    public void LocateNearestCheckpoint(GameObject gameObjectToCheckAround)
    {
        if (CheckpointToLocate == null)
        {
            Gate[] allGates = GameObject.FindObjectsByType<Gate>(FindObjectsSortMode.None);

            if (allGates == null)
            {
                Debug.LogWarning("Unable to locate game manager script.");
                return;
            }

            if (allGates.Count() > 0)
            {
                int closestGateIndex = 0;
                float closestDistance = float.MaxValue;

                for (int i = 0; i < allGates.Length; i++)
                {
                    float distanceBetween = Vector3.Distance(gameObjectToCheckAround.transform.position, allGates[i].transform.position);

                    if (distanceBetween < closestDistance)
                    {
                        closestGateIndex = i;
                        closestDistance = distanceBetween;
                    }
                }

                CheckpointToLocate = allGates[closestGateIndex];
            }
        }
    }

    public void PointTowardsCheckpoint()
    {
        
        if (GameManagerScript != null && CheckpointToLocate != null)
        {
            if (CheckpointToLocate.isActiveAndEnabled)
            {
                // Enable the pointer image if the checkpoint is active in scene.
                if (Pointer.activeSelf == false)
                {
                    Pointer.SetActive(true);
                }

                if (UICanvasWithin.renderMode != RenderMode.WorldSpace)
                {
                    Pointer.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.75f)) + (Camera.main.transform.forward * PositionOffset.x) + (Camera.main.transform.up * PositionOffset.y) + (Camera.main.transform.right * PositionOffset.z);

                    Pointer.transform.LookAt(CheckpointToLocate.transform.position);
                }
                else
                {
                    Pointer.transform.LookAt(CheckpointToLocate.transform.position);
                }
            }
            else
            {
                // Disable the pointer image if the checkpoint is not active in the scene.
                if (Pointer.activeSelf == true)
                {
                    Pointer.SetActive(false);
                }
            }
        }
    }
}

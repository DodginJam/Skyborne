using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBall : MonoBehaviour
{
    [field: SerializeField]
    public float SpeedOfRotation
    { get; private set; } = 10.0f;

    /// <summary>
    /// Reference to the balance game object itself.
    /// </summary>
    [field: SerializeField]
    public GameObject BalanceBallObject
    { get; private set; }

    [field: SerializeField]
    public Vector3 PositionOffset
    { get; private set; }

    public Canvas UICanvasWithin
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (BalanceBallObject == null)
        {
            Debug.LogWarning("Unable to locate BalanceBallObject gameobject.");
        }

        if (UICanvasWithin == null)
        {
            UICanvasWithin = BalanceBallObject.transform.root.GetComponentInChildren<Canvas>();

            if (UICanvasWithin == null)
            {
                Debug.LogWarning("Unable to locate game manager script.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (UICanvasWithin.renderMode != RenderMode.WorldSpace)
        {
            BalanceBallObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.75f)) + (Camera.main.transform.forward * PositionOffset.x) + (Camera.main.transform.up * PositionOffset.y) + (Camera.main.transform.right * PositionOffset.z);

            Transform PlaneTrans = GameObject.FindAnyObjectByType<AircraftController>().PlaneRigidBody.transform;

            BalanceBallObject.transform.localRotation = Quaternion.RotateTowards(BalanceBallObject.transform.rotation, Quaternion.Euler(Vector3.up), SpeedOfRotation);
        }
        else
        {
            BalanceBallObject.transform.rotation = Quaternion.RotateTowards(BalanceBallObject.transform.rotation, Quaternion.Euler(Vector3.up), SpeedOfRotation);
        }
    }
}

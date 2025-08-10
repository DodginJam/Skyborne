using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlaneLoop : MonoBehaviour
{
    [field: SerializeField]
    public GameObject ObjectToMove
    { get; private set; }

    [field: SerializeField]
    public Transform StartPosition
    { get; private set; }

    [field: SerializeField]
    public Transform EndPosition
    { get; private set; }

    [field: SerializeField, Min(1), Range(1, 60)]
    public float TimeBetweenFlyBy
    { get; private set; } = 15.0f;

    [field: SerializeField, Min(1)]
    public float MovementSpeed
    { get; private set; } = 10.0f;

    public Coroutine FlyByRoutine
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        FlyByRoutine = StartCoroutine(FlyByLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool VerifyVariables()
    {
        if (ObjectToMove == null)
        {
            return false;
        }

        if (StartPosition == null)
        {
            return false;
        }

        if (EndPosition == null)
        {
            return false;
        }

        return true;
    }

    public IEnumerator FlyByLoop()
    {
        float currentTimer = 0;
        bool endHit = false;

        while (VerifyVariables())
        {
            if (endHit == false)
            {
                Vector3 newPosition = Vector3.MoveTowards(ObjectToMove.transform.position, EndPosition.transform.position, MovementSpeed * Time.deltaTime);

                if (Vector3.Distance(newPosition, EndPosition.transform.position) <= 0.1f)
                {
                    ObjectToMove.transform.position = StartPosition.transform.position;

                    ObjectToMove.transform.LookAt(EndPosition, Vector3.forward);

                    // Since plane is not facing right direction, manually rotation is needed after LookAt.
                    ObjectToMove.transform.rotation *= Quaternion.AngleAxis(-90, Vector3.right);
                    ObjectToMove.transform.rotation *= Quaternion.AngleAxis(-90, Vector3.up);

                    endHit = true;
                    currentTimer = 0;
                }
                else
                {
                    ObjectToMove.transform.position = newPosition;
                }
            }
            else if (endHit == true)
            {
                if (currentTimer < TimeBetweenFlyBy)
                {
                    currentTimer += Time.deltaTime;
                }
                else
                {
                    currentTimer = 0;
                    endHit = false;
                }
            }

            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(CameraManager))]
public class CameraEffects : MonoBehaviour
{
    /// <summary>
    /// The camera manager script for the current camera can be accessed.
    /// </summary>
    public CameraManager CameraManager
    { get; private set; }

    /// <summary>
    /// The place where aircraft values and the values holder class is instantiated for access to flight information.
    /// </summary>
    public AircraftCurrentValues CurrentAircraftValues
    { get; private set; }

    /// <summary>
    /// The Visual Effect for the speed lines around the camera.
    /// </summary>
    [field: SerializeField]
    public VisualEffect SpeedLines
    { get; private set; }

    [field: SerializeField]
    public AnimationCurve SpeedLineCurve
    { get; private set; }

    /// <summary>
    /// The max rate that the speed lines can be spawned.
    /// </summary>
    [field: SerializeField, Min(0.1f)]
    public int MaxSpeedForVFXLines
    { get; private set; }

    /// <summary>
    /// The min length that the speed lines can be.
    /// </summary>
    [field: SerializeField, Min(0.1f)]
    public int MinLengthForVFXLines
    { get; private set; }

    /// <summary>
    /// The max length that the speed lines can be.
    /// </summary>
    [field: SerializeField, Min(0.1f)]
    public int MaxLengthForVFXLines
    { get; private set; }

    // Cached property IDs for Speed Lines Visual Effect.
    private static int RadiusID
    { get; set; } = Shader.PropertyToID("Radius");
    private static int SpeedID 
    { get; set; } = Shader.PropertyToID("Speed");
    private static int SpawnRateID 
    { get; set; } = Shader.PropertyToID("spawnRate");
    private static int XScaleRangeID
    { get; set; } = Shader.PropertyToID("XScaleRange");

    private void Awake()
    {
        if (TryGetComponent<CameraManager>(out CameraManager cameraManager))
        {
            CameraManager = cameraManager;
        }
        else
        {
            Debug.LogError("Unable to locate a camera manager on the camera object.");
        }

        InitialiseCurrentAircraftsValues();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpeedLines.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Update the speed lines effect.
        UpdateSpeedLines(
            SpeedLines.GetVector2(SpeedID).x, 
            SpeedLines.GetVector2(SpeedID).y,
            SpeedLines.GetFloat(RadiusID),
            SpeedLineCurve.Evaluate(GetNormalisedSpeed(MaxSpeedForVFXLines, CurrentAircraftValues.ValuesHolder.AirSpeed)) * MaxSpeedForVFXLines,
            Mathf.Clamp(SpeedLineCurve.Evaluate(GetNormalisedSpeed(MaxSpeedForVFXLines, CurrentAircraftValues.ValuesHolder.AirSpeed)) * MaxLengthForVFXLines, 0, MaxLengthForVFXLines),
            Mathf.Clamp(SpeedLineCurve.Evaluate(GetNormalisedSpeed(MaxSpeedForVFXLines, CurrentAircraftValues.ValuesHolder.AirSpeed)) * MaxLengthForVFXLines, 0, MaxLengthForVFXLines)
            );
    }

    public void UpdateSpeedLines(float speedX, float speedY, float spawnRadius, float spawnrate, float lengthXmin, float lengthXmax)
    {
        SpeedLines.SetVector2(SpeedID, new Vector3(speedX, speedY));
        SpeedLines.SetFloat(RadiusID, spawnRadius);
        SpeedLines.SetFloat(SpawnRateID, spawnrate);
        SpeedLines.SetVector2(XScaleRangeID, new Vector2(
            lengthXmin,
            lengthXmax
            ));
    }

    public void InitialiseCurrentAircraftsValues()
    {
        if (CameraManager != null && CameraManager.AssignedTarget != null)
        {
            if (CameraManager.AssignedTarget.TryGetComponent<AircraftCurrentValues>(out AircraftCurrentValues aircraftCurrentValues))
            {
                CurrentAircraftValues = aircraftCurrentValues;
            }
            else
            {
                Debug.LogWarning("Unable to locate the aircraft values script for the sake of reading values for informing the camera effects.");
            }
        }
    }

    /// <summary>
    /// Get the time on the animation curve through the normalised speed (based of max speed).
    /// </summary>
    /// <param name="maximumSpeed"></param>
    /// <param name="currentSpeed"></param>
    /// <param name="animationCurve"></param>
    /// <returns></returns>
    public float GetNormalisedSpeed(float maximumSpeed, float currentSpeed)
    {
        return (currentSpeed / maximumSpeed);
    }
}

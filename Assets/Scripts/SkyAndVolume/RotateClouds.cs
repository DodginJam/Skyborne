using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class RotateClouds : MonoBehaviour
{
    public Volume VolumeComp
    {  get; private set; }

    public VolumeProfile Profile
    {  get; private set; }

    public CloudLayer Clouds
    { get; private set; }

    [field: SerializeField]
    public float Speed
    { get; private set; } = 0.1f;

    public float CloudOffset
    { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<Volume>(out Volume volume))
        {
            VolumeComp = volume;

            Profile = Instantiate(VolumeComp.sharedProfile);

            VolumeComp.profile = Profile;
        }

        if (Profile != null)
        {
            if (!Profile.TryGet<CloudLayer>(out CloudLayer cloudLayer))
            {
                cloudLayer = Profile.Add<CloudLayer>(false);
            }

            Clouds = cloudLayer;

            CloudOffset = Clouds.layerA.rotation.value;
        }
        else
        {
            Debug.LogWarning("No VolumeProfile found on this object.");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Clouds != null)
        {
            CloudOffset += Speed * Time.deltaTime;

            // Prevent out of bounds of rotational limits.
            if (CloudOffset > 360)
            {
                CloudOffset -= 360;
            }
            else if (CloudOffset < 0)
            {
                CloudOffset += 360;
            }

            Clouds.layerA.rotation.value = CloudOffset;
        }
    }

    private void OnEnable()
    {
        ResetValues();
    }

    private void OnDisable()
    {
        ResetValues();
    }

    public void ResetValues()
    {
        if (Clouds != null)
        {
            Clouds.layerA.opacityB.value = 0.2f;

            Clouds.layerA.rotation.value = 2f;
        }
    }
}

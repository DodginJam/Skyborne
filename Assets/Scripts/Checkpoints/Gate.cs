using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IGlobalSoundOnCollect
{
    [field: SerializeField]
    public GameManager GameManagerScript
    {  get; private set; }

    [field: SerializeField]
    public AircraftController PlayerController 
    { get; private set; }

    [field: SerializeField]
    public GateSpawning GateSpawner 
    { get; private set; }

    [field: SerializeField]
    public int GatePointValue
    { get; private set; } = 100;

    public bool HasMissed     
    {  get; set; }

    [field: SerializeField]
    public SoundData_SO SoundCollect
    { get; set; }

    [field: SerializeField]
    public SoundData_SO SoundMiss
    { get; set; }

    private void Awake()
    {
        // Initialisation of references.
        if (GameManagerScript == null)
        {
            GameManagerScript = GameObject.FindObjectOfType<GameManager>();

            if (GameManagerScript == null)
            {
                Debug.LogError("Unable to locate the game manager script.");
            }
        }

        if (GateSpawner == null)
        {
            GateSpawner = GameObject.FindObjectOfType<GateSpawning>();

            if (GateSpawner == null)
            {
                Debug.LogError("Unable to locate the GateSpawning script.");
            }
        }

        if (PlayerController == null)
        {
            PlayerController = GameObject.FindObjectOfType<AircraftController>();

            if (PlayerController == null)
            {
                Debug.LogError("Unable to locate the AircraftController script.");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.back);
        Vector3 distanceToPlane = Vector3.Normalize(PlayerController.transform.position - transform.position);

        if (Vector3.Dot(forward, distanceToPlane) < 0)
        {
            HasMissed = true;
            GameManagerScript.IncreasePenalty(1);

            if (this is IGlobalSoundOnCollect soundOnCollect)
            {
                soundOnCollect.PlaySoundOnMiss();
            }

            gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.CompareTag("Player"))
        {
            GameManagerScript.IncreaseScore(GatePointValue);
            gameObject.SetActive(false);

            if (this is IGlobalSoundOnCollect soundOnCollect)
            {
                soundOnCollect.PlaySoundOnCollect();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.gameObject.CompareTag("Player"))
        {
            GameManagerScript.IncreaseScore(GatePointValue);
            gameObject.SetActive(false);

            if (this is IGlobalSoundOnCollect soundOnCollect)
            {
                soundOnCollect.PlaySoundOnCollect();
            }
        }
    }
}
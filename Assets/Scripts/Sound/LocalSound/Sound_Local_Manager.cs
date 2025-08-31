using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sound_Local_Manager : MonoBehaviour
{
    /// <summary>
    /// A list of all the local sounds being managed by a given gameobject, which are managed together in this manager.
    /// </summary>
    public List<Sound_Local> LocalSounds
    { get; private set; } = new List<Sound_Local>();

    protected virtual void Awake()
    {
        LocalSounds = GetComponentsInChildren<Sound_Local>().ToList();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}

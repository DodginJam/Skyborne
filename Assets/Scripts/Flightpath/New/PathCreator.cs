using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public Path Path_
    {  get; private set; }

    public void CreatePath()
    {
        Path_ = new Path(transform.position);
    }
}

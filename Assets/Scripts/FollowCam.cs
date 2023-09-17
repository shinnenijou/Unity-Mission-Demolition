using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set in Inspectpr")]
    public GameObject ground;
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ;
    public float initSize;

    private void Awake()
    {
        camZ = this.transform.position.z;
        initSize = Mathf.Abs(ground.transform.position.y);
        Camera.main.orthographicSize = initSize; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (POI == null)
        {
            return;
        }

        Vector3 destination = POI.transform.position;
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
       transform.position = destination;
        Camera.main.orthographicSize = destination.y + initSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

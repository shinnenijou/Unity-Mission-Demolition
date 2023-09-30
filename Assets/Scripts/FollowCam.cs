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
    public Vector3 initPos;
    public float initSize;

    private void Awake()
    {
        initPos = this.transform.position;
        initSize = Mathf.Abs(ground.transform.position.y);
        Camera.main.orthographicSize = initSize; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;

            if (POI.tag == "Projectile")
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = initPos.z;

        transform.position = destination;
        Camera.main.orthographicSize = destination.y + initSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static bool NearByZero(Vector3 vector)
    {
        return vector.magnitude < 0.0001f;
    }
}

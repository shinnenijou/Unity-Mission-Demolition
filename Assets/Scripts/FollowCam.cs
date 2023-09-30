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

        GameObject viewSlingshot = GameObject.Find("ViewSlingshot");
        POI = viewSlingshot;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        destination = POI.transform.position;

        if (POI.tag == "Projectile")
        {
            if (POI.GetComponent<Rigidbody>().IsSleeping())
            {
                Reset();
                return;
            }
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = initPos.z;

        transform.position = destination;
        Camera.main.orthographicSize = destination.y + initSize;
    }

    private void Reset()
    {
        GameObject viewSlingshot = GameObject.Find("ViewSlingshot");
        POI = viewSlingshot;
        Slingshot.instance.isAllowAiming = true;
    }
}

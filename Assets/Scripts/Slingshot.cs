using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static public Slingshot instance;

    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody projectileRigidbody;
    private float maxMagnitude;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPoint.transform.position;
        maxMagnitude = this.GetComponent<SphereCollider>().radius;
    }

    static public Vector3 LAUNCH_POS { 
        get 
        {
            if (instance == null)
            {
                return Vector3.zero;
            }

            return instance.launchPos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!aimingMode)
        {
            return;
        }

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - launchPos;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
            launchPoint.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        if (FollowCam.POI != null)
        {
            return;
        }

        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (FollowCam.POI != null)
        {
            return;
        }

        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (aimingMode)
        {
            return;
        }

        if (FollowCam.POI != null)
        {
            return;
        }

        aimingMode = true;

        projectile = Instantiate(prefabProjectile);

        projectile.transform.position = launchPos;

        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
}

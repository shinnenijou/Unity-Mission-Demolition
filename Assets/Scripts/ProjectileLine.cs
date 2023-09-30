using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    // Singleton
    static public ProjectileLine S;

    [Header("Set in Inspector")]
    public float minDist = 0.1f;
    
    public LineRenderer line;
    private GameObject _poi;
    public List<Vector3> points;

    private void Awake()
    {
        if (S != null)
        {
            return;
        }

        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();

    }

    public GameObject poi 
    { get 
        { 
            return _poi; 
        }

      set
        {
            _poi = value;
            if (_poi != null)
            {
                line.enabled = true;
                points = new List<Vector3>();
                AddPoint();
            }
        } 
    }

    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;

        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        }

        if (points.Count == 0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            points.Add(pt + launchPosDiff);
            points.Add(pt);

            line.positionCount = 2;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        }
        else
        {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;

        }
    }

    public Vector3 lastPoint { 
        get
        {
            if (points == null)
            {
                return Vector3.zero;
            }

            return points[points.Count - 1];
        }
    }

    private void FixedUpdate()
    {
        if (poi == null)
        {
            if (FollowCam.POI != null && FollowCam.POI.tag == "Projectile")
            { 
                poi = FollowCam.POI;
            }
            else
            {
                return;
            }

        }

        AddPoint();

        if (FollowCam.POI == null)
        {
            poi = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

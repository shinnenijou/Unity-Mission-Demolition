using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd,
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition instance;

    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    private const string SHOW_SLINGSHOT = "Show Slingshot";
    private const string SHOW_CASTLE = "Show Castle";
    private const string SHOW_BOTH = "Show Both";

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject go in gos)
        {
            Destroy(go);
        }

        castle = Instantiate(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken= 0;

        SwitchView(SHOW_SLINGSHOT);
        ProjectileLine.S.Clear();

        Goal.goalMet = false;
        Slingshot.instance.isAllowAiming = true;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        if (mode == GameMode.playing && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView(SHOW_BOTH);
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;

        if (level == levelMax)
        {
            level = 0;
        }

        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }

        showing = eView;

        switch (showing)
        {
            case SHOW_SLINGSHOT:
                FollowCam.POI = GameObject.Find("ViewSlingshot");
                uitButton.text = SHOW_CASTLE;
                break;
            case SHOW_CASTLE:
                FollowCam.POI = castle;
                uitButton.text = SHOW_BOTH;
                break;
            case SHOW_BOTH:
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = SHOW_SLINGSHOT;
                break;
            default:
                break;
        }
    }

    static public void ShotFired()
    {
        instance.shotsTaken++;
    }
}

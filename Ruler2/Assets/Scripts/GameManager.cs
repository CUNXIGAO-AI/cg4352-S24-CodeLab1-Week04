using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject myPlayer;
    public TextMeshProUGUI display;

    private float myPositionZ;
    
    private const string DATA_DIR = "/Data/";
    private const string DATA_P_FILE = "p.txt";
    private string DATA_FULL_P_FILE_PATH;

    public float MyPositionZ
    {
        get
        {
            if (File.Exists(DATA_FULL_P_FILE_PATH))
            {
                recentDistance = new List<float>();

                recentDistanceString = File.ReadAllText(DATA_FULL_P_FILE_PATH);

                recentDistanceString = recentDistanceString.Trim();

                string[] recentDistanceArray = recentDistanceString.Split("\n");

                for (int i = 0; i < recentDistanceArray.Length; i++)
                {
                    float currentDistance = float.Parse(recentDistanceArray[i]);
                    recentDistance.Add(currentDistance);
                }
                
                myPositionZ = float.Parse(recentDistanceArray[0]);
            }

            else
            {
                myPositionZ = 0;
            }
            return myPositionZ;
        }

        set
        {
            myPositionZ = value;
            
            recentDistance.Insert(0,myPositionZ);
            recentDistance = recentDistance.GetRange(0, 5);
            
            string fileContent = "";

            foreach (var distanceValue in recentDistance)
            {
                fileContent += distanceValue + "\n";
            }

            recentDistanceString = fileContent;
            
            if (!File.Exists(DATA_FULL_P_FILE_PATH))
            {
                Directory.CreateDirectory(Application.dataPath + DATA_DIR);
            }
            File.WriteAllText(DATA_FULL_P_FILE_PATH, fileContent);
        }
    }
    
    
    private string recentDistanceString = "";
    private List<float> recentDistance;

    private List<float> RecentDistance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DATA_FULL_P_FILE_PATH = Application.dataPath + DATA_DIR + DATA_P_FILE;
        
        Vector3 position = new Vector3(0f, 0f, MyPositionZ);
        myPlayer.transform.position = position;
        
        if (recentDistance == null)
        {
            recentDistance = new List<float>();

            recentDistanceString = File.ReadAllText(DATA_FULL_P_FILE_PATH);

            recentDistanceString = recentDistanceString.Trim();

            string[] recentDistanceArray = recentDistanceString.Split("\n");

            for (int i = 0; i < recentDistanceArray.Length; i++)
            {
                float currentDistance = float.Parse(recentDistanceArray[i]);
                recentDistance.Add(currentDistance);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        myPositionZ = myPlayer.transform.position.z;
        //Debug.Log("POSITION Z =" + myPositionZ + "\nPOSITION Z CONTENT =" + MyPositionZ);

        if (Input.GetKeyDown(KeyCode.S))
        {
            float currentPosition = myPositionZ;
            MyPositionZ = currentPosition;
            Debug.Log("Your position is saved" + recentDistanceString);
        }
        
        if(Input.GetKeyDown(KeyCode.L))
        {
            Vector3 position = new Vector3(0f, 0f, MyPositionZ);
            myPlayer.transform.position = position;
        }

        display.text = "YOUR SAVED POSITION IS:\n" + recentDistanceString;
    }
}

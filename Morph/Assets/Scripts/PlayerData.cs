using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State{
    Solid,
    Liquid,
    Gas
}

public class PlayerData : MonoBehaviour
{
    private static PlayerData _pd;
    public static PlayerData Pd { get { return _pd; } }

    // 
    [Header("Player Settings")]
    public Transform player;
    public float temperature;
    public float speed = 4f;
    public float jumpForce = 3f;
    public State state;
    public float gravityScale = 1.2f;
    public Vector3 lastCheckedPosition;
    public float lastCheckedTemperature;


    //
    [Header("Level Settings")]
    public int firstLevelIndex;
    public List<float> levelRoomTemperatures;
    public List<float> levelPlayerTemperatures;
    public RoomTemperature roomTemperature;



    private void Awake()
    {
        if (_pd != null && _pd != this)
        {
            Destroy(this.gameObject);
        } else {
            _pd = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }


    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ResetVariables(SceneManager.GetActiveScene().buildIndex);
    }


    private void ResetVariables(int levelIndex)
    {
        // Player Data
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if((levelIndex - firstLevelIndex)>= 0f && (levelIndex - firstLevelIndex) < levelPlayerTemperatures.Count){
            Debug.Log(levelIndex - firstLevelIndex);
            temperature = levelPlayerTemperatures[levelIndex - firstLevelIndex];
            //player.position = levelPlayerPostion[levelIndex];

            // Level Data
            try{
                roomTemperature = GameObject.FindWithTag("RoomTemperature").GetComponent<RoomTemperature>();
                roomTemperature.roomTemperature = levelRoomTemperatures[levelIndex - firstLevelIndex];
            }
            catch{
                Debug.Log("no room temperature");
            }
        }
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the one you want to reset the variables for
        ResetVariables(scene.buildIndex);
    }
    
    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            string curScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(curScene);
        }
        /*
        else if (Input.GetKeyDown(KeyCode.Q)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Z)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        */
    }

}

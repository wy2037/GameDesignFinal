using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get{ return _instance; }}
    private Canvas canvas;
    [SerializeField] private TextMeshProUGUI tempText;
    [SerializeField] private Transform solidTutorial;
    [SerializeField] private Transform liquidTutorial;
    [SerializeField] private Transform obstacleTutorial;
    [SerializeField] private Transform gasTutorial;
    [SerializeField] private Transform interactionTutorial;

    private float temperature = 70;

    private void Awake(){
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ResetVariables(SceneManager.GetActiveScene().buildIndex);

        canvas = GetComponent<Canvas>();
        //tempText = getComponent<TextMeshProUGUI>();
        tempText.text = temperature.ToString() + "°C";
    }

    // Update is called once per frame
    private void FixedUpdate() {
        temperature = PlayerData.Pd.temperature;
        tempText.text =   $"{PlayerData.Pd.temperature:.0} °C";

    }


    private void ResetVariables(int levelIndex)
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the one you want to reset the variables for
        ResetVariables(scene.buildIndex);
    }
    public void activateUI(int n){
        switch (n){
            case 0:
                scalePageUp(solidTutorial);
                break;
            case 1:
                scalePageUp(liquidTutorial);
                break;
            case 2:
                scalePageUp(obstacleTutorial);
                break;
            case 3:
                scalePageUp(gasTutorial);
                break;
            case 4:
                scalePageUp(interactionTutorial);
                break;
        }
    }


    private void scalePageUp(Transform obj){
        obj.gameObject.SetActive(true);
        obj.localScale = Vector3.zero;
        obj.DOScale(
            Vector3.one,
            0.3f
        )
        .SetEase(Ease.OutBack);
    }

    public void scalePageDown(Transform obj){
        obj.localScale = Vector3.one;
        obj.DOScale(
            Vector3.zero,
            0.3f
        )
        .SetEase(Ease.InBack)
        .OnComplete(()=>{
            obj.gameObject.SetActive(false);
        });
    }
}

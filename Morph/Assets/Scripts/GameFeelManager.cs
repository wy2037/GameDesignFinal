using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;
public class GameFeelManager : MonoBehaviour
{
    private static GameFeelManager _pm;
    public static GameFeelManager Pm { get{ return _pm; } }

    // camera
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    // pp component
    public Volume volume;
    [SerializeField]private Vignette vignette;
    [SerializeField]private ChromaticAberration chromaticAberration;
    [SerializeField]private LensDistortion lensDistortion;
    // Color
    public Color heatUpColor;
    public Color coolDownColor;
    public Color normalColor; 

    // float
    [SerializeField] private float lensDistortionIntensity = 0.2f;
    [SerializeField] private float vignetteIntensity = 0.55f;
    [SerializeField] private float chromaticAberrationIntensity = 0.3f;
    [SerializeField] private float zoomInOrthographicSize = 1f;
    [SerializeField] private float zoomOutOrthographicSize = 4f;

    private void Awake() {
        // init
        if (_pm != null && _pm != this)
        {
            Destroy(this.gameObject);
        } else {
            _pm = this;
        }
        DontDestroyOnLoad(this.gameObject);


        
    }

    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ResetVariables(SceneManager.GetActiveScene().buildIndex);
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.B)){
            heatUpEnter();
        }else if(Input.GetKeyDown(KeyCode.N)){
            coolDownEnter();
        }else if(Input.GetKeyDown(KeyCode.M)){
            normalEnter();
        }
    }

    private void ResetVariables(int levelIndex)
    {
        // post processing
        //volume = GameObject.FindGameObjectWithTag("PostProcessingVolume").GetComponent<Volume>();
        volume = transform.GetComponent<Volume>();
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        // camera
        try{
            virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        }
        catch{
            
        }
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the one you want to reset the variables for

        ResetVariables(scene.buildIndex);

    }
    public void heatUpEnter(){
        DOTween.Kill("normalEnter");
        DOTween.Kill("coolDownEnter");

        // change vignette intensity
        DOTween.To(()=> vignette.intensity.value, x=> vignette.intensity.value = x, vignetteIntensity, 2f)
        .SetId("heatUpEnter");
        // change color
        DOTween.To(()=> vignette.color.value, x => vignette.color.value = x, heatUpColor, 2f)
        .SetId("heatUpEnter");
        // change lens distortion
        DOTween.To(()=> lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, lensDistortionIntensity, 2f)
        .SetId("heatUpEnter");
        // change chromaticAberration
        DOTween.To(()=> chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 0.0f, 2f)
        .SetId("heatUpEnter");
        // camera shaking
        DOTween.To(()=> noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, 0.1f, 2f)
        .SetId("heatUpEnter");
    }

    public void normalEnter(){
        DOTween.Kill("heatUpEnter");
        DOTween.Kill("coolDownEnter");

        // change vignette intensity
        DOTween.To(()=> vignette.intensity.value, x=> vignette.intensity.value = x, 0.40f, 2f)
        .SetId("normalEnter");
        // change color
        DOTween.To(()=> vignette.color.value, x => vignette.color.value = x, normalColor, 2f)
        .SetId("normalEnter");
        // change lens distortion
        DOTween.To(()=> lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 0.1f, 0.4f)
        .SetId("normalEnter");
        // change chromaticAberration
        DOTween.To(()=> chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 0.0f, 0.4f)
        .SetId("normalEnter");
        // camera shaking
        DOTween.To(()=> noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, 0.0f, 2f)
        .SetId("normalEnter");
    }

    public void coolDownEnter(){
        DOTween.Kill("normalEnter");
        DOTween.Kill("heatUpEnter");


        // change vignette intensity
        DOTween.To(()=> vignette.intensity.value, x=> vignette.intensity.value = x, vignetteIntensity, 2f)
        .SetId("coolDownEnter");
        // change color
        DOTween.To(()=> vignette.color.value, x => vignette.color.value = x, coolDownColor, 2f)
        .SetId("coolDownEnter");
        // change lens distortion
        DOTween.To(()=> lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, -lensDistortionIntensity, 2f)
        .SetId("coolDownEnter");
        // change chromaticAberration
        DOTween.To(()=> chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, chromaticAberrationIntensity, 1f)
        .SetId("coolDownEnter");
        // camera shaking
        DOTween.To(()=> noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, 0.0f, 2f)
        .SetId("coolDownEnter");
    }

    public IEnumerator levelStart(){
        virtualCamera.m_Lens.OrthographicSize = zoomInOrthographicSize;
        yield return new WaitForSeconds(1.4f);
        DOTween.To(()=> virtualCamera.m_Lens.OrthographicSize, x => virtualCamera.m_Lens.OrthographicSize = x, zoomOutOrthographicSize, 1.2f)
        .SetEase(Ease.InOutSine);
    }

    public IEnumerator levelEnd(){
        DOTween.To(()=> virtualCamera.m_Lens.OrthographicSize, x => virtualCamera.m_Lens.OrthographicSize = x, zoomInOrthographicSize, 1.2f)
        .SetEase(Ease.InOutSine)
        .OnKill(()=>{
            Debug.Log("end pp is killed wtf");
        });
        yield return new WaitForSeconds(1.4f);
    }


    public void gethitPostProcessing(){
        chromaticAberration.intensity.value = 1f;
        DOTween.To(()=> chromaticAberration.intensity.value, x=> chromaticAberration.intensity.value = x, 0, 2.2f)
        .SetEase(Ease.OutSine);

    }
}

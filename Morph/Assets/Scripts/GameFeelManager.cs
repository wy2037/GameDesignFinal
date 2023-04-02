using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Cinemachine;

public class GameFeelManager : MonoBehaviour
{
    private static GameFeelManager _pm;
    public static GameFeelManager Pm { get{ return _pm; } }

    // camera
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    
    // pp component
    public Volume volume;
    private VolumeProfile volumeProfile;

    private Vignette vignette;

    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    // Color
    public Color heatUpColor;
    public Color coolDownColor;
    public Color normalColor; 

    // float


    private void Awake() {
        // init
        if (_pm != null && _pm != this)
        {
            Destroy(this.gameObject);
        } else {
            _pm = this;
        }
        DontDestroyOnLoad(this.gameObject);

        // camera

        // post processing
        volumeProfile = volume.sharedProfile;
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);

        
    }

    private void Start() {

        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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

    public void heatUpEnter(){
        DOTween.Kill("normalEnter");
        DOTween.Kill("coolDownEnter");

        // change vignette intensity
        DOTween.To(()=> vignette.intensity.value, x=> vignette.intensity.value = x, 0.55f, 2f)
        .SetId("heatUpEnter");
        // change color
        DOTween.To(()=> vignette.color.value, x => vignette.color.value = x, heatUpColor, 2f)
        .SetId("heatUpEnter");
        // change lens distortion
        DOTween.To(()=> lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, 0.3f, 2f)
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
        DOTween.To(()=> vignette.intensity.value, x=> vignette.intensity.value = x, 0.55f, 2f)
        .SetId("coolDownEnter");
        // change color
        DOTween.To(()=> vignette.color.value, x => vignette.color.value = x, coolDownColor, 2f)
        .SetId("coolDownEnter");
        // change lens distortion
        DOTween.To(()=> lensDistortion.intensity.value, x => lensDistortion.intensity.value = x, -0.3f, 2f)
        .SetId("coolDownEnter");
        // change chromaticAberration
        DOTween.To(()=> chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 0.3f, 1f)
        .SetId("coolDownEnter");
        // camera shaking
        DOTween.To(()=> noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, 0.0f, 2f)
        .SetId("coolDownEnter");
    }

    public void gethitPostProcessing(){
        chromaticAberration.intensity.value = 1f;
        DOTween.To(()=> chromaticAberration.intensity.value, x=> chromaticAberration.intensity.value = x, 0, 2.2f)
        .SetEase(Ease.OutSine);

    }
}

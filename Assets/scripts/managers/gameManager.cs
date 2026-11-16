using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public GameObject dimension1;
    public GameObject dimension2;
    public Volume globalVolume;

    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private float chromaticAbberationIntensity;
    private float lensDistortionIntensity;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public PlayerController player;
    [HideInInspector] public MeshRenderer playerMesh;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        mainCamera = Camera.main;
        globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        globalVolume.profile.TryGet<LensDistortion>(out lensDistortion);
        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    private void Start()
    {
        musicManager.instance.StartPlaying();
        musicManager.instance.ChangeDimenshionMusic();
    }
    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        DimenshionShiftEffects();
    }
    private void Update()
    {
        if(lensDistortionIntensity < 0f)
        {
            lensDistortionIntensity += Time.deltaTime;
            lensDistortion.intensity.value = lensDistortionIntensity;
        }
        else
        {
            lensDistortion.intensity.value = 0f;
        }
        if (chromaticAbberationIntensity > 0.1f)
        {
            chromaticAbberationIntensity -= Time.deltaTime;
            chromaticAberration.intensity.value = chromaticAbberationIntensity;
        }
        else
        {
            chromaticAberration.intensity.value = 0.1f;
        }
    }
    public void ChangeDimenshion()
    {
        dimension1.SetActive(!dimension1.activeSelf);
        dimension2.SetActive(!dimension2.activeSelf);
        mainCamera.backgroundColor = dimension1.activeSelf ? Color.white : Color.black;
        RenderSettings.fogColor = dimension1.activeSelf ? Color.white : Color.black;
        DimenshionShiftEffects();
        musicManager.instance.ChangeDimenshionMusic();

    }
    private void DimenshionShiftEffects()
    {
        chromaticAbberationIntensity = 1f;
        lensDistortionIntensity = -0.6f;
    }
    public void GameEnd()
    {
        player.SpawnDeathEffect();
        player.gameObject.SetActive(false);
        sceneManager.ReloadScene(2);
        musicManager.instance.playerDeath.Play();
        finalResultManager.deaths++;
    }
}

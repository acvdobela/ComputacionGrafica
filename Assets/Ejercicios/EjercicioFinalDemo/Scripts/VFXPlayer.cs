using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class VFXPlayer : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider mainColorSlider;
    [SerializeField] private Slider secondaryColorSlider;
    
    [SerializeField] private GameObject bonfire;
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject rock;

    [SerializeField] private PlayableDirector waterTimeline;
    [SerializeField] private PlayableDirector rockTimeline;

    private void Start()
    {
        SwitchEffect(Effect.Bonfire);
    }   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) SwitchEffect(Effect.Bonfire);
        if (Input.GetKeyDown(KeyCode.W)) SwitchEffect(Effect.Water);
        if (Input.GetKeyDown(KeyCode.E)) SwitchEffect(Effect.Rock);
    }

    private void SwitchEffect(Effect effect)
    {
        FeatureManager.Instance.CurrentEffect = effect;
        FeatureManager.Instance.SetMainColor(mainColorSlider.value);
        FeatureManager.Instance.SetSecondaryColor(secondaryColorSlider.value);
        switch (effect)
        {
            case Effect.Bonfire: HandleBonfire(); break;
            case Effect.Water: HandleWater(); break;
            case Effect.Rock: HandleRock(); break;
        }
    }
    
    private void HandleBonfire()
    {
        bonfire.SetActive(true);
        water.SetActive(false);
        rock.SetActive(false);
    }
    private void HandleWater()
    {
        bonfire.SetActive(false);
        water.SetActive(true);
        rock.SetActive(false);
        
        rockTimeline.Stop();
        waterTimeline.Play();
    }
    private void HandleRock()
    {
        bonfire.SetActive(false);
        water.SetActive(false);
        rock.SetActive(true);
        
        waterTimeline.Stop();
        rockTimeline.Play();
    }
    
    
}

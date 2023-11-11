using UnityEngine;
using UnityEngine.UI;

public enum Effect { Bonfire, Water, Rock }

public class FeatureManager : MonoBehaviour
{
    public static FeatureManager Instance;
    public Effect CurrentEffect;
    private bool firstColorChange;
    
    [Header("Bonfire")]
    [SerializeField] private ParticleSystem bonfire_fire_a;
    [SerializeField] private ParticleSystem bonfire_fire_b;
    [SerializeField] private ParticleSystem bonfire_debris;
    [SerializeField] private ParticleSystem bonfire_ember;
    [SerializeField] private ParticleSystem bonfire_fake_light;
    [SerializeField] private GameObject bonfire_sword_fire;
    [SerializeField] private Light bonfire_light;
    
    [Header("Water")]
    [SerializeField] private ParticleSystem water_cast_a;
    [SerializeField] private ParticleSystem water_cast_b;
    [SerializeField] private ParticleSystem water_bubbles_a;
    [SerializeField] private ParticleSystem water_bubbles_b;
    [SerializeField] private ParticleSystem water_smoke;
    [SerializeField] private ParticleSystem water_fake_light;
    [SerializeField] private ParticleSystem water_impact_a;
    [SerializeField] private ParticleSystem water_impact_b;
    [SerializeField] private ParticleSystem water_solid_color;
    [SerializeField] private ParticleSystem water_caustics;
    [SerializeField] private ParticleSystem water_caustics_sup;
    [SerializeField] private ParticleSystem water_fresnel;
    [SerializeField] private Material water_postprocess;
    
    [Header("Rock")]
    [SerializeField] private ParticleSystem rock_main_rocks;
    [SerializeField] private ParticleSystem rock_debris_a;
    [SerializeField] private ParticleSystem rock_debris_b;
    [SerializeField] private ParticleSystem rock_fake_light;
    [SerializeField] private ParticleSystem rock_flares;
    [SerializeField] private ParticleSystem rock_spawnzone_a;
    [SerializeField] private ParticleSystem rock_spawnzone_b;
    [SerializeField] private ParticleSystem rock_handglow;
    [SerializeField] private ParticleSystem rock_trail;
    [SerializeField] private ParticleSystem rock_impact;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        firstColorChange = false;
        SetColors();
    }

    private void SetColors()
    {
        CurrentEffect = Effect.Bonfire;
        SetMainColor(0.27f);
        SetSecondaryColor(0.35f);
        CurrentEffect = Effect.Water;
        SetMainColor(0.27f);
        SetSecondaryColor(0.35f);
        CurrentEffect = Effect.Rock;
        SetMainColor(0.27f);
        SetSecondaryColor(0.35f);
    }
    
    public void SetMainColor(float hue)
    {
        float h, s, v;
        float intensity = 4f;
        if (!firstColorChange) { intensity = 4f; firstColorChange = true;}
        else intensity = 1;
        
        if (CurrentEffect == Effect.Bonfire)
        {
            bonfire_light.color = Color.HSVToRGB(hue, 1, 1);
            var bonfireFakeLightMain = bonfire_fake_light.main;
            bonfireFakeLightMain.startColor = Color.HSVToRGB(hue, 1, 1);
            
            Color.RGBToHSV(bonfire_fire_a.GetComponent<ParticleSystemRenderer>().material
                .GetColor("_Fire_Color_B"), out h, out s, out v);
            bonfire_fire_a.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Fire_Color_B", Color.HSVToRGB(hue, s, v) * intensity);
            bonfire_fire_b.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Fire_Color_B", Color.HSVToRGB(hue, s, v) * intensity);
            bonfire_sword_fire.GetComponent<MeshRenderer>().materials[1]
                .SetColor("_Fire_Color_B", Color.HSVToRGB(hue, s, v) * intensity);
            
            var bonfireEmberMain = bonfire_ember.main;
            bonfireEmberMain.startColor = Color.HSVToRGB(hue, 1, 1);
        }
        
        if (CurrentEffect == Effect.Water)
        {
            Color.RGBToHSV(water_solid_color.GetComponent<ParticleSystemRenderer>().material
                .GetColor("_Water_Color_A"), out h, out s, out v);
            water_solid_color.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Water_Color_A", Color.HSVToRGB(hue, s, v) * intensity);
            water_caustics.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Water_Color_A", Color.HSVToRGB(hue, s, v) * intensity);
            water_caustics_sup.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Water_Color_A", Color.HSVToRGB(hue, s, v) * intensity);
            water_fresnel.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Water_Color_A", Color.HSVToRGB(hue, s, v) * intensity);
            water_bubbles_b.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Water_Color_A", Color.HSVToRGB(hue, s, v) * intensity);
            water_postprocess.SetColor("_Color_2", Color.HSVToRGB(hue, s, v) * intensity);
            
            var waterCastAMain = water_cast_a.main;
            waterCastAMain.startColor = Color.HSVToRGB(hue, 1, 1);
            var waterSmokeMain = water_smoke.main;
            waterSmokeMain.startColor = Color.HSVToRGB(hue, 1, 1);
            var waterFakeLightMain = water_fake_light.main;
            waterFakeLightMain.startColor = Color.HSVToRGB(hue, 1, 1);
        }
        
        if (CurrentEffect == Effect.Rock)
        {
            Color.RGBToHSV(rock_main_rocks.GetComponent<ParticleSystemRenderer>().material
                .GetColor("_BaseMap_Color"), out h, out s, out v);
            rock_main_rocks.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_BaseMap_Color", Color.HSVToRGB(hue, s, v) * intensity);
            var rockSpawnZoneBMain = rock_spawnzone_b.main;
            rockSpawnZoneBMain.startColor = Color.HSVToRGB(hue, 1, 1);
            var rockHandGlow = rock_handglow.main;
            rockHandGlow.startColor = Color.HSVToRGB(hue, 1, 1);
            var rockTrail = rock_trail.main;
            rockTrail.startColor = Color.HSVToRGB(hue, 1, 1);
        }
    }

    public void SetSecondaryColor(float hue)
    {
        float h, s, v;
        float intensity = 4f;
        if (!firstColorChange)
        {
            intensity = 4f;
            firstColorChange = true;
        }
        else intensity = 1;

        if (CurrentEffect == Effect.Bonfire)
        {
            var bonfireDebrisMain = bonfire_debris.main;
            bonfireDebrisMain.startColor = Color.HSVToRGB(hue, 1, 1);

            Color.RGBToHSV(bonfire_fire_a.GetComponent<ParticleSystemRenderer>().material
                .GetColor("_Fire_Color_A"), out h, out s, out v);
            bonfire_fire_a.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Fire_Color_A", Color.HSVToRGB(hue, s, v) * intensity);
            bonfire_fire_b.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Fire_Color_A", Color.HSVToRGB(hue, s, v) * intensity);
            bonfire_sword_fire.GetComponent<MeshRenderer>().materials[1]
                .SetColor("_Fire_Color_A", Color.HSVToRGB(hue, s, v) * intensity);
        }
        if (CurrentEffect == Effect.Water)
        {
            Color.RGBToHSV(water_solid_color.GetComponent<ParticleSystemRenderer>().material
                .GetColor("_Water_Color_B"), out h, out s, out v);
            water_solid_color.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Water_Color_B", Color.HSVToRGB(hue, s, v) * intensity);
            water_caustics.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Water_Color_B", Color.HSVToRGB(hue, s, v) * intensity);
            water_caustics_sup.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Water_Color_B", Color.HSVToRGB(hue, s, v) * intensity);
            water_bubbles_b.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Water_Color_B", Color.HSVToRGB(hue, s, v) * intensity);
            water_postprocess.SetColor("_Color", Color.HSVToRGB(hue, s, v) * intensity);
            
            var waterBubblesA = water_bubbles_a.main;
            waterBubblesA.startColor = Color.HSVToRGB(hue, 1, 1);
            var waterImpactAMain = water_impact_a.main;
            waterImpactAMain.startColor = Color.HSVToRGB(hue, 1, 1);
            var waterImpactBMain = water_impact_b.main;
            waterImpactBMain.startColor = Color.HSVToRGB(hue, 1, 1);
            var waterCastBMain = water_cast_b.main;
            waterCastBMain.startColor = Color.HSVToRGB(hue, 1, 1);
        }
        
        if (CurrentEffect == Effect.Rock)
        {
            Color.RGBToHSV(rock_main_rocks.GetComponent<ParticleSystemRenderer>().material
                .GetColor("_LightColor"), out h, out s, out v);
            rock_main_rocks.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_LightColor", Color.HSVToRGB(hue, s, v) * intensity);
            rock_main_rocks.GetComponent<ParticleSystemRenderer>().material
                .SetColor("_Energy_Color", Color.HSVToRGB(hue, s, v) * intensity);
            
            var rockSpawnZoneAMain = rock_spawnzone_a.main;
            rockSpawnZoneAMain.startColor = Color.HSVToRGB(hue, 1, 1);
            var rockFakeLight = rock_fake_light.main;
            rockFakeLight.startColor = Color.HSVToRGB(hue, 1, 1);
            var rockDebrisA = rock_debris_a.main;
            rockDebrisA.startColor = Color.HSVToRGB(hue, 1, 1);
            var rockDebrisB = rock_debris_b.main;
            rockDebrisB.startColor = Color.HSVToRGB(hue, 1, 1);
            var rockFlares = rock_flares.main;
            rockFlares.startColor = Color.HSVToRGB(hue, 1, 1);
            var rockImpact = rock_impact.main;
            rockImpact.startColor = Color.HSVToRGB(hue, 1, 1);
        }
    }
}

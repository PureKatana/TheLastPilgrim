using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0,24)] private float timeOfDay;
    [SerializeField] private float multiplier;

    public float TimeOfDay { get => timeOfDay; set => timeOfDay = value; }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        if(RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }

    private void UpdateLighting(float time)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(time);
        //RenderSettings.fogColor = preset.fogColor.Evaluate(time);

        if(directionalLight != null)
        {
            directionalLight.color = preset.directionalColor.Evaluate(time);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((time * 360f) - 90f, 170f, 0f));
        }
    }

    private void Update()
    {
        if (preset == null)
            return;
        if(Application.isPlaying)
        {
            timeOfDay += Time.deltaTime * multiplier;
            timeOfDay %= 24; //Varies between 0 and 24 hours
            UpdateLighting(timeOfDay/ 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvironmentInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI zoneText;
    [SerializeField] private GameObject climateEffect;
    [SerializeField] private string title;
    [SerializeField] private float startTimeClimateEffect;
    [SerializeField] private float endTimeClimateEffect;
    [SerializeField] private string hexaColor;
    [SerializeField] private float fogDensity;
    private Color fogColor;

    private void Start()
    {
        ColorUtility.TryParseHtmlString(hexaColor, out fogColor);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ZoneText());
        }
        
    }
    public virtual void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            if (this.climateEffect == null)
                return;
            if (GameObject.Find("Day/Night Cycle").GetComponent<LightingManager>().TimeOfDay >= startTimeClimateEffect &&
                GameObject.Find("Day/Night Cycle").GetComponent<LightingManager>().TimeOfDay <= endTimeClimateEffect)
                this.climateEffect.SetActive(true);
            else
                this.climateEffect.SetActive(false);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RenderSettings.fogColor = Color.white;
            RenderSettings.fogDensity = 0.01f;
            if (climateEffect == null)
                return;
            climateEffect.SetActive(false);
        }
    }

    private void Update()
    {
        
    }

    private IEnumerator ZoneText()
    {
        zoneText.enabled = true;
        zoneText.text = title;
        zoneText.canvasRenderer.SetAlpha(0f);
        zoneText.CrossFadeAlpha(1f, 2, true);
        yield return new WaitForSeconds(2f);
        zoneText.CrossFadeAlpha(0f, 2, true);
        StartCoroutine(SetText());
    }

    private IEnumerator SetText()
    {
        yield return new WaitForSeconds(2f);
        zoneText.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class SetGraphics : MonoBehaviour
{
    public RenderPipelineAsset[] qualityLevels;
    public TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.value = QualitySettings.GetQualityLevel();
    }

    public void SetGFX(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevels[value];
    }
}

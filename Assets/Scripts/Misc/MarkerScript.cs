using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MarkerScript : MonoBehaviour
{
    [SerializeField] public RectTransform prefab;
    [SerializeField] public GameObject container;
    [SerializeField] public Transform player;
    private Image prefabImage;
    private TextMeshProUGUI distance;
    private RectTransform marker;

    private Vector3 offset = new Vector3(0, 1.25f, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        marker = Instantiate(prefab, container.transform);
        distance = marker.GetComponentInChildren<TextMeshProUGUI>();
        prefabImage = marker.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.main != null)
        {
            var screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
            marker.position = screenPos;

            distance.text = Vector3.Distance(player.position, transform.position).ToString("0") + " m";
            marker.gameObject.SetActive(screenPos.z > 0);
        }

        if(Vector3.Distance(player.position, transform.position) >= 50)
        {
            distance.enabled = false;
            prefabImage.enabled = false;
        }
        else
        {
            distance.enabled = true;
            prefabImage.enabled = true;
        }
    }
}

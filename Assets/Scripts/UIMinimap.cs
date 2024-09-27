using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIMinimap : MonoBehaviour
{
    [SerializeField]
    private Camera minimapCamera;
    [SerializeField]
    private float zoomMin = 1;
    [SerializeField]
    private float zoomMax = 30;
    [SerializeField]
    private float zoomOneStep = 1;
    [SerializeField]
    private TextMeshProUGUI textMapName;

    private void Awake()
    {
        textMapName.text = SceneManager.GetActiveScene().name;
    }

    public void ZoomIn()
    {
        minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize-zoomOneStep, zoomMin); 
    }

    public void ZoomOut()
    {
        minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize+zoomOneStep, zoomMax);
    }
}

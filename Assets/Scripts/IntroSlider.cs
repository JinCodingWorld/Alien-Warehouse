using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSlider : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider.enabled = true;
        slider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value += Time.deltaTime;

        if(slider.value == slider.maxValue)
        {
            SceneManager.LoadScene(1);
        }
    }
}

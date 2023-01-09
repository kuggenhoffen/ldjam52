using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    [SerializeField]
    Slider sensitivitySlider;

    // Start is called before the first frame update
    void Start()
    {
        float sens = PlayerPrefs.GetFloat("sensitivity", 1f);
        sensitivitySlider.value = sens;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnValueChangeSensitivity(float value)
    {
        PlayerPrefs.SetFloat("sensitivity", value);
    }
}

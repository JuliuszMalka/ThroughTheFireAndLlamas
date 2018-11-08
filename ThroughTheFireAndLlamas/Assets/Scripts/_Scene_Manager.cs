using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Scene_Manager : MonoBehaviour
{

    public GameObject MainPanel;
    public GameObject HelpPanel;
    public GameObject OptionsPanel;
    public GameObject PlayNConnPanel;
	// Use this for initialization
	void Start ()
    {
        MainPanel.SetActive(true);
        HelpPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        PlayNConnPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey("escape"))
        {
            MainPanel.SetActive(true);
            HelpPanel.SetActive(false);
            OptionsPanel.SetActive(false);
            PlayNConnPanel.SetActive(false);
        }
	}

    public void HelpButton()
    {
        MainPanel.SetActive(false);
        HelpPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        PlayNConnPanel.SetActive(false);
    }

    public void OptionsButton()
    {
        MainPanel.SetActive(false);
        HelpPanel.SetActive(false);
        OptionsPanel.SetActive(true);
        PlayNConnPanel.SetActive(false);
    }

    public void PlayButton(string sceneToChangeTo)
    {
        MainPanel.SetActive(false);
        HelpPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        PlayNConnPanel.SetActive(true);
        SceneManager.LoadScene(sceneToChangeTo);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}

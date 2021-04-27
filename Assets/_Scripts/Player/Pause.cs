using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Slider volume;
    public Slider sensitivity;
    [Space]
    public FPSPlayer sens;
    [Space]
    public GameObject pauseMenu;
    public bool paused;

    void Update()
    {
        if (Input.GetKeyDown(sens.pc.pause))
        {
            paused = !paused;
            if (paused)
            {
                ShowPause();
            }
            else
            {
                HidePause();
            }
        }
    }

    public void ShowPause()
    {
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
    }

    public void HidePause()
    {
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
    }
    
    public void ChangeVolume()
    {
        AudioListener.volume = volume.value;
    }

    public void ChangeSensitivity()
    {
        sens.mouseSensitivityX = (200 * sensitivity.value);
        sens.mouseSensitivityY = (1 * sensitivity.value);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

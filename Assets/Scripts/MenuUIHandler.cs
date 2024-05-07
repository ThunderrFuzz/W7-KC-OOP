using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public ColorPicker ColorPicker;

    public void NewColorSelected(Color color)
    {
        // add code here to handle when a color is selected

        GameManager.Instance.unitColor = color;

    }

    private void Start()
    {
        ColorPicker.Init();
        //this will call the NewColorSelected function when the color picker have a color button clicked.
        ColorPicker.onColorChanged += NewColorSelected;
        ColorPicker.SelectColor(GameManager.Instance.unitColor);

    }
    public void startButton(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void quitGame()
    {
        GameManager.Instance.saveColorData();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void saveClickedColour()
    {
        GameManager.Instance.saveColorData();
    }
    public void loadClickedColour()
    {
        GameManager.Instance.loadColorData();
        ColorPicker.SelectColor(GameManager.Instance.unitColor);
    }






    
}




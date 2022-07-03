using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Camera MainCamera;
    public Button PhotoButton;
    public Button RunModelButton;
    public TextMeshProUGUI PhotoCounter;
    public static int NumPhoto = 0;
    public static bool willTakeScreenShot = false;

    public void TakePhoto()
    {
        willTakeScreenShot = true;
        PhotoCounter.text = (NumPhoto+1).ToString() + " / 50";
    }

    public void RunModel()
    {
        
    }

    void Awake()
    {
        PhotoCounter.text = NumPhoto.ToString() + " / 50";
        PhotoButton.GetComponent<Button>().interactable = true;
        RunModelButton.GetComponent<Button>().interactable = false;
    }

    void Update()
    {
        if (NumPhoto>=50) {
            PhotoButton.GetComponent<Button>().interactable = false;
            RunModelButton.GetComponent<Button>().interactable = true;
        }
    }
}

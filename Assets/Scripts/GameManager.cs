using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Camera MainCamera;
    public Button PhotoButton;
    public Button RunModelButton;
    public TextMeshProUGUI PhotoCounter;
    public int NumPhoto = 0;
    public bool willTakeScreenShot = false;

    [Serializable]
    public class FrameInfo
    {
        public string file_path;
        public float transform_matrix;
    }

    [Serializable]
    public class JsonInfo
    {
        public float camera_angle_x;
        public List<FrameInfo> frames;
    }

    public JsonInfo train_json = new JsonInfo();
    public List<FrameInfo> train_frames = new List<FrameInfo>();
    public JsonInfo val_json = new JsonInfo();
    public List<FrameInfo> val_frames = new List<FrameInfo>();
    public JsonInfo test_json = new JsonInfo();
    public List<FrameInfo> test_frames = new List<FrameInfo>();

    public IEnumerator capture()
    {
        yield return new WaitForEndOfFrame();
        FrameInfo frameinfo = new FrameInfo();
        if (NumPhoto<50) {
            string pFileName = $"/Users/parkjongjun/Github Projects/nerf-pytorch/data/temp_model/train/r_{NumPhoto+1}";
            frameinfo.file_path = pFileName;
            zzTransparencyCapture.captureScreenshot(pFileName+".png");
            frameinfo.transform_matrix = 0f;
            train_json.frames.Add(frameinfo);
        }
        else if (NumPhoto<100) {
            string pFileName = $"/Users/parkjongjun/Github Projects/nerf-pytorch/data/temp_model/val/r_{NumPhoto-49}";
            frameinfo.file_path = pFileName;
            zzTransparencyCapture.captureScreenshot(pFileName+".png");
            frameinfo.transform_matrix = 0f;
            train_json.frames.Add(frameinfo);
        }
        else {
            string pFileName = $"/Users/parkjongjun/Github Projects/nerf-pytorch/data/temp_model/test/r_{NumPhoto-99}";
            frameinfo.file_path = pFileName;
            zzTransparencyCapture.captureScreenshot(pFileName+".png");
            frameinfo.transform_matrix = 0f;
            train_json.frames.Add(frameinfo);
        }
        NumPhoto += 1;
    }

    public void TakePhoto()
    {
        if (NumPhoto < 50) {
            willTakeScreenShot = true;
            PhotoCounter.text = "Train\n" + (NumPhoto+1).ToString() + " / 50";
        }
        else if (NumPhoto>=50 && NumPhoto<100) {
            willTakeScreenShot = true;
            PhotoCounter.text = "Val\n" + (NumPhoto-49).ToString() + " / 50";
        }
        else if (NumPhoto>=100 && NumPhoto<200) {
            willTakeScreenShot = true;
            PhotoCounter.text = "Test\n" + (NumPhoto-99).ToString() + " / 100";
        }
        else {
            willTakeScreenShot = false;
            PhotoCounter.text = "Done. Run Model";
        }
    }

    public void RunModel()
    {
        //run nerf model
    }

    void JsonSave()
    {
        string train_path = $"/Users/parkjongjun/Github Projects/nerf-pytorch/data/temp_model/transforms_train.json";
        string val_path = $"/Users/parkjongjun/Github Projects/nerf-pytorch/data/temp_model/transforms_val.json";
        string test_path = $"/Users/parkjongjun/Github Projects/nerf-pytorch/data/temp_model/transforms_test.json";

        string train_save = JsonUtility.ToJson(train_json, true);
        string val_save = JsonUtility.ToJson(val_json, true);
        string test_save = JsonUtility.ToJson(test_json, true);

        File.WriteAllText(train_path, train_save);
        File.WriteAllText(val_path, val_save);
        File.WriteAllText(test_path, test_save);
    }

    void Awake()
    {
        PhotoCounter.text = NumPhoto.ToString() + " / 50";
        PhotoButton.GetComponent<Button>().interactable = true;
        RunModelButton.GetComponent<Button>().interactable = false;
        float radAngle = MainCamera.fieldOfView * Mathf.Deg2Rad;
        float radHFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * MainCamera.aspect);

        train_json.camera_angle_x = radHFOV;
        val_json.camera_angle_x = radHFOV;
        test_json.camera_angle_x = radHFOV;

        train_json.frames = train_frames;
        val_json.frames = val_frames;
        test_json.frames = test_frames;
    }

    void Update()
    {
        if (NumPhoto>=200) {
            PhotoButton.GetComponent<Button>().interactable = false;
            RunModelButton.GetComponent<Button>().interactable = true;
            JsonSave();
        }
        if (willTakeScreenShot) {
            StartCoroutine(capture());
            willTakeScreenShot = false;
        }
    }
}

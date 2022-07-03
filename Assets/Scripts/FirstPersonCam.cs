using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FirstPersonCam : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    private float xRotate = 0.0f;
    public float moveSpeed = 4.0f;

    private void OnPostRender()
    {
        if (GameManager.willTakeScreenShot)
        {
            GameManager.willTakeScreenShot = false;
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();
            File.WriteAllBytes($"{Application.dataPath}/ScreenShots/r_{GameManager.NumPhoto}.png", texture.EncodeToPNG());
            GameManager.NumPhoto += 1;
            Destroy(texture);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;

        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        Vector3 move = 
            transform.forward * Input.GetAxis("Vertical") + 
            transform.right * Input.GetAxis("Horizontal");

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}

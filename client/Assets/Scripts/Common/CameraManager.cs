using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera cameraTank;
    public Camera cameraScene;
    public Camera cameraDown;
    public bool cameraPurseMissile = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToTank()
    {
        cameraDown.gameObject.SetActive(false);
        cameraScene.gameObject.SetActive(false);
        cameraTank.gameObject.SetActive(true);
    }

    public void SwitchToScene()
    {
        cameraDown.gameObject.SetActive(false);
        cameraScene.gameObject.SetActive(true);
        cameraTank.gameObject.SetActive(false);
    }

    public void SwitchToDown()
    {
        cameraDown.gameObject.SetActive(true);
        cameraScene.gameObject.SetActive(false);
        cameraTank.gameObject.SetActive(false);
    }

    public void SwitchToMissile()
    {
        cameraPurseMissile = !cameraPurseMissile;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public WeaponLauncher[] launcherList;
    public CameraManager cameraMgr = null;
    
    // Start is called before the first frame update
    void Start()
    {
        launcherList = this.gameObject.GetComponentsInChildren<WeaponLauncher>();
        cameraMgr = this.gameObject.GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        //launcher.Shoot();
        for(int i=0; i<launcherList.Length; i++)
        {
            var launcher = launcherList[i];
            var bullet = launcher.LaunchMissile();
            if(cameraMgr.cameraPurseMissile)
            {
                var cameraObj = bullet.gameObject.transform.Find("camera");
                var camera = cameraObj.GetComponent<Camera>();
                if (camera != null)
                    camera.enabled = true;
                
            }

        }
    }
}

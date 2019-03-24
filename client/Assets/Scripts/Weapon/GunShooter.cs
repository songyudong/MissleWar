using UnityEngine;
using System.Collections;

public class GunShooter : MonoBehaviour
{
    public GameObject[] Guns;
    public int CurrentGun;
    public bool Shooting;
    public int CoolDown = 0;

    private void Start()
    {
        CurrentGun = 0;        
    }
	
    private void Update()
    {        
        if (Input.GetMouseButtonUp(0))
        {
            Shooting = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shooting = true;
        }


        if (Shooting)
        {
            //Guns[CurrentGun].gameObject.GetComponent<WeaponLauncher>().Shoot(); // Just Call Shoot(); the gun will fire.
        }
    }
}

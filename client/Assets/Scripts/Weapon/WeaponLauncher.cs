using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WeaponLauncher : MonoBehaviour
{

    public GameObject missile;
    public int noise;
    public GameObject Owner;
    public GameObject Flash;
    public int FlashSize = 4;
    public float fireRate = 0.1f;
    private float nextFireTime = 0.0f;
    private int timeFlash = 0;
    public AudioClip[] SoundGun;
    public int ForceShoot = 0;
    public int NumBullet = 0;
    public Texture2D targetlockontexture;
    public Texture2D targetlockedtexture;
    private GameObject target;
    private bool locked;
    public float distanceLock = 200;
    public float timetolock = 2;
    private float timetolockcount = 0;
    public string targettag = "Enemy";
    public float aimdirection = 0.8f;
    public bool Seeker;
	public GameObject Shell;
	public Transform ShellOuter;
	public int ShellOutForce = 300;
	
    private void Start()
    {
		if(!Owner){
			Owner = this.gameObject;
		}
        nextFireTime = 0.0f;
        timetolockcount = Time.time;
    }

    private void Update()
    {
        if (Flash)
        {
            if (timeFlash > 0)
            {
                timeFlash -= 1;
            }
            else
            {
                Flash.GetComponent<Renderer>().enabled = false;
                if (Flash.GetComponent<Light>())
                    Flash.GetComponent<Light>().enabled = false;
            }
        }

        if (Seeker)
        {
            if (GameObject.FindGameObjectsWithTag(targettag).Length > 0)
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag(targettag);
                float distance = int.MaxValue;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i])
                    {
                        Vector3 dir = (objs[i].transform.position - transform.position).normalized;
                        float direction = Vector3.Dot(dir, transform.forward);
                        if (direction >= aimdirection)
                        {
                            if (timetolockcount + timetolock < Time.time)
                            {
                                if (!locked)
                                {
                                    float dis = Vector3.Distance(objs[i].transform.position, transform.position);
                                    if (distanceLock > dis)
                                    {
                                        if (distance > dis)
                                        {
                                            distance = dis;
                                            target = objs[i];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (target)
                {
                    locked = true;
                    float targetdistance = Vector3.Distance(transform.position, target.transform.position);
                    if (targetdistance > distanceLock)
                    {
                        Unlock();
                    }
                }


            }
        }

    }

    private void DrawTargetLockon(Texture2D locktexture, Transform aimtarget, bool locked)
    {
        if (locktexture && GameObject.Find("Main Camera"))
        {
            Vector3 screenPos =
                GameObject.Find("Main Camera").gameObject.GetComponent<Camera>().WorldToScreenPoint(aimtarget.transform.position);
            float distance = Vector3.Distance(transform.position, aimtarget.transform.position);
            GUI.DrawTexture(new Rect(screenPos.x - 32, Screen.height - screenPos.y - 32, 64, 64), locktexture);
            GUI.Label(new Rect(screenPos.x + 40, Screen.height - screenPos.y, 200, 30),
                      "Target " + Mathf.Floor(distance) + "m.");
        }
        else
        {
            Debug.Log(
                "Can't Find (Main Camera), you can fix here by changing the (Main Camera) to any camera name you have.");
        }
    }

    private void OnGUI()
    {
        if (Seeker)
        {
            if (locked)
            {
                if (target)
                {
                    DrawTargetLockon(targetlockedtexture, target.transform, true);
                }
            }
            if (GameObject.FindGameObjectsWithTag(targettag).Length > 0)
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag(targettag);
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i])
                    {
                        Vector3 dir = (objs[i].transform.position - transform.position).normalized;
                        float direction = Vector3.Dot(dir, transform.forward);
                        if (direction >= aimdirection)
                        {
                            float dis = Vector3.Distance(objs[i].transform.position, transform.position);
                            if (distanceLock > dis)
                            {
                                DrawTargetLockon(targetlockontexture, objs[i].transform, true);
                            }
                        }
                    }
                }
            }
        }

    }

    private void Unlock()
    {
        // Disable Target
        locked = false;
        timetolockcount = Time.time;
        target = null;
    }

    public GameObject LaunchMissile()
    {
        if (SoundGun.Length > 0)
        {
            AudioSource.PlayClipAtPoint(SoundGun[Random.Range(0, SoundGun.Length)], transform.position);
        }

        Vector3 noisGun =
                    new Vector3(Random.Range(-noise, noise), Random.Range(-noise, noise), Random.Range(-noise, noise)) /
                    200;
        Vector3 direction = transform.TransformDirection(Vector3.forward + noisGun);
        Quaternion rotate = transform.rotation;

        GameObject bullet = (GameObject)Instantiate(missile, transform.position, rotate);
        if (bullet.GetComponent<MoverMissile>())
        {
            bullet.GetComponent<MoverMissile>().target = target;
        }
        if(bullet.GetComponent<MoverBallistic>())
        {
            bullet.GetComponent<MoverBallistic>().target = target;
        }
        if (bullet.GetComponent<Damage>())
        {
            bullet.GetComponent<Damage>().Owner = Owner;
        }
        if (bullet.GetComponent<Rigidbody>())
        {
            bullet.GetComponent<Rigidbody>().AddForce(direction * ForceShoot);
        }

        if (Shell && ShellOuter)
        {
            GameObject shell = (GameObject)Instantiate(Shell, ShellOuter.position, Random.rotation);
            GameObject.Destroy(shell.gameObject, 4);
            if (shell.GetComponent<Rigidbody>())
            {
                shell.GetComponent<Rigidbody>().AddForce(ShellOuter.forward * ShellOutForce);
            }
        }


        timeFlash = 1;
        if (Flash)
        {
            Flash.GetComponent<Renderer>().enabled = true;
            if (Flash.GetComponent<Light>())
                Flash.GetComponent<Light>().enabled = true;

            Flash.transform.localScale = new Vector3(Random.Range(FlashSize / 2, FlashSize) / 100.0f,
                                                     Flash.transform.localScale.x, Flash.transform.localScale.x);
        }

        return bullet;
    }

    public void Shoot()
    {
        if (Time.time > nextFireTime + fireRate)
        {

            nextFireTime = Time.time;
            if (SoundGun.Length > 0)
            {
                AudioSource.PlayClipAtPoint(SoundGun[Random.Range(0, SoundGun.Length)], transform.position);
            }

            for (int i = 0; i < NumBullet; i++)
            {

                Vector3 noisGun =
                    new Vector3(Random.Range(-noise, noise), Random.Range(-noise, noise), Random.Range(-noise, noise))/
                    200;
                Vector3 direction = transform.TransformDirection(Vector3.forward + noisGun);
                Quaternion rotate = transform.rotation;

                GameObject bullet = (GameObject) Instantiate(missile, transform.position, rotate);
                if (bullet.GetComponent<MoverMissile>())
                {
                    bullet.GetComponent<MoverMissile>().target = target;
                }
                if (bullet.GetComponent<MoverBallistic>())
                {
                    bullet.GetComponent<MoverBallistic>().target = target;
                }
                if (bullet.GetComponent<Damage>())
                {
                    bullet.GetComponent<Damage>().Owner = Owner;
                }
                if (bullet.GetComponent<Rigidbody>())
                {
                    bullet.GetComponent<Rigidbody>().AddForce(direction*ForceShoot);
                }
            }
            Unlock();

			if(Shell && ShellOuter){
    			GameObject shell = (GameObject)Instantiate(Shell, ShellOuter.position, Random.rotation);
    			GameObject.Destroy(shell.gameObject,4);
    			if(shell.GetComponent<Rigidbody>()){
    				shell.GetComponent<Rigidbody>().AddForce(ShellOuter.forward * ShellOutForce);
    			}
    		}


            timeFlash = 1;
            if (Flash)
            {
                Flash.GetComponent<Renderer>().enabled = true;
                if (Flash.GetComponent<Light>())
                    Flash.GetComponent<Light>().enabled = true;

                Flash.transform.localScale = new Vector3(Random.Range(FlashSize/2, FlashSize)/100.0f,
                                                         Flash.transform.localScale.x, Flash.transform.localScale.x);
            }
            nextFireTime += fireRate;
        }

    }

}

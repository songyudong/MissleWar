using UnityEngine;
using System.Collections;

public class Unit_Helicopter : MonoBehaviour 
{
    public GameObject[] m_objDest;
    public float m_fSpeed = 100;

    public GameObject[] Guns;    
    public bool Shooting;
    public int CoolDown = 0;

	// Use this for initialization
	void Start () 
    {        

        m_objDest[0] = GameObject.Find("Heli_Dest1").gameObject;
        m_objDest[1] = GameObject.Find("Heli_Dest2").gameObject;
        m_objDest[2] = GameObject.Find("Heli_Dest3").gameObject;
        m_objDest[3] = GameObject.Find("Heli_Dest4").gameObject;
        StartCoroutine(DoPatrol());
        StartCoroutine(DoShoot());
        StartCoroutine(DoSeek());
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    IEnumerator DoPatrol()
    {
        while(true)
        {
            for( int i=0; i<m_objDest.Length; i++ )
            {
                yield return StartCoroutine(DoMoveTo(m_objDest[i]));
            }
        }
    }

    IEnumerator DoMoveTo(GameObject objDest)
    {
        while(true)
        {
            Vector3 vSelf = transform.position;
            Vector3 vDest = objDest.transform.position;
            Vector3 vDir = (vDest - vSelf).normalized;
            float fDistance = (vDest - vSelf).magnitude;
            if(fDistance < m_fSpeed*Time.deltaTime)
            {
                transform.position = vDest;
                yield break;
            }
            transform.position = vSelf + vDir * m_fSpeed * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(-vDir);
            yield return 0;
        }
    }

    IEnumerator DoShoot()
    {
        while(true)
        {
            
            Guns[0].gameObject.GetComponent<WeaponLauncher>().Shoot();
            Guns[1].gameObject.GetComponent<WeaponLauncher>().Shoot();            
                
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator DoSeek()
    {
        while(true)
        {
            if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
                GameObject target = null;
                float distance = float.MaxValue;
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i])
                    {
                        float dis = Vector3.Distance(objs[i].transform.position, transform.position);

                        if (distance > dis)
                        {
                            distance = dis;
                            target = objs[i];
                        }
                    }
                }

                Vector3 vDirL = (target.transform.position - Guns[0].transform.position).normalized;                
                Guns[0].transform.rotation = Quaternion.LookRotation(vDirL);
                Vector3 vDirR = (target.transform.position - Guns[1].transform.position).normalized;
                Guns[1].transform.rotation = Quaternion.LookRotation(vDirR);

            }


            yield return 0;
        }
    }
}

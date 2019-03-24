using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
    public GameObject m_prefabUnit;
	// Use this for initialization
	void Start () 
    {
        if(m_prefabUnit)
        {
            GameObject obj = Instantiate(m_prefabUnit, transform.position, transform.rotation) as GameObject;
            obj.transform.parent = this.transform;

            StartCoroutine(DoCheckSpawn());
        }        
	}
	
	// Update is called once per frame
	void Update () 
    {       

	}

    IEnumerator DoCheckSpawn()
    {
        while(true)
        {
            if (this.transform.childCount == 0)
            {
                GameObject obj = Instantiate(m_prefabUnit, transform.position, transform.rotation) as GameObject;
                obj.transform.parent = this.transform;
            }
            
            yield return new WaitForSeconds(10);
        }        
    }
}

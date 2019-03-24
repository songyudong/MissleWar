using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameInit : MonoBehaviour 
{
    
    void Awake()
    {

    }

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        if (SceneManager.GetActiveScene().name == "login")
        {
            return;
        }
        SceneManager.LoadScene("login");
    }



    void Init()
    {
        StopCoroutine("DoInit");
        StartCoroutine("DoInit");


    }

    IEnumerator DoInit()
    {
        return null;
    }

    

	// Use this for initialization
	void Start () 
    {
        Init();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}


}

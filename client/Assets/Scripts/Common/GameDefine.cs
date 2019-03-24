using UnityEngine;
using System.Collections;

public class GameDefine : MonoBehaviour 
{
    public static int INVALID_ID = -1;
    public const string ServerName = "127.0.0.1";
    public static string GetHost()
    {
        return ServerName;
    }

    public static int GetPort()
    {
        return 3500;
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}

using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    private bool _isInMenu;

	void Start()
    {
	   _isInMenu = true;
	}
	
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Application.LoadLevel("TestScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Application.Quit();
        }
	}

}

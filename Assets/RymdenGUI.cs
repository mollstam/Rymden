using UnityEngine;
using System.Collections;

public class GUISetup : MonoBehaviour {

    public GUISkin Skin;

    public void OnGUI()
    {
        GUI.skin = Skin;
    }

}

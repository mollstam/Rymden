using UnityEngine;
using System.Collections;

public class EngineeringRoom : MonoBehaviour {

	// Use this for initialization
	void Start () {

        RoomController.Instance.OnRoomChanged += room =>
        {
            if (room.Type == RoomType.Engineering)
                audio.Play();
            else
                audio.Stop();
        };

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

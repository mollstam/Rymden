using UnityEngine;
using System.Collections;

public class DiningHall : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    RoomController.Instance.OnRoomChanged += room =>
	    {
            if (room.Type == RoomType.DiningRoom)
                audio.Play();
            else
                audio.Stop();
	    };

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

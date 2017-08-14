using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector3 playerPos;
    public GameObject player;
    public float yOffset = 0.4f;
    public float zOffset = -0.4f;

    float aimX;
    float aimY;

    public float lookSensitivity;

    float lookX;
    float lookY;

    void Start () {
		
	}

	void Update () {
        playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y + yOffset, playerPos.z + zOffset);

        aimY = Input.GetAxis("Mouse Y");
        aimX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.left, aimY * Time.deltaTime * lookSensitivity, Space.Self);
        transform.Rotate(Vector3.up, aimX * Time.deltaTime * lookSensitivity, Space.Self);

        //lookX += aimX * Time.deltaTime * lookSensitivity;
        //lookY += aimY * Time.deltaTime * lookSensitivity;

        //transform.rotation = new Quaternion(lookX, lookY, 0, 0);
    }
}

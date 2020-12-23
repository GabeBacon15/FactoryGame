using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed, multiplier, maxPitch, rotSpeed;
    float verticalRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float rotH = Input.GetAxis("Mouse X") * rotSpeed;
        this.transform.Rotate(0f, rotH, 0f);

        verticalRot -= Input.GetAxis("Mouse Y") * rotSpeed;
        verticalRot = Mathf.Clamp(verticalRot, -maxPitch, maxPitch);
        this.transform.rotation = Quaternion.Euler(verticalRot, this.transform.eulerAngles.y, 0f);
        Vector3 movment = (Vector3.forward * speed * Input.GetAxis("Vertical") + new Vector3(1, 0, 0) * speed * Input.GetAxis("Horizontal")) * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift))
            movment = movment * multiplier;
        this.transform.Translate(movment);
    }
}

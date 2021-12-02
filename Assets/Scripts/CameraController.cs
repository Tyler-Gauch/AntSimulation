using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private int Speed = 5;

    [SerializeField]
    private float FastSpeedMultiplier = 3;

    [SerializeField]
    private int RotationSpeed = 5;

    [SerializeField]
    private bool InvertY = false;
    
    [SerializeField]
    private bool InvertX = false;

    private Vector3 Angles;

    [SerializeField]
    private float distance = 50f;

    // Start is called before the first frame update
    void Start()
    {
        Angles = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false;
        }

        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? FastSpeedMultiplier : 1;
        float horizontalSpeed = Input.GetAxis("Horizontal") * Speed * speedMultiplier;
        float verticalSpeed = Input.GetAxis("Vertical") * Speed * speedMultiplier;
        float turnX = Input.GetAxis("Mouse Y") * (InvertY ? 1 : -1);
        float turnY = Input.GetAxis("Mouse X") * (InvertX ? -1 : 1);

        Angles.x = (Angles.x + turnX) % 360;
        Angles.y = (Angles.y + turnY) % 360;
        Angles.z = 0;
        transform.localEulerAngles = Angles;

        transform.Translate((Vector3.forward * verticalSpeed + Vector3.right * horizontalSpeed) * Time.deltaTime);
    }
}

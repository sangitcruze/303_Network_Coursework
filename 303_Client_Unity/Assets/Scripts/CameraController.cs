
using UnityEngine;



public class CameraController : MonoBehaviour
{
    public PlayerManager player;
    public float Mouse_sensitivity = 300f;
    public float Angle_Clamp = 85f;

    private float _rotationV;
    private float _rotationH;

    private void Start()
    {
        _rotationV = transform.localEulerAngles.x;
        _rotationH = player.transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Look();
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
    }

    private void Look()
    {
        float _mouseV = -Input.GetAxis("Mouse Y");
        float _mouseH = Input.GetAxis("Mouse X");

        _rotationV += _mouseV * Mouse_sensitivity * Time.deltaTime;
        _rotationH += _mouseH * Mouse_sensitivity * Time.deltaTime;

        _rotationV = Mathf.Clamp(_rotationV, -Angle_Clamp, Angle_Clamp);

        transform.localRotation = Quaternion.Euler(_rotationV, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, _rotationH, 0f);
    }
}


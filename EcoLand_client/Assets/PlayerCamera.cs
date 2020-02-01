using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    private PlayerInput Input;

    public Controls input;

    public Vector3 Velocity;
    public float acceleration = 5.0f;
    private Camera _camera;

    public void Awake()
    {
        Input = GetComponent<PlayerInput>();
        
        _camera = GetComponentInChildren<Camera>();

        input = new Controls();;
        input.Enable();
        input.GamePlay.Move.Enable();
        input.GamePlay.Zoom.Enable();
        input.GamePlay.Zoom.performed += ZoomOnperformed;
    }

    private void ZoomOnperformed(InputAction.CallbackContext obj)
    {
        var val = -obj.ReadValue<int>() / 100f;

        const float min = -30;
        const float max = 5;
        
        _camera.transform.position += Vector3.forward * val;

        if (_camera.transform.position.z > max)
        {
            _camera.transform.position = Vector3.forward * max;
        }

        if (_camera.transform.position.z < min)
        {
            _camera.transform.position = Vector3.forward * min;
        }
    }

    public void Update()
    {
        var move = -input.GamePlay.Move.ReadValue<Vector2>() * acceleration * Time.deltaTime;

        var cameraRight = Vector3.Cross(_camera.transform.forward, Vector3.up);
        var cameraForward = Vector3.Cross(cameraRight, Vector3.up);

        var right = move.x * cameraRight;
        var forward = move.y * cameraForward;
        
        Velocity += right + forward ;

        Velocity -= Velocity * 0.95f * Time.deltaTime;
        
        transform.position = transform.position + Velocity * Time.deltaTime;
    }
}

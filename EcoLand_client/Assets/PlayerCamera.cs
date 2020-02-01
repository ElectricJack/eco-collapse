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
        
        _camera = GetComponent<Camera>();

        input = new Controls();;
        input.Enable();
        input.GamePlay.Move.Enable();
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

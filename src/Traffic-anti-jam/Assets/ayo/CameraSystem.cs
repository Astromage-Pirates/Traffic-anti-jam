using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraSystem : MonoBehaviour
{
    private Vector3 direction;
    private Vector2 inputValue;
    private float speed = 10f;

    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;

    }

    // Update is called once per frame
    public void Movement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputValue = context.ReadValue<Vector2>();
            direction = transform.forward * inputValue.y + transform.right * inputValue.x;
        }
        else if (context.canceled)
        {
            direction = Vector2.zero;
        }
    }

}

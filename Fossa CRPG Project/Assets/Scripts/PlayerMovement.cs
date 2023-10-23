using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool inCombat;

    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float turnSpeed = 360;
    private Vector3 _input;

    private void Start()
    {
        CombatEvents.current.onStartCombat += StartCombat;
        CombatEvents.current.onEndCombat += EndCombat;
    }

    private void Update()
    {
        if (inCombat) { return; }
        GatherInput();
        Look();
    }

    private void FixedUpdate()
    {
        if (inCombat) { return; }
        Move();
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (_input != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

            var skewedInput = matrix.MultiplyPoint3x4(_input);

            var relative = (transform.position + skewedInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    private void Move()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 RelativeVerticalInput = _input.z * forward;
        Vector3 RelativeHorizontalInput = _input.x * right;

        Vector3 cameraRelativeMovement = RelativeVerticalInput + RelativeHorizontalInput;
        cameraRelativeMovement.Normalize();

        rb.MovePosition(transform.position + (transform.forward * cameraRelativeMovement.magnitude) * speed * Time.deltaTime);
    }

    private void StartCombat()
    {
        inCombat = true;
    }
    private void EndCombat()
    {
        inCombat = false;
    }
}

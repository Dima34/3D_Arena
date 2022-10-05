using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] float _sensetive;
    [SerializeField] GameObject _headObject;
    [SerializeField] float _verticalCameraLimit = 75f;

    CharacterController characterController;
    float xCameraRotation;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        setHeadRotation(transform.rotation);
        lockCoursor();
    }

    private void Update()
    {
        handlePlayerRotate();
        handlePlayerMove();
    }

    void handlePlayerMove()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 moveVector = (transform.right.normalized * horizontalAxis + transform.forward.normalized * verticalAxis);
        characterController.Move(moveVector * Time.deltaTime * _movementSpeed);
    }

    void handlePlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensetive * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * _sensetive * Time.deltaTime;

        transform.rotation *= Quaternion.EulerAngles(new Vector3(0, mouseX, 0));

        Quaternion newHeadRotation = _headObject.transform.rotation * Quaternion.EulerAngles(new Vector3(mouseY, 0, 0));
        Quaternion bodyRotation = transform.rotation;

        float verticalDegreeDiff = Quaternion.Angle(newHeadRotation, bodyRotation);
        if (verticalDegreeDiff <= _verticalCameraLimit)
            setHeadRotation(newHeadRotation);
    }

    void setHeadRotation(Quaternion newRotation)
    {
        _headObject.transform.rotation = newRotation;
    }

    void lockCoursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDestroy()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] float _sensetive;
    [SerializeField] GameObject _headObject;
    [SerializeField] float _verticalCameraLimit = 75f;
 
    CharacterController characterController;
    PlayerInputActions playerIpnutActions;
    float xCameraRotation;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        initInputManager();
        setHeadRotation(transform.rotation);
        lockCoursor();
    }

    private void FixedUpdate()
    {
        handlePlayerMove();
    }

    void initInputManager()
    {
        playerIpnutActions = new PlayerInputActions();
        playerIpnutActions.Player.Enable();
        playerIpnutActions.Player.CameraMovement.performed += handlePlayerRotate;
    }

    void handlePlayerMove()
    {
        Vector3 movementVector = playerIpnutActions.Player.Movement.ReadValue<Vector3>();
        float horizontalAxis = movementVector.x;
        float verticalAxis = movementVector.z;

        if(movementVector.magnitude != 0){
            Vector3 moveVector = (transform.right.normalized * horizontalAxis + transform.forward.normalized * verticalAxis);
            characterController.Move(moveVector * Time.deltaTime * (_movementSpeed / 10));
        }
    }

    void handlePlayerRotate(InputAction.CallbackContext context){
        Vector2 mouseMove = context.ReadValue<Vector2>() * Time.deltaTime * (_sensetive / 100);
        float yAxis = mouseMove.x;
        float xAxis = -mouseMove.y;

        transform.rotation *= Quaternion.EulerAngles(new Vector3(0, yAxis, 0));

        Quaternion newHeadRotation = _headObject.transform.rotation * Quaternion.EulerAngles(new Vector3(xAxis, 0, 0));
        Quaternion bodyRotation = transform.rotation;

        float verticalDegreeDiff = Quaternion.Angle(newHeadRotation, bodyRotation);
        if(verticalDegreeDiff <= _verticalCameraLimit)
            setHeadRotation(newHeadRotation);   
    }

    void setHeadRotation(Quaternion newRotation){
        _headObject.transform.rotation = newRotation;
    }

    void lockCoursor(){
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        playerIpnutActions.Player.CameraMovement.performed -= handlePlayerRotate;

    }
}

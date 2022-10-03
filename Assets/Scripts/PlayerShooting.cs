using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] float _fireRate = 20f;
    [SerializeField] BombShell _bombShellPrefab;
    [SerializeField] float _shellSpeed = 10f;

    
    PlayerInputActions playerInputActions;
    Coroutine shootProcess;

    private void Start() {
        initInput();
        _shellSpeed /= 10;
    }

    void initInput(){
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Fire.performed += shoot;
    }

    void shoot(InputAction.CallbackContext context){
        if(shootProcess == null)
            shootProcess = StartCoroutine(shootSequence());
    }

    IEnumerator shootSequence(){
        Camera mainCamera = Camera.main;
        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward.normalized / 10;

        BombShell shell = Instantiate(_bombShellPrefab, spawnPosition, mainCamera.transform.rotation);
        shell.MoveInDirection(mainCamera.transform.forward, _shellSpeed);

        yield return new WaitForSeconds(100 / _fireRate);
        shootProcess = null;
    }
}

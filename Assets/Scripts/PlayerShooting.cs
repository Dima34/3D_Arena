using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] float _fireRate = 20f;
    [SerializeField] BombShell _bombShellPrefab;
    [SerializeField] float _shellSpeed = 10f;

    
    Coroutine shootProcess;

    private void Start() {
        _shellSpeed /= 10;
    }

    public void Shoot(){
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

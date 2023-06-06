using System.Collections;
using System.Collections.Generic;
using Services.Network.Movement;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private NetworkMovementComponent _playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var moveVector = new Vector3(x, 0f, z);
      

        if (IsClient && IsLocalPlayer)
        {
            if (moveVector.magnitude < 0.01f)
            {
                return;
            }
            _playerMovement.ProcessLocalPlayerMovement(moveVector);
        }
        else
        {
            _playerMovement.ProcessSimulatedPlayerMovement();
        }
        
    }
}

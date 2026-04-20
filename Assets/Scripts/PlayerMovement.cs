//using System.IO;
using Unity.Netcode;
//using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private InputActionAsset hostControls;
    [SerializeField] private InputActionAsset clientControls;

    private InputAction moveAction;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        var asset = IsHost ? hostControls : clientControls;
        moveAction = asset.FindActionMap("Player").FindAction("Move");
        moveAction.Enable();

        //If you had one Input Action file (player controls)
        //moveAction = playerControls.FindActionMap("Player").FindAction("Move");

        //if (IsHost)
        //{
        //    //rebind to arrow keys for host
        //    moveAction.ApplyBindingOverride(1, "<Keyboard>/upArrow");
        //    moveAction.ApplyBindingOverride(2, "<Keyboard>/downArrow");
        //    moveAction.ApplyBindingOverride(3, "<Keyboard>/leftArrow");
        //    moveAction.ApplyBindingOverride(4, "<Keyboard>/rightArrow");
        //}

        //moveAction.Enable();
    }
    private void Update()
    {
        // Wait until network ownership is properly assigned
        if (!IsOwner || moveAction == null) return;

        Vector2 movement = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, movement.y, 0) * moveSpeed * Time.deltaTime;

        // Move using RPC so server knows about it
        MoveServerRpc(move);
        Debug.Log($"Moving: {move}");
    }

    [ServerRpc]
    private void MoveServerRpc(Vector3 move)
    {
        transform.Translate(move);
    }
    //transform.Translate(movement * moveSpeed * Time.deltaTime);
    
    

    private void OnDisable()
    {
        moveAction?.Disable();
    }
}

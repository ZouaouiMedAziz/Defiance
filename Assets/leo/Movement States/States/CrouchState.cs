using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : MovementBaseState
{
    public override void EnterState(PlayerScript movement)
    {
        movement.animator.SetBool("Crouching", true);
    }

    public override void UpdateState(PlayerScript movement)
    {
        if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (movement.Dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
            else ExitState(movement, movement.Walk);
        }

        if (movement.vInput < 0) movement.PlayerSpeed = movement.crouchBackSpeed;
        else movement.PlayerSpeed = movement.crouchSpeed;
    }

    void ExitState(PlayerScript movement, MovementBaseState state)
    {
        movement.animator.SetBool("Crouching", false);
        movement.SwitchState(state);
    }
}

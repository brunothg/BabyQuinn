using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    virtual public float getHorizontalMovement(){
        return Mathf.Max(-1, Mathf.Min(Input.GetAxisRaw("Horizontal"), +1));
    }

    virtual public bool isJump(){
        return Input.GetButton("Jump") || Input.GetButton("Vertical");
    }

    virtual public bool isDuck(){
        return Input.GetButton("Duck");
    }

    virtual public bool isCancel(){
        return Input.GetButton("Cancel");
    }

    virtual public bool isFire(){
        return Input.GetButton("Fire1") || Input.GetButton("Fire2");
    }
}

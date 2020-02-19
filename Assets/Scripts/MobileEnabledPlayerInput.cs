using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileEnabledPlayerInput : PlayerInput
{
    
    private float eventMovement = 0;
    private bool eventJump = false;
    private bool eventDuck = false;
    private bool eventCancel = false;
    private bool eventFire = false;

    public void onMove(float movement){
        Debug.Log("onMove: " + movement);

        movement = Mathf.Max(-1, Mathf.Min(movement, +1));
        eventMovement = movement;
    }

    public void onJump(bool jump){
        Debug.Log("onJump: " + jump);

        eventJump = jump;
    }

    public void onDuck(bool duck){
        Debug.Log("onDuck: " + duck);

        eventDuck = duck;
    }

    public void onFire(bool fire){
        Debug.Log("onFire: " + fire);

        eventFire = fire;
    }

    public void onCancel(bool cancel){
        Debug.Log("onCancel: " + cancel);

        eventCancel = cancel;
    }

    override public float getHorizontalMovement(){
        var hMovement = Mathf.Max(-1, Mathf.Min(base.getHorizontalMovement() + eventMovement, +1));
        //Debug.Log("getHorizontalMovement: " + hMovement);

        return hMovement;
    }

    override public bool isJump(){
        var jump = base.isJump() || eventJump;
        //Debug.Log("isJump: " + jump);

        return jump;
    }

    override public bool isDuck(){
        var duck = base.isDuck() || eventDuck;
        //Debug.Log("isDuck: " + duck);

        return duck;
    }

    override public bool isFire(){
        var fire = base.isFire() || eventFire;
        //Debug.Log("isFire: " + fire);

        return fire;
    }

    override public bool isCancel(){
        var cancel = base.isCancel() || eventCancel;
        //Debug.Log("isCancel: " + cancel);

        return cancel;
    }
}

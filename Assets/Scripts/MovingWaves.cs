using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWaves : MonoBehaviour
{
    public float maxMoveDeltaX = 1;
    public float moveSpeed = 1;
    public bool moveLeft = true;

    private float moveCount = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveDelta = Mathf.Max(-maxMoveDeltaX - moveCount, Mathf.Min(moveSpeed * Time.deltaTime * ((moveLeft) ? -1 : +1), maxMoveDeltaX - moveCount));
        moveCount = moveCount + moveDelta;
        if(Mathf.Abs(moveCount) >= maxMoveDeltaX){
            moveLeft = !moveLeft;
        }

        gameObject.transform.Translate(Vector3.right * moveDelta);
    }
}

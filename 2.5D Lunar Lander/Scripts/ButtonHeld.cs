using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.

public class ButtonHeld : MonoBehaviour, IUpdateSelectedHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool isHeld = false;
    private string isHeldButtonName = null, notHeldButtonName = null;

    private static bool isHeldLeft = false, isHeldUp = false, isHeldRight = false, isHeldDown = false;

    private static PlayerController playerControllerInstance = null;

    private float pointerDownTimer = 0.0f, requiredHoldTime = 2.0f, horizontalMove = 0.0f, verticalMove = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerInstance = PlayerController.GetInstance();

        requiredHoldTime = playerControllerInstance.engineWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Button is being held - Left: " + isHeldLeft + " - Right: " + isHeldRight + " - Up: " + isHeldUp + " - Down: " + isHeldDown);

        // The OnUpdateSelected logic can be placed here as well.
        if (isHeldLeft) {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime) {
                if (horizontalMove > -1.0f) {
                    horizontalMove -= 0.1f;
                }

            } else {
                horizontalMove = -1.0f;
            }

            //Debug.Log("OnUpdateSelected - " + this.gameObject.name + " - horizontalMove: " + horizontalMove);

            Left(horizontalMove);
        } else if (isHeldRight) {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime) {
                if (horizontalMove < 1.0f) {
                    horizontalMove += 0.1f;
                }
            } else {
                horizontalMove = 1.0f;
            }

            //Debug.Log("OnUpdateSelected - " + this.gameObject.name + " - horizontalMove: " + horizontalMove);

            Right(horizontalMove);
        } else {
            pointerDownTimer = 0.0f;
            horizontalMove = 0.0f;
            Left(horizontalMove);
            Right(horizontalMove);
        }

        if (isHeldUp) {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime) {
                if (verticalMove < 1.0f) {
                    verticalMove += 0.1f;
                }
            } else {
                verticalMove = 1.0f;
            }

            //Debug.Log("OnUpdateSelected - " + this.gameObject.name + " - horizontalMove: " + verticalMove);

            Up(verticalMove);
        } else if (isHeldDown) {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime) {
                if (verticalMove > -1.0f) {
                    verticalMove -= 0.1f;
                }

            } else {
                verticalMove = -1.0f;
            }

            //Debug.Log("OnUpdateSelected - " + this.gameObject.name + " - horizontalMove: " + verticalMove);

            Down(verticalMove);
        } else {
            pointerDownTimer = 0.0f;
            verticalMove = 0.0f;
            Up(verticalMove);
            Down(verticalMove);
        }

    }

    private void Left(float horizontalMove) {
        //Debug.Log("Left");
        // verticalMove = -1.0f;
        playerControllerInstance.Left(horizontalMove);
    }

    private void Up(float movementSpeed) {
        //Debug.Log("Up");
        // verticalMove = 1.0f;
        playerControllerInstance.Up(movementSpeed);
    }

    private void Right(float horizontalMove) {
        //Debug.Log("Right");
        // horizontalMove = 1.0f;
        playerControllerInstance.Right(horizontalMove);
    }

    private void Down(float movementSpeed) {
        //Debug.Log("Down");
        // verticalMove = -1.0f;
        playerControllerInstance.Down(movementSpeed);
    }

    // Start is called before the first frame update
    public void OnUpdateSelected(BaseEventData data)
    {
        //Debug.Log("OnUpdateSelected - " + this.gameObject.name);

        /*
        if (isHeldLeft) {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime) {
                if (horizontalMove > -1.0f) {
                    horizontalMove -= 0.1f;
                }

            } else {
                horizontalMove = -1.0f;
            }

            Debug.Log("OnUpdateSelected - " + this.gameObject.name + " - horizontalMove: " + horizontalMove);

            Left(horizontalMove);
        } else if (isHeldRight) {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime) {
                if (horizontalMove < 1.0f) {
                    horizontalMove += 0.1f;
                }
            } else {
                horizontalMove = 1.0f;
            }

            Debug.Log("OnUpdateSelected - " + this.gameObject.name + " - horizontalMove: " + horizontalMove);

            Right(horizontalMove);
        } else {
            pointerDownTimer = 0.0f;
            horizontalMove = 0.0f;
            Left(horizontalMove);
            Right(horizontalMove);
        }

        if (isHeldUp) {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime) {
                if (verticalMove < 1.0f) {
                    verticalMove += 0.1f;
                }
            } else {
                verticalMove = 1.0f;
            }

            Debug.Log("OnUpdateSelected - " + this.gameObject.name + " - horizontalMove: " + verticalMove);

            Up(verticalMove);
        } else if (isHeldDown) {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime) {
                if (verticalMove > -1.0f) {
                    verticalMove -= 0.1f;
                }

            } else {
                verticalMove = -1.0f;
            }

            Debug.Log("OnUpdateSelected - " + this.gameObject.name + " - horizontalMove: " + verticalMove);

            Down(verticalMove);
        } else {
            pointerDownTimer = 0.0f;
            verticalMove = 0.0f;
            Up(verticalMove);
            Down(verticalMove);
        }
        */
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerControllerInstance == null) {
            playerControllerInstance = PlayerController.GetInstance();
        }
        
        if (this.gameObject.name == "Left") {
            isHeldLeft = true;
            isHeldRight = false;
        } else if (this.gameObject.name == "Right") {
            isHeldRight = true;
            isHeldLeft = false;
        } 
        
        if (this.gameObject.name == "Up") {
            isHeldUp = true;
            isHeldDown = false;
        } else if (this.gameObject.name == "Down") {
            isHeldDown = true;
            isHeldUp = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (playerControllerInstance == null) {
            playerControllerInstance = PlayerController.GetInstance();
        }
        
        if (this.gameObject.name == "Left") {
            isHeldLeft = false;
        }
        
        if (this.gameObject.name == "Right") {
            isHeldRight = false;
        }
        
        if (this.gameObject.name == "Up") {
            isHeldUp = false;
        }
        
        if (this.gameObject.name == "Down") {
            isHeldDown = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit - " + this.gameObject.name);

        if (playerControllerInstance == null) {
            playerControllerInstance = PlayerController.GetInstance();
        }
    }
}

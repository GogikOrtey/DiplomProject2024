using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_01 : MonoBehaviour
{
    CharacterController characterController;

    public float Speed = 2;
    public float SpeedRot = 2;
    float Gravity = 20;
    private bool isRunning;
    private Vector3 moveDirection;

    public bool isOverWallGo = true; // ����� ������������ ������. ���� true, �� ��������������

    float objSpeed = 1f; // ����������� ��������                    // � ���� 2f
    const float suspendSpeed = 0.1f; // ����������� ����������
    float minSpeed = 0.5f; // ����������� ��������, �� ����� ����������

    // ���������� ��� ����������� ��������:
    public bool isFinished = false;         // ������������, ����� �� ����� �� �������� ����� ��������
    public bool isStopped = false;          // ������������, ����� �� ������ �������� ���������� �����

    public IconOfStats IconOfStats;

    // ���� ����� ����������, ���� ����� ������� �����������, � ��� ����� �����������
    public void PlayerControl_Suspend(float currSusSpeed = suspendSpeed) // �������� ���������� ����� ��� �������� � �����, ��� � ���
    {
        if (Speed > minSpeed)
        {
            Speed -= currSusSpeed;
        }
    }

    // ���� ����� ����������, ���� ����������� ����� ������� ��������� ��������� ������
    public void PlayerControl_EmergyStop()
    {
        Speed = 0;
        //print("������ ���������");
    }

    // ���� ����� ����������, ���� ������� ��� �����������
    public void PlayerControl_StandartSpeedMove()
    {
        if (Speed < objSpeed)
        {
            if (Speed < minSpeed)
            {
                Speed = minSpeed;
            }
            else
            {
                Speed += suspendSpeed;
            }
        }
        else
        {
            Speed = objSpeed;
        }
    }

    // ��������� ������� ����� �������� ����� ������������ �������, ��� ������
    public int isPriority = 0;

    // � priority ����� ���������� ������� ��������, ���� 100

    // ���� ����� ����������, ����� ������ ����� ����������� �����
    public void PlayerControl_TurnLeft(float multipler = 1, int priority = 0)
    {
        if(priority > 0) isPriority = priority;
        if (isPriority <= 0 || priority > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - (SpeedRot * multipler), transform.rotation.eulerAngles.z);
            //print("]< PlayerController_01: ����������� ������� ������, ��������� = " + priority + ", multipler = " + multipler);
            print("]< PlayerController_01: mult = " + multipler + ", p =  " + priority);
        }
    }

    // ���� ����� ����������, ����� ������ ����� ����������� ������
    public void PlayerControl_TurnRight(float multipler = 1, int priority = 0)
    {
        if (priority > 0) isPriority = priority;
        if (isPriority <= 0 || priority > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (SpeedRot * multipler), transform.rotation.eulerAngles.z);
            //print("]> PlayerController_01: ����������� ������� �������, ��������� = " + priority + ", multipler = " + multipler);
            print("]> PlayerController_01: mult = " + multipler + ", p =  " + priority);
        }        
    }

    void Start()
    {
        TryGetComponent(out characterController);
    }

    void FixedUpdate()
    {
        if (isFinished == false && isStopped == false)
        {
            if (isOverWallGo == true)
            {
                OverWallGoMove();
            }
            else
            {
                //StandartMove();
            }
        }
        else if(isFinished == true)
        {
            IconOfStats.setBlue();
        }
    }

    // �������������� ����� �������� ���������
    void OverWallGoMove()
    {
        if (isPriority > 0) isPriority--;

        moveDirection = new Vector3(0f * Speed, moveDirection.y, Speed);

        moveDirection.y -= Gravity * Time.deltaTime;
        characterController.Move(transform.TransformVector(moveDirection) * Time.deltaTime);
    }

    //// ����������� ����� ���������� ����������, �������������
    //void StandartMove()
    //{ 
    //    Vector2 inputs = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1);
    //    isRunning = Input.GetKey(KeyCode.LeftShift);

    //    moveDirection = new Vector3(0, moveDirection.y, inputs.y * Speed);

    //    if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
    //    {
    //        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + SpeedRot, transform.rotation.eulerAngles.z);
    //    }
    //    if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
    //    {
    //        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - SpeedRot, transform.rotation.eulerAngles.z);
    //    }

    //    if (characterController.isGrounded)
    //    {
    //        moveDirection.y = 0;
    //    }

    //    moveDirection.y -= Gravity * Time.deltaTime; 
    //    characterController.Move(transform.TransformVector(moveDirection) * Time.deltaTime);        
    //}
}

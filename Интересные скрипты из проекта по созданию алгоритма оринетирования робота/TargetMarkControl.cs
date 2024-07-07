using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetMarkControl : MonoBehaviour
{
    // ����� ����������� - ������
    public GameObject MainMarkRot;

    //public GameObject Player;

    // ����������� ����, ������� �������
    [Range(0f, 180f)] public float directionOfRotation = 90f;
    // 90 = ����� ������

    // ��� ���� ����� �������� ��� ��� ���������� ���� ������
    // �.�. �������� 0 = 90, 1 = 0 ��������, � 0 = 180 ��������

    // ������� ���� ��������
    private float currentRotation = 0f;
    // 0 = ����� ������� �����

    // �������� �������� ����� �� ������� ��������
    float speedOfRotMark = 5f;

    public PlayerController_01 PlayerController_01;

    public Image Arr_left;
    public Image Arr_right;

    void Start()
    {

    }

    // ���������, ��� ���������� �������� ������

    //public bool isRotatingRight = false;
    //public bool isRotatingLeft = false;

    //public void RotateRight()
    //{
    //    if (isCanRot)
    //    {
    //        if (isRotatingLeft == false && isRotatingRight == false)
    //        {
    //            isRotatingRight = true;

    //            statrRotPlayerRotation = Player.transform.rotation.eulerAngles.y;
    //            // ������������� ������� ���� �������� ������, ��� ���������. ������ �� ������ ����������� ������� �� 90 ��������, �� ����

    //            print("�������������� ������� ������. ��������� ���� ������ = " + statrRotPlayerRotation);
    //        }
    //    }
    //}
    
    //public void RotateLeft()
    //{
    //    if (isCanRot)
    //    {
    //        if (isRotatingLeft == false && isRotatingRight == false)
    //        {
    //            isRotatingLeft = true;

    //            statrRotPlayerRotation = Player.transform.rotation.eulerAngles.y;
    //            // ������������� ������� ���� �������� ������, ��� ���������. ������ �� ������ ����������� ������� �� 90 ��������, �� ����

    //            print("�������������� ������� �����. ��������� ���� ������ = " + statrRotPlayerRotation);
    //        }
    //    }
    //}

    float currentPlayerRotation;
    public float statrRotPlayerRotation;

    float dill = 5f; // ���� ������� ��� �������� ������ ������������ ������� ��������

    int timer = 0;
    bool isCanRot = false; // �� ����� ������������?



    void FixedUpdate()
    {
        // ������� ������, �.�. ���� ��������� �������� � ���������, ����� ����� �������
        if (timer < 5) timer++;
        else if (timer == 5)
        {
            timer++;
            isCanRot = true;
        }

        //// ----- �������� � ��������� ������
        {


            //currentPlayerRotation = Player.transform.rotation.eulerAngles.y;

            //if (isRotatingRight == true)
            //{
            //    float diffAngle = Mathf.Abs(Mathf.DeltaAngle(currentPlayerRotation, statrRotPlayerRotation + 90f));

            //    if (diffAngle > dill)
            //    {
            //        directionOfRotation = 0;

            //        //print("�� � �������� �������� ������. ������� ����� ������� � ��������� ����� �������� ����������: " + diffAngle);
            //    }
            //    else
            //    {
            //        // �� ��������� ���������� ������
            //        print("�� ��������� ���������� ������");
            //        Player.transform.rotation = Quaternion.Euler(Player.transform.rotation.eulerAngles.x, statrRotPlayerRotation + 90f, Player.transform.rotation.eulerAngles.z);

            //        directionOfRotation = 90;
            //        isRotatingRight = false;
            //    }
            //}

            //if (isRotatingLeft == true)
            //{
            //    float diffAngle = Mathf.Abs(Mathf.DeltaAngle(currentPlayerRotation, statrRotPlayerRotation - 90f));

            //    if (diffAngle > dill)
            //    {
            //        directionOfRotation = 180;

            //        //print("�� � �������� �������� �����. ������� ����� ������� � ��������� ����� �������� ����������: " + diffAngle);
            //    }
            //    else
            //    {
            //        // �� ��������� ���������� �����
            //        print("�� ��������� ���������� �����");
            //        Player.transform.rotation = Quaternion.Euler(Player.transform.rotation.eulerAngles.x, statrRotPlayerRotation - 90f, Player.transform.rotation.eulerAngles.z);

            //        directionOfRotation = 90;
            //        isRotatingLeft = false;
            //    }
            //}
        }


        // ----- �������� � �������� ��������� �����:

        // �������� ���� ��������
        currentRotation = MainMarkRot.transform.rotation.eulerAngles.z;

        //print("currentRotation = " + currentRotation);

        // ��������� ������� ����� ������� ����� � �������
        float delta = directionOfRotation - currentRotation;

        // ���� ������� ������ �������� ��������, ������� ������
        if (Mathf.Abs(delta) > speedOfRotMark)
        {
            float newRotation = currentRotation + Mathf.Sign(delta) * speedOfRotMark;

            // ����� ����� ���� ��������
            MainMarkRot.transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
        else
        {
            MainMarkRot.transform.rotation = Quaternion.Euler(0, 0, directionOfRotation);
        }

        float offset = 3;
        float deletelCoeff = 10f * 100;

        int delitRot = 20; // ��������, �� �������� �� ����������, ������ ����� ������ ���������, ��� ������� �������
        // �.�. �������� � ����� ��������: �� 0 �� delitRot - ������� ���� ��������, � �� delitRot �� 90 - ���������

        // ��� ���������� ����������
        if (directionOfRotation != 90)
        {
            // ��������� ��������:

            if (directionOfRotation >= (90 + offset) && directionOfRotation <= (180 - delitRot))
            {
                float locCoff = (directionOfRotation - 90) / deletelCoeff;

                print("< TargetMarkControl: ��������� ������� �����");

                PlayerController_01.PlayerControl_TurnLeft(locCoff);
                Arr_left.enabled = true;
                Arr_right.enabled = false;
            }
            else if (directionOfRotation <= (90 - offset) && directionOfRotation >= delitRot)
            {
                float locCoff = (90 - directionOfRotation) / deletelCoeff;

                print("> TargetMarkControl: ��������� ������� ������");

                PlayerController_01.PlayerControl_TurnRight(locCoff);
                Arr_right.enabled = true;
                Arr_left.enabled = false;
            }

            // ������� ��������:
            // ��������, ������ ���� ���� > 80

            else
            if (directionOfRotation > (180 - delitRot) && directionOfRotation <= 180)
            {
                print("<< TargetMarkControl: ������� ������� �����");

                PlayerController_01.PlayerControl_TurnLeft(0.1f);
                Arr_left.enabled = true;
                Arr_right.enabled = false;
            }
            else if (directionOfRotation < delitRot && directionOfRotation >= 0)
            {
                print(">> TargetMarkControl: ������� ������� ������");

                PlayerController_01.PlayerControl_TurnRight(0.1f);
                Arr_right.enabled = true;
                Arr_left.enabled = false;
            }
        }
        
        if (directionOfRotation > (90 - offset) && directionOfRotation < (90 + offset))
        {
            // ��������� ��� ������

            Arr_right.enabled = false;
            Arr_left.enabled = false;
        }

        // ����� � �� �������� ������, ��� ������� ��������, ��� �� ��� ���������� ��������, � ����� ������ �� ������������
        // ��� ������ ����� ���������� � �������������� ���������� �����
    }
}

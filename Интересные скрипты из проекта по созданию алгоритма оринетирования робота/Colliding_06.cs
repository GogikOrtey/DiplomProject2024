using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliding_06 : MonoBehaviour
{
    public PlayerController_01 PlayerController_01;
    public GameObject Player;
    public TargetMarkControl TargetMarkControl;

    int isRotRight = 0;     // 1 - ���� ����� ����������� ������, -1 - ���� ����� ����������� �����
    float whRot = 0;        // �� ������� �������� �����������
    float startRot = 0;     // ��������� ���� ��������

    bool isNotItPriority = false;   // = true, ���� ��� ��������� > 0, � ���� = 0
    bool counterStaOnWayB = false;  // ����������� ����� �������� ����?
    bool isOverRot = false;         // ��� ���������� ���������� = true, ����� �� ��������� ��������� ����� �������, �� ����� �� �������� ����� ����. ����� �� ������������ ��� ��� �������

    public int currentRotate = 90;
    // 90 �������� - ��� �������, �� ���������� �����
    // 0 �������� - ��� �����

    int counterFromStayWay = 0; // �������, ��� ����� �������� ��� ����� �� ����. �� ������ 500.

    public NavigatorLogAddText NavigatorLogAddText;

    void Update()
    {
        if (counterStaOnWayB == true)
        {
            if (counterFromStayWay < 500)
            {
                counterFromStayWay++;
            }
        }

        if (Colliding_10.isMarkFortrCenterForward2 == true)
        {
            Colliding_10.isMarkFortrCenterForward2 = false;
            GlobalRotateLeft();
        }

        if (Colliding_11.isMarkFortrCenterForward2R == true)
        {
            Colliding_11.isMarkFortrCenterForward2R = false;
            GlobalRotateRight();
        }

        if (Colliding_12.isMarkFortrCenterForward2RDown == true)
        {
            Colliding_12.isMarkFortrCenterForward2RDown = false;
            GlobalRotateRightDown();
        }

        // ���� �� ������ �� ��������� ���������� ��������� �� 90 �������� ������ ��� �����
        if (isGlobalRotate == false)
        {
            if (PlayerController_01.isPriority == 0)
            {
                // ��������� � ����� �������� ������ �����, ����� ����� ������������ ��������� ���

                if (isNotItPriority == true)
                {
                    isNotItPriority = false;

                    //print("PlayerController_01.isPriority = " + PlayerController_01.isPriority);

                    print(" --- �������� ��������� �� ���������� ����� ���������, isRotRight = " + isRotRight);

                    // ����� ��������� ���������� ������������� ��������� ��������
                    // ��� ������ �������� ����, � ��������� �� � ������ �������������

                    if (isRotRight == -1)
                    {
                        isRotRight = 0;
                        RotateLeft();
                    }
                    else if (isRotRight == 1)
                    {
                        isRotRight = 0;
                        RotateRight();
                    }
                }

                print("isRotRight = " + isRotRight);

                if (isRotRight == 0)
                {
                    if (Colliding_13.isMarkFortrCenterForwardToCollidingGlobalRot == false)
                    {
                        if (Colliding_07.isLeftCenter == true && Colliding_08.isRightCenter == false)
                        {
                            RotateLeft();
                        }
                        else if (Colliding_07.isLeftCenter == false && Colliding_08.isRightCenter == true)
                        {
                            RotateRight();
                        }
                    }
                    else
                    {
                        print("!><! ��� ����� ��������� �������, �� �������� ������� ��������� ����������� �������, � �� �� ������������");   
                    }
                }
                else
                {
                    if (Colliding_07.isLeftCenter == false && Colliding_08.isRightCenter == false && counterStaOnWayB == true) // ���� ����������� ����� �������� ����, � ������ � ����� - �� ��������
                    {
                        // ���� �� ��������� �� �������� ����, ��� ��� �� ����� ������ ��������������

                        //isRotRight = 0;
                        //print("||| �� ��������� �� �������� ����, �������� ��� ��������");
                        print("||| �� ��������� �� �������� ����");
                    }
                    else
                    {
                        float playerRot = Player.transform.rotation.eulerAngles.y;

                        if (playerRot >= 270)
                        {
                            playerRot = 360 - playerRot;
                        }

                        if (isRotRight == -1)
                        {
                            if (Mathf.Abs(playerRot - (startRot - whRot)) < 2)
                            {
                                if (counterStaOnWayB == false && isOverRot == false) // ���� �� �� ��������� �� ����, � ��� �� ��������� +10 �������� � ��������
                                {
                                    isOverRot = true;

                                    if (Colliding_07.isLeftCenter == true)
                                    {
                                        whRot += 10;        // ��������� ��� +10 �������� � ��������, �� ������ ����� ���
                                        print("�������� +10 �������� � ���� ��������, whRot = " + whRot);
                                    }
                                    else // ���� ���� � ������ ������� �� ��������
                                    {
                                        print("�������� ��������� �������� � ������ �������");
                                        isRotRight = 0;
                                        RotateRight();
                                    }
                                }
                                else // ���� �� ��������� �� ����, ��� ��� �������� +10 �������� � ��������, � ����������� �� ���
                                {
                                    // ��������� �������, ���� �������� ����� �� ���� ��� ������ 20 ������ �������
                                    // ��� ���� ������� ����� ������� ����� ��������� � ������ > 10 ��������
                                    // ����������, ��� ����� �����, ���� ������� ������ > 10 ��������, ���� ����� ������ ������ 20 �����
                                    if ((counterStaOnWayB == true && counterFromStayWay > 30) || (Mathf.Abs(playerRot - currentRotate) > 10))
                                    {
                                        isRotRight = 0;
                                        print("��������� ������� �����, ������ �� ������� ����� �����");
                                        TargetMarkControl.directionOfRotation = 90;
                                        isOverRot = false;
                                    }
                                    else
                                    {
                                        print("������������� ������������ �����");
                                        TargetMarkControl.directionOfRotation = 90 + (whRot);
                                    }
                                }
                            }
                            else
                            {
                                print("������������ �����, whRot = " + whRot);
                                //, Mathf.Abs(Player.transform.rotation.eulerAngles.y - (startRot - whRot)) = " + Mathf.Abs(Player.transform.rotation.eulerAngles.y - (startRot - whRot)));
                                TargetMarkControl.directionOfRotation = 90 + (whRot);
                                //PlayerController_01.PlayerControl_TurnLeft(0.1f);
                            }
                        }
                        else if (isRotRight == 1)
                        {
                            if (Mathf.Abs(playerRot - (startRot + whRot)) < 2)
                            {
                                if (counterStaOnWayB == false && isOverRot == false)
                                {
                                    isOverRot = true;
                                    if (Colliding_08.isRightCenter == true)
                                    {
                                        whRot += 10;        // ��������� ��� +10 �������� � ��������, �� ������ ����� ���
                                        print("�������� +10 �������� � ���� ��������, whRot = " + whRot);
                                    }
                                    else
                                    {
                                        print("�������� ��������� �������� � ������ �������");
                                        isRotRight = 0;
                                        RotateLeft();
                                    }
                                }
                                else
                                {
                                    //print("Mathf.Abs(playerRot - currentRotate) = " + Mathf.Abs(playerRot - currentRotate));

                                    // ��������� �������, ���� �������� ����� �� ���� ��� ������ 20 ������ �������
                                    // ��� ���� ������� ����� ������� ����� ��������� � ������ > 10 ��������
                                    if ((counterStaOnWayB == true && counterFromStayWay > 20) || (Mathf.Abs(playerRot - currentRotate) > 10))
                                    {
                                        isRotRight = 0;
                                        print("��������� ������� ������, ������ �� ������� ����� �����");
                                        //print("counterStaOnWay = " + counterStaOnWay);
                                        TargetMarkControl.directionOfRotation = 90;
                                        isOverRot = false;
                                    }
                                    else
                                    {
                                        print("������������� ������������ ������");
                                        TargetMarkControl.directionOfRotation = 90 - (whRot);
                                    }
                                }
                            }
                            else
                            {
                                //print("������������ ������"); ///, Mathf.Abs(Player.transform.rotation.eulerAngles.y - (startRot - whRot)) = " + Mathf.Abs(Player.transform.rotation.eulerAngles.y - (startRot - whRot)));
                                //print("������������ ������, Mathf.Abs(Player.transform.rotation.eulerAngles.y - (startRot + whRot)) = " + Mathf.Abs(Player.transform.rotation.eulerAngles.y - (startRot + whRot)));
                                //print("������������ ������, Mathf.Abs(Player.transform.rotation.eulerAngles.y = " + Player.transform.rotation.eulerAngles.y + ", startRot = " + startRot + ", whRot = " + whRot);
                                print("������������ ������, whRot = " + whRot);

                                TargetMarkControl.directionOfRotation = 90 - (whRot);
                            }
                        }
                    }
                }
            }
            else
            {
                isNotItPriority = true;
            }
        }
        else
        {
            float rotY = Player.transform.rotation.eulerAngles.y;

            if (rotY >= 270)
            {
                rotY = 360 - rotY;
            }

            print("rotY ��������� = " + rotY + ", currentRotate = " + currentRotate);

            if (Mathf.Abs(rotY - currentRotate) < 2.5f)
            {
                print("�� ��������� ���������� �������");
                isGlobalRotate = false;
                TargetMarkControl.directionOfRotation = 90;

                // ������������ ��������� ��������

                if (isRotRight == -1)
                {
                    isRotRight = 0;
                    RotateLeft();
                }
                else if (isRotRight == 1)
                {
                    isRotRight = 0;
                    RotateRight();
                }
            }
        }
    }

    float minLock = 10f; // ���� ���� ���������� �� ������� ����������� ������ ����� �������� - �� �� ��������� ��������� ��������
    float mini_minLock = 2f;

    // �������, ������� �������������� ������� �����
    void RotateLeft()
    {
        if (isRotRight == 0)
        {
            print("ii ������� �����?");

            float rotY = Player.transform.rotation.eulerAngles.y;

            if (rotY >= 270)
            {
                rotY = 360 - rotY;
            }

            float currRotate = 0; // ����, �� ������� ����� �����������

            currRotate = rotY - currentRotate;
            currRotate = Mathf.Abs(currRotate);

            //if (currRotate > 80)
            //{
            //    print("currRotate > 80, currRotate = " + currRotate);
            //    return;
            //}

            if (counterStaOnWayB == false)
            {
                if (currRotate < mini_minLock)
                {
                    print("&& ���� �� ������� ����� ���������, �� ����� �� ��������� �� ����. ��������� +10 � ����, currRotate = " + currRotate);
                    // ���� ���� �� ������� ����� ���������, �� ����� �� ��������� �� ���� - ������ ��� ������

                    currRotate += 10;
                }
            }

            if (currRotate >= minLock) // ���� ����������� ������ ������������
            {
                isOverRot = false;
                whRot = currRotate;
                startRot = rotY;
                isRotRight = -1;

                print("i< �������������� ����� ������� ����� �� " + currRotate + " ��������, ������� ������� ������ = " + rotY);
            }
            else if (currRotate >= mini_minLock) // ���� ���������� ������� ���������, ��� �� ������ ������� - �� �� ������ ������� ������������ ������ � ������ �������
            {
                print("i�< ��������� ������ �� 0.5f");
                PlayerController_01.PlayerControl_TurnLeft(0.5f);

                // �������� ��������, �� ������ ������
                isRotRight = 0;
                TargetMarkControl.directionOfRotation = 90;
                isOverRot = false;
            }
            else
            {
                //Player.transform.rotation = Quaternion.Euler(Player.transform.rotation.eulerAngles.x, currentRotate, Player.transform.rotation.eulerAngles.z);
                print("�-i ���� �������� ����� ����� ���������, �� ������ ���");
            }
        }
    }

    void RotateRight()
    {
        if (isRotRight == 0)
        {
            print("ii ������� ������?");

            float rotY = Player.transform.rotation.eulerAngles.y;

            if (rotY >= 270)
            {
                rotY = 360 - rotY;
            }

            float currRotate = 0; // ����, �� ������� ����� �����������

            currRotate = rotY - currentRotate;
            currRotate = Mathf.Abs(currRotate);

            //if (currRotate > 80)
            //{
            //    print("currRotate > 80");
            //    return;
            //}

            if (counterStaOnWayB == false)
            {
                if (currRotate < mini_minLock)
                {
                    print("&& ���� �� ������� ����� ���������, �� ����� �� ��������� �� ����. ��������� +10 � ����, currRotate = " + currRotate);
                    // ���� ���� �� ������� ����� ���������, �� ����� �� ��������� �� ���� - ������ ��� ������

                    //int l = 1; l = currRotate > 0 ? 1 : -1;
                    //currRotate = currRotate + (10 * l);

                    currRotate += 10;
                }
            }

            if (currRotate >= minLock) // ���� ����������� ������ ������������
            {
                isOverRot = false;
                whRot = currRotate;
                startRot = rotY;
                isRotRight = 1;

                print("i> �������������� ����� ������� ������ �� " + currRotate + " ��������, ������� ������� ������ = " + rotY);
            }
            else if (currRotate >= mini_minLock) // ���� ���������� ������� ���������, ��� �� ������ ������� - �� �� ������ ������� ������������ ������ � ������ �������
            {
                print("i�> ��������� ������� �� 0.5f");
                PlayerController_01.PlayerControl_TurnRight(0.5f);

                // �������� ��������, �� ������ ������
                isRotRight = 0;
                TargetMarkControl.directionOfRotation = 90;
                isOverRot = false;
            }
            else
            {
                //Player.transform.rotation = Quaternion.Euler(Player.transform.rotation.eulerAngles.x, currentRotate, Player.transform.rotation.eulerAngles.z);
                print("�-i ���� �������� ������ ����� ���������, �� ������ ���, currRotate = " + currRotate);
            }
        }
    }

    bool isGlobalRotate = false;

    // ������� �������� ����� �� 90 ��������
    // currentRotate = 0
    // ��������� ������� lastCurrentRotate ���� ��������
    // ������ �������� �� ��������� �����, ����� TargetMarkControl
    // ��������� ��������� ��������� ��������
    // ��������� ���������: ���� ������� ���� �������� ��������� - currentRotate < 5-10 �� ������, �� ��������� ���� �������
    // ������������� ������ ���� �� TargetMarkControl, �������� �����������
    // �������� ��������� �������� - ������������� ��

    // ��� �������� �� ������������.
    
    void GlobalRotateLeft()
    {
        // ���� ������� ��������� ��������� ������ �� ����������� ������� - ������

        isGlobalRotate = true;  // ��������� ��������� ��������
        currentRotate = 0;      // ����, ������� ������ ����� � ��������� (������� �� y)
        TargetMarkControl.directionOfRotation = 180; // ������, ��� � TargetMarkControl ����� ���������� ����, ��������� � ������ �������
    }

    void GlobalRotateRight()
    {
        // ���� ������� ��������� ��������� ������ �� ����������� ������ - �������

        isGlobalRotate = true; 
        currentRotate = 90;
        TargetMarkControl.directionOfRotation = 0;
    }

    void GlobalRotateRightDown()
    {
        // ���� ������� ��������� ��������� ������ �� ����������� ������� - ����

        isGlobalRotate = true; 
        currentRotate = 180;     
        TargetMarkControl.directionOfRotation = 0; 
    }

    // ����� �������� - ��� ������� Colliding_10 � Colliding_11

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GlobalR1")
        {
            NavigatorLogAddText.AddText("����� 20 ������ ��������� ������");
            print("����� 20 ������ ��������� ������");
        } 
        
        if (collision.tag == "GlobalR2" || collision.tag == "GlobalR3")
        {
            NavigatorLogAddText.AddText("����� 20 ������ ��������� �������");
            print("����� 20 ������ ��������� �������");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "TheWay")
        {
            //print("����� ��������� �� ����");
            counterStaOnWayB = true;
        }

        if (collision.tag == "TargetZone")
        {
            print("END - �� �������� ������ ����������");
            PlayerController_01.isFinished = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TheWay")
        {
            //print("����� ���� � ����");
            counterStaOnWayB = false;
            counterFromStayWay = 0;
        }
    }
}

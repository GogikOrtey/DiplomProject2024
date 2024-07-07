using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using System.IO;
using System.Diagnostics;

using System;
using UnityEngine.UI;

using System.Linq;
using TMPro;

using System.Threading.Tasks;

//
// !!! ����� ����� ����� �������� � ����� "Diplom Project 01_Data/Resources" - ��� ����� �� ������ MyScript
// ������ ���� ����� ����� � ����� ".../2024/��� ������, ������� ������ ����� �������"
//

public class Screen_Shots_01 : MonoBehaviour
{
    /*///////////////////////////////////////////////////
    //                 ��� ����������                  //
    ///////////////////////////////////////////////////*/
    

    ////// ������ �� ������� ������:

    public PlayerController_01 PlayerController_01; // ����� �� ��������, ������� ��������� ����������

    public RenderTexture LeftCam;       // ������ �� RenderTexture � ������ � ����� ������ ���������
    public RenderTexture RightCam;

    public GameObject DephMapImage;     // ������ �� ������ ��� ������� � �������� ���� ����� �������,
    public GameObject OutLineTexture;   // ������ ����� �������
    public GameObject OutMarcerLine;    // � ��������� �����
    public GameObject Outp2DMapTex;     // ��������� �����
    public GameObject Outp2DMapTexOversize;     // ��������� �����

    public Text speedText;              // �����, � ������� ������������ ������� �������� ���������

    public Image Wall;                  // 4 �����������, ������������ ������� ������� ��� ���������
    public Image Arr_left;
    public Image Arr_right;
    public Image Em_Stop;

    public Text TPS;                    // ����� ��� FPS

    public Color orangeColor;



    ////// ���� � ������:

    string fileWayRoot = @"\Resources\"; // �������� ����� � �������� � �������, � ������ �������

    string OpenCVScript;                 // 3 ����, � ������ ��� ������, � �������� �������
    string ExportImageWay;
    string ImportDephMapWay;
    string mainImportDephMapWay;

    Process process;                     // ����� �������� ������� �������. ��������� ��� �����, ��� ����, ��� �� ����� ����� ����������

    public string newOpenCVScript;       // �������������� ����, � ������

    

    ////// �������� ������ ����� �������:

    const int allCountPixels = 58; // ����� �������� � �������� �����������
    float[] colArray = new float[allCountPixels]; // ������ �������� - �� � �������� ������ ������ �������



    ////// ���������� ����������:

    bool isShadowOn_In2DMap = true;    // �������� �� ���� �� ��������� �����? 
                                        // ��� �������� �� 200+ �������, �� ������ ������� fps. ��� ������� - ����� ���������

    bool isDephTexMiddling = false;     // �� ��������� �������� ����� �������?

    bool isPrint = false;               // ��� ������ ���������� ���������

    public bool isLineScene = false;



    ////// ������ ����������:

    bool isStartNewGame = true;         // ����������, ������� �����, ��� �� �������� ���������� ����� ������
    bool isThisScriptOnWork = false;    // ��� �� ������ OpenCV �������? ���������� �����, ��� ��� ���������� ���������
                                        //public TextMeshProUGUI debugText;


    // ��� ��������� ���������� ��������� ����, ����� �������� �������� - ��� �� ���� ����� � ���� ��������

    /*////////////////////////////////////////////////////
    //                   ����������:                    //
    //                                                  //
    //  ������ ���������                   110 �������  //                                
    //  Start                              120 �������  //                                
    //  ������� ����� Update               200 �������  //                                
    //  LOAD TEXTURE                       280 �������  //                                
    //  �������� ������� ����������� ----- 460 �������  //                                
    //  ������� � ���������                520 �������  //                                
    //  OnPixelCompression 1               600 �������  //                                
    //  OnPixelCompression 2 ------------- 720 �������  //                                
    //  Generate2DMap                      800 �������  //                                
    //  --- ����� ��������� ---           1400 �������  //                                
    //                                                  //                                
    ////////////////////////////////////////////////////*/


    /*///////////////////////////////////////////////////
    //                ������ ���������                 //
    ///////////////////////////////////////////////////*/

    private void Awake()
    {
        // ��� ���������� ������ - ��� ����� ����, ��� �� ���� ����� ����������

        Screen.SetResolution(1280, 720, true); // ������������� ���������� ������
        Screen.fullScreen = false; // ������������� ����� �������� ������
    }



    /*//////////////////////////////////////////////////
    //                     Start                      //
    //////////////////////////////////////////////////*/

    public void Start()
    {
        isThisScriptOnWork = true;

        // ���� ��� ������ ����� ��������
        CreateWhiteTexture();

        //print("____Start");

        // ��� �� ����������� ����, � ������ �������, �������, � � ������� OpenCV
        if (File.Exists(Directory.GetCurrentDirectory() + @"\Diplom Project 01_Data" + fileWayRoot + @"MyC_02.exe"))
        {
            // ��� ����, ���� ���� - ��� ����
            //print("���� ���������� �� ����: " + newOpenCVScript);
            fileWayRoot = Directory.GetCurrentDirectory() + @"\Diplom Project 01_Data" + fileWayRoot;

            OpenCVScript = fileWayRoot + @"MyC_02.exe";
            ExportImageWay = fileWayRoot + @"_ExportImage\";
            ImportDephMapWay = fileWayRoot + @"_ImportDephMap\NewDephMap.png";

            newOpenCVScript = OpenCVScript;
        }
        else
        {
            // � ��� ����, ���� ���� ��� ����������� � ���������
            fileWayRoot = Directory.GetCurrentDirectory() + @"\Assets" + fileWayRoot;

            newOpenCVScript = fileWayRoot + @"MyC_02.exe";
            ExportImageWay = fileWayRoot + @"_ExportImage\";
            ImportDephMapWay = fileWayRoot + @"_ImportDephMap\NewDephMap.png";
        }

        mainImportDephMapWay = ImportDephMapWay;

        //debugText.text = newOpenCVScript;

        // ������ � ��������� ������� - �������� ����� .exe - ������� �������� ���������������� �������� OpenCV, ��� ��������� ����� ������
        var processStartInfo = new ProcessStartInfo(newOpenCVScript);
        processStartInfo.WorkingDirectory = Path.GetDirectoryName(newOpenCVScript);
        process = Process.Start(processStartInfo); /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // ����������� �������� ��������� �� ������� ������, ��������������� 4� �������� ����������� �������
        Wall.enabled = false;
        Arr_left.enabled = false;
        Arr_right.enabled = false;
        Em_Stop.enabled = false;

        CreateMarcerLine(); // ������ ��������� ����� ������������ �����������
    }


    int marcerOffset = 6; // �����, ��������� ������� ����� ����������� �������

    void CreateMarcerLine()
    {
        Texture2D myNewTex = new Texture2D(allCountPixels, 1);

        Color32[] allWhitePixels = Enumerable.Repeat(new Color32(255, 255, 255, 255), myNewTex.width * myNewTex.height).ToArray();
        myNewTex.SetPixels32(allWhitePixels);
        myNewTex.Apply();

        // ����� ����������� ����
        // ���� �����-���� ����� ����� ���������� � ���� �������, ������ �������� ����������� ��������� � ������������, ���� ��������� ��������
        for (int i = (allCountPixels / 2) - marcerOffset; i < (allCountPixels / 2) + marcerOffset; i++)
        {
            myNewTex.SetPixel(i, 1, orangeColor);
        }

        myNewTex.Apply();
        OutMarcerLine.GetComponent<RawImage>().texture = myNewTex;
    }

    // ���� ��� ������ ����� ��������
    void CreateWhiteTexture()
    {
        whiteTex = new Texture2D(allCountPixels_toLocalMap, 100);

        Color[] whitePixels = new Color[allCountPixels_toLocalMap * 100];
        Array.Fill(whitePixels, Color.white);

        whiteTex.SetPixels(whitePixels);
        whiteTex.Apply();
    }




    /*////////////////////////////////////////////////////
    //               ������� ����� Update               //
    ////////////////////////////////////////////////////*/

    // ����� ������������ ������ ��� ����, ��� �� �������� MyWorkCoroutine 5 ��� � �������, � �� ������ ����
    private float timer = 0.0f;
    private float interval = 0.2f; // �������� � �������� (5 ��� � ������� = ������ 0.2 �������)

    public void FixedUpdate()
    {
        timer += Time.deltaTime;    // ����������� ������ �� �����, ��������� � ���������� �����

        if (timer >= interval)      // ���� ������ ������ ���������
        {
            MyWorkCoroutine();      // �������� �������
            timer = 0;              // ���������� ������
        }
    }




    // ��� ��������, � ������ ���� ��� �� Update � ���� �����
    void MyWorkCoroutine()
    {
        string fileName = "CamTexture_01";

        //if (isPrint) print("��������� ��������� ����� ������" + System.DateTime.Now.ToString("HH:mm:ss:ms"));
        //if (isPrint) print("��������� 2 �������� � �����" + System.DateTime.Now.ToString("HH:mm:ss:ms"));

        string l_way = ExportImageWay + "Left" + fileName;
        string r_way = ExportImageWay + "Right" + fileName;

        /////// ��������� ����� � ������ �������� � �����, � ����������� 

        try
        {
            // ��� ��� ����� ���������� ������, ���� ������� �� ��� ������� � ��������� ���� ������
            // �� �� � ����������
            SaveTextureToFileUtility.SaveRenderTextureToFile(LeftCam, l_way);
            SaveTextureToFileUtility.SaveRenderTextureToFile(RightCam, r_way);
        }
        catch (Exception ex)
        {
            // ��� ������ ������ �� ������, ������ ���������� ���������� ���������
            //UnityEngine.Debug.Log("��� ���������� ����������� ��������� ������");  //: " + ex);
            //MyWorkCoroutine(); // � ��������� ��� ��������� ��� ���
            return;
        }

        //if (isPrint) print("��������� 2 �������� � �����" + System.DateTime.Now.ToString("HH:mm:ss:ms"));

        // ������� ����������� ����� ������� � 1 �� ����� ��������, � ������ ����������� ��� �

        //// �������� ���������� � ���� � �����
        //var directory = Path.GetDirectoryName(mainImportDephMapWay);
        //var fileName1 = Path.GetFileNameWithoutExtension(mainImportDephMapWay);
        //var extension = Path.GetExtension(mainImportDephMapWay);

        //// ������� ����� ��� �����
        //var newFileName = $"{fileName1}1{extension}";

        //// ������� ����� ���� � �����
        //var newFilePath = Path.Combine(directory, newFileName);

        //// �������� ����
        //File.Copy(mainImportDephMapWay, newFilePath, true);

        //// ��������� ���������� ImportDephMapWay ����� �����
        //ImportDephMapWay = newFilePath;

        /////// ��������� ����� �������, � ���������� � ��� ������ �����������

        try
        {
            LoadTexture();  ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ��������!
        }
        catch (Exception ex)
        {
            //UnityEngine.Debug.Log("��� �������� ����������� ��������� ������");  //: " + ex);
            //MyWorkCoroutine(); // � ��������� ��� ��������� ��� ���
            return;
        }

        print("LoadTexture �����������");

        //if (isPrint) print("��������� ����� ������� ���������" + System.DateTime.Now.ToString("HH:mm:ss:ms"));
    }




    /*////////////////////////////////////////////////////
    //                   LOAD TEXTURE                   //
    ////////////////////////////////////////////////////*/

    // ��� ����������, ��� ����, ��� �� ��������� ��������� ����� �� ������ ������, � ��� � ��������� �������� �������� ���������
    int countOnOffRet_Generate2DMap = 0;

    Texture2D buferDephTex; // �������� �������� ����� �������, ������� ������������ ��� ����������

    float pervSummPixelOfDephMapTexture = 0;

    int stopCounter = 0; // ����� ����� ����� - ������� ��� ����������� �������� ����� ������� �� �����

    void LoadTexture()
    {
        if (stopCounter > 0)
        {
            stopCounter--;
            print("���������� �������� ����� �������");
            return;
        }

        string imagePath = ImportDephMapWay; 
        
        // �������� ����������� � ��������� Texture2D
        Texture2D tex = new Texture2D(2, 2); // ������� ��������� Texture2D

        byte[] fileData = File.ReadAllBytes(imagePath); // ������ ������ ����������� �� �����

        tex.LoadImage(fileData); // ��������� ������ � Texture2D



        if (isStartNewGame)
        {
            // ���� �� ��������� ���� ������ ������� � ���� ������, �� ��� ����� �������� ���������� �������� ����� �������
            isStartNewGame = false;

            return; // ������������� �������� ����� ������� � ������
            // ��������� �����, ������ OpenCV �������� ������� ����� �������, � � ������ ������ ������ ��������� �� �������� ������ ����� �����
        }



        // ����� - �������� �� ��, ���������� �� �������� �����������
        // � �������� ��� �������� �������� �� ������� ��������, � ��������� �� � ������ �������� ����� �������� �� ������� ����
        // ���� ������� > 20% - �� ��� �������� �� ������������

        float sum = 0;
        Color[] pixels = tex.GetPixels();
        foreach (Color pixel in pixels)
        {
            sum += pixel.r;
        }

        if (pervSummPixelOfDephMapTexture == 0 || sum == 0)
        {
            pervSummPixelOfDephMapTexture = sum;
        }
        else
        {
            if (sum > 1000)
            {
                if (Mathf.Abs(sum - pervSummPixelOfDephMapTexture) / pervSummPixelOfDephMapTexture > 0.20f)
                {
                    //print("������ ����� �������! ����������� ������������. perv_summ = " + pervSummPixelOfDephMapTexture + ", sum = " + sum + ", �������: " + Mathf.Abs(sum - pervSummPixelOfDephMapTexture) / pervSummPixelOfDephMapTexture);
                    print("������ ����� �������. ���������� ���� ���");
                    pervSummPixelOfDephMapTexture = sum;

                    // if (buferDephTex != null) DephMapImage.GetComponent<RawImage>().texture = buferDephTex; // ������������ �������� ����������� � ������ ���� ����� �������, �� �� ����������� ��� ������
                    // ��� ���������, �� ����� �����

                    return;
                }                
            }

            // ���� sum < 1000, �� ������ ����� �� ������� �� ������ ����� ������� - ����� ���������� ������������ ������ 

            pervSummPixelOfDephMapTexture = sum;
        }


        if (buferDephTex == null)
        {
            // ���� �������� ����� ������, �� �� ��������� � �� ������� ����� �������

            buferDephTex = new Texture2D(2, 2);
            buferDephTex.LoadImage(fileData); // ��������� ������ � Texture2D
        }

        // ���������� ����������� ����� �������
        // ����� �� ���� ������� �� ����������� ����� �������, ����� �� ������, ���������� � ����� �� ����� �������� ������� �������
        // ��� ��� ������ ����� �������� ��������� �����������
        if (isDephTexMiddling)
        {
            // ��������������, ��� tex � buferDephTex ��� ���������� � ����� ���������� �������
            int width = tex.width;
            int height = tex.height;

            // ������� ����� ������ ������ ��� �������� ����������� ��������
            Color[] averageColors = new Color[width * height];

            // �������� ������� �� ����� �������
            Color[] texColors = tex.GetPixels();
            Color[] buferDephTexColors = buferDephTex.GetPixels();

            for (int i = 0; i < width * height; i++)
            {
                // ��������� �������� ��������
                float averageR = (texColors[i].r + buferDephTexColors[i].r) / 2;
                float averageG = (texColors[i].g + buferDephTexColors[i].g) / 2;
                float averageB = (texColors[i].b + buferDephTexColors[i].b) / 2;

                // ��������� ����������� ���� � ����� �������
                averageColors[i] = new Color(averageR, averageG, averageB);
            }

            // ��������� ����� ����� � �������� tex
            tex.SetPixels(averageColors);
            tex.Apply();

            // ����������� �������� �� tex � buferDephTex
            Graphics.CopyTexture(tex, buferDephTex);
        }

        /// ��� ����� ����� ���, ��� � ������ �� �� ����� ��������� �� ����� �������                          ////////////// �� �������. �������� �����, ���� � ���� ������������ �������� �� �����
        // ��� ��� ���������� ������ �����, ������� ���� �������� �� ����� �������, ��� �� ��� �� ����������

        //float localAdder = 0;
        //int errorsCounter = 0;
        //float errorValue = 0.1f; // �������� ������ ����� �������, �� �������� �� ���������� ���

        //for (int h = 0; h < tex.height; h++)
        //{
        //    for (int w = 0; w < tex.width; w++)
        //    {
        //        float currPixel = tex.GetPixel(w, h).r;

        //        localAdder = currPixel;
        //        localAdder = (float)Math.Round(localAdder, 2);

        //        if (h >= 18 && h <= 132)
        //        {
        //            double yCoeff = -0.005486725 * (h - 17) + 0.87548672;

        //            double adder_avg = localAdder - yCoeff;

        //            if (Math.Abs(adder_avg) < errorValue)
        //            {
        //                //print("������� �������� �� ������ " + h + ", �� ��������� ���������� ����������, ���������� = " + adder_avg);

        //                // ������������ ���� ������� � ������
        //                tex.SetPixel(w, h, Color.black);
        //            }
        //            else
        //            {
        //                // �� ����� �������� ������� - � ����� ��? - �������� ���
        //                //print("!!! �� ������ " + h + " ���������� �� " + adder_avg + ", yCoeff = " + yCoeff + ", localAdder = " + localAdder);
        //                errorsCounter++;
        //            }

        //        }
        //    }
        //}

        //print("� ���� ������ ����� ������� ���� " + errorsCounter + " ��������� ��������");

        //tex.Apply();

        ///

        print("��������� ����� �������");

        DephMapImage.GetComponent<RawImage>().texture = tex; // ��������� ����������� ����� ������ � ���� �� ����, � ���������� ���������

        //if (isPrint) print("�������� �������� " + System.DateTime.Now.ToString("HH:mm:ss:ms"));

        OnPixelCompression(tex); // ������� �������� ����� ������� � ������������ �����

        // ���������� ��������� ����� ����������� ����� 3� ����� ��������� LoadTexture
        // �.�. �������� 1-2 ���� � �������
        if (countOnOffRet_Generate2DMap < 2) countOnOffRet_Generate2DMap++;
        else
        {
            countOnOffRet_Generate2DMap = 0;
            OnPixelCompression2(tex); 
            Generate2DMap();
        }        

        CheckObstacles(); // ��������� ������� �����������, ��������� ������ ����� ������
    }




    /*////////////////////////////////////////////////////
    //           �������� ������� �����������           //
    ////////////////////////////////////////////////////*/

    public bool thereIsObstacle = false;    // � ����������� ���� ���� �����������?
    public bool isEmergyStop = false;       // ����������� � ����������� ���� ��������� ������?

    // ��������, ��� �����������, � ����� ������� ������� ��������������
    public float leftNoiseValue;
    public float rightNoiseValue;

    public int whatTurn = 0; // ������������, � ����� ������� ����� ��������������

    // ���� �������� ����� ������� �� ������ ����� ������ ������
    float doorstepObst = 0.45f;  // ����� �������� - ������ ��������� ������
    float emergyObstacle = 0.9f; // - ������ ������������� ������, �� ������ ���������

    // �� ��������, ��� 1.0 - ��� ����� �������, ��������� ����� �� ���������� � ������

    void CheckObstacles() 
    {
        //print("����� ������� ������ �� ������� ���������");

        thereIsObstacle = false;    // ��������� ����������
        isEmergyStop = false;       // ��������� ������ ���������

        leftNoiseValue = 0f;
        rightNoiseValue = 0f;

        // ������� �� ���� ��������� � �������, ������� �������� ��� ������ ������������ �����������
        for (int i = (allCountPixels / 2) - marcerOffset; i < (allCountPixels / 2) + marcerOffset; i++)
        {
            if (colArray[i] > doorstepObst) thereIsObstacle = true;     // ���� ���� ����� �����������
            if (colArray[i] > emergyObstacle) isEmergyStop = true;  // ���� ���� ��������� ������� �����������
        }

        if (thereIsObstacle == true) // ���� ����������� ��-�� ����, �����������, � ����� ������� ������ ����������� �������� �����������
        {
            float bufer = 0;

            for (int i = 0; i < (allCountPixels / 2) - marcerOffset; i++)
            {
                bufer += colArray[i];
            }

            leftNoiseValue = (bufer / (allCountPixels / 2));                                                                                                  

            bufer = 0;

            for (int i = (allCountPixels / 2) + marcerOffset + 1; i < allCountPixels; i++)
            {
                bufer += colArray[i];
            }

            rightNoiseValue = (bufer / ((allCountPixels / 2) - 1));

            //print("����������� �������! leftNoiseValue = " + leftNoiseValue + ", rightNoiseValue = " + rightNoiseValue);
        }

        if (Colliding_14_Player.isCollidingStaris == false)
        {
            // ����� �������, ������ ���� �� �� ��������� � ���� ��������
            DodgingObstacles(); // �����, � ������� �, ��� �������������, ����� ������� ���������
        }
    }




    /*/////////////////////////////////////////////////////
    //                ������� � ���������                //
    /////////////////////////////////////////////////////*/

    int emStopCounterInit = 2;
    public int emStopCounter = 2;       // ����� �������� ��� ������ ���������, ��� ��� �� ����� ������� ����� ��������� ������ ���������� ����

    void DodgingObstacles()
    {
        bool wall_p = false;
        bool arr_l = false;
        bool arr_r = false;
        bool em_st = false;

        if (thereIsObstacle == false)
        {
            PlayerController_01.PlayerControl_StandartSpeedMove(); // ���� ����������� ���
            whatTurn = 0;
        }
        else // ���� ��� ����
        {
            if (isEmergyStop == true) // ���� ����� ������ ���������
            {
                if (emStopCounter > 0)
                {
                    emStopCounter--;
                    //print("������������ ������ ��� ������ ���������");
                }
                else
                {
                    PlayerController_01.PlayerControl_EmergyStop(); // ���� ��� ��-���� �� ������, � ��������� ������������� ���������
                    //print("������ ��������� ���������");
                    em_st = true;
                }
            }
            else
            {
                PlayerController_01.PlayerControl_Suspend(); // ���� ����� ���������� ������ ����������
                wall_p = true;
            }

            if (whatTurn == 0) // ���� ����������� �������� ��� �� �������
            {
                if (leftNoiseValue > rightNoiseValue)
                {
                    //print("������������� �������");
                    PlayerController_01.PlayerControl_TurnRight(1, 100); 
                    arr_r = true;
                }
                else
                {
                    //print("������������� ������");
                    PlayerController_01.PlayerControl_TurnLeft(1, 100); 
                    arr_l = true;
                }
            }
        }

        if (isEmergyStop == false)
        {
            if (emStopCounter < emStopCounterInit) emStopCounter++;
        }

        speedText.text = "�������� = " + (Math.Round(PlayerController_01.Speed, 1) * 4).ToString() + " ��/�"; // ����� �������� ��� ������� ����������

        // ������� � �������� ��������, � ������� ����:
        Wall.enabled = wall_p;
        Arr_left.enabled = arr_l;
        Arr_right.enabled = arr_r;
        Em_Stop.enabled = em_st;
    }




    /*/////////////////////////////////////////////////////
    //                OnPixelCompression 1               //
    /////////////////////////////////////////////////////*/

    int loadTimer = 10;

    // ������� ��������� ������ �������� �� ����� �������, ������� � ���������� ������������ ��� �������� �� ����������� �������
    Texture2D OnPixelCompression(Texture2D texture)
    {
        int currCountUse = 0;           // ����� �����, ������� �� ������. 10 ����� ����������� � ���� ������ � �������
        int currIndOfMass = 0;          // ������ � �������        
        int countNoisyPixels = 50; //20 // ������� ������ �������� �� ��������� � �����, ����� �������������� ������ �������� �������� //////////////////////// !!!!!! �������������� �� ���������� �����
        int noDetection = countNoisyPixels;

        //if (isLineScene == true)
        //{
        //    countNoisyPixels = 50;
        //}

        Array.Clear(colArray, 0, colArray.Length); // ������� ������� ������

        if (loadTimer > 0)
        {
            loadTimer--;
        }

        for (int w = 0; w < texture.width; w++)
        {
            float adder = 0;

            for (int h = 0; h < texture.height; h++)
                //for (int h = (texture.height / 2) + 10; h < texture.height; h++)
            {
                float currPixel = texture.GetPixel(w, h).r;

                if (noDetection > 0)
                {
                    if (currPixel > doorstepObst)
                    {
                        noDetection--;
                    }
                }
                else
                {
                    if (currPixel > colArray[currIndOfMass]) colArray[currIndOfMass] = currPixel;
                }
            }

            currCountUse++;

            if (currCountUse >= 10) // ���� �� ��� ������ 10 �����, �� ��������� �� ���������� ��������� ������ � �������
            {
                currCountUse = 0;
                currIndOfMass++;
                noDetection = countNoisyPixels;
            }
        }

        //// ��� ��� ���������� ������ �����, ������� ���� �������� �� ����� �������, ��� �� ��� �� ����������
        //// ���� ��� - ��� �������. �� ��������.
        //if (loadTimer == 0)
        //{
        //    float adder = 0;

        //    for (int h = 0; h < texture.height; h++)
        //    {              
        //        for (int w = 0; w < texture.width; w++) 
        //        {
        //            float currPixel = texture.GetPixel(w, h).r;

        //            adder += currPixel;
        //        }

        //        if (true) // ���� �� ��� ������ 10 �������, �� ��������� �� ���������� ��������� ������ � �������
        //        {
        //            adder = (adder / texture.width);
        //            currCountUse_2 = 0;

        //            int th = (h);
        //            //margArr[w] = adder; // ������� �����, ����� �� ������ ���������� �����

        //            adder = (float)Math.Round(adder, 2);

        //            // ��� ����� ������� ������� ������� �������� ����:

        //            if (h >= 18 && h <= 132)
        //            {
        //                double yCoeff = -0.005486725 * (h - 17) + 0.87548672;

        //                double adder_avg = adder - yCoeff;

        //                if (Math.Abs(adder_avg) < 0.05f)
        //                {
        //                    print("������� �������� �� ������ " + h + ", �� ��������� ���������� ����������, yCoeff = " + yCoeff + ", adder = " + adder + ", adder_avg = " + adder_avg);
        //                }
        //                else 
        //                {
        //                    print("!!! �� ������ " + h + " ���������� �� " + adder_avg + ", yCoeff = " + yCoeff + ", adder = " + adder);
        //                }

        //            }

        //            //print("adder[" + th + "] = " + adder);
        //            adder = 0;
        //        }
        //    }

        //    loadTimer--;
        //}

        Texture2D myTex = new Texture2D(58, 1);

        for (int w1 = 0; w1 < 58; w1++)
        {
            Color newPixelColor = new Color(colArray[w1], colArray[w1], colArray[w1], 1f);
            myTex.SetPixel(w1, 0, newPixelColor);
        }

        myTex.Apply();

        OutLineTexture.GetComponent<RawImage>().texture = myTex;

        return myTex;
    }




    /*/////////////////////////////////////////////////////
    //                OnPixelCompression 2               //
    /////////////////////////////////////////////////////*/

    // ��� ��������� �����:
    const int allCountPixels_toLocalMap = 116;  // ����� �������� � �������� �����������
    float[] colArray_toLocalMap = new float[allCountPixels_toLocalMap];
    float doorstep_toLocalMap = 0.1f;           // ����� ������������� �����������. ��� �������� ������ - ��� ������ ����������� �������� �� �������� �� �����
    int countNoisyPixels_toLocalMap = 150;      // 75

    // ����� ������� ����� �������, �� ���������� � 2 ���� ������, � ��������������� - ������� ����
    Texture2D OnPixelCompression2(Texture2D texture)
    {
        // ���������� �� ��������� ������ 1, ��� ����� � 2 ���� ������ ������ ����������� - �� 58, � 116 ��������

        int currCountUse = 0;               // ����� �����, ������� �� ������. 10 ����� ����������� � ���� ������ � �������
        int currIndOfMass = 0;              // ������ � �������        
        int noDetection = countNoisyPixels_toLocalMap;

        Array.Clear(colArray_toLocalMap, 0, colArray_toLocalMap.Length); // ������� ������� ������

        for (int w = 0; w < texture.width; w++)
        {
            for (int h = 0; h < texture.height; h++)
            //for (int h = (texture.height / 2) + 10; h < texture.height; h++)
            {
                float currPixel = texture.GetPixel(w, h).r;

                if (noDetection > 0)
                {
                    if (currPixel > doorstep_toLocalMap)
                    {
                        noDetection--;
                    }
                }
                else
                {
                    if (currPixel > colArray_toLocalMap[currIndOfMass]) colArray_toLocalMap[currIndOfMass] = currPixel;
                }
            }

            currCountUse++;

            if (currCountUse >= 5) // ���� �� ��� ������ 5 �����, �� ��������� �� ���������� ��������� ������ � �������
            {
                currCountUse = 0;
                currIndOfMass++;

                noDetection = countNoisyPixels_toLocalMap;
            }
        }

        Texture2D myTex = new Texture2D(116, 1);

        for (int w1 = 0; w1 < 116; w1++)
        {
            Color newPixelColor = new Color(colArray_toLocalMap[w1], colArray_toLocalMap[w1], colArray_toLocalMap[w1], 1f);
            myTex.SetPixel(w1, 0, newPixelColor);
        }

        myTex.Apply();

        //OutLineTexture.GetComponent<RawImage>().texture = myTex; // ����� ��� ����������� � ���� � ���������
        return myTex;
    }




    /*////////////////////////////////////////////////////
    //                   Generate2DMap                  //
    ////////////////////////////////////////////////////*/

    Texture2D resultTex;

    float massMultipler = 2.6f; // ����� �����������, ��� �������� �������� ����� �������, � ���������� �� ��������, � ������ 
    // ���� ����������� ������

    // ������� ������� 116*100, ��� 116 - ��� ���������� ��������, � 100 - ��� ����� �� 0 �� 10 ������ (���������� �� �����������)
    bool[,] matrix = new bool[allCountPixels_toLocalMap, 100];

    Texture2D whiteTex; // ����� ��������, ��� �� ������ ��� �� ��������� � ������ // � ������ � � ������ Start

    // �������� ������ �������� ������ ������ �������� colArray
    // �����, ����������� �������� ����� � ���������� �� �������, � ������
    // ������, ��������� ����� ���������� � �������, � ��� ������� ��������������� � ��������� �����
    void Generate2DMap()
    {
        matrix = new bool[allCountPixels_toLocalMap, 100]; // ������ �������

        // ������ ������� �������� ����-�����
        resultTex = new Texture2D(116, 100);

        resultTex.SetPixels(whiteTex.GetPixels());
        resultTex.Apply();

        // ���������� �������� ����� ������ ��������, � ���������� �� �������
        for (int i = 0; i < colArray_toLocalMap.Length; i++)
        {
            if (colArray_toLocalMap[i] != 0)
            {
                colArray_toLocalMap[i] = (1 / (colArray_toLocalMap[i])) * massMultipler;
                colArray_toLocalMap[i] = (float)Math.Round(colArray_toLocalMap[i], 2);    // �������� �� 2� ������, ����� �������, ��� �� ���� ����� ������� ��� ��� �����
            }
        }

        // ��������� �� ������� ����������, �������� ��� � �������, � ����� � 0.1 ����. �����, ������� ���������� � �����������
        for (int i = 0; i < colArray_toLocalMap.Length; i++)
        {
            int index = (int)Math.Round(colArray_toLocalMap[i] * 10); // �������� �������� float �� 0 �� 10 � ����� �������� int �� 0 �� 100

            if ((index > 0) && (index < 100))
            {
                int j = index;
                //j = 50; // ��� ������������ Sin �������

                // ��������� �������� �� ������� � �������: � ������� ������������� ������� = true
                {
                    matrix[i, j] = true; // ������������� ��������������� ������� � true
                }

                // �������� ����� ��������� �� ����� � ������ �� ����������� = 60 ��������, ��� fov � ������
                {
                    // �������������� ������� i � ������� �� 60 �� 120, ����� � �������
                    double angleInRadians = ((i * 60 / 115.0) + 60) * Math.PI / 180.0;

                    //double angleInRadians = (i * 180 / 57.0) * Math.PI / 180.0; // �������������� � �������� �� 0 �� 180 ��������

                    double curSin = Math.Sin(angleInRadians);

                    curSin = Math.Round(curSin, 2); // ��������, ��� ��������� ����������

                    int newVal = 100 - (j - (int)(j * curSin));
                    newVal = newVal - (100 - j);

                    matrix[i, j] = false;

                    if (newVal >= 0 && newVal < 100)
                    {
                        matrix[i, newVal] = true;
                        //resultTex.SetPixel(i, newVal, Color.black); // ���� �������� � ������� true, ������������� ������� � ������ ����
                    }
                }
            }
        }

        //// ������ ��� ���������� �������� �����
        //for (int i = 0; i < colArray_toLocalMap.Length; i++)
        //{
        //    for (int j = 0; j < 100; j++)
        //    {
        //        if (matrix[i, j] == true)
        //        {
        //            resultTex.SetPixel(i, j, Color.black); // ���� �������� � ������� true, ������������� ������� � ������ ����
        //        }
        //    }
        //}

        //int leavePixels = 0;
        int dimm = 2;

        // �������� ������� ������� ����� (� ������� � ������� �������� 5�5 ��� ������ ������ �����) - ������ ��
        for (int i = 0; i < colArray_toLocalMap.Length; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                if (matrix[i, j] == true)
                {
                    bool hasNeighbor = false;

                    // ��������� �������� ������ � ������� 2� �����
                    for (int di = i - dimm; di <= i + dimm; di++)
                    {
                        for (int dj = j - dimm; dj <= j + dimm; dj++)
                        {
                            // ���������, ��� �� ������� �� ������� �������
                            if (di >= 0 && di < colArray_toLocalMap.Length && dj >= 0 && dj < 100)
                            {
                                // ������� ��� ����, ��� �� ����������� ����� �� �����������
                                if ((di != i) && (dj != j)) 
                                {
                                    // ���� ������� �������� ������ �� ��������� true, �� ������������� ���� hasNeighbor � true
                                    if (matrix[di, dj] == true)
                                    {
                                        hasNeighbor = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (hasNeighbor)
                        {
                            break;
                        }
                    }

                    // ���� ���� �������� ������ �� ��������� true, �� ������������� ������� � ������ ����
                    if (hasNeighbor)
                    {
                        resultTex.SetPixel(i, j, Color.black);
                    }
                    //else
                    //{
                    //    leavePixels++;
                    //}
                }
            }
        }

        //if (leavePixels > 0)
        //{
        //    print("���������� ��������: " + leavePixels);
        //}

        Texture2D resultTexOversize;

        int resultTexWidth = resultTex.width;
        int resultTexHeight = resultTex.height;

        // ������ �������� � ����������� ��������
        {
            // ������� ����� �������� � ����������� ����� ��������
            resultTexOversize = new Texture2D(resultTexWidth + 58 * 2, resultTexHeight + 23);

            // ��������� ����� �������� ������ ���������
            for (int y = 0; y < resultTexOversize.height; ++y)
            {
                for (int x = 0; x < resultTexOversize.width; ++x)
                {
                    resultTexOversize.SetPixel(x, y, Color.white);
                }
            }

            // �������� ������� �� �������� �������� � �����
            for (int y = 0; y < resultTexHeight; ++y)
            {
                for (int x = 0; x < resultTexWidth; ++x)
                {
                    resultTexOversize.SetPixel(x + 58, y, resultTex.GetPixel(x, y));
                }
            }

            // ��������� ��������� � ��������
            resultTexOversize.Apply();
        }

        int resultTexOversizeWidth = resultTexOversize.width;
        int resultTexOversizeHeight = resultTexOversize.height;


        // ���� � ����������� �������� �����������
        if (isShadowOn_In2DMap)
        {
            Color shadowColor = new Color(0.7f, 0.7f, 0.7f); // ����� ���� ��� ����

            // ������ ����
            for (int i = 0; i < resultTexOversizeWidth; i++)
            {
                for (int j = 0; j < resultTexOversizeHeight; j++)
                {
                    Color pixelColor = resultTexOversize.GetPixel(i, j);

                    if (pixelColor == Color.black) // ���� ������� �������� ��������
                    {
                        float dx = i - (resultTexOversizeWidth / 2); // ���������� ��������� ����� �� X
                        float dy = j - 0; // ���������� ��������� ����� �� Y

                        float angle = Mathf.Atan2(dy, dx);

                        int shadowLength = resultTexOversizeWidth + resultTexOversizeHeight;

                        // ������ ����
                        for (int k = 2; k <= shadowLength; k++)
                        {
                            int shadowX = i + (int)(k * Mathf.Cos(angle));
                            int shadowY = j + (int)(k * Mathf.Sin(angle));

                            // ���������, ��� ���������� ���� �� ������� �� ������� �����������
                            if (shadowX >= 0 && shadowX < resultTexOversizeWidth && shadowY >= 0 && shadowY < resultTexOversizeHeight)
                            {
                                resultTexOversize.SetPixel(shadowX, shadowY, shadowColor);

                                float expansionCoefficient = 0.4f; // ����������� ���������� ����

                                // ������ ������������� ���� ����� � ������ �� �������� ����� ����
                                for (int l = 1; l <= Math.Ceiling(k * expansionCoefficient); l++)
                                {
                                    int shadowLeftX = shadowX - (int)Math.Ceiling(l * Mathf.Sin(angle));
                                    int shadowLeftY = shadowY + (int)Math.Ceiling(l * Mathf.Cos(angle));

                                    int shadowRightX = shadowX + (int)Math.Ceiling(l * Mathf.Sin(angle));
                                    int shadowRightY = shadowY - (int)Math.Ceiling(l * Mathf.Cos(angle));

                                    // ���������, ��� ���������� ���� �� ������� �� ������� �����������
                                    if (shadowLeftX >= 0 && shadowLeftX < resultTexOversizeWidth && shadowLeftY >= 0 && shadowLeftY < resultTexOversizeHeight)
                                    {
                                        // ��������, ��� � �� ���������� ����� ������ �������
                                        if (resultTexOversize.GetPixel(shadowLeftX, shadowLeftY) != Color.black)
                                        {
                                            resultTexOversize.SetPixel(shadowLeftX, shadowLeftY, shadowColor);
                                        }
                                    }

                                    if (shadowRightX >= 0 && shadowRightX < resultTexOversizeWidth && shadowRightY >= 0 && shadowRightY < resultTexOversizeHeight)
                                    {
                                        if (resultTexOversize.GetPixel(shadowLeftX, shadowLeftY) != Color.black)
                                        {
                                            resultTexOversize.SetPixel(shadowRightX, shadowRightY, shadowColor);
                                        }
                                    }
                                }
                            }
                            else if (shadowX >= resultTexOversizeWidth && shadowY >= resultTexOversizeHeight)
                            {
                                // ������ �� ����� ������ �����, ����� ���� ��������� ����������
                                break;
                            }
                        }
                    }
                }
            }

            // ������� ������� ������� ����, � ����������� ��
            for (int i = 0; i < resultTexOversizeWidth; i++)
            {
                for (int j = 0; j < resultTexOversizeHeight; j++)
                {
                    int rCount = 0; // ���������� ������� �������� ����� � ����
                    int dimm2 = 1;  // ������ ������ ������� ��������

                    if (resultTexOversize.GetPixel(i, j) == Color.white)
                    {
                        //print("����� ����� �������");

                        // ��������� �������� ������ � ������� 1 ������
                        for (int di = i - dimm2; di <= i + dimm2; di++)
                        {
                            for (int dj = j - dimm2; dj <= j + dimm2; dj++)
                            {
                                // ���������, ��� �� ������� �� ������� �������
                                if (di >= 0 && di < resultTexOversizeWidth && dj >= 0 && dj < resultTexOversizeHeight)
                                {
                                    float tolerance = 0.01f; // ������

                                    Color pixelColor = resultTexOversize.GetPixel(di, dj);
                                    if (Mathf.Abs(pixelColor.r - shadowColor.r) <= tolerance &&
                                        Mathf.Abs(pixelColor.g - shadowColor.g) <= tolerance &&
                                        Mathf.Abs(pixelColor.b - shadowColor.b) <= tolerance)
                                    {
                                        rCount++;
                                        //print("�������� ����� - �������");
                                        //print("pixelColor.r = " + pixelColor.r + ", pixelColor.g = " + pixelColor.g + ", pixelColor.b = " + pixelColor.b);

                                        if (rCount >= 4)
                                        {
                                            //print("����� ������� ������������� �����");
                                            resultTexOversize.SetPixel(i, j, shadowColor);

                                            di = i + dimm2 + 1;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // ���� ��� ��������� �����
        {

            //if (isShadowOn_In2DMap)
            //{
            //    Color shadowColor = new Color(0.7f, 0.7f, 0.7f); // ����� ���� ��� ����

            //    // ������ ����
            //    for (int i = 0; i < resultTexWidth; i++)
            //    {
            //        for (int j = 0; j < resultTexHeight; j++)
            //        {
            //            Color pixelColor = resultTex.GetPixel(i, j);

            //            if (pixelColor == Color.black) // ���� ������� �������� ��������
            //            {
            //                float dx = i - (resultTexWidth / 2); // lightSourceX - ���������� ��������� ����� �� X
            //                float dy = j - 0; // lightSourceY - ���������� ��������� ����� �� Y

            //                float angle = Mathf.Atan2(dy, dx);

            //                int shadowLength = resultTexWidth + resultTexHeight;

            //                // ������ ����
            //                for (int k = 2; k <= shadowLength; k++)
            //                {
            //                    int shadowX = i + (int)(k * Mathf.Cos(angle));
            //                    int shadowY = j + (int)(k * Mathf.Sin(angle));

            //                    // ���������, ��� ���������� ���� �� ������� �� ������� �����������
            //                    if (shadowX >= 0 && shadowX < resultTexWidth && shadowY >= 0 && shadowY < resultTexHeight)
            //                    {
            //                        resultTex.SetPixel(shadowX, shadowY, shadowColor);

            //                        float expansionCoefficient = 0.4f; // ����������� ���������� ����

            //                        // ������ ������������� ���� ����� � ������ �� �������� ����� ����
            //                        for (int l = 1; l <= Math.Ceiling(k * expansionCoefficient); l++)
            //                        {
            //                            int shadowLeftX = shadowX - (int)Math.Ceiling(l * Mathf.Sin(angle));
            //                            int shadowLeftY = shadowY + (int)Math.Ceiling(l * Mathf.Cos(angle));

            //                            int shadowRightX = shadowX + (int)Math.Ceiling(l * Mathf.Sin(angle));
            //                            int shadowRightY = shadowY - (int)Math.Ceiling(l * Mathf.Cos(angle));

            //                            // ���������, ��� ���������� ���� �� ������� �� ������� �����������
            //                            if (shadowLeftX >= 0 && shadowLeftX < resultTexWidth && shadowLeftY >= 0 && shadowLeftY < resultTexHeight)
            //                            {
            //                                // ��������, ��� � �� ���������� ����� ������ �������
            //                                if (resultTex.GetPixel(shadowLeftX, shadowLeftY) != Color.black)
            //                                {
            //                                    resultTex.SetPixel(shadowLeftX, shadowLeftY, shadowColor);
            //                                }
            //                            }

            //                            if (shadowRightX >= 0 && shadowRightX < resultTexWidth && shadowRightY >= 0 && shadowRightY < resultTexHeight)
            //                            {
            //                                if (resultTex.GetPixel(shadowLeftX, shadowLeftY) != Color.black)
            //                                {
            //                                    resultTex.SetPixel(shadowRightX, shadowRightY, shadowColor);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    else if (shadowX >= resultTexWidth && shadowY >= resultTexHeight)
            //                    {
            //                        // ������ �� ����� ������ �����, ����� ���� ��������� ����������
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    // ������� ������� ������� ����, � ����������� ��
            //    for (int i = 0; i < colArray_toLocalMap.Length; i++)
            //    {
            //        for (int j = 0; j < 100; j++)
            //        {
            //            int rCount = 0; // ���������� ������� �������� ����� � ����
            //            int dimm2 = 1;  // ������ ������ ������� ��������

            //            if (resultTex.GetPixel(i, j) == Color.white)
            //            {
            //                //print("����� ����� �������");

            //                // ��������� �������� ������ � ������� 1 ������
            //                for (int di = i - dimm2; di <= i + dimm2; di++)
            //                {
            //                    for (int dj = j - dimm2; dj <= j + dimm2; dj++)
            //                    {
            //                        // ���������, ��� �� ������� �� ������� �������
            //                        if (di >= 0 && di < colArray_toLocalMap.Length && dj >= 0 && dj < 100)
            //                        {
            //                            float tolerance = 0.01f; // ������

            //                            Color pixelColor = resultTex.GetPixel(di, dj);
            //                            if (Mathf.Abs(pixelColor.r - shadowColor.r) <= tolerance &&
            //                                Mathf.Abs(pixelColor.g - shadowColor.g) <= tolerance &&
            //                                Mathf.Abs(pixelColor.b - shadowColor.b) <= tolerance)
            //                            {
            //                                rCount++;
            //                                //print("�������� ����� - �������");
            //                                //print("pixelColor.r = " + pixelColor.r + ", pixelColor.g = " + pixelColor.g + ", pixelColor.b = " + pixelColor.b);

            //                                if (rCount >= 4)
            //                                {
            //                                    //print("����� ������� ������������� �����");
            //                                    resultTex.SetPixel(i, j, shadowColor);

            //                                    di = i + dimm2 + 1;
            //                                    break;
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        // ��������������� - �� �������� ��� ����
        {
            /*
        
        if (isShadowOn_In2DMap)
        {
            Color shadowColor = new Color(0.7f, 0.7f, 0.7f); // ����� ���� ��� ����

            // �������������� �������� � �������
            int[,] matrix_texture = new int[resultTex.width, resultTex.height];
            for (int i = 0; i < resultTex.width; i++)
            {
                for (int j = 0; j < resultTex.height; j++)
                {
                    Color pixelColor = resultTex.GetPixel(i, j);
                    if (pixelColor == Color.white)
                        matrix_texture[i, j] = 0;
                    else if (pixelColor == shadowColor)
                        matrix_texture[i, j] = 1;
                    else if (pixelColor == Color.black)
                        matrix_texture[i, j] = 2;
                }
            }

            //// ���������� �������� � ������������ �������
            //Parallel.For(0, resultTex.width, i =>
            //{
            //    for (int j = 0; j < resultTex.height; j++)
            //    {
            //        // ��� ���, �� ������ ����������� matrix[i, j] ������ resultTex.GetPixel(i, j)
            //    }
            //});

            int resultTexWidth = resultTex.width;
            int resultTexHeight = resultTex.height;

            Parallel.For(0, resultTexWidth, i =>
            {
                print("i = " + i);
                for (int j = 0; j < resultTexHeight; j++)
                {
                    int pixelColor_int = matrix_texture[i, j];

                    if (pixelColor_int == 2) // ���� ������� �������� ��������
                    {
                        float dx = i - (resultTexWidth / 2); // lightSourceX - ���������� ��������� ����� �� X
                        float dy = j - 0; // lightSourceY - ���������� ��������� ����� �� Y

                        float angle = Mathf.Atan2(dy, dx);

                        int shadowLength = resultTexWidth + resultTexHeight;

                        // ������ ����
                        for (int k = 2; k <= shadowLength; k++)
                        {
                            int shadowX = i + (int)(k * Mathf.Cos(angle));
                            int shadowY = j + (int)(k * Mathf.Sin(angle));

                            // ���������, ��� ���������� ���� �� ������� �� ������� �����������
                            if (shadowX >= 0 && shadowX < resultTexWidth && shadowY >= 0 && shadowY < resultTexHeight)
                            {
                                matrix_texture[shadowX, shadowY] = 1;

                                float expansionCoefficient = 0.4f; // ����������� ���������� ����

                                // ������ ������������� ���� ����� � ������ �� �������� ����� ����
                                for (int l = 1; l <= Math.Ceiling(k * expansionCoefficient); l++)
                                {
                                    int shadowLeftX = shadowX - (int)Math.Ceiling(l * Mathf.Sin(angle));
                                    int shadowLeftY = shadowY + (int)Math.Ceiling(l * Mathf.Cos(angle));

                                    int shadowRightX = shadowX + (int)Math.Ceiling(l * Mathf.Sin(angle));
                                    int shadowRightY = shadowY - (int)Math.Ceiling(l * Mathf.Cos(angle));

                                    // ���������, ��� ���������� ���� �� ������� �� ������� �����������
                                    if (shadowLeftX >= 0 && shadowLeftX < resultTexWidth && shadowLeftY >= 0 && shadowLeftY < resultTexHeight)
                                    {
                                        // ��������, ��� � �� ���������� ����� ������ �������
                                        if (matrix_texture[shadowLeftX, shadowLeftY] != 2)
                                        {
                                            matrix_texture[shadowLeftX, shadowLeftY] = 1;
                                        }
                                    }

                                    if (shadowRightX >= 0 && shadowRightX < resultTexWidth && shadowRightY >= 0 && shadowRightY < resultTexHeight)
                                    {
                                        if (matrix_texture[shadowLeftX, shadowLeftY] != 2) 
                                        {
                                            matrix_texture[shadowRightX, shadowRightY] = 1;
                                        }
                                    }
                                }
                            }
                            else if (shadowX >= resultTexWidth && shadowY >= resultTexHeight)
                            {
                                // ������ �� ����� ������ �����, ����� ���� ��������� ����������
                                break;
                            }
                        }
                    }
                }
            });

            // ������� ������� ������� ����, � ����������� ��
            for (int i = 0; i < colArray_toLocalMap.Length; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    int rCount = 0; // ���������� ������� �������� ����� � ����
                    int dimm2 = 1;  // ������ ������ ������� ��������

                    if (matrix_texture[i, j] == 0)
                    {
                        //print("����� ����� �������");

                        // ��������� �������� ������ � ������� 1 ������
                        for (int di = i - dimm2; di <= i + dimm2; di++)
                        {
                            for (int dj = j - dimm2; dj <= j + dimm2; dj++)
                            {
                                // ���������, ��� �� ������� �� ������� �������
                                if (di >= 0 && di < colArray_toLocalMap.Length && dj >= 0 && dj < 100)
                                {
                                    if (matrix_texture[di, dj] == 1)
                                    {
                                        rCount++;
                                        //print("�������� ����� - �������");
                                        //print("pixelColor.r = " + pixelColor.r + ", pixelColor.g = " + pixelColor.g + ", pixelColor.b = " + pixelColor.b);

                                        if (rCount >= 4)
                                        {
                                            //print("����� ������� ������������� �����");
                                            matrix_texture[i, j] = 1;


                                            di = i + dimm2 + 1;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // �������������� ������� ������� � ��������
            for (int i = 0; i < resultTex.width; i++)
            {
                for (int j = 0; j < resultTex.height; j++)
                {
                    if (matrix_texture[i, j] == 0)
                        resultTex.SetPixel(i, j, Color.white);
                    else if (matrix_texture[i, j] == 1)
                        resultTex.SetPixel(i, j, shadowColor);
                    else if (matrix_texture[i, j] == 2)
                        resultTex.SetPixel(i, j, Color.black);
                }
            }
        }
            */
        }



        // ��������� ��������� � ��������
        resultTex.Apply();
        resultTexOversize.Apply();

        // ������� �����������, ��� �� ��� ���� ��������, � �� ��������
        resultTex.filterMode = FilterMode.Point;
        resultTexOversize.filterMode = FilterMode.Point;

        // �������� ��� �������� � ������ �� ������
        //Outp2DMapTex.GetComponent<RawImage>().texture = resultTex;

        Outp2DMapTexOversize.GetComponent<RawImage>().texture = resultTexOversize;

        //print("����������� ����������� �����");
    }




    void OnApplicationQuit() // ��� �������� ����, �� �������� ��������� ������ ��������� ����� ������
    {
        // ���� ���, ���� ����� ����������� ������, ���� ����� ���� ������ ��������

        if (isThisScriptOnWork == true)
        {
            //print("____End");
            process.Kill();
            //process.CloseMainWindow();
            //process.Close();
            process.WaitForExit(); // �������� ���������� ��������
        }
    }
}

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

    public bool isOverWallGo = true; // –ежим передвижени€ игрока. ≈сли true, то автоматический

    float objSpeed = 1f; // —тандартна€ скорость                    // ј было 2f
    const float suspendSpeed = 0.1f; // —тандартное замедление
    float minSpeed = 0.5f; // ћинимальна€ скорость, во врем€ замедлени€

    // ѕеременные дл€ перкращени€ движени€:
    public bool isFinished = false;         // »спользуетс€, когда мы дошли до конечной точки маршрута
    public bool isStopped = false;          // »спользуетс€, когда мы делаем анимацию построени€ карты

    public IconOfStats IconOfStats;

    // Ётот метод вызываетс€, если перед игроком преп€тствие, и ему нужно замедлитьс€
    public void PlayerControl_Suspend(float currSusSpeed = suspendSpeed) // —корость замедлени€ можно как передать в метод, так и нет
    {
        if (Speed > minSpeed)
        {
            Speed -= currSusSpeed;
        }
    }

    // Ётот метод вызываетс€, если преп€тствие перед игроком находитс€ угрожающе близко
    public void PlayerControl_EmergyStop()
    {
        Speed = 0;
        //print("ѕолна€ остановка");
    }

    // Ётот метод вызываетс€, если впереди нет преп€тствий
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

    // Ќекоторые скрипты могут посылать более приоритетные команды, чем другие
    public int isPriority = 0;

    // ¬ priority нужно передавать большое значение, типо 100

    // Ётот метод вызываетс€, когда игроку нужно повернутьс€ влево
    public void PlayerControl_TurnLeft(float multipler = 1, int priority = 0)
    {
        if(priority > 0) isPriority = priority;
        if (isPriority <= 0 || priority > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - (SpeedRot * multipler), transform.rotation.eulerAngles.z);
            //print("]< PlayerController_01: ¬ыполн€етс€ поворот налево, приоритет = " + priority + ", multipler = " + multipler);
            print("]< PlayerController_01: mult = " + multipler + ", p =  " + priority);
        }
    }

    // Ётот метод вызываетс€, когда игроку нужно повернутьс€ вправо
    public void PlayerControl_TurnRight(float multipler = 1, int priority = 0)
    {
        if (priority > 0) isPriority = priority;
        if (isPriority <= 0 || priority > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (SpeedRot * multipler), transform.rotation.eulerAngles.z);
            //print("]> PlayerController_01: ¬ыполн€етс€ поворот направо, приоритет = " + priority + ", multipler = " + multipler);
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

    // јвтоматический режим движени€ персонажа
    void OverWallGoMove()
    {
        if (isPriority > 0) isPriority--;

        moveDirection = new Vector3(0f * Speed, moveDirection.y, Speed);

        moveDirection.y -= Gravity * Time.deltaTime;
        characterController.Move(transform.TransformVector(moveDirection) * Time.deltaTime);
    }

    //// —тандартный режим управлени€ персонажем, пользователем
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

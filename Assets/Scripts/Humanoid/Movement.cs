using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
public class Movement : StateManagerComponent
{
    public  event UnityAction Moved;
    public Movement(MonoCalls.MonoAcessors manager, CharacterController pController, Transform groundCheck, LayerMask gMask, Transform body) : base(manager)
    {
        this.pController = pController;
        this.groundCheck = groundCheck;
        this.gMask = gMask;
        this.body = body;
        manager.AwakeCall.Listen(Awake);
        manager.StartCall.Listen(Start);
        manager.UpdateCall.Listen(Update);
    }

    
    //https://www.youtube.com/watch?v=hkMo59axs6A
    CharacterController pController; //Reference to PlayerControler
    Transform body;
    float speed = 6f;
    float jumpHeight = 0.02f;
    float grav = -0.1f;
    float groundDistance = 0.4f;
    float stamina;
    public Vector3 Velocity;
  
   
    Transform groundCheck;
    LayerMask gMask;
  
    Vector3 moveTranslation;
   
    float x; // Get the wasd inputs
    float z;
    public Vector3 xzChange;
    Vector3 yVeloc;
    public Action<float, float> MovingFunction;
    Vector3 postPosition;
    Vector3 prePosition;


    // Start is called before the first frame update
      void Awake()
    {
        MovingFunction = Move;
    }
      void Start()
    {

        prePosition = body.position;
    }
    

    // Update is called once per frame
    void CalculateTotalVeloicty()
    {
        prePosition = body.position;
        Velocity = (postPosition - prePosition) / Time.deltaTime;
        postPosition = body.position;
    }
      void Update()
    {
        CalculateTotalVeloicty();

        GravityApply();
    }
    //
   
  
  void CalcXZChange(float x, float z )
    {
        float p = TimeController.PlayerDelta;
        float f = Time.deltaTime;
        this.x = x  * f *p;
        this.z = z * f*p;
        xzChange = new Vector3(this.x,0,this.z);
    }
    void Move( float x, float z) 
    {
        //this.x = x * speed * Time.deltaTime;
        //this.z = z * speed * Time.deltaTime;
        CalcXZChange(x, z);
        Moved?.Invoke();
        moveTranslation = body.right * xzChange.x + body.forward * xzChange.z;// assign the x moveamount and the y move amount to the vector
       pController.Move(moveTranslation); // call the move function from pController, transalte by the vector, multiply by speed and Time.Deltatime for speed and smoothing.
    }
 

    
    public bool GroundCheck()
    {
        //    bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, gMask); //create an invisible sphere (physics.checksphere), at the position, with this radius, return true if layer is matching.
        bool isGrounded = pController.isGrounded;
        if (isGrounded == true && yVeloc.y < 0)
        {
            yVeloc.y = -2f;
            
            return true;
        }
        return false;
    }
    public void Fall ()
    {
        yVeloc.y = 0;
    }
    public void Jump()
    {
          yVeloc.y = Mathf.Sqrt(jumpHeight * -2f * grav) ;     // if space is pressed and you are grounded, set the y velocity to jump physics equation 
      //  yVeloc.y = 1;
        }
   
    //
     public void GravityApply()
    {
          yVeloc.y += grav * Time.deltaTime; // Vector3 velocity decrease amount;
       // yVeloc.y += -9.81f * Time.deltaTime;
        pController.Move(yVeloc*TimeController.Slow); // move by the velocity
    }

    protected override void CleanUp()
    {
    }


    //

    //







}
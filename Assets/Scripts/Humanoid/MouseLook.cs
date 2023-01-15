using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseLook :StateManagerComponent
{
    public float mouseSensitivity = 100f;

     Transform playerBody;
    Transform camera;
    UnityAction LookDele;
    float xRotation = 0f;
    float range = 10f;

    public MouseLook(MonoCalls.MonoAcessors manager, Transform playerBody, Transform camera) :base (manager)
    {
        this.playerBody = playerBody;
        this.camera = camera;
        manager.StartCall.Listen(Start);
        manager.UpdateCall.Listen(Update);
    }
    LayerMask ignoreMask = ~(1 << Constants.playerMask);
    public void PickUp(PInputManager player)
    {

        RaycastHit rayInfo;

        Physics.Raycast(camera.position, camera.forward, out rayInfo, range, ignoreMask);
        if (rayInfo.transform == null)   // needed so there is no reference error when you shoot at the sky
        {
            return;
        }


        Iinteractable interactHit = rayInfo.transform.GetComponent<Interactable>();
        if (interactHit != null)
        {
            interactHit.Interacted(player);
        }
    }

 

    // Start is called before the first frame update
     void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void Enable()
    {
        LookDele = Look;
    }
    public void Disable()
    {
        LookDele = null;
    }
    void Look ()
    {
        float d = Time.deltaTime;
        float p = TimeController.PlayerDelta;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * d*p;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * d*p;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Quaternion rotation = Quaternion.Euler(xRotation, 0f, 0f);
        camera.localRotation = rotation;
        playerBody.Rotate(Vector3.up * mouseX);
    }
      void Update()
    { if(LookDele != null)
        LookDele();
    }

    protected override void CleanUp()
    {
        
    }
}

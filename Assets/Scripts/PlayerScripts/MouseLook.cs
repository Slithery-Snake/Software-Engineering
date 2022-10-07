using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseLook : StateManagerComponent<PInputManager>
{
    public float mouseSensitivity = 100f;

     Transform playerBody;
    Transform camera;
    UnityAction LookDele;
    float xRotation = 0f;
    float range = 10f;

    public MouseLook(PInputManager manager, List<StateManagerComponent<PInputManager>> list, Transform playerBody, Transform camera) : base (manager, list)
    {
        this.playerBody = playerBody;
        this.camera = camera;
    }
 
    public void PickUp(PInputManager player)
    {

        RaycastHit rayInfo;

        Physics.Raycast(camera.position, camera.forward, out rayInfo, range);
        if (rayInfo.transform == null)   // needed so there is no reference error when you shoot at the sky
        {
            return;
        }


        Iinteractable interactHit = rayInfo.transform.GetComponent<Iinteractable>();
        if (interactHit != null)
        {
            interactHit.Interacted(player);
        }
    }
    public override void Awake()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void LateUpdate()
    {
    }

    public override void OnDisabled()
    {
    }

    public override void OnEnabled()
    {
    }

    // Start is called before the first frame update
    public override void Start()
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    public override void Update()
    { if(LookDele != null)
        LookDele();
    }
}

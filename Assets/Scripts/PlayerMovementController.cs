using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerMovementController : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public float speed = 1;
    public float gravityMultiplier = 1.0f;
    private CharacterController characterController;
    public Transform head;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromFloor = Vector3.Dot(head.localPosition, Vector3.up);
        characterController.height = Mathf.Max(characterController.radius, distanceFromFloor);
        characterController.center = transform.InverseTransformPoint(head.position.x, head.position.y/2, head.position.z);

        if(input.axis.magnitude >= 0.0f){
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
            characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 9.8f * gravityMultiplier, 0) * Time.deltaTime);
        }
    }
}

using UnityEngine;


public class PlayerInputManager : Singleton<PlayerInputManager> {

    private PlayerControls playerControls;
    
    // LOCOMOTION
    public Vector2 movementInput;
    public bool jumpInput; 
    public bool sprintInput;
    // CAMERA CONTROL
    public Vector2 rotationInput;
    public bool lockOnInput;
    // INTERACTION
    public bool interactionInput;


    
    // on scene changed, Check the scene is world scene
    // if scene is not a world scene, disable the input


    private void OnEnable() {

        if (playerControls == null) {
            
            playerControls = new PlayerControls();
            playerControls.Enable();
            
            #region Locomotion
            playerControls.Locomotion.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Locomotion.Jump.performed += i => jumpInput = true; // IF INPUT IS PERFORMED, SET BOOL TO TRUE
            
            playerControls.Locomotion.Sprint.performed += i => sprintInput = true;
            playerControls.Locomotion.Sprint.canceled += i => sprintInput = false;
            #endregion

            #region Camera Control
            playerControls.CameraControl.Rotation.performed += i => rotationInput = i.ReadValue<Vector2>();
            playerControls.CameraControl.LockOn.performed += i => lockOnInput = true; // IF INPUT IS PERFORMED, SET BOOL TO TRUE
            #endregion

            #region Interaction
            playerControls.Interaction.Interaction.performed += i => interactionInput = true;
            #endregion

        }

        

    }

    void Update() {
        
    }
}

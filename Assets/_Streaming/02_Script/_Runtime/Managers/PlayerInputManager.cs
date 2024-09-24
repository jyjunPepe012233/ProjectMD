using UnityEngine;


public class PlayerInputManager : Singleton<PlayerInputManager> {

    private PlayerControls playerControls;

    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector2 rotationInput;
    [SerializeField] private float jumpInput;
    [SerializeField] private bool sprintInput;

    public Vector2 MovementInput => movementInput;
    public Vector2 RotationInput => rotationInput;
    
    public float JumpInput => jumpInput;
    public bool SprintInput => sprintInput;


    
    // on scene changed, Check the scene is world scene
    // if scene is not a world scene, disable the input


    void OnEnable() {

        if (playerControls == null) {
            
            playerControls = new PlayerControls();
            playerControls.Enable();
            
            playerControls.Locomotion.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Locomotion.Jump.performed += i => jumpInput = i.ReadValue<float>();
            playerControls.CameraControl.Rotation.performed += i => rotationInput = i.ReadValue<Vector2>();
            
            playerControls.Locomotion.Sprint.performed += i => sprintInput = true;
            playerControls.Locomotion.Sprint.canceled += i => sprintInput = false;

        }

        

    }

    void Update() {
        
    }
}

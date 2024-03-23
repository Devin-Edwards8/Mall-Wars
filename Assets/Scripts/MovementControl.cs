using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem.EnhancedTouch;

public class MovementControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject controlStick;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject attackControl;
    [SerializeField] private GameObject leftRef;
    [SerializeField] private GameObject rightRef;
    private Vector3 movementDirection;
    private bool isTouching;
    private Rigidbody2D playerRb;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
        playerRb = player.GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
    void Update()
    {
        if(Touch.activeTouches.Count == 0) {
            ZeroControl();
            return;
        }

        if(!isTouching) {return;} // check if left side is touched

        SetTogglePosition();

        MovePlayer();
    }
    private void SetTogglePosition()
    {
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        worldPosition.z = 0f;

        // define movement direction
        movementDirection = worldPosition - transform.position;
        movementDirection.z = 0f;
        movementDirection.Normalize();

        // move controlStick only as far as the outer circle
        Vector3 maxPosition = transform.position + movementDirection;
        float maxDistance = Vector3.Distance(transform.position, maxPosition);
        float touchDistance = Vector3.Distance(transform.position, worldPosition);
        controlStick.transform.position = (touchDistance > maxDistance) ? maxPosition : worldPosition;
    }
    private void MovePlayer()
    {
        Vector2 direction2d = (Vector2)movementDirection;
        playerRb.position += speed * Time.deltaTime * direction2d;
        
        // update all object positions for camera follow
        Vector3 positionChange = speed * Time.deltaTime * movementDirection;
        transform.position += positionChange;
        attackControl.transform.position += positionChange;
        leftRef.transform.position += positionChange;
        rightRef.transform.position += positionChange;
    }
    private void ZeroControl()
    {
        controlStick.transform.position = transform.position;
        playerRb.velocity = Vector3.zero;
    }

    public void SetIsTouching(bool isT)
    {
        isTouching = isT;
    }
}

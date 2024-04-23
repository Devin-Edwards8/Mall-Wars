using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem.EnhancedTouch;
using Unity.VisualScripting;

public class MovementControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject controlStick;
    [SerializeField] private Player player;
    [SerializeField] private GameObject attackControl;
    [SerializeField] private GameObject leftRef;
    [SerializeField] private GameObject rightRef;
    private Vector3 movementDirection;
    private bool isTouching;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
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
        // update player values
        player.Position += speed * Time.deltaTime * movementDirection;
        float angle = Vector2.Angle(new(0, 1), movementDirection);
        angle = movementDirection.x > 0 ? -angle : angle;
        player.MovementDirection = angle;
        
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
    }

    public void SetMovement(bool isT)
    {
        isTouching = isT;
    }
}

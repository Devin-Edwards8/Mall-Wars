using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem.EnhancedTouch;

public class AttackControl : MonoBehaviour
{
    [SerializeField] private GameObject controlStick;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject aimLine;
    [SerializeField] private int projectileSpeed;
    [SerializeField] private Player player;
    private Vector3 aimDirection;
    private Camera mainCamera;
    private bool isTouching = false;
    private SpriteRenderer srAL;

    void Start()
    {
        mainCamera = Camera.main;
        srAL = aimLine.GetComponent<SpriteRenderer>();
        srAL.enabled = false;
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
        if(isTouching && Touch.activeTouches.Count == 0) {
            isTouching = false;
            Shoot();
            return;
        } else if(Touch.activeTouches.Count == 0) {
            ZeroControl();
            return;
        }

        if(!isTouching) {return;} // check that right side is touched

        SetTogglePosition();
    }

    private void SetTogglePosition()
    {
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        worldPosition.z = 0f;

        // define movement direction
        aimDirection = worldPosition - transform.position;
        aimDirection.z = 0f;
        aimDirection.Normalize();

        // set player values
        player.IsAiming = true;
        player.Rotation = GetRotation();

        SetAimLine();

        // move controlStick only as far as the outer circle
        Vector3 maxPosition = transform.position + aimDirection;
        float maxDistance = Vector3.Distance(transform.position, maxPosition);
        float touchDistance = Vector3.Distance(transform.position, worldPosition);
        controlStick.transform.position = (touchDistance > maxDistance) ? maxPosition : worldPosition;
    }

    private void SetAimLine() 
    {
        srAL.enabled = true;
        aimLine.transform.SetPositionAndRotation(
            player.Position + (3.4f * aimDirection), 
            Quaternion.Euler(0f,0f,GetRotation()));
    }

    private void Shoot()
    {
        player.IsAiming = false;

        srAL.enabled = false;
        ZeroControl();

        // launch projectile shot
        GameObject projectile = Instantiate(projectilePrefab, 
                    player.Position, Quaternion.Euler(0f, 0f, GetRotation()));

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = aimDirection * projectileSpeed;
        Destroy(projectile, 3.0f);
    }

    private float GetRotation()
    {
        Vector2 mv = aimDirection;
        Vector2 forward = new(0, 1);
        float angle = Vector2.Angle(forward, mv);
        if(mv.x > 0) 
        {
            angle *= -1;
        }
        return angle;
    }

    private void ZeroControl() 
    {
        controlStick.transform.position = transform.position;
    }

    public void SetAttack(bool isT)
    {
        isTouching = isT;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.InputSystem.EnhancedTouch;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEngine.XR;

public class AttackControl : MonoBehaviour
{
    [SerializeField] private GameObject controlStick;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectileFlashPrefab;
    [SerializeField] private GameObject aimLine;
    [SerializeField] private int projectileSpeed;
    [SerializeField] private GameObject player;
    private Vector3 movementDirection;
    private Camera mainCamera;
    private bool isTouching = false;
    private Rigidbody2D playerRb;

    private SpriteRenderer srAL;

    void Start()
    {
        mainCamera = Camera.main;
        playerRb = player.GetComponent<Rigidbody2D>();
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
        movementDirection = worldPosition - transform.position;
        movementDirection.z = 0f;
        movementDirection.Normalize();

        SetAimLine();

        // move controlStick only as far as the outer circle
        Vector3 maxPosition = transform.position + movementDirection;
        float maxDistance = Vector3.Distance(transform.position, maxPosition);
        float touchDistance = Vector3.Distance(transform.position, worldPosition);
        controlStick.transform.position = (touchDistance > maxDistance) ? maxPosition : worldPosition;
    }

    private void SetAimLine() {
        srAL.enabled = true;
        aimLine.transform.SetPositionAndRotation(
            player.transform.position + (3.4f * movementDirection), 
            Quaternion.Euler(0f,0f,GetRotation()));
    }

    private void Shoot()
    {
        srAL.enabled = false;
        ZeroControl();

        // projectile gun flash
        Vector2 mv = movementDirection;
        GameObject projectileFlash = Instantiate(projectileFlashPrefab, 
                    playerRb.position + mv, Quaternion.Euler(0f, 0f, GetRotation()));
        Destroy(projectileFlash, 0.1f);

        // launch projectile shot
        GameObject projectile = Instantiate(projectilePrefab, 
                    playerRb.position, Quaternion.Euler(0f, 0f, GetRotation()));

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = movementDirection * projectileSpeed;
        Destroy(projectile, 3.0f);
    }

    private float GetRotation()
    {
        Vector2 mv = movementDirection;
        Vector2 straight = new Vector2(1, 0);
        float angle = Vector2.Angle(straight, mv);
        if(mv.y < 0) 
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
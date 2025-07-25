using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("카메라 & 플레이어 설정")]
    public Transform playerCamera;
    public float mouseSensitivity = 100f;

    [Header("이동 설정")]
    public float speed = 5f;
    public float gravity = -9.81f;

    [Header("물리 상호작용 설정")]
    public float pushPower = 2.0f;
    
    [Header("헤드밥 연동")]
    public HeadBobController headBobController;

    // 내부 변수들
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        // 게임이 시작되면 커서를 잠그고 보이지 않게 만듭니다.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        // 항상 카메라를 회전시킵니다.
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        // 이동 처리
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        // 헤드밥
        if (controller.isGrounded)
        {
            headBobController.DoHeadBob(moveInput.x, moveInput.y);
        }

        // 중력
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // --- Input System 이벤트 함수들 ---
    public void OnMove(InputValue value) { moveInput = value.Get<Vector2>(); }
    public void OnLook(InputValue value) { lookInput = value.Get<Vector2>(); }

    // --- 물리 상호작용 함수 ---
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) { return; }
        if (hit.moveDirection.y < -0.3f) { return; }
        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.AddForce(pushDirection * pushPower, ForceMode.VelocityChange);
    }
}
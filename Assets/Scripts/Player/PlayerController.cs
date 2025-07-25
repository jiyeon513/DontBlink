using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))] // 이 스크립트는 CharacterController가 꼭 필요합니다.
public class PlayerController : MonoBehaviour
{
    [Header("카메라 & 플레이어 설정")]
    public Transform playerCamera;
    public float mouseSensitivity = 100f;

    [Header("이동 설정")]
    public float speed = 5f;
    public float gravity = -9.81f;

    [Header("물리 상호작용 설정")]
    public float pushPower = 2.0f; // 오브젝트를 밀어내는 힘의 세기

    // 내부 변수들
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private Vector3 velocity; // 중력에 의한 속도

    void Awake()
    {
        // CharacterController 컴포넌트를 가져옵니다.
        controller = GetComponent<CharacterController>();
        // 게임 시작 시 커서 숨기기
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- 카메라 회전 처리 ---
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // 상하 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 좌우 회전 (플레이어 전체를 회전)
        transform.Rotate(Vector3.up * mouseX);

        // --- 플레이어 이동 처리 ---
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        // --- 중력 처리 ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // --- Input System 이벤트 함수들 ---
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    // --- 물리 상호작용 함수 ---
    // 이 함수는 CharacterController가 다른 Collider와 부딪혔을 때 호출됩니다.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) { return; }
        if (hit.moveDirection.y < -0.3f) { return; }
        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.AddForce(pushDirection * pushPower, ForceMode.VelocityChange);
    }
}
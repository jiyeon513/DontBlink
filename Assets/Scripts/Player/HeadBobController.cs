using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [Header("헤드밥 설정")]
    [Tooltip("헤드밥 효과를 켤지 여부")]
    public bool enableHeadBob = true;
    [Tooltip("카메라가 흔들리는 속도")]
    [Range(0.0f, 20.0f)]
    public float bobSpeed = 14f;
    [Tooltip("카메라가 흔들리는 강도")]
    [Range(0.0f, 1.0f)]
    public float bobAmount = 0.05f;

    // 내부 변수
    private Vector3 startPos; // 카메라의 원래 위치
    private float timer = 0.0f;

    void Start()
    {
        // 스크립트 시작 시 카메라의 초기 위치를 저장합니다.
        startPos = transform.localPosition;
    }

    // 이 함수는 PlayerController에서 호출하여 헤드밥 효과를 실행합니다.
    public void DoHeadBob(float horizontal, float vertical)
    {
        if (!enableHeadBob) return;

        // 플레이어가 움직이는지 확인합니다 (제자리에서 뛰는 것 방지).
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            // 타이머를 시간에 따라 증가시킵니다.
            timer += Time.deltaTime * bobSpeed;

            // Mathf.Sin을 사용하여 부드러운 상하 움직임을 만듭니다.
            float newY = startPos.y + Mathf.Sin(timer) * bobAmount;
            
            // 카메라의 위치를 새로운 Y 값으로 업데이트합니다.
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            // 움직이지 않을 때는 타이머를 리셋합니다.
            timer = 0;
            // 카메라 위치를 원래 위치로 부드럽게 되돌립니다.
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * bobSpeed);
        }
    }
}

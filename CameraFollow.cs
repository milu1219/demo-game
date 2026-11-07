using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Mục tiêu mà camera sẽ theo dõi (kéo Capsule vào đây)
    public Transform target;

    // Khoảng cách vị trí tương đối giữa Camera và nhân vật
    public Vector3 offset = new Vector3(0f, 4f, -6f);

    // Tốc độ mượt mà khi camera bám đuôi theo nhân vật
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        // Đảm bảo không bị báo lỗi nếu chưa gán target trong Inspector
        if (target == null) return;

        // 1. Tính toán vị trí mong muốn của camera
        Vector3 desiredPosition = target.position + offset;

        // 2. Dùng Vector3.Lerp để di chuyển camera lướt theo một cách mượt mà
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // 3. Giữ cho camera luôn tập trung hướng về phía nhân vật
        transform.LookAt(target);
    }
}

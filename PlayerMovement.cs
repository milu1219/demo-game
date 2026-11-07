using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Cài đặt di chuyển")]
    // Tốc độ di chuyển của nhân vật
    public float moveSpeed = 5f;

    // Tốc độ xoay người theo hướng di chuyển
    public float rotationSpeed = 10f;

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        // 1. Đọc phím từ New Input System (mặc định của Unity 6)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;
        }

        // Fallback đọc Input cũ nếu có (bọc try-catch an toàn)
        if (horizontal == 0f && vertical == 0f)
        {
            try
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
            }
            catch { }
        }

        // Tạo vector di chuyển trên mặt phẳng XZ
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        // Xác định đối tượng cần di chuyển (Capsule hoặc chính GameObject này)
        Transform target = (transform.parent != null && transform.parent.name == "Capsule") ? transform.parent : transform;

        // 2. Di chuyển nhân vật
        target.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // 3. Tự động xoay người mượt mà theo hướng di chuyển
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

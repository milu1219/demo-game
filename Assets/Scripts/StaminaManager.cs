using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;
    public Slider staminaSlider;

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        // Mỗi giây trừ đi 2 điểm currentStamina
        currentStamina -= 2f * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // Đồng bộ giá trị với Slider UI nếu có
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    public void DrinkCoffee()
    {
        // Cộng thêm 30 điểm currentStamina
        currentStamina += 30f;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }
}

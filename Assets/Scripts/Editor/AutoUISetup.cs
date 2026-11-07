using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AutoUISetup
{
    [MenuItem("Tools/Tạo Thanh Thể Lực")]
    public static void CreateStaminaBar()
    {
        // 1. Tìm hoặc tạo Canvas nếu chưa có
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(canvasGO, "Tạo Canvas");
        }

        // Đảm bảo có EventSystem cho UI hoạt động
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            Undo.RegisterCreatedObjectUndo(eventSystemGO, "Tạo EventSystem");
        }

        // 2. Dùng DefaultControls.CreateSlider để tạo UI Slider chuẩn
        DefaultControls.Resources uiResources = new DefaultControls.Resources();
        GameObject sliderGO = DefaultControls.CreateSlider(uiResources);
        sliderGO.name = "StaminaSlider";
        sliderGO.transform.SetParent(canvas.transform, false);
        Undo.RegisterCreatedObjectUndo(sliderGO, "Tạo Thanh Thể Lực");

        // 3. Đưa Slider về tọa độ trung tâm màn hình (PosX = 0, PosY = 0)
        RectTransform rectTransform = sliderGO.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }

        // 4. Set Max Value = 100 và khởi tạo giá trị ban đầu là 100
        Slider slider = sliderGO.GetComponent<Slider>();
        if (slider != null)
        {
            slider.maxValue = 100f;
            slider.value = 100f;

            // 5. Đổi màu thanh Fill thành màu xanh lá cây
            Transform fillArea = sliderGO.transform.Find("Fill Area");
            if (fillArea != null)
            {
                Transform fill = fillArea.Find("Fill");
                if (fill != null)
                {
                    Image fillImage = fill.GetComponent<Image>();
                    if (fillImage != null)
                    {
                        fillImage.color = Color.green;
                    }
                }
            }
        }

        // 6. Tìm vật thể Capsule trên Scene có gắn StaminaManager
        GameObject capsule = GameObject.Find("Capsule");
        StaminaManager staminaManager = null;
        if (capsule != null)
        {
            staminaManager = capsule.GetComponent<StaminaManager>();
            if (staminaManager == null)
            {
                // Nếu Capsule chưa có StaminaManager thì tự động thêm component
                staminaManager = Undo.AddComponent<StaminaManager>(capsule);
            }
        }
        else
        {
            staminaManager = Object.FindFirstObjectByType<StaminaManager>();
        }

        // 7. Tự động gán Slider này vào biến staminaSlider của StaminaManager
        if (staminaManager != null && slider != null)
        {
            Undo.RecordObject(staminaManager, "Gán Stamina Slider");
            staminaManager.staminaSlider = slider;
            EditorUtility.SetDirty(staminaManager);
            Debug.Log("<color=green>[AutoUISetup] Đã tạo thanh Thể Lực và tự động liên kết thành công vào Capsule (StaminaManager)!</color>");
        }
        else
        {
            Debug.LogWarning("[AutoUISetup] Đã tạo Slider nhưng không tìm thấy Capsule trên Scene để gán component StaminaManager.");
        }

        // Focus lựa chọn vào Slider vừa tạo
        Selection.activeGameObject = sliderGO;
    }
}

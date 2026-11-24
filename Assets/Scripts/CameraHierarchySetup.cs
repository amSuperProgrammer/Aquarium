using UnityEngine;

/// <summary>
/// Создаёт иерархию камеры при старте сцены: Player → CameraPivot → Main Camera.
/// Назначает ссылки в PlayerController автоматически.
/// Просто положи этот скрипт на тот же GameObject, где PlayerController.
/// </summary>
[DefaultExecutionOrder(-100)]
public class CameraHierarchySetup : MonoBehaviour
{
    void Start()
    {
        // 1. Ищем или создаём CameraPivot
        var existing = transform.Find("CameraPivot");
        Transform pivotGO;
        if (existing != null)
            pivotGO = existing;
        else
        {
            pivotGO = new GameObject("CameraPivot").transform;
            pivotGO.SetParent(transform, false);
            pivotGO.localPosition = Vector3.zero;
            pivotGO.localRotation = Quaternion.identity;
        }

        // 2. Находим камеру и делаем её дочерней линзой
        var cam = GetComponentInChildren<Camera>(true);
        if (cam == null) return;

        cam.transform.SetParent(pivotGO, false);

        // 3. Назначаем ссылки в PlayerController
        var ctrl = GetComponent<PlayerController>();
        if (ctrl != null)
            ctrl.AssignCameraTransforms(pivotGO, cam.transform);

        Debug.Log("[CameraSetup] Hierarchy ready: " + gameObject.name +
                  " → CameraPivot → " + cam.gameObject.name);
    }
}

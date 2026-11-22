using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] Light[] lights;
    [SerializeField] Renderer lightRenderer;
    [SerializeField] Material lightMaterial;

    public string InteractionText => isOn ? "Выключить свет" : "Включить свет";

    Material materialInstance;
    Color originalEmissionColor;

    bool isOn = true;

    void Start()
    {
        if (lightRenderer != null && lightMaterial != null)
        {
            materialInstance = new Material(lightMaterial);
            originalEmissionColor = materialInstance.GetColor("_EmissionColor");
            lightRenderer.material = materialInstance;
        }
    }

    public void Interact()
    {
        isOn = !isOn;

        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.enabled = isOn;
            }
        }

        if (materialInstance != null)
        {
            if (isOn)
            {
                materialInstance.EnableKeyword("_EMISSION");
                materialInstance.SetColor("_EmissionColor", originalEmissionColor);
            }
            else
            {
                materialInstance.DisableKeyword("_EMISSION");
                materialInstance.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    void OnDestroy()
    {
        if (lightRenderer != null && lightMaterial != null)
        {
            lightRenderer.sharedMaterial = lightMaterial;
        }

        if (materialInstance != null)
        {
            Destroy(materialInstance);
            materialInstance = null;
        }
    }
}

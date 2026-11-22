using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gramophone : MonoBehaviour, IInteractable
{
    public string InteractionText => "Сменить трек";

    [Header("Music Settings")]
    [SerializeField] private AudioClip[] tracks;

    private AudioSource audioSource;
    private int currentTrackIndex = -1;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Настройки по умолчанию для граммофона
        audioSource.playOnAwake = false;
        audioSource.loop = false; // Мы сами меняем треки
    }

    public void Interact()
    {
        if (tracks == null || tracks.Length == 0)
        {
            Debug.LogWarning($"[Gramophone] На объекте {gameObject.name} не заданы треки!");
            return;
        }

        // Переключаем на следующий трек
        currentTrackIndex = (currentTrackIndex + 1) % tracks.Length;

        // Останавливаем текущий, если он играет, и запускаем новый
        audioSource.Stop();
        audioSource.clip = tracks[currentTrackIndex];
        audioSource.Play();

        Debug.Log($"[Gramophone] Сейчас играет: {tracks[currentTrackIndex].name} (Трек {currentTrackIndex + 1}/{tracks.Length})");
    }
}

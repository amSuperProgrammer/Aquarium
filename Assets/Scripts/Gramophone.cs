using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gramophone : MonoBehaviour, IInteractable
{
    public string InteractionText => "Сменить трек";

    [Header("Music Settings")]
    [SerializeField] private AudioClip[] tracks;

    private AudioSource audioSource;
    private int currentTrackIndex = -1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 配置音频源：不自动播放（由我们控制），循环关闭
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // 场景加载完成后立即开始播放第一首音乐
        if (tracks != null && tracks.Length > 0)
        {
            currentTrackIndex = 0;
            audioSource.clip = tracks[currentTrackIndex];
            audioSource.Play();
        }
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

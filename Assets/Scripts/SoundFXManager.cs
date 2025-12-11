using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject; // prefab with AudioSource component

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (audioClip == null || soundFXObject == null)
        {
            Debug.LogWarning("AudioClip or soundFXObject prefab is not assigned!");
            return;
        }

        // Instantiate the prefab at the given position
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign clip and volume
        audioSource.clip = audioClip;
        audioSource.volume = volume;

        // Play the sound
        audioSource.Play();

        // Destroy after clip finishes
        Destroy(audioSource.gameObject, audioClip.length);
    }
}

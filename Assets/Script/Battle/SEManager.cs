using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;  // シングルトン

    [Header("設定")]
    public int maxSimultaneousSE = 5;  // SEの最大同時再生数
    public AudioMixerGroup seMixerGroup;  // SE用のAudioMixerGroup

    private Queue<AudioSource> availableSE = new Queue<AudioSource>(); // 再利用可能なSE
    private List<AudioSource> activeSE = new List<AudioSource>(); // 現在使用中のSE

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも保持
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        for (int i = 0; i < maxSimultaneousSE; i++)
        {
            AudioSource newSource = new GameObject($"SE_{i}").AddComponent<AudioSource>();
            newSource.transform.parent = transform; // SEManagerの子オブジェクトにする
            newSource.outputAudioMixerGroup = seMixerGroup;
            newSource.playOnAwake = false;
            newSource.gameObject.SetActive(false);
            availableSE.Enqueue(newSource);
        }
    }

    public void PlaySE(AudioClip clip)
    {
        if (availableSE.Count > 0)
        {
            // 使用可能なAudioSourceを取り出す
            AudioSource source = availableSE.Dequeue();
            source.gameObject.SetActive(true);
            source.clip = clip;
            source.Play();
            activeSE.Add(source);

            // 再生が終わったらキューに戻す
            StartCoroutine(ReturnToPoolAfterPlaying(source));
        }
        else
        {
            Debug.LogWarning("SEの同時再生上限に達しました");
        }
    }

    private IEnumerator ReturnToPoolAfterPlaying(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        source.Stop();
        source.gameObject.SetActive(false);
        activeSE.Remove(source);
        availableSE.Enqueue(source); // キューに戻す
    }
}

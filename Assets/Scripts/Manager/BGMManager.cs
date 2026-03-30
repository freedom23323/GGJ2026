using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    public AudioSource audioSource;

    // 存每个clip播放到哪里
    private Dictionary<AudioClip, float> clipTimeDict = new Dictionary<AudioClip, float>();

    private AudioClip currentClip;

    public AudioMixer mixer;
    public Slider slider;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        slider.onValueChanged.AddListener(SetVolume);
    }

    public void PlayBGM(AudioClip newClip)
    {
        // 1️⃣ 记录当前播放进度
        if (currentClip != null)
        {
            clipTimeDict[currentClip] = audioSource.time;
        }

        // 2️⃣ 切换clip
        currentClip = newClip;
        audioSource.clip = newClip;

        // 3️⃣ 恢复进度（如果有）
        if (clipTimeDict.ContainsKey(newClip))
        {
            audioSource.time = clipTimeDict[newClip];
        }
        else
        {
            audioSource.time = 0f;
        }

        // 4️⃣ 播放
        audioSource.Play();
    }

    public void SetVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        //mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        Debug.Log("设置音量: " + Mathf.Log10(value) * 20);
        audioSource.volume = value;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
public class GameManager : MonoBehaviour
{
    public GameObject[] birds;
    public ParticleSystem fireParticle;
    public ParticleSystem shootParticle;
    
   
    public TextMeshProUGUI scroeText;
    public Slider musicSlider;
    public Toggle sfxToggle;

    private float highScore = 0;
    private float score = 0;
    [HideInInspector]
    public float bulletCount;

    public bool isGameOver;
    public bool isGamePaused = false;
   // public SoundManager soundManager;
    public static GameManager shared;
    private void Awake()
    {
        shared = this;
        isGameOver = false;
    }
    void Start()
    {
        StartCoroutine(SpawnEnemyWave());
        if(scroeText != null)  scroeText.text = "Score: 0, High Scroe: 0, Bullet: 0";
        if (musicSlider != null) musicSlider.value = PlayerPrefs.HasKey("music") ? PlayerPrefs.GetFloat("music"): SoundManager.shared.backgroundAudioSource.volume;
        if (sfxToggle != null) sfxToggle.isOn = PlayerPrefs.HasKey("sfx") ? Convert.ToBoolean(PlayerPrefs.GetInt("sfx")) : SoundManager.shared.otherAudioSource.enabled;
       
       
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.shared.isGamePaused)
        {
            SoundManager.shared.PlayClickSound();
        }
    }
    private IEnumerator SpawnEnemyWave()
    {
        //yield return new WaitForSeconds(1.0f);
        while (true)
        {
            GameObject bird = birds[UnityEngine.Random.Range(0, birds.Length)];
            float selectedHorizontalPoint = Camera.main.pixelWidth;
            float selectedVerticalPoint = UnityEngine.Random.Range(Camera.main.pixelHeight / 4, Camera.main.pixelHeight);
            Vector3 birdPosition = Camera.main.ScreenToWorldPoint(new Vector3(selectedHorizontalPoint, selectedVerticalPoint, 0.0f));
            Instantiate(bird, new Vector3(birdPosition.x, birdPosition.y, 0), Quaternion.identity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 5.0f));
           
            }
    }
    public void ScoreUp()
    {
        score++;
        if (score > highScore)
        {
            highScore++;
            PlayerPrefs.SetInt("highscore", (int)highScore);
        }
           
       
       
    }
    public void ScoreUpdate()
    {
        int hs = PlayerPrefs.HasKey("highscore") ? PlayerPrefs.GetInt("highscore") :0;
        scroeText.text = $"Score: {(int)score}, High Scroe: {hs}, Bullet: {(int)bulletCount}";
    }
    public void PlayFireParticle(Transform transform)
    {
       
        Instantiate(fireParticle, transform);
        //fireParticle.transform.rotation = Quaternion.Euler(90, 90, 90);
        fireParticle.Play();
        // SoundManager.shared.isPlay = true;
        SoundManager.shared.PlayFireSound(transform.position);
       // soundManager.PlayFireSound(transform.position);
    }

    public void PlayShootParticle(Vector3 position)
    {
        Instantiate(shootParticle, position, Quaternion.identity);
        shootParticle.Play();
         SoundManager.shared.PlayDuckDeathSound();
       // soundManager.PlayDuckDeathSound();
    }
   
    public void PausedGame()
    {
        Time.timeScale = 0.0f;
       
        isGamePaused = true;
        SoundManager.shared.backgroundAudioSource.Pause();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1.0f ;
       
        isGamePaused = false;
        SoundManager.shared.PlayBackgroundMusic();
    }
    public void RestartGame()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
      
    }
    public void QuitGame()
    {
        Application.Quit();
       
    }
    
   public void MusicVolumeController()
    {
        SoundManager.shared.backgroundAudioSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("music", musicSlider.value);

    }
    public void SFXController()
    {
        SoundManager.shared.otherAudioSource.enabled = sfxToggle.isOn;
        PlayerPrefs.SetInt("sfx", sfxToggle.isOn ? 1 : 0);
    }
    
}

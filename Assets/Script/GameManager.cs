using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
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

    public bool gameOver;
   // public SoundManager soundManager;
    public static GameManager shared;
    private void Awake()
    {
        shared = this;
        gameOver = false;
    }
    void Start()
    {
        StartCoroutine(SpawnEnemyWave());
        scroeText.text = "Score: 0, High Scroe: 0, Bullet: 0";

        musicSlider.value =  SoundManager.shared.backgroundAudioSource.volume;
        sfxToggle.isOn = SoundManager.shared.otherAudioSource.enabled;
    }
    private IEnumerator SpawnEnemyWave()
    {
        //yield return new WaitForSeconds(1.0f);
        while (true)
        {
            GameObject bird = birds[Random.Range(0, birds.Length)];
            float selectedHorizontalPoint = Camera.main.pixelWidth;
            float selectedVerticalPoint = Random.Range(Camera.main.pixelHeight / 4, Camera.main.pixelHeight);
            Vector3 birdPosition = Camera.main.ScreenToWorldPoint(new Vector3(selectedHorizontalPoint, selectedVerticalPoint, 0.0f));
            Instantiate(bird, new Vector3(birdPosition.x, birdPosition.y, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(0.5f, 5.0f));
           
            }
    }
    public void ScoreUp()
    {
        score++;
        if (score > highScore)
            highScore++;
        Debug.Log("Score " + score);
       
    }
    public void ScoreUpdate()
    {
        
        scroeText.text = "Score: " + score + ", High Scroe: " + highScore + ", Bullet: " + bulletCount;
    }
    public void PlayFireParticle(Transform transform)
    {
        Instantiate(fireParticle, transform);
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
    public void PushGame()
    {
        Time.timeScale = 0.0f;
        SoundManager.shared.PlayClickSound();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1.0f ;
        SoundManager.shared.PlayClickSound();
    }
    public void RestartGame()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
        SoundManager.shared.PlayClickSound();
    }
    public void QuitGame()
    {
        Application.Quit();
        SoundManager.shared.PlayClickSound();
    }
    
   public void MusicVolumeController()
    {
        SoundManager.shared.backgroundAudioSource.volume = musicSlider.value;
    }
    public void SFXController()
    {
        SoundManager.shared.otherAudioSource.enabled = sfxToggle.isOn;
    }
}

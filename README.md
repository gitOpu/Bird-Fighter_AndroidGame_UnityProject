![](doc/Cover.gif)

My first Indie Unity Android Game
**BirdFighter**
*Game Play: Simple Android game, start with welcome screen, Tapped on Start Game Button, game will to the next scene and started. Birds are playing from right to left with different spawn time and velocity, main player gun fire by gun, birds hit by bullet, its will die and point will be counted. A control preset will appear when push button will be pressed, here you can control music, sound or may game replay, restart or quit.*
![](doc/1.png)
![](doc/2.png)

**All Written Script**
### Assistant Class (Plug & Play)
*Boundary.cs*
```C#
public class Boundary : MonoBehaviour
{
    public enum BoundaryLocation
    {
        LEFT, TOP, RIGHT, BOTTOM
    };
    public BoundaryLocation direction;
    private BoxCollider2D boxCollider;

    public float boundaryWidth = 0.5f;
    public float overHang = 0.5f;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 0));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
        Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 lowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 0));

        if (direction == BoundaryLocation.TOP)
        {
            boxCollider.size = new Vector2(Mathf.Abs(topLeft.x) + Mathf.Abs(topRight.x) + overHang, boundaryWidth);
            boxCollider.offset = new Vector2(0, boundaryWidth / 2);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight, 1));
        }
        if (direction == BoundaryLocation.BOTTOM)
        {
            boxCollider.size = new Vector2(Mathf.Abs(lowerLeft.x) + Mathf.Abs(lowerRight.x) + overHang, boundaryWidth);
            boxCollider.offset = new Vector2(0, -boundaryWidth / 2);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, 1));
        }
        if (direction == BoundaryLocation.LEFT)
        {
            boxCollider.size = new Vector2(boundaryWidth, Mathf.Abs(lowerLeft.y) + Mathf.Abs(topLeft.y) + overHang);
            boxCollider.offset = new Vector2(-boundaryWidth / 2, 0);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, 1));
        }
        if (direction == BoundaryLocation.RIGHT)
        {
            boxCollider.size = new Vector2(boundaryWidth, Mathf.Abs(lowerRight.y) + Mathf.Abs(topRight.y) + overHang);
            boxCollider.offset = new Vector2(boundaryWidth / 2, 0);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, 1));
        }
    }

    // Draw Gizmos, To show gizmos, active it from game panel and select boundary object
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.up, Color.red);
        Debug.DrawRay(transform.position, Vector3.left, Color.red);
        Debug.DrawRay(transform.position, Vector3.right, Color.red);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
    }
}
```

*BirdController.cs*
```C#
public class BirdController : MonoBehaviour
{
   
    public float birdSpeedMin = 5.0f; 
    public float birdSpeedMax = 10.0f; 
    Vector3 offset;
   
    void Start()
    {
       
        offset = new Vector3(Vector3.left.x * Random.Range(birdSpeedMin, birdSpeedMax) * Time.deltaTime, 0, 0);
       
    }

  
    void Update()
    {
        
        transform.position += offset;
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
            GameManager.shared.PlayShootParticle(transform.position);
            GameManager.shared.ScoreUp();
        }
        //Debug.Log("Bird Controller: OnTriggerEnter2D");
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boundary") && other.gameObject.name != "Right Boundary")
        {
            Destroy(this.gameObject);
        }
        //Debug.Log("Bird Controller: OnTriggerExit2D");
    }

}

```


*BulletController.cs*
```C#
public class BulletController : MonoBehaviour
{
    private Rigidbody2D bulletRigidbody;
    [Range(1, 50)]
    public float bulletSpeed;
    void Start()
    {
        bulletRigidbody = gameObject.GetComponent<Rigidbody2D>();
        //Debug.Log("Bullet transform" + transform.rotation);
    }

    void Update()
    { 
        bulletRigidbody.velocity = transform.up * bulletSpeed;
        StartCoroutine(DestroyBullet(this.gameObject));
    }
    IEnumerator DestroyBullet(GameObject go)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(go);
    }
}
```

*ImageScroller.cs*
```C#
public class ImageScroller : MonoBehaviour
{

    private Renderer meshRenderer;
    public float scrollSpeed = 0.5f;
    //public float playerOffset;


    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "Scenary";
        meshRenderer.sortingOrder = -10;
    }
    void FixedUpdate()
    {
        float offset = Time.time * scrollSpeed;
        meshRenderer.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
```

### Main Class 
*GameManager.cs*
```C#
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
```

*PlayerController.cs*
```C#
public class PlayerController : MonoBehaviour
{
    
    public GameObject bullet;
    public GameObject[] playerPrefab;
    public int currentPlayerIndex = 0;
    private Transform turret;

    GameObject currentPlayer;
    private Vector3 userActionInPlace;
    private void Start()
    {
      InstantiatePlayer();
        Vector3 cameraPosition = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)); 
        gameObject.transform.position = new Vector3(-cameraPosition.x + 1.5f,  -cameraPosition.y,  0);
        


    }
    void Update()
    {


        //if (Input.GetButtonDown("Fire1") || Input.touchCount > 0)
        //{
        //    if (Input.touchCount > 0)
        //    {
        //        userActionInPlace = Camera.main.WorldToScreenPoint(Input.GetTouch(0).position);
        //    }
        //    else
        //    { 
        //        userActionInPlace = Camera.main.WorldToScreenPoint(Input.mousePosition);                
        //    }
        //    RotaedSprite(userActionInPlace); 
        //}
        //}

        if (Input.GetMouseButtonDown(0) && !GameManager.shared.isGamePaused)
        {
            userActionInPlace = Camera.main.WorldToScreenPoint(Input.mousePosition);
            RotaedSprite(userActionInPlace);
        }

        //if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        //{
        //    userActionInPlace = Camera.main.WorldToScreenPoint(Input.GetTouch(0).position);
        //    RotaedSprite(userActionInPlace);
        //}
    }

    void InstantiatePlayer()
    {
        if (currentPlayer != null) { Destroy(currentPlayer); }
        currentPlayer = Instantiate(playerPrefab[currentPlayerIndex], gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        currentPlayer.transform.parent = gameObject.transform;

        if (userActionInPlace.magnitude > 0)
        {
          RotaedSprite(userActionInPlace);
        }

    }
    void RotaedSprite(Vector3 userActionInPlace)
    {
        turret = currentPlayer.transform.Find("Turret");
        Vector3 turretPosition = Camera.main.WorldToScreenPoint(turret.position);
        Vector3 direction = userActionInPlace - turretPosition;
        float angle = (Mathf.Atan2(direction.y, direction.x) ) * Mathf.Rad2Deg;
         currentPlayer.transform.rotation = Quaternion.AngleAxis(angle % 15, Vector3.forward);
        turret.rotation = Quaternion.AngleAxis(angle - Mathf.PI / 2 * Mathf.Rad2Deg, Vector3.forward);
        int temPlayerIndex = SelectTurret(angle);
        if (temPlayerIndex != currentPlayerIndex)
        {
            currentPlayerIndex = temPlayerIndex;
            InstantiatePlayer();
        }
        else
        {
            Fire();
           
        }
       

    }
    
    

    void Fire(){
        if (bullet != null)
            Instantiate(bullet, turret.position, turret.rotation);
            GameManager.shared.PlayFireParticle(turret.transform);
         GameManager.shared.bulletCount++;
         GameManager.shared.ScoreUpdate();
       
    }
    int SelectTurret(float angle)
    {
        int temPlayerIndex = 0;
        if (angle >= 0 && angle < 15)
        {
            temPlayerIndex = 0;
        }
        else if (angle >= 15 && angle < 30)
        {
            temPlayerIndex = 1;
        }
        else if (angle >= 30 && angle < 45)
        {
            temPlayerIndex = 2;
        }
        else if (angle >= 45 && angle < 60)
        {
            temPlayerIndex = 3;
        }
        else if (angle >= 60 && angle < 75)
        {
            temPlayerIndex = 4;
        }
        else if (angle >= 75 && angle < 90)
        {
            temPlayerIndex = 5;
        }
        return temPlayerIndex;


    }
    //public void ClearLog() 
    //{
    //    var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
    //    var type = assembly.GetType("UnityEditor.LogEntries");
    //    var method = type.GetMethod("Clear");
    //    method.Invoke(new object(), null);
    //}
}

```
*SoundManager.cs*
```C#
public class SoundManager : MonoBehaviour
{

    
    public AudioSource otherAudioSource;
    public AudioSource backgroundAudioSource;

     public AudioClip backgrundMusicClip;
    public AudioClip duckDeathClip;
    public AudioClip fireClip;
    public AudioClip clickClip;
    GameManager gameManager;
    public bool isPlay;
    public static SoundManager shared;
    private void Awake()
    {
        shared = this;
        gameManager = GetComponent<GameManager>();
    }   

        void Start()
    {
        PlayBackgroundMusic();
    }
    public void PlayBackgroundMusic()
    {
        backgroundAudioSource.volume = PlayerPrefs.HasKey("music") ? PlayerPrefs.GetFloat("music") : 0.5f;
        // backgroundAudioSource.PlayOneShot(backgrundMusicClip);
        backgroundAudioSource.clip = backgrundMusicClip;
       
        if (GameManager.shared.isGamePaused) return;
        if (backgroundAudioSource.isPlaying) return;
        backgroundAudioSource.Play();

    }
    public void PlayDuckDeathSound()
    {
        otherAudioSource.volume = 0.8f;
      otherAudioSource.PlayOneShot(duckDeathClip);
       
    }
    public void PlayFireSound(Vector3 position)
    {
       
            otherAudioSource.volume = 0.5f;
       otherAudioSource.PlayOneShot(fireClip);
      // AudioSource.PlayClipAtPoint(fireClip, position);
       // otherAudioSource.clip = fireClip;
        //otherAudioSource.Play();
       
       
       
    }
    public void PlayClickSound()
    {
        otherAudioSource.volume = 0.9f; 
        otherAudioSource.PlayOneShot(clickClip);
    }
}
```

### Partial Class


*PanelController.cs*
```C#
public class PanelController : MonoBehaviour
{
    
    public bool isActive;
    void Start()
    {
        gameObject.SetActive(isActive);
    }

    
}
```


*StartGame.cs*
```C#
public class StartGame: MonoBehaviour
{
   
    public void StartGames()
    {
        SceneManager.LoadScene("Scene1");
        
    }
}
```
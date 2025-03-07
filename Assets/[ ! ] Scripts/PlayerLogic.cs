using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerLogic : MonoBehaviour
{
    [Header("---- Movement Data ----")]
    
        private Rigidbody2D PlayerRB;
        [SerializeField] private float MoveSpeed = 2f;
        [SerializeField] private float NormalPush = 10f;
        [SerializeField] private float ExtraPush = 14f;

        private Vector3 TempPosition;

    [Header("---- Button Gathering ----")]

        private GameObject LeftMovement_Button_GO;
        private GameObject RightMovement_Button_GO;
        private bool isMovingLeft = false;
        private bool isMovingRight = false;

    [Header("---- State Data ----")]

        private SpriteRenderer _spriteRenderer01;
        private SpriteRenderer _spriteRenderer02;
        private SpriteRenderer _spriteRenderer03;
        private SpriteRenderer _spriteRenderer04;

    private bool InitialPush = false;
        private int PushCount;

        private int ColorID;
        [SerializeField] private GameObject Color01;
        [SerializeField] private GameObject Color02;
        [SerializeField] private GameObject Color03;
        [SerializeField] private GameObject Color04;

    [Header("---- Animation ----")]

        [SerializeField] private Animator RocketEffect;

    [Header("---- Health Data ----")]

        [SerializeField] private int PlayerMaxHealth;    
        private int PlayerCurrentHealth;
        private bool IsAlive = true;
        private GameObject Health_GO;
        private TMP_Text Health_text;

        private float HitCoolDown = 0.5f;
        private float CurrentHitCooldown = 0f;
        [SerializeField] private AudioClip HitSound;
        [SerializeField] private GameObject DeathEffect;

    [Header("---- Coin Data ----")]

        private GameObject CoinIncreasedUI_GO;
        private TMP_Text CoinIncreasedUI;
        private int CoinGatheredNumber;

    [Header("---- Sound Effects ----")]

        private AudioSource _audioSource;
        [SerializeField] private AudioClip JumpSound;
        [SerializeField] private AudioClip CoinGatheringSound;
        [SerializeField] private AudioClip DeathSound;

    void Start()
    {
        ResourcesGathering();

        //-------------------- Random Color System -------------------- // 

        ColorID = Random.Range(0, 4);

        Color01.SetActive(ColorID == 0);
        Color02.SetActive(ColorID == 1);
        Color03.SetActive(ColorID == 2);
        Color04.SetActive(ColorID == 3);

        //-------------------- Player Local Resource -------------------- //
        
        IsAlive = true;
        PlayerCurrentHealth = PlayerMaxHealth;
        CoinGatheredNumber = 0;
        PlayerRB = this.gameObject.GetComponent<Rigidbody2D>();
        _audioSource = this.gameObject.GetComponent<AudioSource>();
        
        _spriteRenderer01 = Color01.gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer02 = Color02.gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer03 = Color03.gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer04 = Color04.gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (CurrentHitCooldown > 0)
        {
            CurrentHitCooldown -= Time.deltaTime;
        }

        if (PlayerCurrentHealth <= 0 && IsAlive)
        {
            StartCoroutine(WaitToRealDeath());
            Instantiate(DeathEffect, this.gameObject.transform.position, Quaternion.identity);
            IsAlive = false;
        }

        CoinIncreasedUI.text = $"X{CoinGatheredNumber}";
        Health_text.text = $"x{PlayerCurrentHealth}";
    }

    void FixedUpdate()
    {
        MoveFunction();
    }

    void MoveFunction()
    {
        if (!IsAlive)
            return;

        if (isMovingLeft || Input.GetAxisRaw("Horizontal") < 0)
        {
            PlayerRB.linearVelocity = new Vector2(-MoveSpeed, PlayerRB.linearVelocity.y);
        }
        if (isMovingRight || Input.GetAxisRaw("Horizontal") > 0)
        {
            PlayerRB.linearVelocity = new Vector2(MoveSpeed, PlayerRB.linearVelocity.y);
        }

        TempPosition = this.gameObject.transform.position;

        this.gameObject.transform.position = TempPosition;

        RocketEffect.SetFloat("Velocity", PlayerRB.linearVelocityY);

    }
    void OnTriggerEnter2D(Collider2D target)
    {

        if (!IsAlive)
            return;

        if (target.GetComponent<ItemChanging>() != null) 
        {
            ColorID = target.GetComponent<ItemChanging>().RandomAppearance;

            Color01.SetActive(ColorID == 0);
            Color02.SetActive(ColorID == 1);
            Color03.SetActive(ColorID == 2);
            Color04.SetActive(ColorID == 3);
        }

        if (target.tag == "NormalPush" || target.tag == "Enemy")
        {

            PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, NormalPush);
            Destroy(target.gameObject);
            PushCount++;

            if (target.CompareTag("NormalPush"))
            {
                CoinGatheredNumber += 1;
                GameDataManager.AddCoins(1);
                _audioSource.PlayOneShot(CoinGatheringSound);
            }

            if (target.CompareTag("Enemy") && PlayerCurrentHealth >= 1 && CurrentHitCooldown <= 0 && IsAlive)
            {
                PlayerCurrentHealth--;
                CurrentHitCooldown = HitCoolDown;
                StartCoroutine(LostHealthEffect());
            }

        }

        if (target.tag == "ExtraPush")
        {
            if (!InitialPush)
            {
                InitialPush = true;
                PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, 18f);
                target.gameObject.SetActive(false);
                return;
            }
            else
            {
                PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, ExtraPush);
                target.gameObject.SetActive(false);
                PushCount++;
            }
        }
        
        if (target.tag == "DieLine")
        {
            PlayerCurrentHealth -= PlayerCurrentHealth;
            CurrentHitCooldown = HitCoolDown;
            StartCoroutine(LostHealthEffect());
        }
        
        if (PushCount == 2)
        {
            PushCount = 0;
            PlatformSpawner.instance.SpawnPlatforms();
        }

    }

    IEnumerator LostHealthEffect()
    {
        _spriteRenderer01.color = new Color(_spriteRenderer01.color.r, _spriteRenderer01.color.g, _spriteRenderer01.color.b, 0.5f);
        _spriteRenderer02.color = new Color(_spriteRenderer02.color.r, _spriteRenderer02.color.g, _spriteRenderer02.color.b, 0.5f);
        _spriteRenderer03.color = new Color(_spriteRenderer03.color.r, _spriteRenderer03.color.g, _spriteRenderer03.color.b, 0.5f);
        _spriteRenderer04.color = new Color(_spriteRenderer04.color.r, _spriteRenderer04.color.g, _spriteRenderer04.color.b, 0.5f);

        RocketEffect.gameObject.GetComponent<SpriteRenderer>().color = new Color(RocketEffect.gameObject.GetComponent<SpriteRenderer>().color.r,
                                                                                 RocketEffect.gameObject.GetComponent<SpriteRenderer>().color.g,
                                                                                 RocketEffect.gameObject.GetComponent<SpriteRenderer>().color.b, 0.5f);
        _audioSource.PlayOneShot(HitSound);
        yield return new WaitForSeconds(CurrentHitCooldown);
        _spriteRenderer01.color = new Color(_spriteRenderer01.color.r, _spriteRenderer01.color.g, _spriteRenderer01.color.b, 1f);
        _spriteRenderer02.color = new Color(_spriteRenderer02.color.r, _spriteRenderer02.color.g, _spriteRenderer02.color.b, 1f);
        _spriteRenderer03.color = new Color(_spriteRenderer03.color.r, _spriteRenderer03.color.g, _spriteRenderer03.color.b, 1f);
        _spriteRenderer04.color = new Color(_spriteRenderer04.color.r, _spriteRenderer04.color.g, _spriteRenderer04.color.b, 1f);

        RocketEffect.gameObject.GetComponent<SpriteRenderer>().color = new Color(RocketEffect.gameObject.GetComponent<SpriteRenderer>().color.r,
                                                                         RocketEffect.gameObject.GetComponent<SpriteRenderer>().color.g,
                                                                         RocketEffect.gameObject.GetComponent<SpriteRenderer>().color.b, 1f);
    }


    IEnumerator WaitToRealDeath()
    {
        this.gameObject.transform.localScale = Vector3.zero;
        _audioSource.PlayOneShot(DeathSound);
        GameDataManager.NewestCoinNumbGathered(CoinGatheredNumber);
        yield return new WaitForSeconds(2f);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Result");
    }

    /*************************************************** Calculate & Resource Data ****************************************************************/

    void ResourcesGathering()
    {
        LeftMovement_Button_GO = GameObject.FindGameObjectWithTag("LeftMoveButton");

        if (LeftMovement_Button_GO != null)
        {
            EventTrigger trigger = LeftMovement_Button_GO.AddComponent<EventTrigger>();
            AddEventTrigger(trigger, EventTriggerType.PointerDown, (eventData) => isMovingLeft = true);
            AddEventTrigger(trigger, EventTriggerType.PointerUp, (eventData) => isMovingLeft = false);
        }

        RightMovement_Button_GO = GameObject.FindGameObjectWithTag("RightMoveButton");

        if (RightMovement_Button_GO != null)
        {
            EventTrigger trigger = RightMovement_Button_GO.AddComponent<EventTrigger>();
            AddEventTrigger(trigger, EventTriggerType.PointerDown, (eventData) => isMovingRight = true);
            AddEventTrigger(trigger, EventTriggerType.PointerUp, (eventData) => isMovingRight = false);
        }

        CoinIncreasedUI_GO = GameObject.FindGameObjectWithTag("Coin Increased UI");
        if (CoinIncreasedUI_GO != null)
        {
            CoinIncreasedUI = CoinIncreasedUI_GO.GetComponent<TMP_Text>();
        }

        Health_GO = GameObject.FindGameObjectWithTag("Player Health");
        if (Health_GO != null)
        {
            Health_text = Health_GO.GetComponent<TMP_Text>();
        }
    }

    void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
}

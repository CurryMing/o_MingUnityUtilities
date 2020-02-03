using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class o_Player : MonoBehaviour
{
    #region
    private Camera mainCamera;

    private Vector2 movement = Vector2.zero;

    private Animator playerAnim;

    private Rigidbody2D rb;
    public float speed;

    [Header("UpgradeSpeed")]
    public Text upSpeedStayTime;
    public Animator anim;
    public float upSpeed = 5f;
    public float upSpeedTime = 5f;
    public float spriteUpSpeedRate = 0.25f;
    private GameObject[] spritePoolUpSpeed = new GameObject[20];
    private float upCounter = 0;

    private Vector2 mouPos;

    private TrailRenderer tr;

    private SpriteRenderer playerSr;
    [Header("Dash")]
    public float spriteRate = 0.02f;
    public float dashTime = 0.1f;
    public float dashSpeed = 20f;
    public float dashRate = 2f;
    public Image coolImage;
    public Text coolText;
    private float dashRateCounter = 0f;
    private GameObject[] spritePool;
    private float couter = 0f;
    #endregion

    private void Awake()
    {
        playerAnim = GetComponent<Animator>();

        mainCamera = FindObjectOfType<Camera>();
        Debug.Log(mainCamera.gameObject.name);

        Transform SpritePool = GameObject.Find("SpritePool").transform;
        for (int i = 0; i < 20f; i++)
        {
            spritePoolUpSpeed[i] = GameObject.Find("SpritePoolUpSpeed").transform.GetChild(i).gameObject;
        }
        
        spritePool = new GameObject[SpritePool.childCount];
        for (int i = 0; i < spritePool.Length; i++)
        {
            spritePool[i] = SpritePool.GetChild(i).gameObject;
        }

        rb = GetComponent<Rigidbody2D>();
        playerSr = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        dashRateCounter -= Time.deltaTime;

        //UpdateSkillUI();

        if (Input.GetKeyDown(KeyCode.Space) && dashRateCounter <= 0f && movement != Vector2.zero)
        {
            mouPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.B) && dashRateCounter <= 0f)
        {
            StartCoroutine(UpGradeSpeed());
        }
        if (upCounter > upSpeedTime && tr.enabled == true)
        {
            tr.enabled = false;
        }
    }

    private void UpdateSkillUI()
    {
        coolImage.fillAmount = dashRateCounter / 2;
        coolText.text = dashRateCounter.ToString("f1");
        if (dashRateCounter <= 0)
            coolText.gameObject.SetActive(false);
        else
            coolText.gameObject.SetActive(true);


        upSpeedStayTime.text = upCounter.ToString("f1");
        if (upCounter > 0)
        {
            upSpeedStayTime.gameObject.SetActive(true);
            anim.SetBool("run",true);
        }
        else
        {
            upSpeedStayTime.gameObject.SetActive(false);
            anim.SetBool("run", false);
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        movement = movement.normalized;
        if (movement != Vector2.zero)
        {
            //playerAnim.SetFloat("H", movement.x);
            //playerAnim.SetFloat("V", movement.y);

            if (movement.x < 0)
            {
                playerSr.flipX = true;
            }
            else
            {
                playerSr.flipX = false;
            }
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        playerAnim.SetFloat("speed", movement.magnitude);
    }

    private IEnumerator Dash()
    {
        dashRate = 2;
        dashRateCounter = dashRate;
        spriteRate = 0.02f;
        StartCoroutine(DashSprites());
        couter = 0;

        while (couter <= dashTime)
        {
            couter += Time.deltaTime;
            rb.position = Vector2.MoveTowards(rb.position, mouPos, dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator DashSprites()
    {
        for (int i = 0; i < spritePool.Length; i++)
        {
            spritePool[i].SetActive(true);
            spritePool[i].GetComponent<SpriteRenderer>().color = Color.white;
            spritePool[i].GetComponent<SpriteRenderer>().flipX = playerSr.flipX;

            spritePool[i].transform.position = transform.position;

            yield return new WaitForSeconds(spriteRate);
        }
    }

    private IEnumerator UpGradeSpeed()
    {
        spriteRate = 0.25f;
        dashRate = 5;
        dashRateCounter = dashRate;

        StartCoroutine(UpSpeedSprites());
        upCounter = upSpeedTime;
        //tr.enabled = true;
        while (upCounter >= 0f)
        {
            upCounter -= Time.deltaTime;
            rb.position = Vector2.MoveTowards(rb.position, rb.position + movement, upSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator UpSpeedSprites()
    {
        for (int i = 0; i < spritePoolUpSpeed.Length; i++)
        {
            spritePoolUpSpeed[i].SetActive(true);
            spritePoolUpSpeed[i].GetComponent<SpriteRenderer>().color = Color.white;
            spritePoolUpSpeed[i].GetComponent<SpriteRenderer>().flipX = playerSr.flipX;

            spritePoolUpSpeed[i].transform.position = transform.position;

            yield return new WaitForSeconds(spriteRate);
        }
    }
}

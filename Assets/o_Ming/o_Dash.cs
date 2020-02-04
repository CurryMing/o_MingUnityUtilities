using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class o_Dash : MonoBehaviour
{
    #region
    private Camera mainCamera;

    private Vector2 movement = Vector2.zero;

    private Animator playerAnim;

    private Rigidbody2D rb;
    public float speed;

    [Header("UpgradeSpeed")]
    public Transform animSprite;
    public Text upSpeedStayTime;
    public Animator anim;
    public float upSpeed = 5f;
    public float upSpeedTime = 5f;
    public float spriteUpSpeedRate = 0.25f;
    private Vector3 offset;
    private bool isUpSpeeding = false;
    private GameObject[] spritePoolSpeedUp;
    private float upCounter = 0;

    private Vector2 mouPos = Vector2.zero;

    private SpriteRenderer playerSr;
    [Header("Dash")]
    public float spriteRate = 0.02f;
    public float dashTime = 0.1f;
    public float dashSpeed = 20f;
    public float dashRate = 2f;
    public Image coolImage;
    public Text coolText;
    private bool isDashing = false;
    private float dashRateCounter = 0f;
    private GameObject[] spritePoolDash;
    private float couter = 0f;
    #endregion

    private void Awake()
    {
        offset = transform.position - animSprite.position;

        playerAnim = GetComponent<Animator>();

        mainCamera = FindObjectOfType<Camera>();

        //
        Transform dashPool = GameObject.Find("SpritePoolDash").transform;
        spritePoolDash = new GameObject[dashPool.childCount];
        for (int i = 0; i < spritePoolDash.Length; i++)
        {
            spritePoolDash[i] = dashPool.GetChild(i).gameObject;
        }

        Transform speedUpPool = GameObject.Find("SpritePoolSpeedUp").transform;
        spritePoolSpeedUp = new GameObject[speedUpPool.childCount];
        for (int i = 0; i < spritePoolSpeedUp.Length; i++)
        {
            spritePoolSpeedUp[i] = speedUpPool.GetChild(i).gameObject;
        }

        rb = GetComponent<Rigidbody2D>();
        playerSr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        CameraFollow();

        Vector3 newPos = transform.position - offset;
        animSprite.position = Vector3.Lerp(animSprite.position, newPos, 5 * Time.deltaTime);

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (dashRateCounter > 0)
        {
            dashRateCounter -= Time.deltaTime;
        }
        
        UpdateSkillUI();

        if (Input.GetKeyDown(KeyCode.Space) && movement != Vector2.zero)
        {
            if (isDashing || isUpSpeeding || dashRateCounter > 0)
                return;
            //mouPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouPos = movement.normalized * 5;
            Debug.Log(mouPos);
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isDashing || isUpSpeeding)
                return;
            StartCoroutine(UpGradeSpeed());
        }
    }

    private void CameraFollow()
    {
        Vector3 offset = transform.position - mainCamera.transform.position;
        Vector3 newPos = mainCamera.transform.position + offset;
        newPos.z = -10;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newPos, 5 * Time.deltaTime);
    }

    private void UpdateSkillUI()
    {
        coolImage.fillAmount = dashRateCounter / dashRate;
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
        dashRateCounter = dashRate;
        spriteRate = 0.02f;
        StartCoroutine(DashSprites());
        couter = 0;
        isDashing = true;

        while (couter <= dashTime)
        {
            couter += Time.deltaTime;
            rb.position = Vector2.MoveTowards(rb.position, rb.position + mouPos, dashSpeed * Time.deltaTime);
            if (couter > dashTime)
            {
                isDashing = false;
            }
            yield return null;
        }
    }

    private IEnumerator DashSprites()
    {
        for (int i = 0; i < spritePoolDash.Length; i++)
        {
            spritePoolDash[i].SetActive(true);
            spritePoolDash[i].GetComponent<SpriteRenderer>().color = Color.white;
            spritePoolDash[i].GetComponent<SpriteRenderer>().flipX = playerSr.flipX;

            spritePoolDash[i].transform.position = transform.position;

            yield return new WaitForSeconds(spriteRate);
        }
    }

    private IEnumerator UpGradeSpeed()
    {
        spriteRate = 0.25f;
        isUpSpeeding = true;
        StartCoroutine(UpSpeedSprites());
        upCounter = upSpeedTime;
        while (upCounter >= 0f)
        {
            upCounter -= Time.deltaTime;
            rb.position = Vector2.MoveTowards(rb.position, rb.position + movement, upSpeed * Time.deltaTime);
            if (upCounter <= 0)
            {
                isUpSpeeding = false;
            }
            yield return null;
        }
    }
    private IEnumerator UpSpeedSprites()
    {
        for (int i = 0; i < spritePoolSpeedUp.Length; i++)
        {
            spritePoolSpeedUp[i].SetActive(true);
            spritePoolSpeedUp[i].GetComponent<SpriteRenderer>().color = Color.white;
            spritePoolSpeedUp[i].GetComponent<SpriteRenderer>().flipX = playerSr.flipX;

            spritePoolSpeedUp[i].transform.position = transform.position;

            yield return new WaitForSeconds(spriteRate);
        }
    }
}

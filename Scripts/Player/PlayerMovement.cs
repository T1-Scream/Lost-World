using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;
    public float gravity = -9f;
    public Transform ground;
    public float distanceToGround = 1f;
    public float jumpForce;
    public float fall;
    public LayerMask groundLayer;
    public AudioClip[] footStep;

    [Header("Health")]
    public int Health;
    public int maxHealth = 100;
    public Slider healthSlider;
    [HideInInspector] public Text healthText;
    public bool isDead
    { get { return Health <= 0; } }
    public GameObject deadUI;
    private bool deadTrigger;

    Animator characterAni;
    Rigidbody rb;
    Vector3 velocity;
    bool isGround;
    AudioSource audio;
    float crouchSpeed;
    GroundFootstep footStepCheck;

    internal bool isWalking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Health = maxHealth;
        healthText = healthSlider.transform.Find("Text").GetComponent<Text>();
        healthText.text = Health.ToString();
    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        isGround = false;
        isWalking = false;
        crouchSpeed = speed / 2;
    }

    void Update()
    {
        if (isDead) return;

        PlayerMove();
        WalkAnimation();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        GroundCheck();
    }

    public float GetHorizontal()
    {
        float x = Input.GetAxis("Horizontal");
        return x;
    }

    public float GetVertical()
    {
        float z = Input.GetAxis("Vertical");
        return z;
    }

    void PlayerMove()
    {
        float x = GetHorizontal();
        float z = GetVertical();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            isWalking = true;
        else
            isWalking = false;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void GroundCheck()
    {
        isGround = Physics.Raycast(ground.position, Vector3.down, 1f + 0.1f);

        if(isGround && velocity.y < 0)
            velocity.y -= 2f;

        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fall - 1) * Time.deltaTime;

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
            velocity = Vector3.up * jumpForce;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // crouch
        if(Input.GetKey(KeyCode.C)) {
            controller.height = 1.4f;
            speed = crouchSpeed;
        }
        else {
            controller.height = 2.0f;
            speed = crouchSpeed * 2;
        }
    }

    // walk animation && foot step sound
    void WalkAnimation()
    {
        float x = GetHorizontal();
        float z = GetVertical();

        Vector3 move = transform.right * x + transform.forward * z;
        if (move != Vector3.zero && isGround && characterAni != null) {
            characterAni.SetBool("Walking", true);
            // && isGround fixed animation not work
            if(!audio.isPlaying && isGround)
                audio.PlayOneShot(footStep[0]);
        }
        else {
            if (characterAni != null)
            characterAni.SetBool("Walking", false);

            audio.Stop();
        }
    }

    public void SetHealthBar(int damage)
    {
        healthSlider = GameObject.Find("Health bar").GetComponent<Slider>();
        healthText = healthSlider.transform.Find("Text").GetComponent<Text>();
        Health -= damage;
        healthSlider.value = Health;
        healthText.text = healthSlider.value.ToString();

        if (Health > 30 && Health < 70)
            healthSlider.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        else if (Health > 0 && Health < 30)
            healthSlider.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        else if (Health <= 0)
            Health = 0;
    }

    public void AddHP(int hp)
    {
        healthSlider = GameObject.Find("Health bar").GetComponent<Slider>();
        healthText = healthSlider.transform.Find("Text").GetComponent<Text>();
        Health += hp;
        healthSlider.value = Health;
        healthText.text = healthSlider.value.ToString();

        if (Health >= 70)
            healthSlider.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        else if (Health > 30 && Health < 70)
            healthSlider.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;

        if (Health >= 100)
            Health = 100;
    }

    public void GameOver()
    {
        if (isDead && !deadTrigger) {
            deadUI.SetActive(true);
            FindObjectOfType<GameManager>().UnLockMouse();
            deadTrigger = true;
        }
    }

    internal void setAnimator(Animator ani)
    {
        characterAni = ani;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;

    public float speed = 10f;
    public float jumpForce = 5f;
    public float dashForce = 15f;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private float movementX;
    private float movementY;
    private bool isGrounded;
    private float currentScale = 1.0f;
    private float baseSpeed;

    [Header("Effects")]
    public ParticleSystem pickupParticle;
    public AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip jumpSound;
    public AudioClip scaleSound;
    public AudioClip dashSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        baseSpeed = speed;
        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 v = movementValue.Get<Vector2>();
        movementX = v.x;
        movementY = v.y;
    }

    void Update()
    {
        float radius = transform.localScale.y / 2f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, radius + 1f);

        if (Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded) Jump();
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame) Dash();
            
            if (Keyboard.current.rKey.wasPressedThisFrame) ChangeScale(1.5f);
            if (Keyboard.current.fKey.wasPressedThisFrame) ChangeScale(0.5f);
            if (Keyboard.current.tKey.wasPressedThisFrame) ChangeScale(1.0f);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        if (Camera.main != null)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0;
            camRight.y = 0;
            
            movement = camForward.normalized * movementY + camRight.normalized * movementX;
        }

        rb.AddForce(movement * speed);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (audioSource && jumpSound) audioSource.PlayOneShot(jumpSound);
    }

    void Dash()
    {
        Vector3 dashDir = new Vector3(movementX, 0, movementY);
        
        if (Camera.main != null)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0; 
            camRight.y = 0;
            dashDir = camForward.normalized * movementY + camRight.normalized * movementX;
        }

        if (dashDir == Vector3.zero) 
            dashDir = Camera.main != null ? Camera.main.transform.forward : transform.forward;
            
        dashDir.y = 0;

        rb.AddForce(dashDir.normalized * dashForce, ForceMode.Impulse);
        if (audioSource && dashSound) audioSource.PlayOneShot(dashSound);
    }

    void ChangeScale(float targetScale)
    {
        currentScale = targetScale;
        transform.localScale = Vector3.one * currentScale;
        
        rb.mass = currentScale * currentScale;
        speed = baseSpeed / currentScale; 
        
        if (audioSource && scaleSound) audioSource.PlayOneShot(scaleSound);
        GetComponent<MeshRenderer>().material.color = (currentScale > 1.0f) ? Color.blue : (currentScale < 1.0f ? Color.green : Color.white);
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Killbox"))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = new Vector3(0, 2, 0);
            return;
        }

        if (other.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            if (pickupParticle) Instantiate(pickupParticle, other.transform.position, Quaternion.identity);
            if (audioSource && pickupSound) audioSource.PlayOneShot(pickupSound);
            count++;
            SetCountText();
        }
    }

    void SetCountText() 
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8) 
        {
            winTextObject.SetActive(true);
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy) Destroy(enemy);
            
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You win!";
            FindObjectOfType<GameManager>().ShowEndMenu(true, count);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false); 
            FindObjectOfType<GameManager>().ShowEndMenu(false, count);
        }
    }
}
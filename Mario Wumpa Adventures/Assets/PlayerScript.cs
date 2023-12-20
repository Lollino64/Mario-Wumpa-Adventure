using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
//This is the Mario Script
public class PlayerScript : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] TMP_Text WumpaText;
    bool isGameOver;
    int wumpacount = 0;
    bool Run;
    bool Jump;
    bool Death;
    bool AkuShield;
    [SerializeField] Animator anim;
    [SerializeField] AudioClip jumpSFX, spinSFX, itemSFX, akuSFX, crateSFX, deathSFX, tntSFX, noakuSFX, nitroSFX, lifeSFX;
    [SerializeField] AudioSource sound, music;
    bool Spin;
    bool isGrounded = true;
    Vector3 direction;
    [SerializeField] GameObject itemVFX, crateVFX, MarioAku, boomVFX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            sound.clip = jumpSFX;
            sound.Play();
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            anim.SetBool("Jump", true);
            anim.SetBool("Run", false);
            Jump = true;
        }
        if (direction.x != 0 || direction.z != 0)
        {
            anim.SetBool("Run", true);
        }
        if (direction.x == 0 && direction.z == 0)
        {
            anim.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Spin = true;
            anim.SetBool("Spin", true);
            sound.clip = spinSFX;
            sound.Play();
            Invoke("NoSpin", 0.75f);

        }
        if (direction != Vector3.zero)
        {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        if (wumpacount>99)
        {
            wumpacount = wumpacount-100;
            Wumpacounter("" + wumpacount);
            sound.clip = lifeSFX;
            sound.Play();
        }
    }
    void NoSpin()
    {
        anim.SetBool("Spin", false);
        Spin = false;
    }
    void FixedUpdate()
    {
        if (!isGameOver)
        {
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Floor":
                anim.SetBool("Jump", false);
                Jump = false;
                isGrounded = true;
                break;
            case "Water":
                death();
                break;
            case "FxTemporaire":
                if (AkuShield)
                    NoAku();
                else
                    death();
                break;
            case "TNT":
                sound.clip = tntSFX;
                sound.Play();
                break;
            case "Nitro":
                sound.clip = nitroSFX;
                sound.Play();
                GameObject vfx = Instantiate(boomVFX, transform.position, transform.rotation);
                Destroy(vfx, 1.3f);
                Destroy(other.gameObject);
                break;
            case "Level1Warp":
                SceneManager.LoadScene("LEVEL1", LoadSceneMode.Single);
                break;
        }
        if (other.gameObject.CompareTag("Crate") && (Spin || Jump))
        {
            Destroy(other.gameObject);
            GameObject vfx = Instantiate(crateVFX, other.transform.position, other.transform.rotation);
            sound.clip = crateSFX;
            sound.Play();
            wumpacount=wumpacount+ Random.Range(1, 5);
            Wumpacounter("" + wumpacount);
            Destroy(vfx, 3f);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Wumpa":
                GameObject vfx = Instantiate(itemVFX, other.transform.position, other.transform.rotation);
                sound.clip = itemSFX;
                sound.Play();
                wumpacount++;
                Wumpacounter("" + wumpacount);
                Destroy(vfx, 3f);
                break;
            case "Aku":
                AkuShield = true;
                MarioAku.SetActive(true);
                sound.clip = akuSFX;
                sound.Play();
                break;
        }
        Destroy(other.gameObject);
    }
    public void death()
    {
        anim.SetBool("Death", true);
        music.Stop();
        sound.clip = deathSFX;
        sound.Play();
        Invoke("restart", 4.5f);
        this.enabled = false;
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void NoAku()
    {
        sound.clip = noakuSFX;
        sound.Play();
        AkuShield = false;
        MarioAku.SetActive(false);
    }
    void Wumpacounter(string message)
    {
        // Aggiunta di un messaggio
        WumpaText.text = message;
    }
}
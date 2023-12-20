using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTLogic : MonoBehaviour
{
    [SerializeField] GameObject smokeVFX, boomVFX;
    [SerializeField] AudioClip tntSFX;
    [SerializeField] AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sound.clip = tntSFX;
            sound.Play();
            GameObject vfx = Instantiate(smokeVFX, transform.position, transform.rotation);
            Destroy(vfx, 3.3f);
            Destroy(gameObject, 3.6f);
            Invoke("boom", 3.5f);
        }
    }
    void boom()
    {
        GameObject vfx = Instantiate(boomVFX, transform.position, transform.rotation);
        Destroy(vfx, 1.3f);
    }
}
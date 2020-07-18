using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun1 : MonoBehaviour
{

    public GameObject bullet;
    public Transform leftHandIkPose;
    public Transform rightHandIkPose;
    public Transform lookAT;
    public float numOfBulletsPerSec=10;
    float nextSHootTime;
    bool fire;
    bool aiming;
    public List<GameObject> muzzleFlashes;
    public List<AudioClip> muzzleSounds;
    public GameObject player;
    Animator anim;
    AudioSource audio;

    private void Start()
    {
        anim = player.GetComponent<Animator>();
        audio =GetComponent<AudioSource>();
    }
    private void LateUpdate()
    {
        aiming =Input.GetButton(InputStatics.aim);
        fire = Input.GetButton(InputStatics.fire);
        if(aiming)
            handleFiering();
    }
    void MuzzleSound()
    {
        audio.clip =muzzleSounds[Random.Range(0,muzzleSounds.Count)];
        audio.Play();
    }
    void handleFiering()
    {
        if (fire && Time.time > nextSHootTime)
        {

            GameObject muzzle = Instantiate(muzzleFlashes[Random.Range(0, muzzleFlashes.Count)]);
            muzzle.transform.position = transform.position + transform.forward * 0.8f;
            muzzle.transform.rotation = transform.rotation * Quaternion.Euler(180,0,0);
            muzzle.transform.SetParent(transform);
            GameObject b = Instantiate(bullet);
            b.transform.position = transform.position + transform.forward * 0.8f;
            b.AddComponent<Rigidbody>();
            b.GetComponent<Rigidbody>().velocity = (lookAT.position - b.transform.position).normalized * 100;
            b.GetComponent<Rigidbody>().useGravity = false;
            Destroy(b, 3);
            Destroy(muzzle, (1 / numOfBulletsPerSec));

            nextSHootTime = Time.time + 1 / numOfBulletsPerSec;

            Ray ray = new Ray(b.transform.position, (lookAT.position - b.transform.position).normalized);
            Debug.DrawRay(ray.origin, ray.direction * 10);

            if (Input.GetButton(InputStatics.fire))
            {
                Transform spine = anim.GetBoneTransform(HumanBodyBones.Spine);
                spine.rotation = Quaternion.Lerp(spine.rotation, transform.rotation * Quaternion.Euler(Random.Range(-40, 0), Random.Range(-40, 40), Random.Range(-40, 40)), Time.deltaTime * 20);
                MuzzleSound();
                transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.Euler(Random.Range(-40, 0), Random.Range(-40, 40), Random.Range(-40, 40)), Time.deltaTime * 80);
            }

           
        }
    }
}

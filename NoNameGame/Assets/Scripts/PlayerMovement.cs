using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    private AudioSource audioSource;
    public AudioClip walking;
    public AudioClip running;

    public float walkSpeed;
    public float runSpeed;

    public float gravity = -9.81f;

    public float jumpHeight = 3f;
    private Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private bool isGrounded;

    public bool isRunning;
    private bool isWalking;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1;
    }

    public void Idle()
    {
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 move = transform.right * x + transform.forward * z;

        if (isRunning == false && move != Vector3.zero)
        {
            isWalking = true;
            controller.Move(move * walkSpeed * Time.deltaTime);

            //if (audioSource.isPlaying == false)
            //{
            //    audioSource.clip = walking;
            //    audioSource.Play();
            //}
        }
        else if (move == Vector3.zero || isRunning == true)
        {
            isWalking = false;

            //if (audioSource.isPlaying == true && isRunning == false)
            //{
            //    audioSource.Pause();
            //}
        }

        if (Input.GetKey(KeyCode.LeftShift) == true && move != Vector3.zero)
        {
            isRunning = true;
            controller.Move(move * runSpeed * Time.deltaTime);

            //if (audioSource.isPlaying == false || audioSource.clip != running)
            //{
            //    audioSource.clip = running;
            //    audioSource.Play();
            //}

        }
        else if (move == Vector3.zero || isWalking == false)
        {
            isRunning = false;
            //if (audioSource.isPlaying == true && isWalking == false)
            //{
            //    audioSource.Pause();
            //}
        }

        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //}

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

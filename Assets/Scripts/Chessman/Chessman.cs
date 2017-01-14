using UnityEngine;
using System.Collections;
using System;

public abstract class Chessman : MonoBehaviour {
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public bool isWhite;

    public abstract string Annotation();

    private Quaternion originRotation;
    private float speed = 1f;
    private float incSpeed = 0.1f;

    private Animator anim;

    private bool startMove = false;


    private AudioSource _audioSource = null;
    private AudioSource AudioSource
    {
        get
        {
            if (_audioSource == null)
            {
                this.gameObject.AddComponent<AudioSource>();
                return this.gameObject.GetComponent<AudioSource>();
            }
            return _audioSource;
        }
    }
    private static AudioClip _manDeadAudio;
    private static AudioClip ManDeadAudio
    {
        get {
               if (_manDeadAudio == null)
            {
                _manDeadAudio = Resources.Load<AudioClip>("Sound/Game/dead_man");
            }
            return _manDeadAudio;
        }
    }

    private static AudioClip _moveSurfAudio;
    private static AudioClip MoveSurfAudio
    {
        get
        {
            if (_moveSurfAudio == null)
            {
                _moveSurfAudio = Resources.Load<AudioClip>("Sound/Game/move_surf");
            }
            return _moveSurfAudio;
        }
    }

    private void Start()
    {
        originRotation = transform.rotation;

        Animator[] anims = GetComponentsInChildren<Animator>();

        anim = anims[0];

        if (anims.Length >= 2)
            anim = anims[anims.Length - 1];

        if (anim == null)
        {
            Debug.Log("Chessman's animator is null");
        }
    }

    private void Update()
    {
        if (startMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);

            speed += incSpeed;

            if (transform.position == newPosition)
            {
                newPosition = Vector3.zero;
                startMove = false;
                speed = 1f;
            }
                
        }
    }

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[8, 8];
    }

    public virtual bool[,] PossibleEat()
    {
        return new bool[8, 8];
    }

    public bool CanGo(int x, int y)
    {
        bool[,] possible = this.PossibleMove();

        return possible[x, y];
    }

    internal void RotateEach(float seconds)
    {
        anim.SetTrigger("KillChessman");
        InvokeRepeating("Rotate", 0f, seconds);
    }

    private int t = 5;
    private int dt = 5;
    private void Rotate()
    {
        transform.Rotate(Vector3.up * t);
        t += dt;
    }

    internal void DestroyAfter(float seconds)
    {
        AudioSource.PlayOneShot(ManDeadAudio);
        Invoke("DestroyGameObject", seconds);
    }

    private void DestroyGameObject()
    {
        BoardManager.Instance.GetAllChessmans().Remove(gameObject);
        Destroy(gameObject);
    }

    private Vector3 newPosition = Vector3.zero;
    internal void MoveAfter(float seconds, Vector3 position)
    {
        newPosition = position;
        Invoke("Move", seconds);
    }

    private void Move()
    {
        startMove = true;
        AudioSource.PlayOneShot(MoveSurfAudio);
        //transform.position = newPosition;

        // stop rotation
        // CancelInvoke("Rotate");

        // restore origin rotation
        //transform.rotation = originRotation;

        // restore rotate speed
        //t = 10;
    }
}
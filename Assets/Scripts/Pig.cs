using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pig : MonoBehaviour
{
    #region Inspector

    [SerializeField] private Joystick joystick;

    [SerializeField] private float speed = 2.0f;

    [SerializeField] private float bombOffset = 2.0f;

    [SerializeField] private float waitBombTime = 2.0f;

    [SerializeField] private List<Sprite> moveSprites;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject bombPrefab;

    [SerializeField] private Transform[] corners = new Transform[4];

    [SerializeField] private HealthBar hpBar;

    [SerializeField] public UnityEvent onDamaged;

    #endregion

    #region Fields

    private Vector3 movement;

    private MoveState moveState;

    private Animator anim;

    private int HP = 100;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        anim = GetComponent<Animator>();
        onDamaged.AddListener(() => StartCoroutine(WaitDamageAnim()));
        hpBar.SetStartHealth(100);
    }

    void Start()
    {
        moveState = MoveState.Right;
    }

    void Update()
    {
        float inputX = joystick.Horizontal;
        float inputZ = joystick.Vertical;

        movement = new Vector3(inputX, 0, inputZ);
        if (movement != Vector3.zero)
        {
            if (Mathf.Abs(inputX) >= Mathf.Abs(inputZ))
            {
                moveState = inputX >= 0 ? MoveState.Right : MoveState.Left;
                /*if (rb.velocity.x >= 0)
                {
                    moveState = MoveState.Right;
                }
                else
                {
                    moveState = MoveState.Left;
                }*/
            }
            else
            {
                moveState = inputZ >= 0 ? MoveState.Up : MoveState.Down;
                /*if (rb.velocity.z >= 0)
                {
                    moveState = MoveState.Up;
                }
                else
                {
                    moveState = MoveState.Down;
                }*/
            }
        }

        spriteRenderer.sprite = moveSprites[(int) moveState];
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Farmer"))
        {
            onDamaged.Invoke();
            rb.AddForce(collision.rigidbody.velocity *1000);
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator WaitDamageAnim()
    {
        anim.SetBool("IsDamaged", true);
        yield return new WaitForSeconds(waitBombTime);
        anim.SetBool("IsDamaged", false);
    }

    #endregion


    public void GetDamage(int damage)
    {
        var hp = HP - damage;
        hpBar.SetHealth(hp);
        if (hp > 0)
        {
            HP = hp;
        }
        else
        {
            //Destoy or Invoke?
        }
    }

    public void PlantBomb()
    {
        Vector3 bombPos = transform.position;
        switch (moveState)
        {
            case MoveState.Right:
                bombPos.x += bombOffset;
                break;
            case MoveState.Left:
                bombPos.x -= bombOffset;
                break;
            case MoveState.Up:
                bombPos.z += bombOffset;
                break;
            case MoveState.Down:
                bombPos.z -= bombOffset;
                break;
        }

        if (!(bombPos.x < corners[0].position.x) && !(bombPos.x > corners[1].position.x) &&
            !(bombPos.z > corners[0].position.z) && !(bombPos.z < corners[3].position.z))
        {
            Instantiate(bombPrefab, bombPos, Quaternion.Euler(new Vector3(45, 0, 0)));
        }
    }
}
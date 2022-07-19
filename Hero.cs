using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Hero : MonoBehaviour
{
    [Header("Меняемые перемены")]
    public float speed; // тут не надо коментарий
    public float jumpForce; // сила прыжка
    public float normalSpeed; //тут тоже не надо коментариев
    public float checkRadius; //проверка растояние feetPos от земли
    public int damage; //урон атаки
    public float attackRange; //радиус атаки
    public LayerMask whatIsGround; //это чтобы определить слой земли
    public LayerMask enemy; // а это чтобы определить слой где находиться враг
    public Transform feetPos; // это ссылка на обьект который в ноге героя

    [Header("Определенные перемены")]
    public bool facingRight = true; //это перемена для того, чтобы поварачивать нашего героя по направлению
    private bool isGrounded; //проерка на то, что герой на земле
    public Transform attackPos; //тут тоже начинается перемены для атаки
    private Rigidbody2D rb;//rigidbody от героя 
    private Animator anim; //ну это логично, это для того чтобы реализовать все наши анимаций героя
    public float health;
    bool m_isDead;

    private void Start()
    {
        speed = 0f;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position,checkRadius, whatIsGround);

        if (health <= 0)
        {
            if (!m_isDead)
            {
                anim.SetTrigger("Death");
                transform.tag = "Untagged";
                m_isDead = !m_isDead;
            }
        }

        if (!m_isDead)
        {
            if (isGrounded)
            {
                anim.SetBool("IsJump", false);
            }
            else
            {
                anim.SetBool("IsJump", true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!m_isDead)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    public void OnJumpButtonDown()
    {
        if (isGrounded == true)
        {
            anim.SetBool("IsJump", true);
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    public void OnLeftButtonDown()
    {
       // UIAnimators[1].SetTrigger("LeftButton");
        if (speed >= 0f)
        {
            speed = -normalSpeed;
        }
        rb.GetComponent<SpriteRenderer>().flipX = true; 
        anim.SetBool("IsRunning", true); 
    }
    public void OnRightButtonDown()
    {
        if (speed <= 0f)
        {
            speed = normalSpeed;
        }
        rb.GetComponent<SpriteRenderer>().flipX = false;
        anim.SetBool("IsRunning", true);
    }

    public void OnAttackButtonDown()
    {
        anim.SetBool("Attack", true);
    }

    public void OnAttack()
    {
        Collider2D[] enemyes = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
        if (enemyes.Length!=0)
        {
            enemyes[0].GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public void OnButtonDown()
    {
        speed = 0f;
        anim.SetBool("IsRunning", false);       
        anim.SetBool("Attack", false);
    }
}

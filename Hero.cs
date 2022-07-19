using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Hero : MonoBehaviour
{
    [Header("�������� ��������")]
    public float speed; // ��� �� ���� ����������
    public float jumpForce; // ���� ������
    public float normalSpeed; //��� ���� �� ���� �����������
    public float checkRadius; //�������� ��������� feetPos �� �����
    public int damage; //���� �����
    public float attackRange; //������ �����
    public LayerMask whatIsGround; //��� ����� ���������� ���� �����
    public LayerMask enemy; // � ��� ����� ���������� ���� ��� ���������� ����
    public Transform feetPos; // ��� ������ �� ������ ������� � ���� �����

    [Header("������������ ��������")]
    public bool facingRight = true; //��� �������� ��� ����, ����� ������������ ������ ����� �� �����������
    private bool isGrounded; //������� �� ��, ��� ����� �� �����
    public Transform attackPos; //��� ���� ���������� �������� ��� �����
    private Rigidbody2D rb;//rigidbody �� ����� 
    private Animator anim; //�� ��� �������, ��� ��� ���� ����� ����������� ��� ���� �������� �����
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

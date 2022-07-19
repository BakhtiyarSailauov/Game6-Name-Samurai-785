using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    [Header("Меняемые перемены: ")]//эти перемены специально каждому врагу индивидуальные, что бы сложности реагировать
    [SerializeField] float      m_speed;
    [SerializeField] float      m_jumpForce;
    public int health;
    public int positionOfPatrol; 
    public float stoppingDistance;
    public Hero player;
    public float startTimeBtwAttack;
    public float damage;
    public float startsStopTime;
    public float normalSpeed;
    public Image imageBlackGround;

    [Header("Определенные перемены: ")]
    float timeBtwAttack;
    private float stopTime;
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_isDead = false;
    bool moveingRight;
    Vector2 pointForPatrul;
    bool chill = false;
    bool angry = false;
    bool goBack = false;

    // Use this for initialization
    void Start () { 
        normalSpeed = m_speed;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        pointForPatrul = GetComponent<Transform>().position;
    }
	
	// Update is called once per frame
	void Update () {
        if (stopTime <= 0)
        {
            m_speed = normalSpeed;
        }
        else
        {
            m_speed = 0;
            stopTime -= Time.deltaTime;
        }
        if (health <= 0 && !m_isDead)
        {
            if (!m_isDead)
            { m_animator.SetBool("Deathh", true);
                transform.tag = "Untagged";
                m_animator.SetInteger("AnimState", 0);
            }
            /*else
            { m_animator.SetTrigger("Recover"); } вернемся в будущем*/
            m_isDead = !m_isDead;
        }

        if (Vector2.Distance(transform.position, pointForPatrul) < positionOfPatrol && angry == false && !m_isDead && player.health > 0)
        {
            chill = true;
        }

        if (Vector2.Distance(transform.position, player.transform.position) < stoppingDistance && !m_isDead && player.health > 0)
        {
            angry = true;
            chill = false;
            goBack = false;
        }

        if (Vector2.Distance(transform.position, player.transform.position) > stoppingDistance && !m_isDead || player.health <= 0)
        {
            goBack = true;
            angry = false;
        }

        if (chill == true && !m_isDead)
        {
            Chill();
        }
        else if (angry == true && !m_isDead)
        {
            Angry();
        }
        else if (goBack == true && !m_isDead)
        {
            Goback();
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

               // Move
              //  m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

               //Set AirSpeed in animator
              //m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
             // -- Handle Animations --
            //Attack
           // m_animator.SetTrigger("Attack");
          //Change between idle and combat idle
         // if (Input.GetKeyDown("f"))
        //    m_combatIdle = !m_combatIdle;

            //Jump потом надо будет
            /* else if (Input.GetKeyDown("space") && m_grounded) {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
            }*/

          //Combat Idle
         // else if (m_combatIdle)
        //     m_animator.SetInteger("AnimState", 1);
       //Idle
      //  else
     //m_animator.SetInteger("AnimState", 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !m_isDead)
        {
            if (timeBtwAttack <= 0)
            {
                m_animator.SetBool("Attack", true);
                timeBtwAttack = normalSpeed;
            }
            else
            {
                timeBtwAttack = timeBtwAttack -Time.deltaTime;
            }
        }
    }


    public void OnEnemyAttack()
    {       
        m_animator.SetBool("Attack", false);
        player.health = player.health - damage;
        if (damage > 1)
        {
        imageBlackGround.color = new Color(imageBlackGround.color.r, imageBlackGround.color.g, imageBlackGround.color.b, imageBlackGround.color.a + 0.08f);
        }
        if (damage <= 1)
        {
            imageBlackGround.color = new Color(imageBlackGround.color.r, imageBlackGround.color.g, imageBlackGround.color.b, imageBlackGround.color.a + 0.02f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!m_isDead)
        {
            m_animator.SetTrigger("Hurt");
            health -= damage;
        }
    }

    void Chill()
    {
        m_animator.SetInteger("AnimState", 2);
        if (transform.position.x > pointForPatrul.x + positionOfPatrol)
        {
            moveingRight = false;
        }
        else if (transform.position.x < pointForPatrul.x - positionOfPatrol)
        {
            moveingRight = true;
        }

        if (moveingRight)
        {
            transform.position = new Vector2(transform.position.x + m_speed * Time.deltaTime, transform.position.y);
            m_body2d.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        { 
            transform.position = new Vector2(transform.position.x - m_speed * Time.deltaTime, transform.position.y);
            m_body2d.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void Angry()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, m_speed * Time.deltaTime);
        m_animator.SetInteger("AnimState", 2);
    }

    void Goback()
    {
        transform.position = Vector2.MoveTowards(transform.position, pointForPatrul, m_speed * Time.deltaTime);
        m_body2d.GetComponent<SpriteRenderer>().flipX = true;
        m_animator.SetInteger("AnimState", 2);
    }
}

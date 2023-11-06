using System.Collections;
using UnityEngine;
using System;


public class PlayerMove : MonoBehaviour
{
    [Header("Referencias")]
    private Rigidbody2D rb2D;
    private Animator animator;

    RaycastHit2D hit;
    public Vector3 v3;
    public LayerMask layer;
    public float distancia;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [Range(0, 0.3f)][SerializeField] private float MoveSuavizacion;
    private float InputX;
    private float horizontalMove = 0f;
    private Vector3 moveVelocity = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]

    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector3 boxDimensions;
    [SerializeField] private bool enSuelo;
    private bool salto = false;

    [Header("Salto Regulable")]
    [Range(0, 2)][SerializeField] private float multiplicadorCancelarSalto;
    [SerializeField] private float multiplicadorGravedad;
    private float gravedad;

    private bool botonSalto = true;

    [Header("Salto Pared")]
    [SerializeField] private Transform paredCheck;
    [SerializeField] private Vector3 boxDimensionsPared;
    private bool enPared;
    private bool dezlizarPared;
    [SerializeField] private float dezlizamientoVelocidad;
    [SerializeField] private float jumpForceParedX;
    [SerializeField] private float jumpForceParedY;
    [SerializeField] private float jumpTimePared;
    private bool saltandoPared;

    [Header("Sistema de particulas")]
    [SerializeField] private ParticleSystem particulas;

    [Header("Reset")]
    public static float xInicial;
    public static float yInicial;

    public bool alive;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravedad = rb2D.gravityScale;
        alive = true;
    }

    private void Update()
    {
        DetectorMovile();
        InputX = Input.GetAxisRaw("Horizontal");
        horizontalMove = InputX * moveSpeed;

        UpdateAnimations();

        if (Input.GetButton("Jump") && botonSalto)
        {
            salto = true;
        }
        if (!enSuelo && enPared && InputX != 0)
        {
            dezlizarPared = true;
        }
        else
        {
            dezlizarPared = false;
        }

        if (Input.GetButtonUp("Jump"))
        {
            // boton up
            BotonSalto();
        }
    }
    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(groundCheck.position, boxDimensions, 0f, WhatIsGround);
        animator.SetBool("enSuelo", enSuelo);

        enPared = Physics2D.OverlapBox(paredCheck.position, boxDimensionsPared, 0f, WhatIsGround);

        Mover(horizontalMove * Time.fixedDeltaTime, salto);

        salto = false;

        if (dezlizarPared)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -dezlizamientoVelocidad, float.MaxValue));
        }
        Debug.Log(rb2D.velocity.y);

        if (rb2D.velocity.y <= 4)
        {
            saltandoPared = false;
        }
        else
        {
            saltandoPared = true;
        }
    }
    private void Mover(float mover, bool saltar)
    {
        if (!saltandoPared)
        {
            Vector3 targetVelocity = new Vector2(mover * 10f, rb2D.velocity.y);
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref moveVelocity, MoveSuavizacion);
        }

        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        if (enSuelo && saltar && botonSalto && !dezlizarPared)
        {
            Saltar();
        }
        if (enPared && saltar && botonSalto && dezlizarPared)
        {
            // Salto pared
            SaltoPared();
            // mirandoDerecha = !mirandoDerecha;
        }
        if (rb2D.velocity.y < 0 && !enSuelo)
        {
            rb2D.gravityScale = multiplicadorCancelarSalto;
        }
        else
        {
            rb2D.gravityScale = gravedad;
        }
    }
    private void SaltoPared()
    {
        enPared = false;
        rb2D.velocity = new Vector2(jumpForceParedX * -InputX, jumpForceParedY);
        //espere

    }


    private void Saltar()
    {
        rb2D.AddForce(new Vector2(0f, jumpForce));
        enSuelo = false;
        salto = false;
        botonSalto = false;
    }
    private void BotonSalto()
    {
        if (rb2D.velocity.y > 0)
        {
            rb2D.AddForce(Vector2.down * rb2D.velocity.y * (1 - multiplicadorCancelarSalto), ForceMode2D.Impulse);
        }

        botonSalto = true;
        salto = false;
    }
    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
    bool CheckCollision
    {
        get
        {
            hit = Physics2D.Raycast(transform.position + v3, transform.up * -1, distancia, layer);
            return hit.collider != null;
        }
    }
    public void DetectorMovile()
    {
        if (CheckCollision)
        {
            transform.parent = hit.collider.transform;
        }
        else
        {
            transform.parent = null;
        }
    }
    public void TomarDaño()
    {
        if (alive == false)
        {
            Destroy(gameObject);
        }
    }
    private void UpdateAnimations()
    {
        animator.SetFloat("Horizontal", Mathf.Abs(horizontalMove));
        animator.SetFloat("VelocidadY", rb2D.velocity.y);
        animator.SetBool("dezlizarPared", dezlizarPared);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, boxDimensions);
        Gizmos.DrawWireCube(paredCheck.position, boxDimensionsPared);


        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + v3, transform.position + v3 + transform.up * -1 * distancia);
        //        Gizmos.DrawRay(transform.position + v3, Vector3.up * -1 * distancia);
    }
}

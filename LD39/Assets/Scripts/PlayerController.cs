using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    Norm,
    Dash
}

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float walkLerp = 0.25f;
    public float dashSpeed = 30f;
    public float dashDur = 0.2f;
    public float dashLerp = 0.05f;
    public float dashDrag = 0.75f;

    private float power;

    private float dashTimer = 0;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private ParticleSystem ps;
    private Vector2 atkDir;
    private Vector2 moveInput;
    public State state;
    private float atkDur = 0.3f;
    private float atkTimer = 0;
    public bool isSlice = false;
    private bool isDead = false;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
        state = State.Norm;
        atkDir = new Vector2(1, 0);
    }

    public void Kill()
    {
        if (!isDead)
        {
            isDead = true;
            ps.Emit(1);
            spr.color = new Color(0, 0, 0, 0);
            rb.simulated = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!isDead)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            moveInput = moveInput.normalized;

            if (moveInput != Vector2.zero)
            {
                atkDir = moveInput;
            }

            DecTimers();

            switch (state)
            {
                case State.Norm:
                    spr.color = Color.white;
                    Move();
                    break;
                case State.Dash:
                    Dashing();
                    break;
            }

            if (rb.velocity.magnitude > 0)
            {
                spr.flipX = rb.velocity.x < 0;
            }

            Timers.DecTimer(ref atkTimer, new Timers.TimerCallback(EndAtk));
            isSlice = atkTimer > 0;

            if (isSlice)
            {
                spr.color = Color.cyan;
            }
        }
        else if (Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
        }
	}

    private void EndAtk()
    {

    }

    private void Move()
    {
        if (moveInput == Vector2.zero && rb.velocity.magnitude < walkLerp)
        {
            rb.velocity = Vector2.zero;
        }

        if (rb.velocity.magnitude > walkSpeed)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, moveInput * walkSpeed, dashLerp);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, moveInput * walkSpeed, walkLerp);
        }

        if (Input.GetButtonDown("Attack"))
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        state = State.Dash;
        rb.velocity = atkDir * dashSpeed;
        dashTimer = dashDur;
        Game.Instance.Power -= 5;
        atkTimer = atkDur;
    }

    private void Dashing()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, dashLerp);
    }

    private void EndDash()
    {
        state = State.Norm;
    }

    private void DecTimers()
    {
        Timers.DecTimer(ref dashTimer, new Timers.TimerCallback(EndDash));
    }

    private void OnGUI()
    {
        if (isDead)
        {
            float midW = Screen.width / 2;
            float midH = Screen.height / 2;
            GUIStyle style = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter
            };
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(midW - 128, midH - 32, 256, 32), "GAME OVER", style);
            GUI.Label(new Rect(midW - 128, midH + 32, 256, 32), "Press <Enter> to Restart", style);
        }
    }
}

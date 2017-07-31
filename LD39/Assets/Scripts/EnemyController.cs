using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int level = 1;

    public GameObject spawnParticlePrefab;
    public GameObject triggerPrefab;
    private GameObject trigger;
    private GameObject nearCell;
    private GameObject player;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private ParticleSystem ps;
    private Vector2 dir;

    public float speed = 8/3;
    public float drainRate = 0.2f;
    public float drainDur = 5;
    private float drainTimer = 0;
    private bool tryDrain = true;
    private bool atCell = false;

    private float spawnDur = 0.8f;
    public float spawnTimer = 0;

    // Use this for initialization
    void Start ()
    {
        Instantiate(spawnParticlePrefab, transform.position, transform.rotation);

        trigger = Instantiate(triggerPrefab, transform.position, transform.rotation);
        trigger.GetComponent<EnemyTrigger>().SetEnemyParent(this.gameObject);
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
        player = GameObject.Find("Player");
        FindNearestCell();
        level = 1;
        float r = Random.value;
        if (r < 0.15)
        {
            level = 3;
            tryDrain = false;
        }
        else if (r < 0.35)
        {
            level = 2;
            if (Random.value > 0.30)
            {
                tryDrain = false;
            }
        }

        spawnTimer = spawnDur;
        switch (level)
        {
            case 1:
                spr.color = new Color(1, 1, 1, 0.5f);
                break;
            case 2:
                spr.color = new Color(0.1f, 0.1f, 1f, 0.5f);
                break;
            case 3:
                spr.color = new Color(1, 0.2f, 0, 0.5f);
                break;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        Timers.DecTimer(ref spawnTimer, new Timers.TimerCallback(EndSpawn));

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        trigger.transform.position = this.transform.position;

        dir = Vector2.zero;

        Vector2 towardsCell = nearCell.transform.position - transform.position;
        float d = towardsCell.magnitude;
        if (tryDrain)
        {
            if (d < 3 && !atCell)
            {
                StartDrainPower();
            }
            if (atCell)
            {
                Timers.DecTimer(ref drainTimer, new Timers.TimerCallback(EndDrainPower));
                Game.Instance.Power -= drainRate * Time.deltaTime;
            }
            dir = (nearCell.transform.position - this.transform.position).normalized;
        }
        else
        {
            // move towards and shoot at player
            if (Game.Instance.Power > 0)
            {
                dir = (player.transform.position - this.transform.position).normalized;
            }
        }

        rb.velocity = Vector2.Lerp(rb.velocity, dir * speed * level, 0.25f);
        spr.flipX = rb.velocity.x > 0;

        if (spawnTimer == 0)
        {
            switch (level)
            {
                case 1:
                    spr.color = new Color(1, 1, 1, 1);
                    break;
                case 2:
                    spr.color = new Color(0.1f, 0.1f, 1, 1);
                    break;
                case 3:
                    spr.color = new Color(1, 0.2f, 0, 1);
                    break;
            }
        }
    }

    private void EndSpawn()
    {

    }

    private void StartDrainPower()
    {
        drainTimer = drainDur * level;
        atCell = true;
        ps.Play();
    }

    private void EndDrainPower()
    {
        ps.Stop();
        level += 1;
        float r = Random.value;
        atCell = false;
        if (level == 2 && r <= 0.30)
        {
            tryDrain = true;
        }
        else
        {
            tryDrain = false;
        }
    }

    private void FindNearestCell()
    {
        GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");

        int ni = 0;
        float nd = 99999999;
        for (int i = 0; i < cells.Length; i++)
        {
            GameObject cell = cells[i];
            Vector2 towardsCell = cell.transform.position - transform.position;
            float d = towardsCell.magnitude;
            if (d < nd)
            {
                ni = i;
                nd = d;
            }
        }

        nearCell = cells[ni];
    }

}

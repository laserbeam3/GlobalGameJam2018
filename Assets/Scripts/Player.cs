using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InclinedPlanePosition))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public Color damageColor = Color.red;
    public InclinedPlanePosition inclined { get; private set; }
    public SpriteRenderer renderer { get; private set; }
    public Animator anim { get; private set; }
    public float fwdSpeed = 4.0f;
    public float sideSpeed = 6.0f;

    public float f;
    public float s;

    public Vector2 pos {
        get { return inclined.pos; }
        set { inclined.pos = value; }
    }

    public int maxHP = 3;
    private int _health;
    public int health {
        get { return _health; }
        set { _health = Mathf.Clamp(value, 0, maxHP); }
    }

    void Awake()
    {
        inclined = GetComponent<InclinedPlanePosition>();
        renderer = GetComponent<SpriteRenderer>();
        anim     = GetComponent<Animator>();
        inclined.limitForward = true;
    }

    void Start()
    {
        pos = pos;
    }

    void Update()
    {
        if (!ClimbGame.instance.allowPlayerMovement) return;

        float delta = Time.deltaTime;

        float fwdInput  = Input.GetAxisRaw("Horizontal") * fwdSpeed  * delta;
        float sideInput = Input.GetAxisRaw("Vertical")   * sideSpeed * delta;

        pos += new Vector2(fwdInput, sideInput);
    }

    public void TakeDamage()
    {
        // health--;
        iTween.StopByName("damageAnim");
        renderer.color = Color.white;
        iTween.ValueTo(gameObject, iTween.Hash("name", "damageAnim",
                                               "from", Color.white,
                                               "to", damageColor,
                                               "onupdate", "SetRenderColor",
                                               "easeType", "easeInOutExpo",
                                               "loopType", "pingPong",
                                               "time", 0.1f));
        AppManager.CallWithDelay(() => {
            iTween.StopByName("damageAnim");
            renderer.color = Color.white;
        }, 0.4f);

        if (health <= 0) {
            anim.SetTrigger("death");
            AppManager.instance.climbGame.LoseGame();
        }
    }

    public void SetRenderColor(Color c)
    {
        renderer.color = c;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    #region Singleton
    private static Game _instance;
    public static Game Instance { get { return _instance; } }
    void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }
    #endregion

    public const float MAX_POWER = 100;
    private float power;

    public GameObject enemyPrefab;

    public float Power
    {
        get
        {
            return power;
        }
        set
        {
            power = value;
            if (power > 100)
            {
                power = 100;
            }
            else if (power < 0)
            {
                GameObject.Find("Player").GetComponent<PlayerController>().Kill();
                power = 0;
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        power = MAX_POWER;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Random.value < 0.03)
        {
            float y = Random.value * 17 - 8.5f;
            float x = Random.value * 27 - 13.5f;
            if (y >= -1 && y <= 2 && (x > 6 || x < -6))
            {
                x = Random.value * 12 - 6;
            }
            Instantiate(enemyPrefab, new Vector3(x, y, y), transform.rotation);
        }
	}

    private void OnGUI()
    {
        float midW = Screen.width / 2;
        float midH = Screen.width / 2;
        Rect powerBack = new Rect(midW - 128, 2, 256, 32);
        Rect powerBar = new Rect(midW - 126, 4, 252 * (power / 100), 28);

        GUIDrawRect(powerBack, Color.black);
        GUIDrawRect(powerBar, Color.green);
    }

    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    // Note that this function is only meant to be called from OnGUI() functions.
    public static void GUIDrawRect(Rect position, Color color)
    {
        if (_staticRectTexture == null)
        {
            _staticRectTexture = new Texture2D(1, 1);
        }

        if (_staticRectStyle == null)
        {
            _staticRectStyle = new GUIStyle();
        }

        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();

        _staticRectStyle.normal.background = _staticRectTexture;

        GUI.Box(position, GUIContent.none, _staticRectStyle);
    }

}

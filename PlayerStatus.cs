using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{

    [Header("PlayerStatus Status")]
    public float health = 100f;
    public float mana = 100f;
    public float stamina = 100f;
    public bool stamina_on = false;

    [Header("PlayerStatus UI")]
    public Text health_ui;
    public Text mana_ui;
    public Text stamina_ui;

    void Start()
    {
        health_ui.text = health.ToString();
        mana_ui.text = mana.ToString();
        stamina_ui.text = stamina.ToString();
    }

    void Update()
    {
        health_ui.text = health.ToString();
        mana_ui.text = mana.ToString();
        stamina_ui.text = stamina.ToString();
        check_playerstatus();
    }

    void check_playerstatus()
    {
        if (stamina <= 100 && stamina >= -10)
        {
            stamina += Time.deltaTime * 5f;
        }
        if (stamina <= 20)
        {
            GameObject.Find("Player").GetComponent<PlayerMove>().is_rolling_lock = true;
        }
        if (stamina >= 20f)
        {
            GameObject.Find("Player").GetComponent<PlayerMove>().is_rolling_lock = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifetime : MonoBehaviour
{
    public float m_lifeTime = 2.0f;
    public Transform m_follow;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < m_lifeTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
            timer = 0.0f;
        }

        if(m_follow != null)
            transform.position = m_follow.position;
    }
}

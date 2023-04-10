using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFan : MonoBehaviour
{
    public float windForce = 150;
    ParticleSystem ps;
    private Transform _player;
    private Rigidbody2D _plyRb;
    private PlayerController _plyCon;

    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    public List<ParticleCollisionEvent> collisionEvents;



    // layermask
    public LayerMask ground;
    void OnEnable()
    {

        ps = GetComponent<ParticleSystem>();
        ps.trigger.AddCollider(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
        collisionEvents = new List<ParticleCollisionEvent>();

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _plyRb = _player.GetComponent<Rigidbody2D>();
        _plyCon = _player.GetComponent<PlayerController>();
    }

    void OnParticleTrigger()
    {
        // get the particles which matched the trigger conditions this frame

        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        // iterate through the particles which exited the trigger and make them green

        if(numExit > 0 && (
            (PlayerData.Pd.state == State.Liquid && _plyCon.isAttached) ||
            (PlayerData.Pd.state == State.Gas)
            ))
        {
            _plyRb.AddForce(transform.right * windForce);
        }
        // re-assign the modified particles back into the particle system

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }


}

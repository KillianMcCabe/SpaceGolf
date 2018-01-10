using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBallParticles : MonoBehaviour {

	ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		particleSystem = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		AddForceOnParticles();
	}

	private void AddForceOnParticles(){

		ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[particleSystem.particleCount];
		particleSystem.GetParticles(_particles);

		for (int i = 0; i < _particles.Length; i++){
			// Here you can do whatever you want with each particle by using _particles[i] 
			_particles[i].velocity += _particles[i].velocity.normalized*0.125f;
		}

		particleSystem.SetParticles(_particles, _particles.Length);
	}
}

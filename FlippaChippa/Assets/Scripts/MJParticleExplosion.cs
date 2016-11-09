using System;
using UnityEngine;
 
public class MJParticleExplosion : MonoBehaviour {

	ParticleSystem particles;
	public int maxParticles = 200;
	public int particlesPerChip = 15;

	// Use this for initialization
	void Start () {		
		if (particles == null) {
			particles = gameObject.GetComponentInChildren<ParticleSystem> ();
		}
	}

	void Update() {
		if (particles.isStopped) {
			gameObject.SetActive (false);
		}
	}

	public void Explode(Vector3 position, Color initialColor, Color endColor)
	{
		if (particles == null) {
			particles = gameObject.GetComponentInChildren<ParticleSystem> ();
		}

		Gradient gradient = new Gradient ();
		GradientColorKey[] colorKeys = new GradientColorKey[] {
			new GradientColorKey (initialColor, 0.0f),
			new GradientColorKey (endColor, 1.0f)
		};

		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey (1.0f, 0.0f), 
			new GradientAlphaKey (0.0f, 1.0f)
		};
		gradient.SetKeys (colorKeys, alphaKeys);

		ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particles.colorOverLifetime;	//will not compile if using ps.colorOverLifetime.color = someColor directly
		colorOverLifetime.enabled = true;
		colorOverLifetime.color = gradient;

		ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetime = particles.limitVelocityOverLifetime;
		limitVelocityOverLifetime.dampen = 0.1f;

		transform.position = position;
		particles.Play ();
		particles.Emit (UnityEngine.Random.Range(particlesPerChip, maxParticles));
	}
}



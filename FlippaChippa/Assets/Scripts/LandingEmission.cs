
using UnityEngine;
using System.Collections;

public class LandingEmission : MonoBehaviour, FCEventListener {

	ParticleSystem ps;
	LandingListener controller;
	public int maxParticles = 200;
	public int particlesPerChip = 15;
	float maxSpeeed;
	int maxChips = 13;


	// Use this for initialization
	void Start () {		
		ps = gameObject.GetComponentInChildren<ParticleSystem> ();
		controller = GameObject.FindGameObjectWithTag (Tags.GAME_CONTROLLER).GetComponent<LandingListener>();
		controller.AddLandingListener (this, FCEvent.END);
	}

	#region FCEventListener implementation

	public void OnEvent (FCEvent fcEvent, GameObject gameObject)
	{
		if (fcEvent == FCEvent.END) {
			Transform lowest = GetLowestChildTransform (gameObject.transform);
			Material material = lowest.GetComponent<Renderer> ().material;
			Color materialColor = material.color;
			Texture2D albedoTexture = (Texture2D)material.mainTexture;
			if (albedoTexture != null) {
				materialColor = albedoTexture.GetPixel (20, 20);
			}

			Gradient gradient = new Gradient ();
			GradientColorKey[] colorKeys = new GradientColorKey[] {
				new GradientColorKey (materialColor, 0.0f),
				new GradientColorKey (materialColor, 1.0f)
			};

			GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] {
				new GradientAlphaKey (1.0f, 0.0f), 
				new GradientAlphaKey (0.0f, 1.0f)
			};
			gradient.SetKeys (colorKeys, alphaKeys);

			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = ps.colorOverLifetime;	//will not compile if using ps.colorOverLifetime.color = someColor directly
			colorOverLifetime.enabled = true;
			colorOverLifetime.color = gradient;

			ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetime = ps.limitVelocityOverLifetime;
			limitVelocityOverLifetime.dampen = 1 - 1f * gameObject.transform.childCount / maxChips;

			ps.transform.position = new Vector3(lowest.position.x, lowest.position.y - 0.1f, lowest.position.z);
			ps.Play ();
			ps.Emit (Mathf.Min(gameObject.transform.childCount * particlesPerChip, maxParticles));
		}
	}

	private Transform GetLowestChildTransform(Transform t) {
		Transform lowest = t.GetChild(0);
		for (int i = 0; i < t.childCount; i++) {
			if (t.GetChild(i).transform.position.y < lowest.position.y) {
				lowest = t.GetChild (i);
			}
		}
		return lowest;
	}

	#endregion
}

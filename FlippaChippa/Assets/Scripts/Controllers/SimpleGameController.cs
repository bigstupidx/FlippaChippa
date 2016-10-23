using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

namespace AssemblyCSharp
{
	public class SimpleGameController : MonoBehaviour, FCEventListener, LandingListener
	{
		private Stack targetStack;
		private List<Stack> stacks;

		private int indexOfVisibleStack = 0;

		public GameInputController gameInputController;
		public LandingEmission landingEmission;

		private StackGenerator stackGenerator;
		private PrefabsManager prefabsManager;

		void Awake() {
			stackGenerator = new StackGenerator ();
			prefabsManager = GameObject.FindGameObjectWithTag (Tags.PREFABS_MANAGER).GetComponent<PrefabsManager> ();

			stackGenerator.SetPrefabsManager (prefabsManager);
			int[] chipIds = new int[]{0,1,5,3,4,5};
			bool[] initFlips = new bool[]{ false, true, false, false, true };
			int[] targetFlips = new int[]{1,0,0};
			GameStacks gamestacks = stackGenerator.GenerateStacks (chipIds, initFlips, targetFlips);
			targetStack = gamestacks.Target;
			targetStack.AddListener (this);
			targetStack.gameObject.SetActive (false);

			stacks = new List<Stack> ();
			stacks.Add (gamestacks.Player);
			foreach (Stack stack in stacks) {
				stack.AddListener (this);
			}
		}

		// Use this for initialization
		void Start () {
			ResumeGame ();
			stacks [0].transform.position = new Vector3(0, 0.4f, 0);
		}

		public void ResumeGame() {
			Time.timeScale = 1;
			gameInputController.enabled = true;
		}

		#region FCEventListener implementation

		public void OnEvent (FCEvent fcEvent, GameObject gameObject)
		{
			if (fcEvent == FCEvent.END) 
			{
			}
		}

		#endregion

		private int TotalNumberOfStacks() {
			return stacks.Count + 1;
		}

		private bool IsFlippingStack () {
			foreach (Stack stack in stacks) {
				if (stack.flipper.IsFlipping) {
					return true;
				}
			}
			return false;
		}

		#region LandingListener implementation

		public void AddLandingListener (FCEventListener listener, FCEvent fcEvent)
		{
			foreach (Stack stack in stacks) {
				stack.AddLandingListener (listener, fcEvent);
			}
		}

		#endregion
	}
}


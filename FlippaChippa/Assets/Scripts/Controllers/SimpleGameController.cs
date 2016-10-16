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
	public class SimpleGameController : MonoBehaviour, FCEventListener
	{
		private Stack targetStack;
		private List<Stack> stacks;

		private int indexOfVisibleStack = 0;

		public GameInputController gameInputController;

		private StackGenerator stackGenerator;
		private PrefabsManager prefabsManager;

		void Awake() {
			stackGenerator = new StackGenerator ();
			prefabsManager = GameObject.FindGameObjectWithTag (Tags.PREFABS_MANAGER).GetComponent<PrefabsManager> ();
		}

		// Use this for initialization
		void Start () {
			stackGenerator.SetPrefabsManager (prefabsManager);
			int[] chipIds = new int[]{0,1,5,3,4,5};
			int[] startFlips = new int[]{0,0,0};
			int[] targetFlips = new int[]{1,0,0};
			GameStacks gamestacks = stackGenerator.GenerateStacks (chipIds, startFlips, targetFlips, true);
			targetStack = gamestacks.Target;
			targetStack.AddListener (this);
			targetStack.gameObject.SetActive (false);
			ResumeGame ();

			stacks = new List<Stack> ();
			stacks.Add (gamestacks.Player);
			foreach (Stack stack in stacks) {
				stack.AddListener (this);
			}
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
	}
}


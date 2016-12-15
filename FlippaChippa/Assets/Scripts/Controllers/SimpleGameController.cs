using UnityEngine;

namespace AssemblyCSharp
{
	public class SimpleGameController : MonoBehaviour, FCEventListener, LandingListener
	{
		private Stack demoStack;
		private GameStacks gamestacks;
		private int indexOfVisibleStack = 0;

		public GameInputController gameInputController;
		public LandingEmission landingEmission;

		private GameStacksBuilder stackGenerator;
		private PrefabsManager prefabsManager;

		void Awake() {
			stackGenerator = new GameStacksBuilder ();
			prefabsManager = GameObject.FindGameObjectWithTag (Tags.PREFABS_MANAGER).GetComponent<PrefabsManager> ();

			int[] chipIds = new int[]{0,4,6,3,9,6};
			int[] crushWeights = new int[] { 0, 0, 5, 4, 0, 0};
			bool[] initFlips = new bool[]{ false, true, false, false, true, false };
			int[] targetFlips = new int[]{1,0,0};
			GameGeneratorMeta courseMeta = new GameGeneratorMeta (chipIds, crushWeights, initFlips, targetFlips);
			ApplicationModel.stackMetaPair = GameGenerator.CreateFromCourseMeta (courseMeta, prefabsManager);
			gamestacks = stackGenerator.BuildGameStacks (ApplicationModel.stackMetaPair, prefabsManager);
			gamestacks.Target.gameObject.SetActive(false);

			demoStack = gamestacks.Player;
			demoStack.AddListener(this);
			demoStack.transform.position = new Vector3(0, 0.4f, 0);
			demoStack.gameObject.AddComponent(typeof(ChipListHighligter));
		}

		// Use this for initialization
		void Start () {
			ResumeGame ();
			Destroy(gamestacks.Target.gameObject);
		}

		public void ResumeGame() {
			Time.timeScale = 1;
			gameInputController.enabled = true;
		}

		#region FCEventListener implementation

		public void OnEvent (FCEvent fcEvent, GameObject gameObject)
		{
			if (fcEvent == FCEvent.BEGIN) {
				demoStack.GetComponent<ChipListHighligter>().Stop();
			}
		}

		#endregion

		#region LandingListener implementation

		public void AddLandingListener (FCEventListener listener, FCEvent fcEvent)
		{
			demoStack.AddLandingListener (listener, fcEvent);
		}

		#endregion
	}
}


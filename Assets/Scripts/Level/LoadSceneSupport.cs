using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// for SceneManager

public class LoadSceneSupport : MonoBehaviour {

	public string LevelName = null;

    public Button mStart;
    public Button mExit;


	// Use this for initialization
	void Start () {
        // Workflow assume:
        //      mLevelOneButton: is dragged/placed from UI
        // add in listener
        mStart.onClick.AddListener(
                () => {                     // Lamda operator: define an annoymous function
                LoadScene("Jump");
                });
        mExit.onClick.AddListener(
                () => {                     // Lamda operator: define an annoymous function
                    Application.Quit();
                });
		
	}

	public void gotoMainMenu(){
		LoadScene ("Menu");
	}

    // Update is called once per frame
    void Update () {
	
	}
    
	void LoadScene(string theLevel) {
        SceneManager.LoadScene(theLevel);
	}
}
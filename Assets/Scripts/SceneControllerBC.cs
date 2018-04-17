using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

namespace Com.Mygame{
	public interface IUserInterface {
		void emitDisk ();
	}

	//Scene State
	public interface IQueryStatus{
		bool isCounting ();
		bool isShooting ();
		int getRound ();
		int getPoint ();
		int getEmitTime();
	}

	public interface IJudgeEvent{
		void nextRound ();
		void setPoint (int point);
	}

	public class SceneController: System.Object, IQueryStatus, IUserInterface, IJudgeEvent{
		private static SceneController instance;
		private SceneControllerBC baseCode;
		private GameModel gameModel;
		private Judge judge;

		private int round;
		private int point;

		public static SceneController getInstance(){
			if (instance == null) {
				instance = new SceneController ();
			}
			return instance;
		}

		public void setGameModel(GameModel obj){ gameModel = obj; }
		internal GameModel getGameModel(){ return gameModel; }

		public void setJudge(Judge obj){ judge = obj; }
		internal Judge getJudge(){ return judge; }

		public void setSceneControllerBC(SceneControllerBC obj){ baseCode = obj; }
		internal SceneControllerBC getSceneControlerBC(){ return baseCode; }

		public void emitDisk(){ gameModel.prepareToEmitDisk (); }

		public bool isCounting(){
			return gameModel.isCounting ();
		}
		public bool isShooting(){
			return gameModel.isShooting ();
		}
		public int getRound(){
			return round;
		}
		public int getPoint(){
			return point;
		}
		public int getEmitTime(){
			return (int)gameModel.timeToEmit + 1;
		}

		public void setPoint(int score) {
			point = score;
		}
		public void nextRound(){
			point = 0;
			baseCode.loadRoundData (++round);
		}

	}

}

public class SceneControllerBC : MonoBehaviour {

	private Color color;  
	private Vector3 emitPos;  
	private Vector3 emitDir;  
	private float speed;  

	void Awake() {  
		SceneController.getInstance().setSceneControllerBC(this);  
	}  

	public void loadRoundData(int round) {  
		switch(round) {  
		case 1:     // 第一关  
			color = Color.green;  
			emitPos = new Vector3(-2.5f, 0.2f, -5f);  
			emitDir = new Vector3(24.5f, 40.0f, 67f);  
			speed = 3.5f;  
			SceneController.getInstance().getGameModel().setting(1, color, emitPos, emitDir.normalized, speed, 1);  
			break;  
		case 2:     // 第二关  
			color = Color.red;  
			emitPos = new Vector3(2.5f, 0.2f, -5f);  
			emitDir = new Vector3(-24.5f, 35.0f, 67f);  
			speed = 4f;  
			SceneController.getInstance().getGameModel().setting(1, color, emitPos, emitDir.normalized, speed, 2);  
			break;  
		}  
	}  
}

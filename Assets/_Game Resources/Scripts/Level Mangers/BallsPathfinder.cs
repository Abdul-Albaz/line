using System;
using static System.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class BallsPathfinder : MonoBehaviour {

	static public BallsPathfinder instance;

	public int RowsNumber = 10;						    //Number of rows in the Vars.fields[,] 2D array
	public int ColumnsNumber = 10;                      //Number of columns in the Vars.fields[,] 2D array
	public int numberOfBalls = 7;                   
	private int iPath=100;      
	private GameObject tiles;             
	private bool isFirstWave = true;				
	private int[,] placeholderBalls;
	private int lastScore = 0;

	
	[SerializeField]
	private float _speed=0.02f;



	[SerializeField]
	private float _editRowNum = 7;

	[SerializeField]
	private float _ballCoolectionToWin = 3;

	[SerializeField]
	private float _editColumNum = 7;
	[SerializeField]
	private int _firstWaveNumber = 5;
	[SerializeField]
	private int NumberBallsCreated = 2;
	[SerializeField]
	private int _numberOfBallInGame = 4;


	// Flag variable for handling
	// bottum up diagonal traversing
	private int numberOfConsecutiveBalls = 1;
	private int currentCellValue = 0;
	private int k1 = 0, k2 = 0;
	private bool flag = true;

	void OnEnable () {
		isFirstWave = true;
		numberOfConsecutiveBalls = 1;
		placeholderBalls = null;
		lastScore = 0;
		currentCellValue = 0;
		k1 = 0;
		k2 = 0;
		flag = true;
		
		Initializefields(RowsNumber, ColumnsNumber);
		tiles = GameObject.Find("Tiles");
		CreateTiles();
		CreateNewBalls();
	}

	private void Initializefields(int rows, int columns) {
		Vars.fields = new int[rows, columns];
		for (int i=0; i < Vars.fields.GetLength(0); i++) {
			for (int j=0; j < Vars.fields.GetLength(1); j++) {
				Vars.fields[i,j] = 0;
			}
		}
	}
	      /*******
	      *******
	      *******
	      *******
	      *******
	      *******
	      *******/

	private void CreateTiles() {
		for (int i=0; i < Vars.fields.GetLength(0); i++) {
			for (int j=0; j < Vars.fields.GetLength(1); j++) {
				GameObject tile = Instantiate(Resources.Load("Tile", typeof(GameObject))) as GameObject;
				tile.name = "Tile" + i + "X" + j;
				tile.transform.position = new Vector2(i, j);
				tile.transform.parent = tiles.transform;
			}
		}

		//make camera work with any phone sizes
		Camera.main.transform.position = new Vector3(tiles.transform.position.x + Vars.fields.GetLength(0) / 2, (tiles.transform.position.y + Vars.fields.GetLength(1) / 2), -10);
		float screenAspectRatio =  (float)Screen.height / (float)Screen.width;
		Camera.main.orthographicSize = (ColumnsNumber / 2) * screenAspectRatio + 1.4f;	// can change the margen								 
	}

	private void CreateNewBalls() {
		int numberOfBallsToCreate;
		if(isFirstWave) {
			numberOfBallsToCreate = _firstWaveNumber;														//This many balls will be created on the first wave
																											//you can add the effict the first wave on this line
			isFirstWave = false;

			while(numberOfBallsToCreate != 0) {
				int ballXPos = UnityEngine.Random.Range(0, RowsNumber);
				int ballYPos = UnityEngine.Random.Range(0, ColumnsNumber);
				if(Vars.fields[ballXPos, ballYPos] != 0) continue;


				int ballColor = UnityEngine.Random.Range(0, numberOfBalls);
				Vars.fields[ballXPos, ballYPos] = (ballColor + 1);
				GameObject ball = Instantiate(Resources.Load("Ball" + (ballColor + 1), typeof(GameObject))) as GameObject;
				ball.name = "Ball";
				ball.transform.parent = GameObject.Find("Tile" + ballXPos + "X" + ballYPos).transform;
				ball.transform.localPosition = new Vector2(0, 0);
				numberOfBallsToCreate--;
			}

			CreatePlaceholderBalls(NumberBallsCreated);
		}else {		
			numberOfBallsToCreate = NumberBallsCreated;														//This many balls will be created on all waves except the first one
			CheckForAvailabeFields(numberOfBallsToCreate);

			ConvertPlaceholderBallsToRealBalls();
			CheckScore();
			CreatePlaceholderBalls(numberOfBallsToCreate);
			if(lastScore != Vars.score) {	
				lastScore = Vars.score;
				GameObject.Find("BallBlastSound").GetComponent<AudioSource> ().Play();
			}
		}	
	}

	private void CreateNewBall(int placeholderXPos, int placeholderYPos) {					//This will create a single ball when player steps on a placeholder ball
		int numberOfBallsToCreate = 1;
		while(numberOfBallsToCreate != 0) {
			int ballXPos = UnityEngine.Random.Range(0, RowsNumber);
			int ballYPos = UnityEngine.Random.Range(0, ColumnsNumber);
			if(Vars.fields[ballXPos, ballYPos] != 0) continue;

			int ballColor = placeholderBalls[placeholderXPos, placeholderYPos];
			placeholderBalls[placeholderXPos, placeholderYPos] = 0;
			Vars.fields[ballXPos, ballYPos] = ballColor;
			GameObject ball = Instantiate(Resources.Load("Ball" + ballColor, typeof(GameObject))) as GameObject;
			ball.name = "Ball";
			ball.transform.parent = GameObject.Find("Tile" + ballXPos + "X" + ballYPos).transform;
			ball.transform.localPosition = new Vector2(0, 0);
			numberOfBallsToCreate--;
		}
		CheckScore();
		if(lastScore != Vars.score) {	
			lastScore = Vars.score;
		}
	}

	private int CheckForAvailabeFields(int numberOfBallsToCreate) {
		int numberOfEmptyFields = 0;
		for(int i = 0; i < Vars.fields.GetLength(0); i++) {
			for(int j = 0; j < Vars.fields.GetLength(1); j++) {
				if(Vars.fields[i,j] == 0) {
					numberOfEmptyFields++;
				}
			}
		}

		if(numberOfEmptyFields == 0) {																			//When there is no available field game over menu will pop up
																												//enable the effictc  like pop mune efict 
			
			GetComponent<Menus> ().GameOverMenu();
			return 0;
		}
		return numberOfEmptyFields;
	}
			
	


	private void CreatePlaceholderBalls(int numberOfBallsToCreate) {											// This will create small placeholder balls, that will pop into real balls on the next turn
																												// here we can add the effict when the ball  is try to appering
		int availableFields = CheckForAvailabeFields(numberOfBallsToCreate);
		if(numberOfBallsToCreate > availableFields) {
			numberOfBallsToCreate = availableFields;
		}
		placeholderBalls = new int[Vars.fields.GetLength(0), Vars.fields.GetLength(1)];
		while(numberOfBallsToCreate != 0) {
			int ballXPos = UnityEngine.Random.Range(0, RowsNumber);
			int ballYPos = UnityEngine.Random.Range(0, ColumnsNumber);
			if(Vars.fields[ballXPos, ballYPos] != 0 || placeholderBalls[ballXPos, ballYPos] != 0) continue;

			int ballColor = UnityEngine.Random.Range(0, numberOfBalls);
			placeholderBalls[ballXPos, ballYPos] = (ballColor + 1);
			GameObject ball = Instantiate(Resources.Load("Ball" + (ballColor + 1), typeof(GameObject))) as GameObject;
			ball.name = "BallPlaceholder";
			ball.transform.localScale = new Vector2(0.5f, 0.5f);
			ball.transform.parent = GameObject.Find("Tile" + ballXPos + "X" + ballYPos).transform;
			ball.transform.localPosition = new Vector2(0, 0);
			ball.GetComponent<SpriteRenderer> ().sortingOrder = -1;
			numberOfBallsToCreate--;
			GetComponent<Menus> ().UpdateNextWaveBallsColor(numberOfBallsToCreate, ballColor);
		}
	}

	private void ConvertPlaceholderBallsToRealBalls() {// This is convert placeholder balls from the method above into real balls
		if(placeholderBalls == null) return;
		for (int i=0; i < placeholderBalls.GetLength(0); i++) {
			for (int j=0; j < placeholderBalls.GetLength(1); j++) {
				if(placeholderBalls[i, j] != 0) {
					if(GameObject.Find("Tile" + i + "X" + j).transform.Find("Ball") == null) {
						Vars.fields[i, j] = placeholderBalls[i, j];
						placeholderBalls[i, j] = 0;
					}
					
					GameObject ball = GameObject.Find("Tile" + i + "X" + j).transform.Find("BallPlaceholder").gameObject;
					ball.name = "Ball";
					ball.transform.localScale = new Vector2(1, 1);
					ball.GetComponent<SpriteRenderer> ().sortingOrder = 0;
				}
			}
		}
	}

	private void CheckForScoreVertically() {
		for(int i = 0; i < Vars.fields.GetLength(0); i++) {
			ResetConsecuteveBallsSearchingVariables();
			for(int j = 0; j < Vars.fields.GetLength(1); j++) {
				if(Vars.fields[i,j] != 0) {
					if(Vars.fields[i,j] == currentCellValue) {
						numberOfConsecutiveBalls++;
						if(numberOfConsecutiveBalls >= _ballCoolectionToWin) {


							int index = 0;
							for (int y = j - numberOfConsecutiveBalls + 1; y <= j; y++) {

								DestroyBall(i, y, index);
								index++;

							}
						}
					}else {
						currentCellValue = Vars.fields[i,j];
						numberOfConsecutiveBalls = 1;
					}
				}else {
					ResetConsecuteveBallsSearchingVariables();
				}
			}
		}
	}

	private void CheckForScoreHorizontally() {
		for(int i = 0; i < Vars.fields.GetLength(1); i++) {
			ResetConsecuteveBallsSearchingVariables();
			for(int j = 0; j < Vars.fields.GetLength(0); j++) {
				
				if(Vars.fields[j,i] != 0) {
					if(Vars.fields[j,i] == currentCellValue) {
						numberOfConsecutiveBalls++;
						if(numberOfConsecutiveBalls >= _ballCoolectionToWin) {
							int index = 0;// the number of ball you can change it to collecy score Horizontally
							for (int y = j - numberOfConsecutiveBalls + 1; y <= j; y++) {
								
								DestroyBall(y, i,index);
								index++;
								
							}
						}

					}else {
						currentCellValue = Vars.fields[j,i];
						numberOfConsecutiveBalls = 1;
					}
				}else {
					ResetConsecuteveBallsSearchingVariables();
				}
			}
		}
	}

	private void CheckForScoreDiagonally() {
		for (int line = 1; line <= (Vars.fields.GetLength(0) + Vars.fields.GetLength(1) - 1); line++) {
			ResetConsecuteveBallsSearchingVariables();
			int start_col = Max(0, line - Vars.fields.GetLength(0));
			int count = Min(line, Math.Min((Vars.fields.GetLength(1) - start_col), Vars.fields.GetLength(0)));

			for (int j = 0; j < count; j++) {
				if(Vars.fields[Min(Vars.fields.GetLength(0), line) - j - 1, start_col + j] != 0) {
					if(Vars.fields[Min(Vars.fields.GetLength(0), line) - j - 1, start_col + j] == currentCellValue) {
						numberOfConsecutiveBalls++;
					if(numberOfConsecutiveBalls >= _ballCoolectionToWin)
						{																			 // the number of ball you can change it to collecy score Diagonally
							for (int y = 0; y < numberOfConsecutiveBalls; y++) {
								

								DestroyBall((Min(Vars.fields.GetLength(0), line) - j - 1 + y), start_col + j - y, y);
								

								
							}
						}
					}else {
						currentCellValue =  Vars.fields[Min(Vars.fields.GetLength(0), line) - j - 1, start_col + j];
						numberOfConsecutiveBalls = 1;
					}
				}else {
					ResetConsecuteveBallsSearchingVariables();
				}
			}
		}

	}

	async void DestroyBall(int x,int y,int index)
	{
		await Task.Delay(200 * (1 + index));
		Vars.score++;
		Vars.fields[x, y] = 0;
		Taptic.Heavy();
		try
        {
			GameObject ball = GameObject.Find("Tile" + x + "X" + y).transform.Find("Ball").gameObject;
			ball.transform.DOScale(0f, 0.2f).OnComplete(() => {

				// here we can emliment the destroy effict of the five ball matching destory 																								//ball.GetComponent<BallExplosion>().ActivateBallExplosion();
				Destroy(ball);

			});
		} catch (Exception ignored) { }
	}



	private bool CheckForScoreDiagonallyTraverse(int [,]m, int i, int j, int row, int col) {
		if (i >= row || j >= col) {
			if (flag) {
				int a = k1;
				k1 = k2;
				k2 = a;
				flag = !flag;
				k1++;
			} else {
				int a = k1;
				k1 = k2;
				k2 = a;
				flag = !flag;
			}
			numberOfConsecutiveBalls = 1;
			currentCellValue = 0;
			return false;
		}




		if(Vars.fields[i, j] != 0) {
			if(Vars.fields[i, j] == currentCellValue) {
				numberOfConsecutiveBalls++;
				if(numberOfConsecutiveBalls >= 4) {								 // the number of ball you can change it to collecy score Diagonally
					for (int y = 0; y < numberOfConsecutiveBalls; y++) {
						
					DestroyBall((i - y), (j - y),y);

					}	
				}
			}else {
				currentCellValue =  Vars.fields[i, j];
				numberOfConsecutiveBalls = 1;
			}
		}else {
			numberOfConsecutiveBalls = 1;
			currentCellValue = 0;
		}
		
		if (CheckForScoreDiagonallyTraverse(m, i + 1, j + 1, row, col)) {
			return true;
		}
		
		if (CheckForScoreDiagonallyTraverse(m, k1, k2, row, col)) {
			return true;



		}
		
		return true;
	}

	private void CheckScore() {//This will check if there is a 5 or more consecutive balls vertically, horizontally or diagonally
		CheckForScoreVertically();
		CheckForScoreHorizontally();
		CheckForScoreDiagonally();
		ResetConsecuteveBallsSearchingVariables();
		CheckForScoreDiagonallyTraverse(Vars.fields, 0, 0, Vars.fields.GetLength(0), Vars.fields.GetLength(1));
		GetComponent<Menus> ().UpdateScore();
	}

	private void ResetConsecuteveBallsSearchingVariables() {
		numberOfConsecutiveBalls = 1;
		currentCellValue = 0;
		k1 = 0;
		k2 = 0;
		flag = true;
	}

	public void InitializeBallMovement(GameObject ball, int xTarget, int yTarget) {
		int ballColor = Vars.fields[Vars.ballStartPosX, Vars.ballStartPosY];
		Vars.fields[Vars.ballStartPosX, Vars.ballStartPosY] = 0;
		int[,] path = FindPath(Vars.ballStartPosX, Vars.ballStartPosY, xTarget, yTarget);
		if(path != null) {//Check if path to the choosen location is available

		
			Vars.ball.GetComponent<SelectedBallAnimation> ().enabled = false;
		    Vars.ball.GetComponent<DisableTrail>().enabled = true;

			Vars.ball.transform.localScale = new Vector2(1, 1);
			StartCoroutine(BallMovement(path, xTarget, yTarget, ballColor));//Move the ball to the choosen location
			GameObject.Find("BallMoveSound").GetComponent<AudioSource> ().Play();
		}else {
			Vars.fields[Vars.ballStartPosX, Vars.ballStartPosY] = ballColor;
			Vars.ball.transform.parent = GameObject.Find("Tile" + Vars.ballStartPosX + "X" + Vars.ballStartPosY).transform;
			Vars.ball.GetComponent<DisableTrail>().enabled = false;
			Vars.ball.GetComponent<BuncingBallAnimation> ().enabled = true;
			
			Invoke("RestartAnaimation", 0.5f);
			GameObject.Find("NoAvailableMoveSound").GetComponent<AudioSource> ().Play();
			
			Debug.Log("NO PATH ");					                      // van try the animation in this line for the ball when it not find the path
		}	
	}

	void RestartAnaimation()
    {
		Vars.ball.GetComponent<BuncingBallAnimation>().enabled = false;
		Vars.ball.GetComponent<SelectedBallAnimation>().enabled = true;
		CancelInvoke();
	
	}
			IEnumerator BallMovement(int[,] path, int xTarget, int yTarget, int ballColor) {
		Vars.isBallMoving = true;
        while (new Vector2(Vars.ball.transform.position.x, Vars.ball.transform.position.y) != new Vector2(xTarget, yTarget))
		{
			if ((int)Vars.ball.transform.position.y > 0 && path[(int)Vars.ball.transform.position.x, (int)Vars.ball.transform.position.y-1]==100) {
				path[(int)Vars.ball.transform.position.x, (int)Vars.ball.transform.position.y] = 1;
				path[(int)Vars.ball.transform.position.x, (int)Vars.ball.transform.position.y-1]=1;
				Vector3 postionBall = new Vector3(Vars.ball.transform.position.x, Vars.ball.transform.position.y - 1,transform.position.z);
				Vars.ball.transform.DOMove(postionBall, _speed);
				Debug.Log("1 ");
			}
				
			else if ((int)Vars.ball.transform.position.y+1 < path.GetLength(1) && path[(int)Vars.ball.transform.position.x, (int)Vars.ball.transform.position.y+1]==100) {
				path[(int)Vars.ball.transform.position.x, (int)Vars.ball.transform.position.y] = 1;
				path[(int)Vars.ball.transform.position.x, (int)Vars.ball.transform.position.y+1]=1;
				Vector3 postionBall = new Vector2(Vars.ball.transform.position.x, Vars.ball.transform.position.y+1);
				Vars.ball.transform.DOMove(postionBall, _speed);
			}
			else if (Vars.ball.transform.position.x-1 >= 0 && path[(int)Vars.ball.transform.position.x-1, (int)Vars.ball.transform.position.y]==100) {
				path[(int)Vars.ball.transform.position.x, (int)Vars.ball.transform.position.y] = 1;
				path[(int)Vars.ball.transform.position.x-1, (int)Vars.ball.transform.position.y]=1;
				Vector3 postionBall = new Vector2(Vars.ball.transform.position.x-1, Vars.ball.transform.position.y);
				Vars.ball.transform.DOMove(postionBall, _speed);
			} 
			else if ((int)Vars.ball.transform.position.x+1 < path.GetLength(0) && path[(int)Vars.ball.transform.position.x+1, (int)Vars.ball.transform.position.y]==100) {
				path[(int)Vars.ball.transform.position.x, (int)Vars.ball.transform.position.y] = 1;
				path[(int)Vars.ball.transform.position.x+1, (int)Vars.ball.transform.position.y]=1;
				Vector3 postionBall = new Vector2(Vars.ball.transform.position.x+1, Vars.ball.transform.position.y);
				Vars.ball.transform.DOMove(postionBall, _speed);
			}
			yield return new WaitForSeconds(_speed);								// this for incress or decress the player speed of moving 
        }

		Vars.isBallMoving = false;
		Vars.fields[Vars.ballStartPosX, Vars.ballStartPosY] = 0;
		Vars.fields[xTarget, yTarget] = ballColor;
		Vars.ballStartPosX = -1;
        Vars.ball.transform.parent = null;            
        Vars.ball.transform.parent = GameObject.Find("Tile" + xTarget + "X" + yTarget).transform;
		CheckScore();

		if(lastScore == Vars.score) {
			if(Vars.ball.transform.parent.transform.Find("BallPlaceholder") != null) {
				Destroy(Vars.ball.transform.parent.transform.Find("BallPlaceholder").gameObject);
				CreateNewBall(xTarget, yTarget);
			}
			CreateNewBalls();
		}else {
			GameObject.Find("BallBlastSound").GetComponent<AudioSource> ().Play();
			lastScore = Vars.score;
			
		}
     }

	public int Rows {
		get { return RowsNumber; }
	}

	public int Cols {
		get { return ColumnsNumber; }
	}

	private int GetNodeContents(int[,] iMaze, int iNodeNo) {
		int iCols=iMaze.GetLength(1);
		return iMaze[iNodeNo/iCols,iNodeNo-iNodeNo/iCols*iCols];
	}

	private void ChangeNodeContents(int[,] iMaze, int iNodeNo, int iNewValue) {
		int iCols=iMaze.GetLength(1);
		iMaze[iNodeNo/iCols,iNodeNo-iNodeNo/iCols*iCols]=iNewValue;
	}

	public int[,] FindPath(int iFromY, int iFromX, int iToY, int iToX) {//When user tap on the tile, this method will check if there is available path to that tile. In case there are no available paths this will return null value
		int iBeginningNode = iFromY*this.Cols + iFromX;
		int iEndingNode = iToY*this.Cols + iToX;
		return ( Search(iBeginningNode, iEndingNode) ) ;
	}

	private enum Status{Ready, Waiting, Processed}

	private int[,] Search(int iStart, int iStop) {
		const int empty=0;
	
		int iRows=RowsNumber;
		int iCols=ColumnsNumber;
		int iMax=iRows*iCols;
		int[] Queue=new int[iMax];
		int[] Origin=new int[iMax];
		int iFront=0, iRear=0;

		if ( GetNodeContents(Vars.fields, iStart)!=empty || GetNodeContents(Vars.fields, iStop)!=empty ) return null;
	
		int[,] iMazeStatus=new int[iRows,iCols];
	
		for(int i=0;i<iRows;i++)
			for(int j=0;j<iCols;j++)
				iMazeStatus[i,j]=(int)Status.Ready;
		

		Queue[iRear]=iStart; 
		Origin[iRear]=-1; 
		iRear++;
		int iCurrent, iLeft, iRight, iTop, iDown;
		while(iFront!=iRear)	{
			if (Queue[iFront]==iStop) break;

			iCurrent=Queue[iFront];
		
			iLeft=iCurrent-1;
			if (iLeft>=0 && iLeft/iCols==iCurrent/iCols) 
				if ( GetNodeContents(Vars.fields, iLeft)==empty )
					if (GetNodeContents(iMazeStatus, iLeft) == (int)Status.Ready) {
						Queue[iRear]=iLeft;
						Origin[iRear]=iCurrent;
						Debug.Log(" ready");
					
			ChangeNodeContents(iMazeStatus, iLeft, (int)Status.Waiting);
						iRear++;
						Debug.Log(" WATING ");
					}

			iRight=iCurrent+1;
			if (iRight<iMax && iRight/iCols==iCurrent/iCols) 
				if ( GetNodeContents(Vars.fields, iRight)==empty )
					if (GetNodeContents(iMazeStatus, iRight) == (int)Status.Ready) {
						Queue[iRear]=iRight;
						Origin[iRear]=iCurrent;
						ChangeNodeContents(iMazeStatus, iRight, (int)Status.Waiting);
						iRear++;
					}
	
			iTop=iCurrent-iCols;
			if (iTop>=0 )
				if ( GetNodeContents(Vars.fields, iTop)==empty )
					if (GetNodeContents(iMazeStatus, iTop) == (int)Status.Ready) {
						Queue[iRear]=iTop;
						Origin[iRear]=iCurrent;
						ChangeNodeContents(iMazeStatus, iTop, (int)Status.Waiting );
						iRear++;
					}

			iDown=iCurrent+iCols;
			if (iDown<iMax )
				if ( GetNodeContents(Vars.fields, iDown)==empty )
					if (GetNodeContents(iMazeStatus, iDown) == (int)Status.Ready) {
						Queue[iRear]=iDown;
						Origin[iRear]=iCurrent;
						ChangeNodeContents(iMazeStatus, iDown, (int)Status.Waiting);
						iRear++;
					}

			ChangeNodeContents(iMazeStatus, iCurrent, (int)Status.Processed);
			iFront++;
			Debug.Log(" process");
		}

		int[,] iMazeSolved=new int[iRows,iCols];
		for(int i=0;i<iRows;i++)
			for(int j=0;j<iCols;j++)
				iMazeSolved[i,j]=Vars.fields[i,j];

		iCurrent=iStop;
		ChangeNodeContents(iMazeSolved, iCurrent, iPath);
		for(int i=iFront; i>=0; i--) {
			if (Queue[i]==iCurrent) {
				iCurrent=Origin[i];
				if (iCurrent == -1)
				return ( iMazeSolved );
				ChangeNodeContents(iMazeSolved, iCurrent, iPath);
				Taptic.Light();
				Debug.Log(" stop");
			}
		}
		return null;
	}
}


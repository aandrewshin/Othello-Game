#nullable enable
using System;
using static System.Console;

namespace pa1
{
    static partial class Program
    {
        static void Main( )
        {
            WriteLine( );
            WriteLine( " Welcome to Othello!" );
            WriteLine( );
			int[]rowsCols=CollectInfo();
			string[ , ] game = NewBoard( rowsCols[0], rowsCols[1] );
			int totalMoves=(rowsCols[0]*rowsCols[1]);
			string [,]board=new string [rowsCols[0],rowsCols[1]];
			bool gameEnd=false;
			int [] finalScore=new int[2];
			while (gameEnd==false){
			DisplayBoard( game );
            WriteLine( );
            board=RunGame(game);
            gameEnd=GameEndCheck(totalMoves,board);		
			Console.Clear();
            }
            GameResult(board);

        }

		static string player1;
		static string player2;
		static int turnCount=0;
		static int invalidWhiteMoves=0;
		static int invalidBlackMoves=0;
		static int prevInvalidWhiteMoves=0;
		
		public static int[] CollectInfo()	
		{
			bool validBoard=false;
			//Prompts for user names and te size of the board
			
			Write("Player 1 (White): ");
			player1=ReadLine();
			Write ("Player 2 (Black): ");
			player2=ReadLine();
			int inputRow=0;
			int inputCol=0;
			
			while (validBoard==false){
				validBoard=true;
				Write ("Enter number of rows (4 to 26, even): ");
				inputRow=Int32.Parse(ReadLine());
				Write ("Enter number of columns (4 to 26, even): ");
				inputCol=Int32.Parse(ReadLine());
			//Checks the validity of the board size
				if (inputRow<4||inputRow>26||inputCol<4||inputCol>26||inputCol%2!=0||inputRow%2!=0){
				validBoard=false;
				WriteLine ("invalid parameters. please input even numbers");
				}
			}
			int []rowsCols=new int [2]{inputRow,inputCol};
			return rowsCols;
		
		}
        // -----------------------------------------------------------------------------------------
        // Return the single-character string "a".."z" corresponding to its index 0..25. 
        // Return " " for an invalid index.
        
        static string LetterAtIndex( int number )
        {
            if( number < 0 || number > 25 ) return " ";
            else return "abcdefghijklmnopqrstuvwxyz"[ number ].ToString( );
        }
        
        // -----------------------------------------------------------------------------------------
        // Return the index 0..25 corresponding to its single-character string "a".."z". 
        // Return -1 for an invalid string.
        
        static int IndexAtLetter( string letter )
        {
            if( letter.Length != 1 ) return -1;
            else return "abcdefghijklmnopqrstuvwxyz".IndexOf( letter[ 0 ] );
        }
        
        // -----------------------------------------------------------------------------------------
        // Create a new Othello game board, initialized with four pieces in their starting
        // positions. The counts of rows and columns must be no less than 4, no greater than 26,
        // and not an odd number. If not, the new game board is created as an empty array.
        
        public static string[ , ] NewBoard( int rows, int cols )
        {
            const string blank = " ";
            const string white = "O";
            const string black = "X";
            
            if(    rows < 4 || rows > 26 || rows % 2 == 1
                || cols < 4 || cols > 26 || cols % 2 == 1 ) return new string[ 0, 0 ];
                
            string[ , ] board = new string[ rows, cols ];
            
            for( int row = 0; row < rows; row ++ )
            {
                for( int col = 0; col < cols; col ++ )
                {
                    board[ row, col ] = blank;
                }
            }
            
            board[ rows / 2 - 1, cols / 2 - 1 ] = white;
            board[ rows / 2 - 1, cols / 2     ] = black;
            board[ rows / 2,     cols / 2 - 1 ] = black;
            board[ rows / 2,     cols / 2     ] = white;
            
            return board;
        }

        // -----------------------------------------------------------------------------------------
        // Display the Othello game board on the Console.
        // All information about the game is held in the two-dimensional string array.
        
        public static void DisplayBoard( string[ , ] board )
        {
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            const string mx = "\u256c"; // marked horizontal vertical cross
            const string sp =      " "; // space

            // Nothing to display?
            if( board == null ) return;
            
            int rows = board.GetLength( 0 );
            int cols = board.GetLength( 1 );
            if( rows == 0 || cols == 0 ) return;
            
            // Display the board row by row.
            for( int row = 0; row < rows; row ++ )
            {
                if( row == 0 )
                {
                    // Labels above top edge.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if( col == 0 ) Write( "   {0}{0}{1}{0}", sp, LetterAtIndex( col ) );
                        else Write( "{0}{0}{1}{0}", sp, LetterAtIndex( col ) );
                    }
                    WriteLine( );
                    
                    // Border above top row.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if( col == 0 ) Write( "   {0}{1}{1}{1}", tl, h );
                        else Write( "{0}{1}{1}{1}", hb, h );
                        if( col == cols - 1 ) Write( "{0}", tr );
                    }
                    WriteLine( );
                }
                else
                {
                    // Border above a row which is not the top row.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if(    rows > 5 && cols > 5 && row ==        2 && col ==        2 
                            || rows > 5 && cols > 5 && row ==        2 && col == cols - 2 
                            || rows > 5 && cols > 5 && row == rows - 2 && col ==        2 
                            || rows > 5 && cols > 5 && row == rows - 2 && col == cols - 2 )  
                            Write( "{0}{1}{1}{1}", mx, h );
                        else if( col == 0 ) Write( "   {0}{1}{1}{1}", vr, h );
                        else Write( "{0}{1}{1}{1}", hv, h );
                        if( col == cols - 1 ) Write( "{0}", vl );
                    }
                    WriteLine( );
                }
                
                // Row content displayed column by column.
                for( int col = 0; col < cols; col ++ ) 
                {
                    if( col == 0 ) Write( " {0,-2}", LetterAtIndex( row ) ); // Labels on left side
                    Write( "{0} {1} ", v, board[ row, col ] );
                    if( col == cols - 1 ) Write( "{0}", v );
                }
                WriteLine( );
                
                if( row == rows - 1 )
                {
                    // Border below last row.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if( col == 0 ) Write( "   {0}{1}{1}{1}", bl, h );
                        else Write( "{0}{1}{1}{1}", ha, h );
                        if( col == cols - 1 ) Write( "{0}", br );
                    }
                    WriteLine( );
                }
            }
        }
        //Games takes user input for moves and determines if it is valid
       
        public static string[,] RunGame(string [,] board){
			DisplayScore(board);
			int row=-1;
			int col=-1;
			bool incorrectCoor=false;
				
				if (turnCount%2==0){
				Write (player1+" (white) please make a move (choose a coordinate (Ex. aa or cd), Enter 'quit' to quit or 'skip' to skip your turn: ");
				}
				else {
				Write (player2+" (black) please make a move (choose a coordinate, (Ex. aa or cd), Enter 'quit' to quit or 'skip' to skip your turn: ");
				}
				string move=ReadLine();
				
			while (incorrectCoor==false){
				incorrectCoor=true;
				bool validX=false;
				bool validY=false;
				string [] alphabetCheck=new string [] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
				if (move.Length==2&&move.Length>=2){
					for (int i=0;i<(board.GetLength(0))&&(validX==false);i++){
						if (move.Substring(0,1)==alphabetCheck[i]){
							validX=true;
						}
					}
					for (int j=0;j<(board.GetLength(1))&&(validY==false);j++){
						if (move.Substring(1)==alphabetCheck[j]){
							validY=true;
						}
					}
					if (validY==false||validX==false){
						incorrectCoor=false;
					}
				}
				if (move.Length<2||move.Length>2){
					incorrectCoor=false;
				}
				if (move=="skip"||move=="quit"){
					incorrectCoor=true;
				}
				if (incorrectCoor==false){
					Write ("Invalid Coordinate. Please enter a valid two letter coordinate: ");
					move=ReadLine();
				}
				
			}
			if (move=="quit"){
				//Console.Clear();
				GameResult(board);
			}	
			else if (move=="skip"){
				turnCount++;
			}
			//converts each coordinate into a numerical value
			else {
				string move1=move.Substring(0,1);
				string move2=move.Substring(1);
				
				switch(move1){
					case "a":
						row=0;
						break;
					case "b":
						row=1;
						break;
					case "c":
						row=2;
						break;
					case "d":
						row=3;
						break;
					case "e":
						row=4;
						break;
					case "f":
						row=5;
						break;
					case "g":
						row=6;
						break;
					case "h":
						row=7;
						break;
					case "i":
						row=8;
						break;
					case "j":
						row=9;
						break;
					case "k":
						row=10;
						break;
					case "l":
						row=11;
						break;
					case "m":
						row=12;
						break;
					case "n":
						row=13;
						break;
					case "o":
						row=14;
						break;
					case "p":
						row=15;
						break;
					case "q":
						row=16;
						break;
					case "r":
						row=17;
						break;
					case "s":
						row=18;
						break;
					case "t":
						row=19;
						break;
					case "u":
						row=20;
						break;
					case "v":
						row=21;
						break;
					case "w":
						row=22;
						break;
					case "x":
						row=23;
						break;
					case "y":
						row=24;
						break;
					case "z":
						row=25;
						break;
					default:
					break;
				}
				switch(move2){
					case "a":
						col=0;
						break;
					case "b":
						col=1;
						break;
					case "c":
						col=2;
						break;
					case "d":
						col=3;
						break;
					case "e":
						col=4;
						break;
					case "f":
						col=5;
						break;
					case "g":
						col=6;
						break;
					case "h":
						col=7;
						break;
					case "i":
						col=8;
						break;
					case "j":
						col=9;
						break;
					case "k":
						col=10;
						break;
					case "l":
						col=11;
						break;
					case "m":
						col=12;
						break;
					case "n":
						col=13;
						break;
					case "o":
						col=14;
						break;
					case "p":
						col=15;
						break;
					case "q":
						col=16;
						break;
					case "r":
						col=17;
						break;
					case "s":
						col=18;
						break;
					case "t":
						col=19;
						break;
					case "u":
						col=20;
						break;
					case "v":
						col=21;
						break;
					case "w":
						col=22;
						break;
					case "x":
						col=23;
						break;
					case "y":
						col=24;
						break;
					case "z":
						col=25;
						break;
					default:
					break;
				}
				
				WriteLine ("r="+row+" and c= "+col);
				
				bool validity=ValidMove(row,col,board);
				//if (validity==true){
					//DrawXO(row,col,board);

				//}else
				if(validity==false){
				WriteLine ("Invalid Input: Please input another value");
				turnCount--;
				}
				turnCount++;
			}
			return board;			
		}
		//Chekc all 8 direction to see if the move is valid according to the rule 
		public static bool ValidMove(int row,int col,string [,]board){
			bool validity=false;
			string turn;
			if (turnCount%2==0){
				turn="white";
			}
			else {
				turn="black";
			}
			
			if (board[row,col]=="X"||board[row,col]=="O"){
				validity=false;
				WriteLine ("not valid");
			}
			
			else if (turn=="white"){
				if ((row+1<=board.GetLength(0))&&row>0&&col>0&&board[row-1,col-1]=="X")
				{
					for (int i=1;col-i>=0&&row-i>=0&&(board[row-i,col-i]!=" ");i++){
						if (board[row-i,col-i]=="O"){
							validity=true;
							if (validity){
								for (int j=0;row-j>=row-i;j++){
									board[row-j,col-j]="O";
								}
							}
							else if(validity==false){
								invalidWhiteMoves++;
								WriteLine("note valid");
							}
						}
					}
				}
				if (row>0&&board[row-1,col]=="X")
				{
					for (int i=1;row-i>=0&&(board[row-i,col]!=" ");i++){
							if (board[row-i,col]=="O"){
								validity=true;
								if (validity){
									
									for (int j=0;row-j>=row-i;j++){
										board[row-j,col]="O";
									}						
								}
								else if(validity==false){
								invalidWhiteMoves++;
								}
							}					
					}
				}
				if ((col+1<board.GetLength(1))&&(row-1>0)&&row>0&&board[row-1,col+1]=="X")
				{
					for (int i=1;row-i>=0&&col+i<board.GetLength(1)&&(board[row-i,col+i]!=" ");i++){
						if (board[row-i,col+i]=="O"){
							validity=true;
							if (validity){
								
								for (int j=0;row-j>=row-i;j++){
									board[row-j,col+j]="O";
									}
							}
							else if(validity==false){
								invalidWhiteMoves++;
							}
						}
					}
				}
				if ((col-1>0)&&col>0&&board[row,col-1]=="X")
				{
					for (int i=1;col-i>=0&&(board[row,col-i]!=" ");i++){
						if (board[row,col-i]=="O"){
							validity=true;
							if (validity){
								
								for (int j=0;col-j>=col-i;j++){
									board[row,col-j]="O";
								}
							}
							else if(validity==false){
								invalidWhiteMoves++;
							}
						}
					}
				}
				if ((col+1<board.GetLength(1))&&board[row,col+1]=="X")
				{
					for (int i=1;col+i<board.GetLength(1)&&(board[row,col+i]!=" ");i++){
						if (board[row,col+i]=="O"){
							validity=true;
							if (validity){
								for (int j=0;col+j<=col+i;j++){
									board[row,col+j]="O";
								}	
							}
							else if(validity==false){
								invalidWhiteMoves++;
							}
						}
					}
				}
				if ((col-1>0)&&col>0&&(row+1<board.GetLength(0))&&board[row+1,col-1]=="X")
				{
					for (int i=1;row+i<board.GetLength(0)&&col-i>=0&&(board[row+i,col-i]!=" ");i++){
						if (board[row+i,col-i]=="O"){
							validity=true;
							if (validity){
								
								for (int j=0;col-j>=col-i;j++){
									board[row+j,col-j]="O";
								}
							}
							else if(validity==false){
								invalidWhiteMoves++;
							}
						}
					}
				}
				if ((row+1<board.GetLength(0))&&board[row+1,col]=="X")
				{
					for (int i=1;row+i<board.GetLength(0)&&(board[row+i,col]!=" ");i++){
							if (board[row+i,col]=="O"){
								validity=true;
								if (validity){
									
									for (int j=0;row+j<=row+i;j++){
									board[row+j,col]="O";
									}
								}
								else if(validity==false){
								invalidWhiteMoves++;
								}
							}
					}
				}
				if ((row+1<board.GetLength(0))&&(col+1<board.GetLength(1))&& board[row+1,col+1]=="X")
				{
					for (int i=1;col+i<board.GetLength(1)&&row+i<board.GetLength(0)&&(board[row+i,col+i]!=" ");i++){
						if (board[row+i,col+i]=="O"){
							validity=true;
							if (validity){
								for (int j=0;col-j>=col-i;j++){
									board[row+j,col+j]="O";
								}
							}
							else if(validity==false){
								invalidWhiteMoves++;
							}
						}
					}
				}
			}
			else if (turn=="black"){
				if (row>0&&col>0&&board[row-1,col-1]=="O")
				{
					for (int i=1;row-i>=0&&col-i>=0&&(board[row-i,col-i]!=" ");i++){
						WriteLine ("i is "+i);
						if (board[row-i,col-i]=="X"){
							validity=true;
							if (validity){
								for (int j=0;row-j>=row-i;j++){
									board[row-j,col-j]="X";
								}
							}
							else if(validity==false){
								invalidBlackMoves++;
							}
						}
					}
				}

				if (row>0&&board[row-1,col]=="O")
				{
					for (int i=1;row-i>0&&(board[row-i,col]!=" ");i++){
						if (board[row-i,col]=="X"){
							validity=true;
							if (validity){
								for (int j=0;row-j>=row-i;j++){
									board[row-j,col]="X";
								}
							}
							else if(validity==false){
								invalidBlackMoves++;
							}
						}
					}
				}
				if ((col+1<board.GetLength(1))&&row>0&&board[row-1,col+1]=="O")
				{
					for (int i=1;(col+i<board.GetLength(1))&&(row-i>=0)&&(board[row-i,col+i]!=" ");i++){
						if (board[row-i,col+i]=="X"){
							validity=true;
							if (validity){
								
								for (int j=0;row-j>=row-i;j++){
									board[row-j,col+j]="X";
								}
								
							}
							else if(validity==false){
								invalidBlackMoves++;
							}
						}
					}
				}
				if (col>0&&board[row,col-1]=="O")
				{
					for (int i=1;col-i>=0&&(board[row,col-i]!=" ");i++){
						if (board[row,col-i]=="X"){
							validity=true;
							if (validity){
								for (int j=0;col-j>=col-i;j++){
									board[row,col-j]="X";
								}
							}
							else if(validity==false){
								invalidBlackMoves++;
							}
						}
					}
				}
				if ((col+1<board.GetLength(1))&&board[row,col+1]=="O")
				{
					for (int i=1;col+i<board.GetLength(1)&&(board[row,col+i]!=(" "));i++){
						if (board[row,col+i]=="X"){
							validity=true;
							if (validity){
								
								for (int j=0;col+j<=col+i;j++){
									board[row,col+j]="X";
								}
								
							}
							else if(validity==false){
								invalidBlackMoves++;
							}
						}
					}
				}
				if (col>0&&(row+1<board.GetLength(0))&&board[row+1,col-1]=="O")
				{
					for (int i=1;row+i<board.GetLength(0)&&col-i>=0&&(board [row+i,col-i]!=" ");i++){
						if (board[row+i,col-i]=="X"){
							validity=true;
							if (validity){
								
								for (int j=0;col-j>=col-i;j++){
									board[row+j,col-j]="X";
								}
								
							}
							else if(validity==false){
								invalidBlackMoves++;
							}
						}
					}
				}
				if ((row+1<board.GetLength(0))&&board[row+1,col]=="O")
				{
					for (int i=1;row+i<board.GetLength(0)&&(board[row+i,col]!=" ");i++){
						if (board[row+i,col]=="X"){
							validity=true;
							if (validity){
								
								for (int j=0;row+j<=row+i;j++){
									board[row+j,col]="X";
								}
								
							}
							else if(validity==false){
								invalidBlackMoves++;
							}
						}
					}
				}
				if ((row+1<board.GetLength(0))&&(col+1<board.GetLength(1))&&board[row+1,col+1]=="O")
				{
					for (int i=1;(row+i<board.GetLength(0))&&(col+i<board.GetLength(1))&&(board[row+i,col+i]!=" ");i++){
						if (board[row+i,col+i]=="X"){
							validity=true;
							if (validity){
								
								for (int j=0;col-j>=col-i;j++){
									board[row+j,col+j]="X";
								}
								
							}
							else if(validity==false){
								invalidBlackMoves++;
							}
						}
					}
				}
			}
			else {
				validity=true;
				WriteLine ("valid");
			}
			return validity; 
		}
		public static void DisplayScore(string[,]board){
			int score;
			int blackPoint=0;
			int whitePoint=0;
			for (int i=0;i<board.GetLength(0);i++){
				for (int j=0;j<board.GetLength(1);j++){
					if (board[i,j]=="X"){
						blackPoint=blackPoint+1;
						}
					if (board[i,j]=="O"){
						whitePoint=whitePoint+1;
						}
				}
			}
			WriteLine ("White: "+whitePoint+"      Black: "+blackPoint);
			WriteLine("");
		}
		public static bool GameEndCheck(int totalMoves,string[,]board){
			int filledSpaces=0;
			bool gameCheck=false;
			for (int i=0;i<board.GetLength(0);i++){
				for (int j=0;j<board.GetLength(1);j++){
					if (board[i,j]=="O"||board[i,j]=="X"){
						filledSpaces++;
					}
				}
			}
			if (filledSpaces<totalMoves){
				gameCheck=false;
			}
			else if (filledSpaces>=totalMoves){
				gameCheck=true;
			}
			/*prevInvalidWhiteMoves=invalidWhiteMoves;
			
			if (prevInvalidWhiteMoves==invalidBlackMoves){
				gameCheck=true;
			}		*/
			return gameCheck;
		}
		
		public static void GameResult(string[,]board){
			DisplayBoard(board);
			DisplayScore(board);
			int blackPointFinal=0;
			int whitePointFinal=0;
			for (int i=0;i<board.GetLength(0);i++){
				for (int j=0;j<board.GetLength(1);j++){
					if (board[i,j]=="X"){
						blackPointFinal=blackPointFinal+1;
						}
					if (board[i,j]=="O"){
						whitePointFinal=whitePointFinal+1;
						}
				}
			}
			if (whitePointFinal>blackPointFinal){
				WriteLine (player1+" (white) is the winner!!!");
			}
			else if (whitePointFinal==blackPointFinal){
				WriteLine (player2+" (black) and "+player1+" (white) have tied");
			}
			else {
				WriteLine (player2+" (black) is the winner!!!");
			}
			Environment.Exit(0);
		}
    }
}
/* pa1 rubric

Marked by: Taghi Badakhshan

      Information collection and initialization
[2 / 2] get players' names and create two players
[1 / 1] get board size

      Game flow
[2 / 2] count the number of discs of each player to compute the scores
[3 / 3] display the game board and the score of each player after each move in an appropriate style
[1 / 1] change turn to the other player
[1 / 1] move of 'quit' ends the game
[1 / 1] move of 'skip' switches player
[1 / 1] display the winner

      Making a move
[2 / 2] convert move from a string to integer indexes
[2 / 2] check if the chosen move is within the board
[2 / 2] check if the chosen move is not occupied
[5 / 5] check 8 directions of the chosen move
[3 / 3] make the move and flip disks of the opponent if applicable

      Style
[2 / 2] messages to the player other than game board and score (e.g., Asking player name, etc.)
[1 / 2] variable or method names
[2 / 2] comments as needed
[0 / 2] consistent spacing, indenting, clarity

      Bonus*
[0 / 1] implement the rules allow for skill mismatch
[0 / 1] lift the restriction to two players
[0 / 1] automated player
[0 / 1] animation (seeing the disc flip individually instead of all at once)



Feedback:
* Line 747  The variable 'score' is declared but never used
* You have copied and paste multiple lines. You could use some functions to reduce the lines of code and make it more readable
*/

// Grade: 31/34
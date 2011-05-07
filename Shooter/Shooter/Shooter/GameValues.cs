using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{

	public class GameValues
	{
		private static GameValues instance;
		public static float BASE_WIDTH = 1920;
		public static float BASE_HEIGHT = 1080;
		public static float WIDTH = 1920; //1024;
		public static float HEIGHT = 1080; //768;
		public float SCALEX = HEIGHT / BASE_HEIGHT;
		public float SCALEY = WIDTH / BASE_WIDTH;
		
		public Vector2 playerPos = new Vector2(0, 0);
		public int playerScore = 0;
		public bool gamePaused = false;

		public List<Shot> EnemyWeapons = new List<Shot>();

		private GameValues() 
		{ 
		}

		public static GameValues Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new GameValues();
				}
				return instance;
			}
		}

		public Vector2 PlayerPos
		{
			get { return playerPos; }
			set { playerPos = value; }
		}
	}
}

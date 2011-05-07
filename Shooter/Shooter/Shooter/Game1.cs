using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Shooter
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		public SpriteBatch SpriteBatch
		{
			get { return spriteBatch; }
		}

		private static Random random = new Random();
		public static Random Random
		{
			get { return random; }
		}

		int WIDTH = (int)GameValues.WIDTH;
		int HEIGHT = (int)GameValues.HEIGHT;
		float SCALE = 1.0f;
		int LEFT = -1;
		int RIGHT = 1;
		int UP = -1;
		int DOWN = 1;
		int LEVEL = 1;
		bool gameOver = false;

		SpriteFont gameOverFont;
		SpriteFont scoreFont;
		Level Level;
		Player Player;
		List<Shot> PlayerWeapons = new List<Shot>();
		List<Element> PlayerLifes = new List<Element>();
		List<Element> LevelElements = new List<Element>();
		List<Enemy> LevelEnemies = new List<Enemy>();
		float AVG_SCALE = (GameValues.Instance.SCALEX + GameValues.Instance.SCALEY) / 2;

		SoundEffect sound_explosion;
		SoundEffect sound_laser;
		Song song;
		ExplosionParticleSystem explosion;
		StarField starField;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			InitGraphicsMode(WIDTH, HEIGHT, true);
			explosion = new ExplosionParticleSystem(this, 1);
			Components.Add(explosion);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}



		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			LoadGameLevel(1);

			// TODO: use this.Content to load your game content here
			sound_explosion = this.Content.Load<SoundEffect>("sound_explosion");
			sound_laser = this.Content.Load<SoundEffect>("sound_laser");
			song = this.Content.Load<Song>("theme_v2");
			gameOverFont = this.Content.Load<SpriteFont>("GameOverFont");
			scoreFont = this.Content.Load<SpriteFont>("ScoreFont");

			Texture2D player_tex = this.Content.Load<Texture2D>("fighter");
			Texture2D energyShot_tex = this.Content.Load<Texture2D>("energy_shot");
			Texture2D fighter_sil_tex = this.Content.Load<Texture2D>("fighter_silhouette");

			//create HUD
			Element playerLife1 = new Element(fighter_sil_tex, (5 * GameValues.Instance.SCALEX), 5);
			Element playerLife2 = new Element(fighter_sil_tex, playerLife1.m_pos.X + fighter_sil_tex.Width + (5 * GameValues.Instance.SCALEX), 5);
			Element playerLife3 = new Element(fighter_sil_tex, playerLife2.m_pos.X + fighter_sil_tex.Width + (5 * GameValues.Instance.SCALEX), 5);
			PlayerLifes.Add(playerLife1);
			PlayerLifes.Add(playerLife2);
			PlayerLifes.Add(playerLife3);

			//create player elements
			Shot playerShot = new Shot(energyShot_tex, 0, 30);
			InitPlayerWeapons(playerShot);
			Player = new Player(player_tex, PlayerWeapons, WIDTH / 4, HEIGHT / 2);

			//star field
			List<Texture2D> starField_textures = new List<Texture2D>();
			starField_textures.Add(Content.Load<Texture2D>("star_dot"));
			starField = new StarField(starField_textures, new Vector2(500, 300), SCALE);

			MediaPlayer.Play(song);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyboard = Keyboard.GetState();
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (keyboard.IsKeyDown(Keys.Escape)))
			{
				this.Exit();
			}
			if (Player.m_lifes == 0)
			{
				gameOver = true;
			}

			if (!GameValues.Instance.gamePaused)
			{
				Player.Reset();

				if (Player.m_active)
				{
					if (keyboard.IsKeyDown(Keys.P))
					{
						if (!GameValues.Instance.gamePaused)
						{
							GameValues.Instance.gamePaused = true;
						}
						else
						{
							GameValues.Instance.gamePaused = false;
						}
					}
					if ((keyboard.IsKeyDown(Keys.Up)) || (keyboard.IsKeyDown(Keys.W)))
					{
						Player.m_up = true;
					}
					if ((keyboard.IsKeyDown(Keys.Down)) || (keyboard.IsKeyDown(Keys.S)))
					{
						Player.m_down = true;
					}
					if ((keyboard.IsKeyDown(Keys.Left)) || (keyboard.IsKeyDown(Keys.A)))
					{
						Player.m_left = true;
					}
					if ((keyboard.IsKeyDown(Keys.Right)) || (keyboard.IsKeyDown(Keys.D)))
					{
						Player.m_right = true;
					}
					if (keyboard.IsKeyDown(Keys.Space) || (keyboard.IsKeyDown(Keys.RightControl)))
					{
						Player.Fire();
					}
				}

				// TODO: Add your update logic here
				Level.Update();
				int i = 0;
				for (i = 0; i < PlayerWeapons.Count; i++)
				{
					if (PlayerWeapons[i].m_active)
					{
						PlayerWeapons[i].Update();
					}
				}
				for (i = 0; i < GameValues.Instance.EnemyWeapons.Count; i++)
				{
					if (GameValues.Instance.EnemyWeapons[i].m_active)
					{
						GameValues.Instance.EnemyWeapons[i].Update();
					}
				}

				Player.Update();
				starField.Update();

			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here
			starField.Draw(spriteBatch);
			spriteBatch.Begin();

			spriteBatch.Draw(Player.m_image, Player.m_pos, new Rectangle(0, 0, Player.m_image.Width, Player.m_image.Height), Color.White, 0.0f, Vector2.Zero, AVG_SCALE, SpriteEffects.None, 0);
			DrawEnemyWeapons(spriteBatch);
			DrawPlayerWeapons(spriteBatch);
			DrawLevelElements(spriteBatch);
			DrawLevelEnemies(spriteBatch);
			DrawHUD(spriteBatch);

			if (gameOver)
			{
				spriteBatch.DrawString(gameOverFont, "Game Over", new Vector2((WIDTH / 2) - 100, (HEIGHT / 2) - 100), new Color(28, 117, 188));
			}

			spriteBatch.End();
			UpdateCollisions();
			base.Draw(gameTime);
		}

		public void DrawLevelElements(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < LevelElements.Count; i++)
			{
				if (LevelElements[i].m_active)
				{
					LevelElements[i].m_pos.X = LevelElements[i].m_pos.X * SCALE;
					LevelElements[i].m_pos.Y = LevelElements[i].m_pos.Y * SCALE;
					spriteBatch.Draw(LevelElements[i].m_image, LevelElements[i].m_pos, new Rectangle(0, 0, LevelElements[i].m_image.Width, LevelElements[i].m_image.Height), Color.White,
						LevelElements[i].m_rotation, Vector2.Zero, AVG_SCALE, SpriteEffects.None, 0);
				}
			}
		}

		public void DrawLevelEnemies(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < LevelEnemies.Count; i++)
			{
				if (LevelEnemies[i].m_active)
				{
					LevelEnemies[i].m_pos.X = LevelEnemies[i].m_pos.X * SCALE;
					LevelEnemies[i].m_pos.Y = LevelEnemies[i].m_pos.Y * SCALE;
					spriteBatch.Draw(LevelEnemies[i].m_image, LevelEnemies[i].m_pos, new Rectangle(0, 0, LevelEnemies[i].m_image.Width, LevelEnemies[i].m_image.Height), Color.White,
						LevelEnemies[i].m_rotation, Vector2.Zero, AVG_SCALE, SpriteEffects.None, 0);
				}
			}
		}

		public void DrawPlayerWeapons(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < PlayerWeapons.Count; i++)
			{
				if (PlayerWeapons[i].m_active)
				{
					PlayerWeapons[i].m_pos.X = PlayerWeapons[i].m_pos.X * SCALE;
					PlayerWeapons[i].m_pos.Y = PlayerWeapons[i].m_pos.Y * SCALE;
					spriteBatch.Draw(PlayerWeapons[i].m_image, PlayerWeapons[i].m_pos, new Rectangle(0, 0, PlayerWeapons[i].m_image.Width, PlayerWeapons[i].m_image.Height), Color.White,
						PlayerWeapons[i].m_rotation, Vector2.Zero, AVG_SCALE, SpriteEffects.None, 0);
				}
			}
		}

		public void DrawEnemyWeapons(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < GameValues.Instance.EnemyWeapons.Count; i++)
			{
				if (GameValues.Instance.EnemyWeapons[i].m_active)
				{
					GameValues.Instance.EnemyWeapons[i].m_pos.X = GameValues.Instance.EnemyWeapons[i].m_pos.X * SCALE;
					GameValues.Instance.EnemyWeapons[i].m_pos.Y = GameValues.Instance.EnemyWeapons[i].m_pos.Y * SCALE;
					spriteBatch.Draw(GameValues.Instance.EnemyWeapons[i].m_image, GameValues.Instance.EnemyWeapons[i].m_pos, new Rectangle(0, 0, GameValues.Instance.EnemyWeapons[i].m_image.Width, GameValues.Instance.EnemyWeapons[i].m_image.Height), Color.White,
						GameValues.Instance.EnemyWeapons[i].m_rotation + MathHelper.ToRadians(180), Vector2.Zero, AVG_SCALE, SpriteEffects.None, 0);
				}
			}
		}

		private void DrawHUD(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(scoreFont, "SCORE:  " + GameValues.Instance.playerScore.ToString(), new Vector2((WIDTH / 5) + 40, 0), new Color(28, 117, 188));
			for (int i = 0; i < PlayerLifes.Count; i++)
			{
				spriteBatch.Draw(PlayerLifes[i].m_image, PlayerLifes[i].m_pos, new Rectangle(0, 0, PlayerLifes[i].m_image.Width, PlayerLifes[i].m_image.Height), Color.White,
					0.0f, Vector2.Zero, AVG_SCALE, SpriteEffects.None, 0);
				if (PlayerLifes.Count > Player.m_lifes)
				{
					PlayerLifes.RemoveAt(Player.m_lifes);
					break;
				}
			}
		}

		public void UpdateCollisions()
		{
			int i = 0;
			int j = 0;

			//check enemies collision with player weapons and player colliding with enemies
			for (i = 0; i < LevelEnemies.Count; i++)
			{
				if (LevelEnemies[i].m_active)	//if enemy is active
				{
					for (j = 0; j < PlayerWeapons.Count; j++)
					{
						if (PlayerWeapons[j].m_active)	//if player weapon is active
						{
							//if enemy collides with player weapon...
							if (Element.IntersectPixels(LevelEnemies[i], PlayerWeapons[j]))
							{
								PlayerWeapons[j].Reset();
								if (--LevelEnemies[i].m_curHealth == 0)
								{
									DrawExplosion(LevelEnemies[i].m_center);
									GameValues.Instance.playerScore += LevelEnemies[i].m_points;
									LevelEnemies[i].m_finished = true;
								}

							}
						}
						if (Player.m_active && !Player.m_invulnerable)	//if player is active and not invulnerable
						{
							//if player collides with enemy...
							if (Element.IntersectPixels(LevelEnemies[i], Player))
							{
								DrawExplosion(Player.m_center);
								Player.Explode();
							}
						}
					}
				}
			}

			//check player collision with enemy weapons
			for (i = 0; i < GameValues.Instance.EnemyWeapons.Count; i++)
			{
				if (GameValues.Instance.EnemyWeapons[i].m_active && (Player.m_active && !Player.m_invulnerable))
				{
					if (Element.IntersectPixels(Player, GameValues.Instance.EnemyWeapons[i]))
					{
						DrawExplosion(Player.m_center);
						Player.Explode();
					}
				}
			}

			//check player collision with non-enemies level elements (the environment)
			for (i = 0; i < LevelElements.Count; i++)
			{
				if (LevelElements[i].m_active && (Player.m_active && !Player.m_invulnerable))
				{
					if (Element.IntersectPixels(Player, LevelElements[i]))
					{
						DrawExplosion(Player.m_center);
						Player.Explode();
					}
				}
			}
		}

		public void InitPlayerWeapons(Shot energy_shot)
		{
			int i = 0;
			while (i < 5)	//HARD CODED!!!!
			{
				PlayerWeapons.Add(new Shot(energy_shot));
				i++;
			}
		}

		private void LoadGameLevel(int level)
		{
			XmlDocument xml = new XmlDocument();
			xml.Load("D:\\VS 2010 Projects\\Shooter\\Shooter\\ShooterContent\\Levels\\" + level.ToString() + ".xml");
			XmlNode root = xml.FirstChild;
			XmlNode waveNode;
			XmlNode formationNode;
			XmlNode enemyNode;
			XmlAttributeCollection attributes;

			Level = new Level();				//level contains list of waves
			Wave nextWave;						//wave contains list of formations
			Formation nextFormation;			//formation contains list of enemies
			Enemy nextEnemy;					//enemies will die horrible deaths
			AttackPattern nextAttackPattern;	//attackPattern defines enemy movement between start and end positions
			ShotPattern nextShotPattern;		//defines enemy bullets per shot and how those bullets move
			Texture2D shotImage;				//duh

			float waveDuration = 0;
			float formationTrigger = 0;

			string attackPatternID = "";	//name identifier for AttackPattern
			string shotPatternID = "";		//name identifier for ShotPattern
			string enemyType = "";			//tells which texture will be drawn for enemy
			string enemyShot = "";			//tells which texture will be drawn for enemy shot
			float enemyHealth = 0;			//duh
			float startX = 0;				//Start and End positions the enemy ship will use
			float startY = 0;
			float endX = 0;
			float endY = 0;
			int fireCount = 0;				//number of times the enemy will fire it's shotPattern
			float fireRate = 0;				//how frequently the enemy will fire another shotPattern
			int shotSpeed = 0;				//how fast each shot will move

			List<ShotPattern> shotPatterns;	//list of shotPatterns that will be passed to enemy
			int count = 0;					//counter for adding ShotPatterns

			if (root.HasChildNodes)
			{
				for (int i = 0; i < root.ChildNodes.Count; i++)	//waves
				{
					waveNode = root.ChildNodes[i];
					attributes = waveNode.Attributes;
					waveDuration = Convert.ToSingle(attributes.GetNamedItem("d").Value);
					nextWave = new Wave(waveDuration);	//Create new wave

					if (waveNode.HasChildNodes)
					{
						for (int j = 0; j < waveNode.ChildNodes.Count; j++)	//formations
						{
							formationNode = waveNode.ChildNodes[j];
							attributes = formationNode.Attributes;
							formationTrigger = Convert.ToSingle(attributes.GetNamedItem("d").Value);

							nextFormation = new Formation(formationTrigger);
							if (formationNode.HasChildNodes)
							{
								for (int k = 0; k < formationNode.ChildNodes.Count; k++)	//enemies
								{
									enemyNode = formationNode.ChildNodes[k];
									attributes = enemyNode.Attributes;
									enemyType = attributes.GetNamedItem("type").Value;
									enemyShot = attributes.GetNamedItem("shot").Value;
									try
									{
										enemyHealth = Convert.ToInt32(attributes.GetNamedItem("health").Value);
									}
									catch (FormatException e)
									{
										Console.WriteLine("Enemy Health: Input string is not a sequence of digits.");
									}
									catch (OverflowException e)
									{
										Console.WriteLine("Enemy Health: The number cannot fit in an Int32.");
									}

									try
									{
										startX = Convert.ToInt32(attributes.GetNamedItem("startX").Value);
									}
									catch (FormatException e)
									{
										Console.WriteLine("Enemy Starting X: Input string is not a sequence of digits.");
									}
									catch (OverflowException e)
									{
										Console.WriteLine("Enemy Starting X: The number cannot fit in an Int32.");
									}

									try
									{
										startY = Convert.ToInt32(attributes.GetNamedItem("startY").Value);
									}
									catch (FormatException e)
									{
										Console.WriteLine("Enemy Starting Y: Input string is not a sequence of digits.");
									}
									catch (OverflowException e)
									{
										Console.WriteLine("Enemy Starting Y: The number cannot fit in an Int32.");
									}

									try
									{
										endX = Convert.ToInt32(attributes.GetNamedItem("endX").Value);
									}
									catch (FormatException e)
									{
										Console.WriteLine("Enemy Starting X: Input string is not a sequence of digits.");
									}
									catch (OverflowException e)
									{
										Console.WriteLine("Enemy Starting X: The number cannot fit in an Int32.");
									}

									try
									{
										endY = Convert.ToInt32(attributes.GetNamedItem("endY").Value);
									}
									catch (FormatException e)
									{
										Console.WriteLine("Enemy Starting Y: Input string is not a sequence of digits.");
									}
									catch (OverflowException e)
									{
										Console.WriteLine("Enemy Starting Y: The number cannot fit in an Int32.");
									}

									try
									{
										shotSpeed = Convert.ToInt32(attributes.GetNamedItem("shotSpeed").Value);
									}
									catch (FormatException e)
									{
										Console.WriteLine("Enemy Shot Speed: Input string is not a sequence of digits.");
									}
									catch (OverflowException e)
									{
										Console.WriteLine("Enemy Shot Speed: The number cannot fit in an Int32.");
									}

									try
									{
										fireCount = Convert.ToInt32(attributes.GetNamedItem("fireCount").Value);
									}
									catch (FormatException e)
									{
										Console.WriteLine("Enemy Shot Freqency: Input string is not a sequence of digits.");
									}
									catch (OverflowException e)
									{
										Console.WriteLine("Enemy Shot Freqency: The number cannot fit in an Int32.");
									}

									try
									{
										fireRate = Convert.ToSingle(attributes.GetNamedItem("fireRate").Value);
									}
									catch (FormatException e)
									{
										Console.WriteLine("Enemy Shot Freqency: Input string is not a sequence of digits.");
									}
									catch (OverflowException e)
									{
										Console.WriteLine("Enemy Shot Freqency: The number cannot fit in a float.");
									}

									//enemy attack pattern
									nextAttackPattern = new AttackPattern();
									attackPatternID = attributes.GetNamedItem("ap").Value;
									nextAttackPattern.SetID(attackPatternID);
									nextAttackPattern.SetEndPosition(endX, endY);
									nextAttackPattern.SetStartPosition(startX, startY);

									if ((nextAttackPattern = PopulateAttackPattern(nextAttackPattern)) == null)			//Load AttackPattern moveList
									{
										Console.WriteLine("Failed to load attack pattern " + attackPatternID.ToString());
									}

									//enemy shot pattern

									shotPatternID = attributes.GetNamedItem("sp").Value;
									shotPatterns = new List<ShotPattern>();		//only one shotPattern List per enemy, so new outside while loop
									count = 0;
									while (count < fireCount)
									{
										nextShotPattern = new ShotPattern(shotPatternID, shotSpeed);		//we potentially want multiple ShotPattern objects per enemy, so new in while loop
										shotImage = this.Content.Load<Texture2D>(enemyShot);

										//Create the appropriate number of shotPatterns for the enemy

										if ((nextShotPattern = PopulateShotPattern(nextShotPattern, shotImage)) == null)			//populate ShotPattern's shotList
										{
											Console.WriteLine("Failed to load shot pattern " + shotPatternID.ToString());
										}
										shotPatterns.Add(nextShotPattern);
										count++;
									}

									//ContentManager should only load textures if they aren't already cached
									nextEnemy = new Enemy(this.Content.Load<Texture2D>(enemyType), Convert.ToInt32(enemyHealth), nextAttackPattern, shotPatterns, fireCount, fireRate);
									LevelEnemies.Add(nextEnemy);
									nextFormation.AddEnemy(nextEnemy);
								}

								nextWave.AddFormation(nextFormation);
							}
							else
							{
								Console.WriteLine("Empty formation found");
							}
						}

						Level.AddWave(nextWave);
					}
					else
					{
						Console.WriteLine("Empty wave found");
					}
				}
			}
			else
			{
				Console.WriteLine("root node: no children found");
			}
		}


		public AttackPattern PopulateAttackPattern(AttackPattern pattern)
		{
			XmlDocument xml = new XmlDocument();
			xml.Load("D:\\VS 2010 Projects\\Shooter\\Shooter\\ShooterContent\\AttackPatterns\\" + pattern.m_id + ".xml");
			XmlNode root = xml.FirstChild;
			XmlNode child;
			XmlAttributeCollection attributes;

			float endX = 0;
			float endY = 0;

			float moveX = 0;
			float moveY = 0;
			float speedX = 0;
			float speedY = 0;
			float waitTime = 0;

			float exitSpeed = 0;
			if (root == null)
			{
				return null;
			}

			attributes = root.Attributes;
			try
			{
				exitSpeed = Convert.ToInt32(attributes.GetNamedItem("exitSpeed").Value);
			}
			catch (FormatException e)
			{
				Console.WriteLine("AttackPattern exitSpeed: Input string is not a sequence of digits.");
			}
			catch (OverflowException e)
			{
				Console.WriteLine("AttackPattern exitSpeed: The number cannot fit in a float.");
			}

			pattern.SetExitSpeed(exitSpeed);
			if (root.HasChildNodes)
			{
				for (int j = 0; j < root.ChildNodes.Count; j++)
				{
					child = root.ChildNodes[j];
					attributes = child.Attributes;
					try
					{
						moveX = Convert.ToInt32(attributes.GetNamedItem("moveX").Value);
					}
					catch (FormatException e)
					{
						Console.WriteLine("AttackPattern moveX: Input string is not a sequence of digits.");
					}
					catch (OverflowException e)
					{
						Console.WriteLine("AttackPattern moveX: The number cannot fit in a float.");
					}

					try
					{
						moveY = Convert.ToInt32(attributes.GetNamedItem("moveY").Value);
					}
					catch (FormatException e)
					{
						Console.WriteLine("AttackPattern moveY: Input string is not a sequence of digits.");
					}
					catch (OverflowException e)
					{
						Console.WriteLine("AttackPattern moveY: The number cannot fit in a float.");
					}

					try
					{
						speedX = Convert.ToInt32(attributes.GetNamedItem("speedX").Value);
					}
					catch (FormatException e)
					{
						Console.WriteLine("AttackPattern speedX: Input string is not a sequence of digits.");
					}
					catch (OverflowException e)
					{
						Console.WriteLine("AttackPattern speedX: The number cannot fit in a float.");
					}

					try
					{
						speedY = Convert.ToInt32(attributes.GetNamedItem("speedY").Value);
					}
					catch (FormatException e)
					{
						Console.WriteLine("AttackPattern speedY: Input string is not a sequence of digits.");
					}
					catch (OverflowException e)
					{
						Console.WriteLine("AttackPattern speedY: The number cannot fit in a float.");
					}

					try
					{
						waitTime = Convert.ToSingle(attributes.GetNamedItem("wait").Value);
					}
					catch (FormatException e)
					{
						Console.WriteLine("AttackPattern wait: Input string is not a sequence of digits.");
					}
					catch (OverflowException e)
					{
						Console.WriteLine("AttackPattern wait: The number cannot fit in a float.");
					}

					pattern.AddManuver(pattern.m_startX + moveX, pattern.m_startY + moveY, speedX, speedY, waitTime);
				}
			}
			else
			{
				Console.WriteLine("AttackPattern root: could not find children");
			}
			return pattern;
		}


		public ShotPattern PopulateShotPattern(ShotPattern pattern, Texture2D image)
		{
			XmlDocument xml = new XmlDocument();
			xml.Load("D:\\VS 2010 Projects\\Shooter\\Shooter\\ShooterContent\\ShotPatterns\\" + pattern.m_id + ".xml");
			XmlNode root = xml.FirstChild;
			XmlNode child;
			XmlAttributeCollection attributes;

			Shot nextShot;
			int numShots = 0;
			int angle = 0;

			if (root == null)
			{
				return null;
			}
			attributes = root.Attributes;

			try
			{
				numShots = Convert.ToInt32(attributes.GetNamedItem("numShots").Value);
			}
			catch (FormatException e)
			{
				Console.WriteLine("ShotPattern numShots: Input string is not a sequence of digits.");
			}
			catch (OverflowException e)
			{
				Console.WriteLine("ShotPattern numShots: The number cannot fit in an Int32.");
			}

			if (root.HasChildNodes)
			{
				for (int j = 0; j < root.ChildNodes.Count; j++)
				{
					child = root.ChildNodes[j];
					attributes = child.Attributes;
					try
					{
						angle = Convert.ToInt32(attributes.GetNamedItem("angle").Value);
					}
					catch (FormatException e)
					{
						Console.WriteLine("ShotPattern angle: Input string is not a sequence of digits.");
					}
					catch (OverflowException e)
					{
						Console.WriteLine("ShotPattern angle: The number cannot fit in an Int32.");
					}
					nextShot = new Shot(image, angle, pattern.m_speed);
					GameValues.Instance.EnemyWeapons.Add(nextShot);
					pattern.AddShot(nextShot);
				}
			}
			return pattern;
		}


		private bool InitGraphicsMode(int iWidth, int iHeight, bool bFullScreen)
		{
			//bFullScreen = true;
			// If we aren't using a full screen mode, the height and width of the window can
			// be set to anything equal to or smaller than the actual screen size.
			if (bFullScreen == false)
			{
				if ((iWidth <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
					&& (iHeight <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
				{
					graphics.PreferredBackBufferWidth = iWidth;
					graphics.PreferredBackBufferHeight = iHeight;
					graphics.IsFullScreen = bFullScreen;
					graphics.ApplyChanges();
					return true;
				}
			}
			else
			{
				// If we are using full screen mode, we should check to make sure that the display
				// adapter can handle the video mode we are trying to set.  To do this, we will
				// iterate thorugh the display modes supported by the adapter and check them against
				// the mode we want to set.
				foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
				{
					// Check the width and height of each mode against the passed values
					if ((dm.Width == iWidth) && (dm.Height == iHeight))
					{
						// The mode is supported, so set the buffer formats, apply changes and return
						graphics.PreferredBackBufferWidth = iWidth;
						graphics.PreferredBackBufferHeight = iHeight;
						graphics.IsFullScreen = bFullScreen;
						graphics.ApplyChanges();
						return true;
					}
				}
			}
			return false;
		}

		public static float RandomBetween(float min, float max)
		{
			return min + (float)random.NextDouble() * (max - min);
		}

		public void DrawExplosion(Vector2 pos)
		{
			explosion.AddParticles(pos);
			sound_explosion.Play();
		}

	}
}


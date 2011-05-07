using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;

namespace Shooter
{
	class Enemy : Element
	{

		//members
		System.Timers.Timer m_shotTimer = new System.Timers.Timer();
		List<ShotPattern> m_shotPatterns = new List<ShotPattern>();
		Vector2 m_exitPos;
		public AttackPattern m_attackPattern;
		public bool m_hit = false;
		public float m_speed = 8;
		public int m_curHealth = 0;
		public int m_points = 0;
		public float m_fireRate = 0;
		public int m_fireCount = 0;
		private int m_shotIndex = 0;

		//constructor
		public Enemy(Texture2D texture, int health, AttackPattern ap, List<ShotPattern> sp_list, int fireCount, float fireRate)
		{
			m_image = texture;
			m_curHealth = health;
			m_points = health * 10;
			m_attackPattern = ap;
			m_shotPatterns = sp_list;
			m_fireCount = fireCount;
			m_fireRate = fireRate;

			m_textureData = new Color[m_image.Width * m_image.Height];
			m_image.GetData(m_textureData);
			m_pos.X = m_attackPattern.m_startX;
			m_pos.Y = m_attackPattern.m_startY;

			m_center.X = m_pos.X + m_image.Width / 2;
			m_center.Y = m_pos.Y + m_image.Height / 2;

			m_exitPos = new Vector2(m_attackPattern.m_endX, m_attackPattern.m_endY);
			SetFireRate();
		}

		private void SetFireRate()
		{
			if (m_fireRate > 0)
			{
				m_shotTimer.Interval = m_fireRate * 1000;
			}
			else
			{
				m_shotTimer.Interval = 2000;	//default to 2 seconds between shots, but this isn't supposed to happen
			}
			m_shotTimer.Elapsed += new ElapsedEventHandler(FireShotPattern);
		}

		//Handles enemy movement based on AttackPattern
		public override void Update()
		{
			if (m_active && !m_shotTimer.Enabled)
			{
				m_shotTimer.Enabled = true;
			}

			if (!m_attackPattern.m_patternComplete)
			{
				m_pos = m_attackPattern.Update(m_pos, m_exitPos);
			}
			else
			{
				m_active = false;
			}
			

			if (m_curHealth == 0)
			{
				Explode();
				m_active = false;
			}

			if (m_hit)
			{
				m_curHealth--;
			}
			m_center.X = m_pos.X + m_image.Width / 2;
			m_center.Y = m_pos.Y + m_image.Height / 2;
		}

		//Handles enemy fire based on ShotPattern and fireRate timer
		private void FireShotPattern(object source, ElapsedEventArgs e)
		{
			if (IsVisible())
			{
				int numPatterns = m_shotPatterns.Count;
				if (m_shotIndex < numPatterns)
				{
					m_shotPatterns[m_shotIndex].Start(m_center.X, m_center.Y - (m_image.Height * 0.1f));
					m_shotIndex++;
				}
			}

		}

		private bool IsVisible()
		{
			bool visible = false;
			if ((m_pos.Y + m_image.Height < 0) || (m_pos.X + m_image.Width < 0))
			{
				visible = false;
			}
			else if ((m_pos.Y > HEIGHT) || (m_pos.X > WIDTH))
			{
				visible = false;
			}
			else
			{
				visible = true;
			}
			return visible;
		}

		public void Explode()
		{
			GameValues.Instance.playerScore += m_points;
			Reset();
		}

		private void Reset()
		{
			m_active = false;
			m_pos.X = -1000;
			m_pos.Y = -1000;
		}
	}
}

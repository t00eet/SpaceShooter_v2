using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;

namespace Shooter
{
	class Player : Element
	{
		//members
		System.Timers.Timer respawnTimer = new System.Timers.Timer();
		System.Timers.Timer invulnerableTimer = new System.Timers.Timer();
		System.Timers.Timer shotWaitTimer = new System.Timers.Timer();

		float m_speedX = 7 * GameValues.Instance.SCALEX;
		float m_speedY = 7 * GameValues.Instance.SCALEY;
		List<Shot> m_shots;
		int m_numShots = 3;
		public int m_lifes = 3;

		public bool m_active = true;
		public bool m_invulnerable = false;
		public bool m_movement = false;
		public bool m_up = false;
		public bool m_down = false;
		public bool m_left = false;
		public bool m_right = false;
		bool m_wait = false;

		public Player(Texture2D texture, List<Shot> shots, float x, float y)
		{
			m_image = texture;
			m_pos.X = x;
			m_pos.Y = y;
			
			m_textureData = new Color[m_image.Width * m_image.Height];
			m_image.GetData(m_textureData);
			m_center.X = m_pos.X + m_image.Width / 2;
			m_center.Y = m_pos.Y + m_image.Height / 2;
			m_shots = shots;

			respawnTimer.Interval = 3000;
			respawnTimer.Elapsed += new ElapsedEventHandler(Respawn);
			invulnerableTimer.Interval = 2000;
			invulnerableTimer.Elapsed += new ElapsedEventHandler(InvulnerableOff);
			shotWaitTimer.Interval = 100;
			shotWaitTimer.Elapsed += new ElapsedEventHandler(StopWait);
		}

		public override void Update()
		{
			UpdateMovement();
			UpdateWeapons();
			UpdateHealth();
		}

		private void UpdateMovement()
		{
			CheckBounds();

			if (m_up)
			{
				m_pos.Y -= m_speedY;
			}
			if (m_down)
			{
				m_pos.Y += m_speedY;
			}
			if (m_left)
			{
				m_pos.X -= m_speedX;
			}
			if (m_right)
			{
				m_pos.X += m_speedX;
			}
			GameValues.Instance.playerPos = m_pos;
			m_center.X = m_pos.X + m_image.Width / 2;
			m_center.Y = m_pos.Y + m_image.Height / 2;
		}

		private void UpdateWeapons()
		{

		}

		private void UpdateHealth()
		{

		}

		public void Fire()
		{
			for (int i = 0; i < m_shots.Count(); i++)
			{
				if (!m_shots[i].m_fired && !m_wait)
				{
					m_shots[i].Fire((m_pos.X + m_image.Width * 0.8f), (m_pos.Y + m_image.Height * 0.35f));
					m_wait = true;
					shotWaitTimer.Enabled = true;
				}
			}
		}

		public void Explode()
		{
			if (!m_invulnerable)
			{
				m_active = false;
				m_pos.X = -1000;
				m_pos.Y = -1000;
				m_lifes--;
				if (m_lifes > 0)
				{
					respawnTimer.Enabled = true;
				}
			}
		}

		private void CheckBounds()
		{
			float newX_up = m_pos.Y - m_speedY;
			float newX_down = m_pos.Y + m_speedY;
			float newY_left = m_pos.X - m_speedX;
			float newY_right = m_pos.X + m_speedX;

			if (newX_up < 0)
			{
				m_up = false;
			}
			if (newX_down + m_image.Height > HEIGHT)
			{
				m_down = false;
			}
			if (newY_left < 0)
			{
				m_left = false;
			}
			if (newY_right + m_image.Width > WIDTH)
			{
				m_right = false;
			}
		}

		public void Reset()
		{
			m_up = false;
			m_down = false;
			m_left = false;
			m_right = false;
		}

		private void Respawn(object source, ElapsedEventArgs e)
		{
			respawnTimer.Enabled = false;       //disable respawn timer
			m_invulnerable = true;
			invulnerableTimer.Enabled = true;   //enabled invulnerable timer
			Reset();
			m_active = true;
			m_pos.X = 500;
			m_pos.Y = 300;
		}

		private void InvulnerableOff(object source, ElapsedEventArgs e)
		{
			invulnerableTimer.Enabled = false;
			m_invulnerable = false;
		}

		private void StopWait(object source, ElapsedEventArgs e)
		{
			shotWaitTimer.Enabled = false;
			m_wait = false;
		}

	}
}

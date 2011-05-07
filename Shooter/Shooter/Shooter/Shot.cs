using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;

namespace Shooter
{
	public class Shot : Element
	{
		//members
		System.Timers.Timer m_shotFrequency = new System.Timers.Timer();
		public bool m_fired = false;
		public bool m_shotComplete = false;
		public int m_angle;
		public int m_speed;

		//constructor
		public Shot(Texture2D texture, int angle, int speed)
		{
			m_image = texture;
			m_textureData = new Color[m_image.Width * m_image.Height];
			m_image.GetData(m_textureData);
			m_angle = angle;
			m_rotation = MathHelper.ToRadians(m_angle);
			m_speed = speed;
			m_center.X = m_image.Width / 2;
			m_center.Y = m_image.Height / 2;
		}

		public Shot(Shot copy_shot)
        {
			m_image = copy_shot.m_image;
			m_pos = copy_shot.m_pos;
			m_angle = copy_shot.m_angle;
			m_speed = copy_shot.m_speed;
			m_textureData = copy_shot.m_textureData;
            m_image.GetData(m_textureData);
			m_center = copy_shot.m_center;
        }

		public override void Update()
		{
			m_pos += Vector2.Transform(new Vector2((m_speed * GameValues.Instance.SCALEX), 0), Matrix.CreateRotationZ(m_rotation));
			if (!IsActive())
			{
				Reset();
			}
			m_center.X = m_pos.X + m_image.Width / 2;
			m_center.Y = m_pos.Y + m_image.Height / 2;
		}

		private bool IsActive()
		{

			if ((m_pos.Y + m_image.Height < 0) || (m_pos.X + m_image.Width < 0))
			{
				m_active = false;
				m_shotComplete = true;
			}
			else if ((m_pos.Y > HEIGHT) || (m_pos.X > WIDTH))
			{
				m_active = false;
				m_shotComplete = true;
			}
			else
			{
				m_active = true;
			}
			return m_active;
		}

		public void Fire(float x, float y)
		{
			m_pos.X = x;
			m_pos.Y = y;
			m_fired = true;
			m_active = true;
		}

		public void Reset()
		{
			m_active = false;
			m_fired = false;
			m_pos.X = -1000;
			m_pos.Y = -1000;
		}
	}
}

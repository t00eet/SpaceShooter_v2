using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
	public class ShotPattern
	{
		//members
		public List<Shot> m_shots = new List<Shot>();
		public string m_id;
		public int m_speed;
		public bool m_active = false;
		public bool m_patternComplete = false;

		//constructor
		public ShotPattern(string pattern_id, int speed)
		{
			m_id = pattern_id;
			m_speed = speed;
		}

		public ShotPattern(ShotPattern copy_shotPattern)
		{
			m_shots = copy_shotPattern.m_shots;
			m_id = copy_shotPattern.m_id;
			m_speed = copy_shotPattern.m_speed;
		}

		public void AddShot(Shot s)
		{
			m_shots.Add(s);
		}

		public void Start(float x, float y)
		{
			int completedShots = 0;
			int numShots = m_shots.Count;
			for (int i = 0; i < numShots; i++)
			{
				if (!m_shots[i].m_fired)
				{
					m_shots[i].Fire(x, y);
				}
				if (m_shots[i].m_shotComplete)
				{
					completedShots++;
				}
			}

			if (completedShots == numShots)
			{
				m_active = false;
				m_patternComplete = true;
			}
		}

		public void Reset()
		{
			m_active = false;
			m_patternComplete = false;
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Shooter
{
	class Formation
	{
		//members
		System.Timers.Timer triggerTimer = new System.Timers.Timer();
		List<Enemy> m_enemies = new List<Enemy>();
		float m_triggerTime;
		public bool m_started = false;
		public bool m_formationComplete = false;
		public bool m_queueNext = false;

		//constructor
		public Formation(float triggerTime)
		{
			m_triggerTime = triggerTime;
		}

		public void AddEnemy(Enemy e)
		{
			m_enemies.Add(e);
		}

		public void RemoveEnemy(Enemy e)
		{
			m_enemies.Remove(e);
		}

		public void Start()
		{
			m_started = true;
			ActivateEnemies();
			triggerTimer.Interval = m_triggerTime*1000;
			triggerTimer.Elapsed += new ElapsedEventHandler(QueueNextFormation);
			triggerTimer.Enabled = true;
		}

		public void Update()
		{
			int numEnemies = m_enemies.Count;
			for (int i = 0; i < numEnemies; i++)
			{
				if (m_enemies[i].m_active)
				{
					m_enemies[i].Update();
				}
				else if (m_enemies[i].m_finished)
				{
					m_enemies.Remove(m_enemies[i]);
					numEnemies--;
				}
			}
		}

		public void ActivateEnemies()
		{
			for (int i = 0; i < m_enemies.Count; i++)
			{
				m_enemies[i].m_active = true;
			}
		}

		private void QueueNextFormation(object source, ElapsedEventArgs e)
		{
			triggerTimer.Enabled = false;
			m_queueNext = true;
		}
	}
}

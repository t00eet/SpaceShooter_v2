using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Shooter
{
	class Wave
	{
		//members
		System.Timers.Timer m_waveTimer = new System.Timers.Timer();
		float m_duration;
		List<Formation> m_formations = new List<Formation>();
		public bool m_waveStarted = false;
		public bool m_waveComplete = false;
		public bool m_queueNext = false;

		//constructor
		public Wave(float duration)
		{
			m_duration = duration;
		}

		public void AddFormation(Formation f)
		{
			m_formations.Add(f);
		}

		public void RemoveFormation(Formation f)
		{
			m_formations.Remove(f);
		}

		public void Start()
		{
			m_waveStarted = true;
			m_waveTimer.Interval = m_duration*1000;
            m_waveTimer.Elapsed += new ElapsedEventHandler(QueueNextWave);
			m_waveTimer.Enabled = true;
		}

		public void Update()
		{
			int numFormations = m_formations.Count;
			if (numFormations > 0)
			{
				if (!m_formations[0].m_started)
				{
					m_formations[0].Start();
				}
			}
			for (int i = 0; i < numFormations; i++)
			{
				if (m_formations[i].m_started)
				{
					if (!m_formations[i].m_formationComplete)
					{
						m_formations[i].Update();
						if (m_formations[i].m_queueNext)
						{
							if ((i + 1) < numFormations)
							{
								if (!m_formations[i + 1].m_started)
								{
									m_formations[i + 1].Start();
								}
							}
						}
					}
					else
					{
						m_formations.Remove(m_formations[i]);
						numFormations--;
					}
				}
			}
		}

		private void QueueNextWave(object source, ElapsedEventArgs e)
		{
			m_waveTimer.Enabled = false;
			m_queueNext = true;
		}

	}
}

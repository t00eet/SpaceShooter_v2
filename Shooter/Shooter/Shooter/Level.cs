using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
	class Level
	{
		//members
		List<Wave> m_waves = new List<Wave>();
		bool m_levelStarted = false;

		public Level()
		{

		}

		public void AddWave(Wave w)
		{
			m_waves.Add(w);
		}

		public void Update()
		{
			if (!GameValues.Instance.gamePaused)
			{
				int numWaves = m_waves.Count;
				if (!m_levelStarted)
				{
					m_waves[0].Start();
					m_levelStarted = true;
				}
				for (int i = 0; i < numWaves; i++)
				{
					if (m_waves[i].m_waveStarted)		//if the wave has started..
					{
						if (!m_waves[i].m_waveComplete)	//and it's not finished..
						{
							m_waves[i].Update();		//update that wave

							if (m_waves[i].m_queueNext)	//if the next wave has been queued..
							{
								if ((i + 1) < numWaves)
								{
									if (!m_waves[i + 1].m_waveStarted)	//and that wave hasn't started yet..
									{
										m_waves[i + 1].Start();			//start the next wave
									}
								}
							}
						}
						else
						{
							m_waves.Remove(m_waves[i]);
							numWaves--;
						}
					}
				}
			}
		}



	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;

namespace Shooter
{
	public class AttackPattern
	{

		struct Manuver
		{
			public float moveX;
			public float moveY;
			public float speedX;
			public float speedY;
			public float waitTime;
		}

		//members
		System.Timers.Timer m_waitTimer = new System.Timers.Timer();
		List<Manuver> m_manuvers = new List<Manuver>();
		float m_exitSpeed = 0;
		int m_current = 0;
		bool m_targetX = false;
		bool m_targetY = false;
		bool m_wait = false;

		public float m_startX, m_startY, m_endX, m_endY = 0;
		public bool m_patternComplete = false;
		public string m_id = "";


		//constructor
		public AttackPattern()
		{
			
		}

		public Vector2 Update(Vector2 curPos, Vector2 exitPos)
		{
			m_targetX = false;
			m_targetY = false;
			if (!m_wait)
			{
				int numManuvers = m_manuvers.Count;
				if (m_current < numManuvers)
				{
					float curX = curPos.X;
					float curY = curPos.Y;
					float newX = m_manuvers[m_current].moveX * GameValues.Instance.SCALEX;
					float newY = m_manuvers[m_current].moveY * GameValues.Instance.SCALEY;
					float speedX = m_manuvers[m_current].speedX * GameValues.Instance.SCALEX;
					float speedY = m_manuvers[m_current].speedY * GameValues.Instance.SCALEY;

					float diffX = newX - curX;
					//if (curX != newX)
					if(diffX >= 10 || diffX <= -10)
					{
						if (curX > newX)
						{
							curX -= speedX;
						}
						else
						{
							curX += speedX;
						}
						curPos.X = curX;
					}
					else
					{
						m_targetX = true;
					}

					float diffY = newY - curY;
					//if (curY != newY)
					if (diffY >= 10 || diffY <= -10)
					{
						if (curY > newY)
						{
							curY -= speedY;
						}
						else
						{
							curY += speedY;
						}
						curPos.Y = curY;
					}
					else
					{
						m_targetY = true;
					}

					if (m_targetX && m_targetY)
					{
						if (m_manuvers[m_current].waitTime != 0)		//check wait time
						{
							m_wait = true;
							m_waitTimer.Interval = m_manuvers[m_current].waitTime*1000;
							m_waitTimer.Elapsed += new ElapsedEventHandler(WaitComplete);
							m_waitTimer.Enabled = true;
						}
						m_current++;
					}
				}
				else //Finished Pattern, go to exit
				{
					float curX = curPos.X;
					float curY = curPos.Y;
					float newX = exitPos.X * GameValues.Instance.SCALEX;
					float newY = exitPos.Y * GameValues.Instance.SCALEY;
					float exitSpeed = m_exitSpeed * GameValues.Instance.SCALEX;

					float diffX = newX - curX;
					//if (curX != newX)
					if (diffX >= 10 || diffX <= -10)
					{
						if (curX > newX)
						{
							curX -= exitSpeed;
						}
						else
						{
							curX += exitSpeed;
						}
						curPos.X = curX;
					}
					else
					{
						m_targetX = true;
					}

					float diffY = newY - curY;
					//if (curY != newY)
					if (diffY >= 10 || diffY <= -10)
					{
						if (curY > newY)
						{
							curY -= exitSpeed;
						}
						else
						{
							curY += exitSpeed;
						}
						curPos.Y = curY;
					}
					else
					{
						m_targetY = true;
					}

					if (m_targetX && m_targetY)
					{
						m_patternComplete = true;
					}
				}
			}
			return curPos;
		}

		private void WaitComplete(object source, ElapsedEventArgs e)
		{
			m_waitTimer.Enabled = false;
			m_wait = false;
		}

		public void AddManuver(float moveX, float moveY, float speedX, float speedY, float waitTime)
		{
			Manuver m = new Manuver();
			m.moveX = moveX;
			m.moveY = moveY;
			m.speedX = speedX;
			m.speedY = speedY;
			m.waitTime = waitTime;
			m_manuvers.Add(m);
		}

		public void SetID(string id)
		{
			m_id = id;
		}

		public void SetStartPosition(float startX, float startY)
		{
			m_startX = startX;
			m_startY = startY;
		}

		public void SetEndPosition(float endX, float endY)
		{
			m_endX = endX;
			m_endY = endY;
		}

		public void SetExitSpeed(float exitSpeed)
		{
			m_exitSpeed = exitSpeed;
		}
	}
}

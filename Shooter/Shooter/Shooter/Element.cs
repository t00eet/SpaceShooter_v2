using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
	public class Element
	{
		//members
		public float WIDTH = GameValues.WIDTH;
		public float HEIGHT = GameValues.HEIGHT;
		public float SCALEX = GameValues.Instance.SCALEX;
		public float SCALEY = GameValues.Instance.SCALEY;
		

		public Vector2 m_pos = new Vector2();
		public Vector2 m_center = new Vector2();
		public Texture2D m_image;
		public Color[] m_textureData;
		public bool m_active = false;
		public bool m_finished = false;
		public float m_rotation = 0.0f;


		public Element(Texture2D texture, float x, float y)
		{
			m_image = texture;
			m_pos.X = x;
			m_pos.Y = y;

			m_textureData = new Color[m_image.Width * m_image.Height];
			m_image.GetData(m_textureData);
		}

		public static bool IntersectPixels(Element A, Element B)
		{
			Rectangle rectangleA = new Rectangle((int)A.m_pos.X, (int)A.m_pos.Y, (int)(A.m_image.Width * GameValues.Instance.SCALEX), (int)(A.m_image.Height * GameValues.Instance.SCALEY));
			Rectangle rectangleB = new Rectangle((int)B.m_pos.X, (int)B.m_pos.Y, (int)(B.m_image.Width * GameValues.Instance.SCALEX), (int)(B.m_image.Height * GameValues.Instance.SCALEX));
			Color[] dataA = A.m_textureData;
			Color[] dataB = B.m_textureData;

			// Find the bounds of the rectangle intersection
			int top = Math.Max(rectangleA.Top, rectangleB.Top);
			int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
			int left = Math.Max(rectangleA.Left, rectangleB.Left);
			int right = Math.Min(rectangleA.Right, rectangleB.Right);

			// Check every point within the intersection bounds
			for (int y = top; y < bottom; y++)
			{
				for (int x = left; x < right; x++)
				{
					// Get the color of both pixels at this point
					Color colorA = dataA[(x - rectangleA.Left) +
										 (y - rectangleA.Top) * rectangleA.Width];
					Color colorB = dataB[(x - rectangleB.Left) +
										 (y - rectangleB.Top) * rectangleB.Width];

					// If both pixels are not completely transparent,
					if (colorA.A != 0 && colorB.A != 0)
					{
						// then an intersection has been found
						if (left < GameValues.WIDTH)
						{
							return true;
						}
					}
				}
			}

			// No intersection found
			return false;
		}

		public virtual void Update()
		{

		}
	}
}

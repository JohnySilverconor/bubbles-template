using System;
using System.Collections;
using System.Drawing;

namespace BobbleNet
{
	
	public class MovingSprites
	{

		private ArrayList sprites;
		private Rectangle gameRectangle;

		public MovingSprites(Rectangle GameRectangle)
		{
			sprites = new ArrayList();
			gameRectangle=GameRectangle;
		}

		public void AddMovingSprite(BubbleSprite ASprite)
		{
			sprites.Add(ASprite);
		}

		public void RemoveMovingSprite(BubbleSprite ASprite)
		{
			try
			{
				sprites.Remove(ASprite);
			}
			finally{}
		}

		public void MoveSprites()
		{
			ArrayList marktodelete = new ArrayList();

			// Move each sprite and mark them to be deleted is they move outside the gameRectangle
			foreach(BubbleSprite BS in sprites)
			{
				BS.Move();
				if (BS.BubbleRectangle.IntersectsWith(gameRectangle)==false)
				{
					marktodelete.Add(BS);
				}
			}

			// Remove the Sprites marked for deletion
			foreach (BubbleSprite BS in marktodelete)
			{
				sprites.Remove(BS);
			}
		}

		public ArrayList Sprites
		{
			get { return this.sprites; }
		}


	}
}

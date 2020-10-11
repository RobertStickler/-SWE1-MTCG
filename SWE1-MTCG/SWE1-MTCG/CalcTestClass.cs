using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Dynamic;

namespace MTCG
{
	public class CalcTestClass
	{
		public int Bias
		{ 
			get; 
			set; 
		}

		public int xPlusY(int x, int y)
		{
			return x + y;
		}

		public bool GreaterThanZero(int value)
        {
			//return true;
			return value > 0;
        }
	}
}



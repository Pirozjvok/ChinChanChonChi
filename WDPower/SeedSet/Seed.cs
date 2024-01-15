using System;
using System.Linq;

namespace SeedSet
{
	internal class Seed
	{
		public static readonly short seedLen = 95;

		public static readonly short seedOffset = 32;

		public static readonly short[] seed = getSeed(seedLen);

		public static readonly short[] seedR = rvsSeed(seed);

		private static readonly short keyLen = 6;

		private static short[] keyL = new short[keyLen];

		private static short[] keyA = new short[keyLen];

		private static short[] getSeed(short sdLen)
		{
			short[] array = new short[sdLen];
			short[] array2 = new short[sdLen];
			if (0 < sdLen)
			{
				if (1 < sdLen)
				{
					int num = ((0 < sdLen % 2) ? (sdLen - 1) : (sdLen - 2));
					for (int i = 0; i < sdLen; i++)
					{
						array2[i] = sdLen;
						if (0 == i % 2)
						{
							array[i] = (short)(num - i);
						}
						else
						{
							array[i] = (short)i;
						}
					}
					if (5 < sdLen)
					{
						short num2 = 0;
						int num3 = sdLen - 1;
						int num4 = sdLen / 2;
						for (int j = 0; j < 11; j++)
						{
							num2 = array[num3];
							for (int i = 0; i < num3; i++)
							{
								array[num3 - i] = array[num3 - i - 1];
							}
							array[0] = num2;
							for (int i = 0; i < num4; i++)
							{
								if (0 == i % 2)
								{
									int num5 = i + num4;
									if (num5 > sdLen)
									{
										break;
									}
									num2 = array[i];
									array[i] = array[num5];
									array[num5] = num2;
								}
							}
							int num6 = num4 / 3;
							int num7 = num4 + num6;
							for (int k = 0; k < 5; k++)
							{
								num2 = array[num6];
								for (int i = num6; i < num7; i++)
								{
									array[i] = array[i + 1];
								}
								array[num7] = num2;
							}
						}
					}
					for (int i = 0; i < sdLen; i++)
					{
						array2[i] = array[i];
					}
				}
				else
				{
					array2[0] = 0;
				}
			}
			return array2;
		}

		private static short[] rvsSeed(short[] a)
		{
			int num = 0;
			num = ((a == null) ? 1 : a.Length);
			short[] array = new short[num];
			if (num > 1)
			{
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num; j++)
					{
						if (i == a[j])
						{
							array[i] = (short)j;
							break;
						}
					}
				}
			}
			return array;
		}

		public static short[] getuserKey(short[] keyIn)
		{
			string[] array = new string[6] { "0", "0", "0", "0", "0", "0" };
			if (null != keyIn)
			{
				if (!keyIn.SequenceEqual(keyL))
				{
					keyIn.CopyTo(keyL, 0);
					for (int i = 0; i < keyIn.Length; i++)
					{
						array[i] = keyIn[i].ToString();
					}
					string[] array2 = new string[keyLen];
					array2[0] = array[0] + array[5] + array[4] + array[3] + array[2] + array[1];
					array2[1] = array[1] + array[4] + array[3] + array[0] + array[2] + array[5];
					array2[2] = array[2] + array[1] + array[4] + array[0] + array[5] + array[3];
					array2[3] = array[3] + array[0] + array[4] + array[1] + array[5] + array[2];
					array2[4] = array[4] + array[5] + array[2] + array[1] + array[3] + array[0];
					array2[5] = array[5] + array[3] + array[1] + array[2] + array[0] + array[4];
					for (int i = 0; i < keyLen; i++)
					{
						keyA[i] = (short)(Convert.ToInt32(array2[i]) % 61);
					}
				}
			}
			else
			{
				Array.Clear(keyA, 0, keyLen);
				Array.Clear(keyL, 0, keyLen);
			}
			return keyA;
		}
	}
}

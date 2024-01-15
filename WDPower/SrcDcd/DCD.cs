using System;
using SeedSet;

namespace SrcDcd
{
	internal class DCD
	{
		private static Random rndx = new Random();

		private static short splitNo = 0;

		private static readonly short seedLen = Seed.seedLen;

		private static readonly short seedOffset = Seed.seedOffset;

		private static readonly short[] seed = Seed.seed;

		private static readonly short[] seedR = Seed.seedR;

		private static short[] getuserKey(short[] KeyIn)
		{
			return Seed.getuserKey(KeyIn);
		}

		private static char cToC_seed_rvs(char cit, short ipsswd)
		{
			char result = '\0';
			int num = 0;
			int num2 = 0;
			if (cit >= seedOffset && cit < seedOffset + seedLen)
			{
				num = cit - seedOffset;
				num2 = seedR[num] - ipsswd;
				if (num2 < 0)
				{
					num2 += seedLen;
				}
				result = (char)(num2 + seedOffset);
			}
			return result;
		}

		public static string sToS_seed_rvs(string strI, short ipsswd)
		{
			string text = null;
			int i = 0;
			int num = 0;
			if (strI != null)
			{
				num = strI.Length;
				char c = '\0';
				char c2 = '\0';
				for (; i < num; i++)
				{
					c2 = strI[i];
					c = cToC_seed_rvs(c2, ipsswd);
					text += c;
				}
			}
			return text;
		}

		public static bool lToL_seed_rvs(ref string strI, ref string strR, ref int linNm, short[] Key)
		{
			bool result = false;
			bool flag = false;
			string text = strI;
			string text2 = null;
			string text3 = null;
			char c = '\0';
			int num = 0;
			int length = strI.Length;
			int num2 = 0;
			short[] array = getuserKey(Key);
			num = linNm % 6;
			short ipsswd = (short)((array[num] + linNm) % seedLen);
			while (num2 < length)
			{
				c = cToC_seed_rvs(text[num2], ipsswd);
				num = c - seedOffset;
				num2++;
				bool flag2 = 0 <= num && 90 >= num;
				bool flag3 = 91 == num;
				bool flag4 = 92 <= num && 94 >= num;
				bool flag5 = 92 == num;
				bool flag6 = 93 == num;
				bool flag7 = 94 == num;
				int num3 = num;
				if (flag3)
				{
					num3--;
				}
				if (flag2 || flag3)
				{
					if (length < num3 + num2)
					{
						flag = true;
					}
				}
				else if (flag4)
				{
					flag = false;
					c = cToC_seed_rvs(text[num2], ipsswd);
					num = c - seedOffset;
					num2++;
					flag2 = 0 <= num && 90 >= num;
					flag3 = 91 == num;
					num3 = num;
					if (flag3)
					{
						num3--;
					}
					if (flag2 || flag3)
					{
						if (length < num3 + num2)
						{
							flag = true;
						}
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					strR = text2;
					strI = text.Substring(num2);
					break;
				}
				text3 = text.Substring(num2, num3);
				num2 += num3;
				if (flag7)
				{
					text2 += text3;
				}
				else if (!flag6 && !flag5)
				{
					text2 += sToS_seed_rvs(text3, ipsswd);
				}
				if (!flag3)
				{
					strR = text2;
					strI = text.Substring(num2);
					result = !flag6;
					linNm++;
					break;
				}
			}
			if (0 == strI.Length)
			{
				strI = null;
			}
			if (0 == length)
			{
				result = true;
				linNm++;
				strR = text2;
			}
			return result;
		}
	}
}

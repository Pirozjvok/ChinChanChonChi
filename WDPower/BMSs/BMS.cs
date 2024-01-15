namespace BMSs
{
	public class BMS
	{
		public float voltage = 0f;

		public byte iSOC = 0;

		public sbyte iTemp = 0;

		public ushort iISR = 0;

		public byte FCD = 0;

		public byte eLvl = 0;

		public bool mRly = false;

		public bool nRly = false;

		public bool cRly = false;

		public bool fITLK = false;

		public bool fChg = false;

		public bool fChgEd = false;

		public int[] vlt_c = new int[48];

		public int[] tmp_m = new int[16];

		private byte tmMax = 5;

		private byte tmCnt = 0;

		private bool fTemp = false;

		public BMS()
		{
			ini();
		}

		public void ini()
		{
			voltage = 0f;
			iSOC = 0;
			iTemp = 0;
			iISR = 0;
			FCD = 0;
			eLvl = 0;
			mRly = false;
			nRly = false;
			cRly = false;
			fITLK = false;
			fChg = false;
			fChgEd = false;
			fTemp = false;
			tmMax = 10;
			tmCnt = 0;
			for (int i = 0; i < 48; i++)
			{
				vlt_c[i] = 0;
			}
			for (int i = 0; i < 16; i++)
			{
				tmp_m[i] = 0;
			}
		}

		public void msg1Decode(byte[] data)
		{
			voltage = (data[0] * 256 + data[1]) / 10;
			iSOC = (byte)((data[4] * 256 + data[5]) / 10);
			tmCnt = 0;
		}

		public void msg3Decode(byte[] data)
		{
			byte b = 40;
			byte b2 = 25;
			byte b3 = 15;
			byte b4 = 20;
			byte b5 = 10;
			b2 += b;
			b4 += b;
			b3 += b;
			b5 += b;
			if (data[0] > b2 && data[1] > b3)
			{
				fTemp = false;
			}
			if (data[0] < b4 && data[1] < b5)
			{
				fTemp = true;
			}
			int num = 0;
			num = (fTemp ? data[1] : data[0]);
			iTemp = (sbyte)(num - b);
			tmCnt = 0;
		}

		public void msg4Decode(byte[] data)
		{
			mRly = (((data[4] & 1) > 0) ? true : false);
			nRly = (((data[4] & 2) > 0) ? true : false);
			cRly = (((data[4] & 4) > 0) ? true : false);
			fChg = (((data[5] & 6) > 0) ? true : false);
			fChgEd = (data[5] & 6) == 6;
			fITLK = (((data[5] & 0x20) > 0) ? true : false);
			eLvl = (byte)(data[6] >> 6);
			FCD = data[7];
			tmCnt = 0;
		}

		public void msg7Decode(byte[] data)
		{
			int num = data[0] * 256 + data[1];
			int num2 = data[2] * 256 + data[3];
			iISR = (ushort)((num + num2) / 2);
			tmCnt = 0;
		}

		public void set_vlt_c(int idx, byte[] data)
		{
			for (int i = 0; i < 4; i++)
			{
				vlt_c[idx + i] = data[2 * i] * 256 + data[2 * i + 1];
			}
			tmCnt = 0;
		}

		public void set_temp_m(int idx, byte[] data)
		{
			for (int i = 0; i < 8; i++)
			{
				if (byte.MaxValue == data[i])
				{
					tmp_m[idx + i] = 0;
				}
				else
				{
					tmp_m[idx + i] = data[i] - 40;
				}
			}
			tmCnt = 0;
		}

		public string rdSOC()
		{
			return iSOC.ToString().PadLeft(3) + " %";
		}

		public string rdHvVlt()
		{
			return voltage.ToString("f1").PadLeft(5) + " V";
		}

		public string rdTemp()
		{
			return iTemp.ToString().PadLeft(3) + " ℃";
		}

		public string rdFCD()
		{
			return eLvl.ToString("D1") + " " + FCD.ToString("D3");
		}

		public string rdISR()
		{
			return ((float)(iISR / 1000)).ToString("G2").PadLeft(4) + " MΩ";
		}

		public void liv()
		{
			if (tmCnt < tmMax)
			{
				tmCnt++;
				return;
			}
			ini();
			eLvl = 1;
			FCD = 100;
		}
	}
}

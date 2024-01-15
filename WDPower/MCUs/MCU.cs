namespace MCUs
{
	public class MCU
	{
		public short cTemp = 0;

		public short cCurr = 0;

		public short mTemp = 0;

		public short spd = 0;

		public byte FCD = 0;

		public byte eLvl = 0;

		private byte tmMax = 5;

		private byte tmCnt = 0;

		private ushort spdOffset = 32767;

		public void ini()
		{
			cTemp = 0;
			cCurr = 0;
			mTemp = 0;
			spd = 0;
			FCD = 0;
			eLvl = 0;
			tmMax = 5;
			tmCnt = 0;
		}

		public void msg1Decode(byte[] data)
		{
			spd = (short)(data[2] * 256 + data[3] - spdOffset);
			tmCnt = 0;
		}

		public void msg2Decode(byte[] data)
		{
			byte b = 50;
			ushort num = 2047;
			cTemp = (short)(data[2] - b);
			mTemp = (short)(data[3] - b);
			cCurr = (short)(data[4] * 16 + data[5] / 16 - num);
			eLvl = (byte)(data[0] & 3u);
			FCD = data[6];
			tmCnt = 0;
		}

		public string rdTempMc()
		{
			return cTemp.ToString().PadLeft(3) + " ℃";
		}

		public string rdCurrMc()
		{
			return cCurr.ToString().PadLeft(4) + " A";
		}

		public string rdTempMt()
		{
			return mTemp.ToString().PadLeft(3) + " ℃";
		}

		public string rdSpd()
		{
			return spd.ToString().PadLeft(5) + " Rpm";
		}

		public string rdFCD()
		{
			return eLvl.ToString("D1") + " " + FCD.ToString("D3");
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

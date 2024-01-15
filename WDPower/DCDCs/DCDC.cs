namespace DCDCs
{
	public class DCDC
	{
		public float volt = 0f;

		public byte FCD = 0;

		public byte eLvl = 0;

		private byte tmMax = 5;

		private byte tmCnt = 0;

		public DCDC()
		{
			ini();
		}

		public void ini()
		{
			volt = 0f;
			FCD = 0;
			eLvl = 0;
			tmMax = 5;
			tmCnt = 0;
		}

		public void msg1Decode(byte[] data)
		{
			volt = (float)((data[3] & 0x1F) * 16 + (data[2] >> 4)) / 10f;
			eLvl = (byte)(data[6] & 3u);
			FCD = (byte)((uint)(data[6] >> 2) & 7u);
		}

		public string rdLvVolt()
		{
			return volt.ToString("f1").PadLeft(4) + " V";
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

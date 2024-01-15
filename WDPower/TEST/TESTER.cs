namespace TEST
{
	public class TESTER
	{
		public bool f_tst = false;

		public bool f_EWP = false;

		public bool f_FanL = false;

		public bool f_FanH = false;

		public TESTER()
		{
			ini();
		}

		public void ini()
		{
			f_tst = false;
			f_EWP = false;
			f_FanL = false;
			f_FanH = false;
		}

		public void msg1Eecode(out byte[] data)
		{
			data = new byte[8];
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = 0;
			}
			if (f_tst)
			{
				data[0]++;
			}
			if (f_EWP)
			{
				data[0] += 2;
			}
			if (f_FanL)
			{
				data[0] += 4;
			}
			if (f_FanH)
			{
				data[0] += 8;
			}
		}
	}
}

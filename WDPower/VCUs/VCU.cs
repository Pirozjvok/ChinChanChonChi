namespace VCUs
{
	public class VCU
	{
		public float Vspd = 0f;

		public byte FCD = 0;

		public byte eLvl = 0;

		public byte acclAd = 0;

		public byte strAd = 0;

		public bool hdbrkSw = false;

		public bool brkSw = false;

		public bool seatSw = false;

		public byte[] ver = new byte[5];

		public byte[] sevNm = new byte[2];

		public byte liftAd = 0;

		public bool acclSw = false;

		public bool liftSw = false;

		public bool tiltSw = false;

		public bool vl3Sw = false;

		public bool vl4Sw = false;

		public bool vl5Sw = false;

		private bool f_rdVer = false;

		public float volt = 0f;

		public byte acclAd_i = 0;

		public byte strAd_i = 0;

		public byte liftAd_i = 0;

		private int liftAd_min = 255;

		private int liftAd_max = 0;

		private int strAd_min = 255;

		private int strAd_max = 0;

		private byte tmMax = 5;

		private byte tmCnt = 0;

		private short ix = 0;

		private byte verSrcNm = 0;

		private short vs0_lth = 0;

		private short vs1_lth = 0;

		private short vs2_lth = 0;

		private byte[] vs0_ver = new byte[7];

		private byte[] vs1_ver = new byte[7];

		private byte[] vs2_ver = new byte[7];

		public VCU()
		{
			ini();
		}

		public void ini()
		{
			Vspd = 0f;
			FCD = 0;
			eLvl = 0;
			acclAd = 0;
			liftAd = 0;
			strAd = 0;
			hdbrkSw = false;
			brkSw = false;
			seatSw = false;
			acclSw = false;
			liftSw = false;
			tiltSw = false;
			vl3Sw = false;
			vl4Sw = false;
			vl5Sw = false;
			f_rdVer = false;
			volt = 0f;
			acclAd_i = 0;
			strAd_i = 0;
			liftAd_i = 0;
			liftAd_min = 255;
			liftAd_max = 0;
			strAd_min = 255;
			strAd_max = 0;
			tmMax = 5;
			tmCnt = 0;
			for (ix = 0; ix < ver.Length; ix++)
			{
				ver[ix] = 0;
			}
			verSrcNm = 0;
			vs0_lth = 0;
			vs1_lth = 0;
			vs2_lth = 0;
			for (ix = 0; ix < vs0_ver.Length; ix++)
			{
				vs0_ver[ix] = 0;
				vs1_ver[ix] = 0;
				vs2_ver[ix] = 0;
			}
		}

		public void msg1Decode(byte[] data)
		{
			if (8 == data.Length)
			{
				eLvl = (byte)(data[0] & 3u);
				brkSw = (((data[0] & 0x10) > 0) ? true : false);
				hdbrkSw = (((data[0] & 0x20) > 0) ? true : false);
				seatSw = (((data[0] & 0x40) > 0) ? true : false);
				FCD = data[1];
				Vspd = data[2] / 10;
				acclAd = data[6];
				getVer0(data[7]);
				int idx = (data[5] / 16) & 7;
				getVer1(data[7], idx);
			}
			tmCnt = 0;
		}

		public void msg2Decode(byte[] data)
		{
			acclAd_i = data[3];
			liftAd_i = data[0];
			if (liftAd_max < liftAd_i)
			{
				liftAd_max = liftAd_i;
			}
			if (liftAd_min > liftAd_i)
			{
				liftAd_min = liftAd_i;
			}
			int num = liftAd_max - liftAd_min;
			if (0 < num)
			{
				liftAd = (byte)((liftAd_i - liftAd_min) * 100 / num);
			}
			else
			{
				liftAd = 0;
			}
			strAd_i = data[4];
			if (strAd_max < strAd_i)
			{
				strAd_max = strAd_i;
			}
			if (strAd_min > strAd_i)
			{
				strAd_min = strAd_i;
			}
			num = strAd_max - strAd_min;
			if (0 < num)
			{
				strAd = (byte)((strAd_i - strAd_min) * 100 / num);
			}
			else
			{
				strAd = 0;
			}
			acclSw = (((data[1] & 1) > 0) ? true : false);
			liftSw = (((data[1] & 2) > 0) ? true : false);
			tiltSw = (((data[1] & 4) > 0) ? true : false);
			vl3Sw = (((data[1] & 8) > 0) ? true : false);
			vl4Sw = (((data[1] & 0x10) > 0) ? true : false);
			vl5Sw = (((data[1] & 0x20) > 0) ? true : false);
			tmCnt = 0;
			volt = (int)data[2];
			volt /= 10f;
			int idx = data[7] & 0xF;
			getVer2(data[5], idx);
		}

		public string getver()
		{
			string text = "ECU:";
			for (int i = 0; i < ver.Length - 1; i++)
			{
				text += ver[i].ToString("D2");
			}
			return text + sevNm[0].ToString("D2");
		}

		public string getSev()
		{
			string text = "SevExt:";
			text += ver[4].ToString("D2");
			return text + sevNm[1].ToString("D2");
		}

		public string rdSpd()
		{
			string text = null;
			text = Vspd.ToString("f1");
			return text.PadLeft(4) + " km/h";
		}

		public string rdFCD()
		{
			return eLvl.ToString("D1") + " " + FCD.ToString("D3");
		}

		public string rdLvVolt()
		{
			return volt.ToString("f1").PadLeft(4) + " V";
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

		private void getVer0(byte para)
		{
			if (0 == verSrcNm)
			{
				if (byte.MaxValue == para || 252 == para)
				{
					vs0_lth = 1;
				}
				else if (para < 90)
				{
					if (vs0_lth > 0 && vs0_lth < 8)
					{
						vs0_ver[vs0_lth - 1] = para;
						vs0_lth++;
					}
					else
					{
						vs0_lth = 0;
					}
				}
				else
				{
					vs0_lth = 0;
				}
				if (vs0_lth == 6)
				{
					for (ix = 0; ix < 5; ix++)
					{
						ver[ix] = vs0_ver[ix];
					}
				}
				if (vs0_lth == 7)
				{
					sevNm[0] = vs0_ver[5];
					sevNm[1] = vs0_ver[6];
				}
			}
			else
			{
				vs0_lth = 0;
			}
		}

		private void getVer1(byte para, int idx)
		{
			if (verSrcNm == 0 || 1 == verSrcNm)
			{
				if (idx == 0 && byte.MaxValue == para)
				{
					if (vs1_lth > 6)
					{
						for (ix = 0; ix < 5; ix++)
						{
							ver[ix] = vs1_ver[ix];
						}
						sevNm[0] = vs1_ver[5];
						sevNm[1] = vs1_ver[6];
						verSrcNm = 1;
					}
					vs1_lth = 1;
				}
				else if (vs1_lth > 0 && vs1_lth < 8 && idx > 0 && idx < 8 && para < byte.MaxValue)
				{
					vs1_ver[idx - 1] = para;
					vs1_lth++;
				}
				else
				{
					vs1_lth = 0;
				}
				if (0 == vs1_lth && 1 == verSrcNm)
				{
					verSrcNm = 0;
				}
			}
			else
			{
				vs1_lth = 0;
			}
		}

		private void getVer2(byte para, int idx)
		{
			if (verSrcNm == 0 || 2 == verSrcNm)
			{
				if (idx == 0 && byte.MaxValue == para)
				{
					if (vs2_lth > 6)
					{
						for (ix = 0; ix < 5; ix++)
						{
							ver[ix] = vs2_ver[ix];
						}
						sevNm[0] = vs2_ver[5];
						sevNm[1] = vs2_ver[6];
						verSrcNm = 2;
					}
					vs2_lth = 1;
				}
				else if (vs2_lth > 0 && vs2_lth < 8 && idx > 0 && idx < 8 && para < byte.MaxValue)
				{
					vs2_ver[idx - 1] = para;
					vs2_lth++;
				}
				else
				{
					vs2_lth = 0;
				}
				if (0 == vs2_lth && 2 == verSrcNm)
				{
					verSrcNm = 0;
				}
			}
			else
			{
				vs2_lth = 0;
			}
		}
	}
}

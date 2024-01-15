using System;
using System.IO;
using System.Text;

namespace ParametersCAL
{
	public class Calibration
	{
		public Parameters[] paraList;

		public uint addOffset1 = 16580608u;

		public uint addOffset2 = 1073741824u;

		public uint addrOff = 0u;

		public bool iniSucc = false;

		public string ver = null;

		public bool f_c1 = false;

		public bool f_c2 = false;

		public byte hwNm = 0;

		private byte verMsk = 0;

		public Calibration()
		{
			ini();
		}

		public void ini()
		{
			paraList = null;
			addrOff = 0u;
			iniSucc = false;
			ver = null;
			f_c1 = false;
			f_c2 = false;
			verMsk = 0;
		}

		public byte rdParaFromFile(string Filepath)
		{
			Parameters[] array = new Parameters[100];
			short num = 0;
			int num2 = 0;
			uint num3 = 0u;
			uint num4 = 0u;
			addOffset1 = 16580608u;
			addOffset2 = 1073741824u;
			if (File.Exists(Filepath))
			{
				StreamReader streamReader = new StreamReader(Filepath, Encoding.UTF8);
				string text = null;
				while (streamReader.Peek() >= 0)
				{
					text = streamReader.ReadLine();
					num2 += text.Length;
					string[] array2 = text.Split(',', ';', ':', '\t', '，', '。', '：', '\n', '\r', ' ');
					if (array2.Length > 6)
					{
						short num5 = 0;
						uint num6 = 0u;
						byte b = 0;
						float num7 = 0f;
						float num8 = 0f;
						try
						{
							num5 = Convert.ToInt16(array2[0]);
							num6 = Convert.ToUInt32(array2[3], 16);
							num7 = Convert.ToSingle(array2[5]);
							num8 = Convert.ToSingle(array2[6]);
							if (num6 > num4)
							{
								num4 = num6;
							}
							switch (array2[4])
							{
							case "UBYTE":
								b = 1;
								break;
							case "UWORD":
							case "SWORD":
								b = 2;
								break;
							case "FLOAT32":
								b = 4;
								break;
							}
							if (b > 0)
							{
								array[num].NameCH = array2[1];
								array[num].NameEN = array2[2];
								array[num].Fmt = array2[4];
								array[num].Addr = num6;
								array[num].Lenth = b;
								array[num].pMin = array2[5];
								array[num].pMax = array2[6];
								num++;
							}
						}
						catch
						{
						}
						continue;
					}
					if (array2[0] == "Tooffset")
					{
						try
						{
							num3 = Convert.ToUInt32(array2[1]);
						}
						catch
						{
						}
					}
					if (array2[0] == "ver")
					{
						ver = array2[1];
						verMsk = getVerMsk(ver);
					}
					if (array2[0] == "Hwer")
					{
						if (array2[1] == "2206")
						{
							addOffset1 = 65536u;
							addOffset2 = 1073803264u;
							hwNm = 2;
						}
						else if (array2[1] == "2274KAH")
						{
							addOffset1 = 18874368u;
							addOffset2 = 1384120320u;
							hwNm = 3;
						}
						else if (array2[1] == "2274A02")
						{
							addOffset1 = 16580608u;
							addOffset2 = 1073741824u;
							hwNm = 1;
						}
						else
						{
							addOffset1 = 16580608u;
							addOffset2 = 1073741824u;
							hwNm = 0;
						}
					}
				}
				addrOff = (uint)(num3 - num2 - verMsk);
				if (addrOff >= num4)
				{
					iniSucc = true;
				}
				if (num > 0)
				{
					paraList = new Parameters[num];
					for (short num9 = 0; num9 < num; num9++)
					{
						paraList[num9].NameCH = array[num9].NameCH;
						paraList[num9].NameEN = array[num9].NameEN;
						paraList[num9].Fmt = array[num9].Fmt;
						paraList[num9].Addr = array[num9].Addr;
						paraList[num9].Lenth = array[num9].Lenth;
						paraList[num9].pMin = array[num9].pMin;
						paraList[num9].pMax = array[num9].pMax;
					}
					GC.Collect();
				}
			}
			return (byte)num;
		}

		public void rdValFromFile()
		{
		}

		public void svValToFile()
		{
		}

		public void sdDataToECU()
		{
		}

		public void wtDataToECU()
		{
		}

		private byte getVerMsk(string str1)
		{
			if (null == str1)
			{
				return 0;
			}
			int num = str1.Length / 2 - 2;
			string[] array = new string[num];
			byte[] array2 = new byte[num];
			byte b = 0;
			try
			{
				for (int i = 0; i < num; i++)
				{
					array[i] = str1.Substring(i * 2, 2);
					array2[i] = Convert.ToByte(array[i]);
					b += array2[i];
				}
			}
			catch
			{
				return 0;
			}
			return b;
		}
	}
}

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using KeyAndSeed;
using Peak.Can.Basic;
using Peak.Can.Ccp;

namespace CANLink
{
	public class CANSetting
	{
		public uint m_PccpHandle = 0u;

		public ushort m_Channel;

		public TPCANBaudrate mA_Baudrate;

		public TPCANBaudrate mB_Baudrate;

		public TCCPSlaveData mA_SlaveData_Cal;

		public TCCPSlaveData mA_SlaveData_Pro;

		public TCCPSlaveData mB_SlaveData_Cal;

		public TCCPSlaveData mB_SlaveData_Pro;

		public TCCPExchangeData m_ExchangeData;

		public bool f_cnnt = false;

		public CANSetting()
		{
			devIni();
		}

		public void devIni()
		{
			m_PccpHandle = 0u;
			m_Channel = 81;
			mA_Baudrate = TPCANBaudrate.PCAN_BAUD_500K;
			mA_Baudrate = TPCANBaudrate.PCAN_BAUD_250K;
			mA_SlaveData_Cal.EcuAddress = 274;
			mA_SlaveData_Cal.IdCRO = 256u;
			mA_SlaveData_Cal.IdDTO = 257u;
			mA_SlaveData_Pro.EcuAddress = 565;
			mA_SlaveData_Pro.IdCRO = 256u;
			mA_SlaveData_Pro.IdDTO = 257u;
			mB_SlaveData_Cal.EcuAddress = 274;
			mB_SlaveData_Cal.IdCRO = 256u;
			mB_SlaveData_Cal.IdDTO = 257u;
			mB_SlaveData_Pro.EcuAddress = 565;
			mB_SlaveData_Pro.IdCRO = 256u;
			mB_SlaveData_Pro.IdDTO = 257u;
			m_ExchangeData.IdLength = 0;
			m_ExchangeData.DataType = 0;
			m_ExchangeData.AvailabilityMask = TCCPResourceMask.CCP_RSM_NONE;
			m_ExchangeData.ProtectionMask = TCCPResourceMask.CCP_RSM_NONE;
			mA_SlaveData_Cal.IntelFormat = true;
			mA_SlaveData_Pro.IntelFormat = true;
			mB_SlaveData_Cal.IntelFormat = true;
			mB_SlaveData_Pro.IntelFormat = true;
			f_cnnt = false;
		}

		public bool getIniParaFrmFile(string filePath)
		{
			devIni();
			bool result = false;
			if (File.Exists(filePath))
			{
				bool fl = false;
				bool fl2 = false;
				bool flag = false;
				result = true;
				StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8);
				string text = streamReader.ReadToEnd();
				char[] separator = new char[9] { ',', ';', ':', '\t', '，', '。', '：', '\n', '\r' };
				string[] fileConfig = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				streamReader.Close();
				string str = get_confige(fileConfig, "CANABaudrate");
				string str2 = get_confige(fileConfig, "CANBBaudrate");
				string value = get_confige(fileConfig, "CANACALAddress");
				string value2 = get_confige(fileConfig, "CANAProAddress");
				string value3 = get_confige(fileConfig, "CANBCALAddress");
				string value4 = get_confige(fileConfig, "CANBProAddress");
				string value5 = get_confige(fileConfig, "CANACALCRO");
				string value6 = get_confige(fileConfig, "CANAProCRO");
				string value7 = get_confige(fileConfig, "CANACALDTO");
				string value8 = get_confige(fileConfig, "CANAProDTO");
				string value9 = get_confige(fileConfig, "CANBCALCRO");
				string value10 = get_confige(fileConfig, "CANBProCRO");
				string value11 = get_confige(fileConfig, "CANBCALDTO");
				string value12 = get_confige(fileConfig, "CANBProDTO");
				mA_Baudrate = getBdrt(str, mA_Baudrate, ref fl);
				mB_Baudrate = getBdrt(str2, mB_Baudrate, ref fl2);
				try
				{
					mA_SlaveData_Cal.EcuAddress = Convert.ToUInt16(value, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mA_SlaveData_Pro.EcuAddress = Convert.ToUInt16(value2, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mB_SlaveData_Cal.EcuAddress = Convert.ToUInt16(value3, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mB_SlaveData_Pro.EcuAddress = Convert.ToUInt16(value4, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mA_SlaveData_Cal.IdCRO = Convert.ToUInt32(value5, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mA_SlaveData_Pro.IdCRO = Convert.ToUInt32(value6, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mA_SlaveData_Cal.IdDTO = Convert.ToUInt32(value7, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mA_SlaveData_Pro.IdDTO = Convert.ToUInt32(value8, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mB_SlaveData_Cal.IdCRO = Convert.ToUInt32(value9, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mB_SlaveData_Pro.IdCRO = Convert.ToUInt32(value10, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mB_SlaveData_Cal.IdDTO = Convert.ToUInt32(value11, 16);
				}
				catch
				{
					flag = true;
				}
				try
				{
					mB_SlaveData_Pro.IdDTO = Convert.ToUInt32(value12, 16);
				}
				catch
				{
					flag = true;
				}
				if (fl || fl2 || flag)
				{
					setIniParaToFile(filePath);
				}
				else
				{
					result = true;
				}
			}
			else
			{
				setIniParaToFile(filePath);
			}
			return result;
		}

		public void setIniParaToFile(string fliePath)
		{
			FileStream fileStream = new FileStream(fliePath, FileMode.OpenOrCreate);
			fileStream.Close();
			StreamReader streamReader = new StreamReader(fliePath, Encoding.UTF8);
			string text = streamReader.ReadLine();
			streamReader.Close();
			text = text + "\r\nCANA Baudrate:" + getBdrtNm(mA_Baudrate);
			text = text + "\r\nCANA CAL Address:0x" + mA_SlaveData_Cal.EcuAddress.ToString("X");
			text = text + "\r\nCANA CAL CRO:0x" + mA_SlaveData_Cal.IdCRO.ToString("X");
			text = text + "\r\nCANA CAL DTO:0x" + mA_SlaveData_Cal.IdDTO.ToString("X");
			text = text + "\r\nCANA Pro Address:0x" + mA_SlaveData_Pro.EcuAddress.ToString("X");
			text = text + "\r\nCANA Pro CRO:0x" + mA_SlaveData_Pro.IdCRO.ToString("X");
			text = text + "\r\nCANA Pro DTO:0x" + mA_SlaveData_Pro.IdDTO.ToString("X");
			text = text + "\r\nCANB Baudrate:" + getBdrtNm(mB_Baudrate);
			text = text + "\r\nCANB CAL Address:0x" + mB_SlaveData_Cal.EcuAddress.ToString("X");
			text = text + "\r\nCANB CAL CRO:0x" + mB_SlaveData_Cal.IdCRO.ToString("X");
			text = text + "\r\nCANB CAL DTO:0x" + mB_SlaveData_Cal.IdDTO.ToString("X");
			text = text + "\r\nCANB Pro Address:0x" + mB_SlaveData_Pro.EcuAddress.ToString("X");
			text = text + "\r\nCANB Pro CRO:0x" + mB_SlaveData_Pro.IdCRO.ToString("X");
			text = text + "\r\nCANB Pro DTO:0x" + mB_SlaveData_Pro.IdDTO.ToString("X");
			File.WriteAllText(fliePath, text, Encoding.UTF8);
		}

		public bool canIni(byte CANNm)
		{
			bool result = false;
			TPCANBaudrate btr0Btr = ((CANNm > 0) ? mA_Baudrate : mB_Baudrate);
			TPCANStatus tPCANStatus = PCANBasic.Initialize(m_Channel, btr0Btr);
			if (TPCANStatus.PCAN_ERROR_OK == tPCANStatus)
			{
				result = true;
			}
			else
			{
				tPCANStatus = PCANBasic.Uninitialize(m_Channel);
			}
			return result;
		}

		public string get_confige(string[] fileConfig, string keyword)
		{
			string result = string.Empty;
			string text = keyword.ToUpper();
			string text2 = null;
			if (fileConfig.Length > 1)
			{
				for (int i = 0; i < fileConfig.Length - 1; i++)
				{
					text2 = fileConfig[i].ToUpper();
					text2 = text2.Replace(" ", "");
					if (text.Equals(text2))
					{
						result = fileConfig[i + 1].Replace(" ", "");
						result = result.ToUpper();
					}
				}
			}
			return result;
		}

		private TPCANBaudrate getBdrt(string str, TPCANBaudrate btDft, ref bool fl)
		{
			TPCANBaudrate tPCANBaudrate = btDft;
			fl = false;
			switch (str)
			{
			case "250K":
				tPCANBaudrate = TPCANBaudrate.PCAN_BAUD_250K;
				break;
			case "500K":
				tPCANBaudrate = TPCANBaudrate.PCAN_BAUD_500K;
				break;
			case "800K":
				tPCANBaudrate = TPCANBaudrate.PCAN_BAUD_800K;
				break;
			case "1000K":
			case "1M":
				tPCANBaudrate = TPCANBaudrate.PCAN_BAUD_1M;
				break;
			case "125K":
				tPCANBaudrate = TPCANBaudrate.PCAN_BAUD_125K;
				break;
			default:
				tPCANBaudrate = btDft;
				fl = true;
				break;
			}
			return tPCANBaudrate;
		}

		private string getBdrtNm(TPCANBaudrate tBdrt)
		{
			string result = "250K";
			switch (tBdrt)
			{
			case TPCANBaudrate.PCAN_BAUD_250K:
				result = "250K";
				break;
			case TPCANBaudrate.PCAN_BAUD_125K:
				result = "125K";
				break;
			case TPCANBaudrate.PCAN_BAUD_500K:
				result = "500K";
				break;
			case TPCANBaudrate.PCAN_BAUD_800K:
				result = "800K";
				break;
			case TPCANBaudrate.PCAN_BAUD_1M:
				result = "1000K";
				break;
			}
			return result;
		}

		public bool rdData(uint addr, byte lenth, out byte[] data)
		{
			bool result = false;
			data = new byte[4];
			TCCPResult tCCPResult = CCPApi.SetMemoryTransferAddress(m_PccpHandle, 0, 0, addr, 150);
			if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
			{
				tCCPResult = CCPApi.Upload(m_PccpHandle, lenth, data, 200);
				if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
				{
					result = true;
				}
			}
			return result;
		}

		public bool sdData(uint addr, byte lenth, byte[] data)
		{
			byte MTA0Ext = 0;
			uint MTA0Addr = 0u;
			bool result = false;
			TCCPResult tCCPResult = CCPApi.SetMemoryTransferAddress(m_PccpHandle, 0, 0, addr, 200);
			if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
			{
				tCCPResult = CCPApi.Download(m_PccpHandle, data, lenth, out MTA0Ext, out MTA0Addr, 200);
				if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
				{
					result = true;
				}
			}
			else
			{
				MessageBox.Show("Com error", "Error");
			}
			return result;
		}

		public bool wtDataBlock(uint addr, uint lenth, byte[] data)
		{
			bool result = false;
			byte[] array = new byte[6];
			uint num = lenth;
			uint num2 = 5u;
			int num3 = 0;
			byte MTA0Ext = 0;
			uint MTA0Addr = 0u;
			TCCPResult tCCPResult = CCPApi.SetMemoryTransferAddress(m_PccpHandle, 0, 0, addr, 100);
			if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
			{
				while (num != 0)
				{
					if (num > num2)
					{
						num -= num2;
					}
					else
					{
						num2 = num;
						num = 0u;
					}
					for (int i = 0; i < num2; i++)
					{
						array[i] = data[num3];
						num3++;
					}
					tCCPResult = CCPApi.Program(m_PccpHandle, array, (byte)num2, out MTA0Ext, out MTA0Addr, 150);
					if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK != tCCPResult)
					{
						break;
					}
				}
				if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
				{
					result = true;
				}
			}
			return result;
		}

		public bool erData(uint addr, uint lenth)
		{
			bool result = false;
			TCCPResult tCCPResult = CCPApi.SetMemoryTransferAddress(m_PccpHandle, 0, 0, addr, 100);
			if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
			{
				tCCPResult = CCPApi.ClearMemory(m_PccpHandle, lenth, 1000);
				if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
				{
					result = true;
				}
			}
			return result;
		}

		public bool cnntCALCANB(bool f_rprg)
		{
			TCCPResult tCCPResult = TCCPResult.CCP_ERROR_PCAN;
			if (!f_cnnt)
			{
				for (int i = 0; i < 100; i++)
				{
					tCCPResult = ((!f_rprg) ? CCPApi.Connect(m_Channel, ref mB_SlaveData_Cal, out m_PccpHandle, 20) : CCPApi.Connect(m_Channel, ref mB_SlaveData_Pro, out m_PccpHandle, 20));
					if (tCCPResult == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
					{
						f_cnnt = true;
					}
					if (f_cnnt || tCCPResult == TCCPResult.CCP_ERROR_PCAN)
					{
						break;
					}
					Thread.Sleep(30);
				}
				if (!f_rprg && f_cnnt && CCPApi.SetSessionStatus(m_PccpHandle, (TCCPSessionStatus)0, 100) != 0)
				{
					f_cnnt = false;
				}
			}
			return f_cnnt;
		}

		public bool cnntPro()
		{
			return false;
		}

		public bool ccpIni(byte CANNm)
		{
			TPCANBaudrate btr0Btr = ((CANNm > 0) ? mA_Baudrate : mB_Baudrate);
			if (CCPApi.InitializeChannel(m_Channel, btr0Btr) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
			{
				return true;
			}
			CCPApi.UninitializeChannel(m_Channel);
			if (CCPApi.InitializeChannel(m_Channel, btr0Btr) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
			{
				return true;
			}
			return false;
		}

		public void canUini()
		{
			PCANBasic.Uninitialize(m_Channel);
		}

		public void ccpUini()
		{
			CCPApi.UninitializeChannel(m_Channel);
		}

		public bool rdDataBlock(uint addr, uint lenth, out byte[] data)
		{
			bool result = false;
			data = new byte[lenth];
			byte[] array = new byte[6];
			uint num = lenth;
			uint num2 = 5u;
			int num3 = 0;
			TCCPResult tCCPResult = CCPApi.SetMemoryTransferAddress(m_PccpHandle, 0, 0, addr, 100);
			if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
			{
				while (num != 0)
				{
					if (num > num2)
					{
						num -= num2;
					}
					else
					{
						num2 = num;
						num = 0u;
					}
					tCCPResult = CCPApi.Upload(m_PccpHandle, (byte)num2, array, 200);
					if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
					{
						for (int i = 0; i < num2; i++)
						{
							data[num3] = array[i];
							num3++;
						}
						continue;
					}
					break;
				}
				if (TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK == tCCPResult)
				{
					result = true;
				}
			}
			return result;
		}

		public bool unlock()
		{
			bool result = false;
			byte[] seed = new byte[4];
			bool CurrentStatus = true;
			int num = 0;
			TCCPResourceMask resource = TCCPResourceMask.CCP_RSM_MEMORY_PROGRAMMING;
			for (num = 0; num < 2; num++)
			{
				if (CCPApi.GetSeed(m_PccpHandle, (byte)resource, ref CurrentStatus, seed, 100) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
				{
					break;
				}
			}
			if (num >= 2)
			{
				MessageBox.Show("Get Seed Error", "Error");
				return false;
			}
			if (true)
			{
				byte[] array = new byte[4];
				if (KeyFromSeed.getKeyFromSeed(seed, 4, array, 4, null))
				{
					TCCPResourceMask Privileges = TCCPResourceMask.CCP_RSM_NONE;
					for (num = 0; num < 2; num++)
					{
						if (CCPApi.Unlock(m_PccpHandle, array, 4, out Privileges, 100) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
						{
							break;
						}
					}
					if (num >= 2)
					{
						MessageBox.Show("Unlock fail 1", "Error");
						return false;
					}
					Privileges = TCCPResourceMask.CCP_RSM_MEMORY_PROGRAMMING;
					if (TCCPResourceMask.CCP_RSM_MEMORY_PROGRAMMING != Privileges)
					{
						MessageBox.Show("Unlock fail 2", "Error");
						return false;
					}
					result = true;
				}
			}
			return result;
		}
	}
}

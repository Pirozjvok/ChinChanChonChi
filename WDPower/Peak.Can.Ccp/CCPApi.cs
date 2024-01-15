using System;
using System.Runtime.InteropServices;
using System.Text;
using Peak.Can.Basic;

namespace Peak.Can.Ccp
{
	public static class CCPApi
	{
		public const int CCP_MAX_RCV_QUEUE = 32767;

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_InitializeChannel")]
		public static extern TCCPResult InitializeChannel([MarshalAs(UnmanagedType.U2)] ushort Channel, [MarshalAs(UnmanagedType.U2)] TPCANBaudrate Btr0Btr1, [MarshalAs(UnmanagedType.U1)] TPCANType HwType, uint IOPort, ushort Interrupt);

		public static TCCPResult InitializeChannel(ushort Channel, TPCANBaudrate Btr0Btr1)
		{
			return InitializeChannel(Channel, Btr0Btr1, (TPCANType)0, 0u, 0);
		}

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_UninitializeChannel")]
		public static extern TCCPResult UninitializeChannel([MarshalAs(UnmanagedType.U2)] ushort Channel);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_ReadMsg")]
		public static extern TCCPResult ReadMsg(uint CcpHandle, out TCCPMsg Msg);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Reset")]
		public static extern TCCPResult Reset(uint CcpHandle);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Connect")]
		public static extern TCCPResult Connect([MarshalAs(UnmanagedType.U2)] ushort Channel, ref TCCPSlaveData SlaveData, out uint CcpHandle, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Disconnect")]
		public static extern TCCPResult Disconnect(uint CcpHandle, bool Temporary, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Test")]
		public static extern TCCPResult Test([MarshalAs(UnmanagedType.U2)] ushort Channel, ref TCCPSlaveData SlaveData, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_GetCcpVersion")]
		public static extern TCCPResult GetCcpVersion(uint CcpHandle, [In][Out] ref byte Main, [In][Out] ref byte Release, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_ExchangeId")]
		public static extern TCCPResult ExchangeId(uint CcpHandle, ref TCCPExchangeData ECUData, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] MasterData, int DataLength, ushort TimeOut);

		public static TCCPResult ExchangeId(uint CcpHandle, ref TCCPExchangeData ECUData, ushort TimeOut)
		{
			return ExchangeId(CcpHandle, ref ECUData, new byte[0], 0, TimeOut);
		}

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_GetSeed")]
		public static extern TCCPResult GetSeed(uint CcpHandle, byte Resource, ref bool CurrentStatus, [MarshalAs(UnmanagedType.LPArray)] byte[] Seed, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Unlock")]
		public static extern TCCPResult Unlock(uint CcpHandle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] KeyBuffer, byte KeyLength, [MarshalAs(UnmanagedType.U1)] out TCCPResourceMask Privileges, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_SetSessionStatus")]
		public static extern TCCPResult SetSessionStatus(uint CcpHandle, [MarshalAs(UnmanagedType.U1)] TCCPSessionStatus Status, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_GetSessionStatus")]
		public static extern TCCPResult GetSessionStatus(uint CcpHandle, [MarshalAs(UnmanagedType.U1)] out TCCPSessionStatus Status, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_SetMemoryTransferAddress")]
		public static extern TCCPResult SetMemoryTransferAddress(uint CcpHandle, byte UsedMTA, byte AddrExtension, uint Addr, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Download")]
		public static extern TCCPResult Download(uint CcpHandle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] DataBytes, byte Size, out byte MTA0Ext, out uint MTA0Addr, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Download_6")]
		public static extern TCCPResult Download_6(uint CcpHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 6)] byte[] DataBytes, out byte MTA0Ext, out uint MTA0Addr, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Upload")]
		public static extern TCCPResult Upload(uint CcpHandle, byte Size, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] DataBytes, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_ShortUpload")]
		public static extern TCCPResult ShortUpload(uint CcpHandle, byte UploadSize, byte MTA0Ext, uint MTA0Addr, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] reqData, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Move")]
		public static extern TCCPResult Move(uint CcpHandle, uint SizeOfData, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_SelectCalibrationDataPage")]
		public static extern TCCPResult SelectCalibrationDataPage(uint CcpHandle, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_GetActiveCalibrationPage")]
		public static extern TCCPResult GetActiveCalibrationPage(uint CcpHandle, out byte MTA0Ext, out uint MTA0Addr, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_GetDAQListSize")]
		public static extern TCCPResult GetDAQListSize(uint CcpHandle, byte ListNumber, ref uint DTOId, out byte Size, out byte FirstPID, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_GetDAQListSize")]
		private static extern TCCPResult GetDAQListSize(uint CcpHandle, byte listNumber, UIntPtr ptr, out byte size, out byte firstPID, ushort TimeOut);

		public static TCCPResult GetDAQListSize(uint CcpHandle, byte ListNumber, out byte Size, out byte FirstPID, ushort TimeOut)
		{
			return GetDAQListSize(CcpHandle, ListNumber, UIntPtr.Zero, out Size, out FirstPID, TimeOut);
		}

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_SetDAQListPointer")]
		public static extern TCCPResult SetDAQListPointer(uint CcpHandle, byte ListNumber, byte ODTNumber, byte ElementNumber, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_WriteDAQListEntry")]
		public static extern TCCPResult WriteDAQListEntry(uint CcpHandle, byte SizeElement, byte AddrExtension, uint addrDAQ, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_StartStopDataTransmission")]
		public static extern TCCPResult StartStopDataTransmission(uint CcpHandle, ref TCCPStartStopData Data, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_StartStopSynchronizedDataTransmission")]
		public static extern TCCPResult StartStopSynchronizedDataTransmission(uint CcpHandle, bool StartOrStop, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_ClearMemory")]
		public static extern TCCPResult ClearMemory(uint CcpHandle, uint MemorySize, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Program")]
		public static extern TCCPResult Program(uint CcpHandle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] Data, byte Size, out byte MTA0Ext, out uint MTA0Addr, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_Program_6")]
		public static extern TCCPResult Program_6(uint CcpHandle, [MarshalAs(UnmanagedType.LPArray, SizeConst = 6)] byte[] Data, out byte MTA0Ext, out uint MTA0Addr, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_BuildChecksum")]
		public static extern TCCPResult BuildChecksum(uint CcpHandle, uint BlockSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] out byte[] ChecksumData, out byte ChecksumSize, ushort TimeOut);

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_DiagnosticService")]
		public static extern TCCPResult DiagnosticService(uint CcpHandle, ushort DiagnosticNumber, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] Parameters, byte ParamsLength, out byte ReturnLength, out byte ReturnType, ushort TimeOut);

		public static TCCPResult DiagnosticService(uint CcpHandle, ushort DiagnosticNumber, out byte ReturnLength, out byte ReturnType, ushort TimeOut)
		{
			return DiagnosticService(CcpHandle, DiagnosticNumber, new byte[0], 0, out ReturnLength, out ReturnType, TimeOut);
		}

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_ActionService")]
		public static extern TCCPResult ActionService(uint CcpHandle, ushort ActionNumber, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] Parameters, byte ParamsLength, out byte ReturnLength, out byte ReturnType, ushort TimeOut);

		public static TCCPResult ActionService(uint CcpHandle, ushort ActionNumber, out byte ReturnLength, out byte ReturnType, ushort TimeOut)
		{
			return ActionService(CcpHandle, ActionNumber, new byte[0], 0, out ReturnLength, out ReturnType, TimeOut);
		}

		[DllImport(".\\dll\\PCCP.dll", EntryPoint = "CCP_GetErrorText")]
		public static extern TCCPResult GetErrorText(TCCPResult errorCode, StringBuilder textBuffer);
	}
}

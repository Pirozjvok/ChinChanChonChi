using System.Runtime.InteropServices;
using Peak.Can.Basic;

namespace Peak.Can.Ccp
{
	[StructLayout(LayoutKind.Auto, Size = 4)]
	public struct CCPResult
	{
		[MarshalAs(UnmanagedType.U4)]
		private uint m_pccpResult;

		public TCCPResult CCP
		{
			get
			{
				return IsCanError() ? TCCPResult.CCP_ERROR_PCAN : ((TCCPResult)(m_pccpResult & 0xFFu));
			}
		}

		public TPCANStatus PCAN
		{
			get
			{
				return (TPCANStatus)(m_pccpResult & 0x7FFFFFFFu);
			}
		}

		public TCCPErrorCategory ErrorCategory
		{
			get
			{
				return GetErrorCategory();
			}
		}

		public CCPResult(TCCPResult result)
		{
			m_pccpResult = (uint)result;
		}

		private bool IsCanError()
		{
			return (m_pccpResult & 0x80000000u) == 2147483648u;
		}

		private TCCPErrorCategory GetErrorCategory()
		{
			return (TCCPErrorCategory)((!IsCanError()) ? ((uint)(((byte)CCP & 0xF0) >> 4)) : 0u);
		}
	}
}

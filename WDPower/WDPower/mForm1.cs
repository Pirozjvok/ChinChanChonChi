using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BMSs;
using CANLink;
using DCDCs;
using KeyAndSeed;
using MCUs;
using ParametersCAL;
using Peak.Can.Basic;
using Peak.Can.Ccp;
using SrcDcd;
using TEST;
using VCUs;
using WDPower.Properties;

namespace WDPower
{
	public class mForm1 : Form
	{
		private enum CmdRpro : byte
		{
			Rpro_No_action,
			Rpro_Start,
			Rpro_Pro,
			Rpro_End,
			Rpro_Jmp,
			Rpro_Stop
		}

		private struct TASK
		{
			public short Cnt;

			public short Prd;

			public bool End;
		}

		private TPCANMsg msgBuff = default(TPCANMsg);

		private uint iTmCnt = 0u;

		private byte tmFrom1Renew = 5;

		private byte tmFrom1RenewCnt = 0;

		private VCU VCU1 = new VCU();

		private BMS BMS1 = new BMS();

		private MCU MCUA = new MCU();

		private MCU MCUB = new MCU();

		private DCDC DC1 = new DCDC();

		private Calibration CAL1 = new Calibration();

		private bool f_Test = false;

		private TESTER tester1 = new TESTER();

		private bool f_dcd = false;

		private int iProBar = 0;

		private bool f_log = false;

		private byte usLvl = 0;

		private bool f_pro_ok = false;

		private uint[] addrPro;

		private int[] dtLen;

		private byte[] dtPro;

		private uint addrForPro = 0u;

		private byte[] dataForPro;

		private uint m_PccpHandle;

		private TCCPExchangeData m_ExchangeData;

		private bool fProgramRight = false;

		private short T1ms = 1;

		private short T2ms = 2;

		private short T5ms = 5;

		private short T10ms = 10;

		private short T20ms = 20;

		private short T50ms = 50;

		private short T100ms = 100;

		private short T200ms = 200;

		private short T500ms = 500;

		private short T1s = 1000;

		private TASK Task1;

		private TASK Task2;

		private TASK Task3;

		private TASK Task4;

		private TASK Task5;

		private CANSetting canDev = new CANSetting();

		private bool f_cnnt = false;

		private bool f_lng = false;

		private bool f_RsSw = false;

		private bool f_RsSt = false;

		private IContainer components = null;

		private TabControl tabControl1;

		private TabPage tp2;

		private TabPage tp3;

		internal TabPage tp1;

		private GroupBox gB_VCU_Vspd;

		private Button button_VCU_Vspd;

		private GroupBox gB_BMS_HiVlt;

		private Button button_BMS_HiVlt;

		private GroupBox gB_BMS_SOC;

		private Button button_BMS_SOC;

		private DataGridView dataGridView1;

		private GroupBox gB_MtB_temp;

		private Button button_MtB_temp;

		private GroupBox gB_MtA_temp;

		private Button button_MtA_temp;

		private GroupBox gB_MCUB_Temp;

		private Button button_MCUB_Temp;

		private GroupBox gB_MCUA_Temp;

		private Button button_MCUA_Temp;

		private GroupBox gB_DCDC_LoVlt;

		private Button button_DCDC_LoVlt;

		private GroupBox gB_BMS_ISR;

		private Button button_BMS_ISR;

		private Button button_parSd;

		private Button button_parRd;

		private Button button_parPrgm;

		private GroupBox gB_flags;

		private RadioButton rB_ITLK;

		private RadioButton rB_ChgSt;

		private RadioButton rB_ChgRly;

		private RadioButton rB_NgRly;

		private RadioButton rB_MnRly;

		private RadioButton rB_HdBrkSw;

		private RadioButton rB_BrkSw;

		private RadioButton rB_Vl4Sw;

		private RadioButton rB_Vl3Sw;

		private RadioButton rB_TiltSw;

		private GroupBox gB_progressBar;

		private ProgressBar progressBar_AccleAd;

		private RadioButton rB_LftSw;

		private RadioButton rB_AccSw;

		private Label label_LiftAd;

		private ProgressBar progressBar_LiftAd;

		private Label label_Accle;

		private Button button_Test;

		private Panel panel1;

		private Label label_Test;

		private Button button_EWP;

		private Label label_EWP;

		private Button button_FanH;

		private Label label_FanH;

		private Button button_FanL;

		private Label label_FanL;

		private TextBox textBox_filePath;

		private ProgressBar progressBar_program;

		private Button button_run;

		private Button button_program;

		private Button button_loadFile;

		private Button button_cnnect;

		private Label label_run;

		private Label label_program;

		private Label label_loadFile;

		private RadioButton rB_Vl5Sw;

		private RadioButton rB_StSw;

		private StatusStrip statusStrip_lang;

		private ToolStripDropDownButton toolStripDrpDwBtt_lang;

		private ToolStripMenuItem eNToolStripMenuItem;

		private ToolStripMenuItem cHToolStripMenuItem;

		private GroupBox gB_BMS_Temp;

		private Button button_BMS_Temp;

		private GroupBox gB_MCUA_Curr;

		private Button button_MCUA_Curr;

		private GroupBox gB_MCUB_Curr;

		private Button button_MCUB_Curr;

		private GroupBox gB_MCUA_Spd;

		private Button button_MCUA_Spd;

		private ProgressBar progressBar_TrAngAd;

		private Label label_TrAngAd;

		private GroupBox gB_MCUB_Spd;

		private GroupBox gB_MCUA_FCD;

		private Button button_MCUA_FCD;

		private GroupBox gB_DCDC_FCD;

		private Button button_DCDC_FCD;

		private Button button_MCUB_Spd;

		private GroupBox gB_VCU_FCD;

		private Button button_VCU_FCD;

		private GroupBox gB_MCUB_FCD;

		private Button button_MCUB_FCD;

		private GroupBox gB_BMS_FCD;

		private Button button_BMS_FCD;

		private ToolStripStatusLabel toolStripStatusLabel1;

		private BackgroundWorker bkWk_main;

		private System.Windows.Forms.Timer timer1;

		private BackgroundWorker bkWk_CANRs;

		private Button button_Log;

		private TextBox textBox_skey;

		private Panel panel_prgMsk;

		private Panel panel16;

		private Label label_PCver;

		private Label label_ECUver;

		private ToolTip toolTip_pg1;

		private DataGridViewTextBoxColumn Column1;

		private DataGridViewTextBoxColumn Column2;

		private DataGridViewTextBoxColumn Column3;

		private DataGridViewTextBoxColumn Column4;

		private DataGridViewTextBoxColumn Column5;

		private DataGridViewTextBoxColumn Column6;

		private DataGridView dataGridView2;

		private DataGridView dataGridView3;

		private TabPage tp4;

		private DataGridViewTextBoxColumn C1;

		private DataGridViewTextBoxColumn C2;

		private DataGridViewTextBoxColumn C3;

		private DataGridViewTextBoxColumn C4;

		private DataGridViewTextBoxColumn C5;

		private DataGridViewTextBoxColumn C6;

		private DataGridViewTextBoxColumn C7;

		private DataGridViewTextBoxColumn C8;

		private DataGridViewTextBoxColumn C9;

		private DataGridViewTextBoxColumn C10;

		private DataGridViewTextBoxColumn C11;

		private DataGridViewTextBoxColumn C12;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

		private Label label_verChk;

		private Label label3;

		private Label label_cnnt;

		private Label lab_strad;

		private Label lab_lft;

		private Label lab_accl;

		private BackgroundWorker bkgrd_program;

		private void button_Test_Click(object sender, EventArgs e)
		{
			if (f_Test)
			{
				f_Test = false;
				tester1.f_EWP = false;
				tester1.f_FanL = false;
				tester1.f_FanH = false;
				button_Test.BackgroundImage = Resources.Off;
				button_EWP.BackgroundImage = Resources.Off_D;
				button_FanL.BackgroundImage = Resources.Off_D;
				button_FanH.BackgroundImage = Resources.Off_D;
			}
			else
			{
				f_Test = true;
				tester1.f_tst = true;
				button_Test.BackgroundImage = Resources.On;
				button_EWP.BackgroundImage = Resources.Off;
				button_FanL.BackgroundImage = Resources.Off;
				button_FanH.BackgroundImage = Resources.Off;
			}
		}

		private void button_EWP_Click(object sender, EventArgs e)
		{
			if (f_Test)
			{
				tester1.f_EWP = !tester1.f_EWP;
				button_EWP.BackgroundImage = (tester1.f_EWP ? Resources.On : Resources.Off);
			}
		}

		private void button_FanL_Click(object sender, EventArgs e)
		{
			if (f_Test)
			{
				tester1.f_FanL = !tester1.f_FanL;
				button_FanL.BackgroundImage = (tester1.f_FanL ? Resources.On : Resources.Off);
			}
		}

		private void button_FanH_Click(object sender, EventArgs e)
		{
			if (f_Test)
			{
				tester1.f_FanH = !tester1.f_FanH;
				button_FanH.BackgroundImage = (tester1.f_FanH ? Resources.On : Resources.Off);
			}
		}

		private void taskIni()
		{
			Task1.Cnt = 0;
			Task1.Prd = T1ms;
			Task1.End = false;
			Task2.Cnt = 0;
			Task2.Prd = T100ms;
			Task2.End = false;
			Task3.Cnt = 0;
			Task3.Prd = T200ms;
			Task3.End = false;
			Task4.Cnt = 0;
			Task4.Prd = T500ms;
			Task4.End = false;
			Task5.Cnt = 0;
			Task5.Prd = T1s;
			Task5.End = false;
		}

		private void taskRun()
		{
			Task1.Cnt++;
			Task2.Cnt++;
			Task3.Cnt++;
			Task4.Cnt++;
			Task5.Cnt++;
		}

		private void bkWk_main_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				bool flag = true;
				if (Task1.Cnt >= Task1.Prd)
				{
					Task1.Cnt = 0;
				}
				if (Task2.Cnt >= Task2.Prd)
				{
					if (!canDev.f_cnnt)
					{
						VCU1.liv();
						BMS1.liv();
						MCUA.liv();
						MCUB.liv();
					}
					Task2.Cnt = 0;
				}
				if (Task3.Cnt >= Task3.Prd)
				{
					if (!f_cnnt)
					{
						f_Test = false;
					}
					if (f_Test)
					{
						tester1.f_tst = f_Test;
					}
					if (tester1.f_tst)
					{
						if (!f_Test)
						{
							tester1.ini();
						}
						if (GetCANSrc())
						{
							TPCANMsg MessageBuffer = default(TPCANMsg);
							MessageBuffer.ID = 402827218u;
							tester1.msg1Eecode(out MessageBuffer.DATA);
							MessageBuffer.LEN = (byte)MessageBuffer.DATA.Length;
							MessageBuffer.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED;
							TPCANStatus tPCANStatus = PCANBasic.Write(canDev.m_Channel, ref MessageBuffer);
							if (TPCANStatus.PCAN_ERROR_OK != tPCANStatus)
							{
								tPCANStatus = PCANBasic.Write(canDev.m_Channel, ref MessageBuffer);
							}
							RelsCANSrc();
						}
					}
					Task3.Cnt = 0;
				}
				if (Task4.Cnt >= Task4.Prd)
				{
					Task4.Cnt = 0;
				}
				if (Task5.Cnt >= Task5.Prd)
				{
					DC1.liv();
					Task5.Cnt = 0;
				}
				taskRun();
				GC.Collect();
				Thread.Sleep(1);
			}
		}

		private void mForm_Load(object sender, EventArgs e)
		{
			string text = ".\\Config.ini";
			canDev.getIniParaFrmFile(text);
			string text2 = ReadLgSet(text, "language");
			if (text2 == "1")
			{
				f_lng = true;
				chToEnn();
				cHToolStripMenuItem.Checked = false;
				eNToolStripMenuItem.Checked = true;
			}
			else
			{
				f_lng = false;
				chToChn();
				cHToolStripMenuItem.Checked = true;
				eNToolStripMenuItem.Checked = false;
			}
			taskIni();
			bkWk_main.RunWorkerAsync();
			bkWk_CANRs.RunWorkerAsync();
			bat_info_list_ini();
			timer1.Enabled = true;
			tab2ForCALini();
			CAL1.rdParaFromFile(".\\para.cfg");
			string text3 = CAL1.ver.Substring(0, CAL1.ver.Length - 2);
			label_PCver.Text = "FPC:" + text3 + CAL1.hwNm.ToString("D2");
			dataGridBackCororSet();
			listRenew(CAL1.paraList);
			toolStripStatusLabel1.Width = statusStrip_lang.Width - 170;
			msgBuff.ID = 256u;
			msgBuff.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;
			msgBuff.LEN = 8;
			msgBuff.DATA = new byte[8];
			msgBuff.DATA[0] = 29;
			msgBuff.DATA[1] = byte.MaxValue;
			msgBuff.DATA[2] = 1;
		}

		public mForm1()
		{
			InitializeComponent();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			progressBar_program.Value = iProBar;
			if (f_pro_ok)
			{
				button_run.Enabled = true;
				button_run.Width = 90;
				button_run.Text = "Run";
				button_run.FlatStyle = FlatStyle.Standard;
				button_run.Visible = true;
			}
			else
			{
				button_run.Text = "Restart key,please!";
				button_run.FlatStyle = FlatStyle.Flat;
				button_run.Enabled = false;
				button_run.Width = 250;
			}
			iTmCnt++;
			if (!f_cnnt && iTmCnt % 5 == 1)
			{
				f_cnnt = canDev.canIni(0);
				if (f_cnnt)
				{
					RelsCANSrc();
				}
			}
			if (tmFrom1RenewCnt < tmFrom1Renew)
			{
				tmFrom1RenewCnt++;
			}
			else
			{
				tmFrom1RenewCnt = 0;
				Form1Renew();
				bat_vlt_list_renew(BMS1.vlt_c);
				bat_tmp_list_renew(BMS1.tmp_m);
			}
			if (!f_Test)
			{
				tester1.f_EWP = false;
				tester1.f_FanL = false;
				tester1.f_FanH = false;
				button_Test.BackgroundImage = Resources.Off;
				button_EWP.BackgroundImage = Resources.Off_D;
				button_FanL.BackgroundImage = Resources.Off_D;
				button_FanH.BackgroundImage = Resources.Off_D;
			}
			if (!f_cnnt)
			{
				tab2ForCALini();
			}
		}

		private string ReadLgSet(string filepath, string keyword)
		{
			string result = null;
			if (File.Exists(filepath))
			{
				StreamReader streamReader = new StreamReader(filepath, Encoding.UTF8);
				string text = streamReader.ReadToEnd();
				char[] separator = new char[9] { ',', ';', ':', '\t', '，', '。', '：', '\n', '\r' };
				string[] fileConfig = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				streamReader.Close();
				result = canDev.get_confige(fileConfig, keyword);
			}
			return result;
		}

		private void chToChn()
		{
			gB_BMS_HiVlt.Text = "锂电池电压";
			gB_BMS_SOC.Text = "电量";
			gB_BMS_Temp.Text = "电池温度";
			gB_BMS_ISR.Text = "绝缘电阻";
			rB_ChgRly.Text = "充电继电器";
			rB_ChgSt.Text = "充电标志";
			rB_ITLK.Text = "高压互锁";
			rB_MnRly.Text = "主继电器";
			rB_NgRly.Text = "负继电器";
			gB_BMS_FCD.Text = "电池故障码";
			gB_MCUA_Curr.Text = "电控电流1";
			gB_MCUA_FCD.Text = "行走故障码";
			gB_MCUA_Spd.Text = "电机转速1";
			gB_MCUA_Temp.Text = "电控温度1";
			gB_MCUB_Curr.Text = "电控电流2";
			gB_MCUB_FCD.Text = "油泵故障码";
			gB_MCUB_Spd.Text = "电机转速2";
			gB_MCUB_Temp.Text = "电控温度2";
			gB_MtA_temp.Text = "电机温度1";
			gB_MtB_temp.Text = "电机温度2";
			toolTip_pg1.SetToolTip(gB_MCUA_Curr, "驱动电机电流");
			toolTip_pg1.SetToolTip(gB_MCUA_Spd, "驱动电机转速");
			toolTip_pg1.SetToolTip(gB_MCUA_Temp, "驱动控制单元温度");
			toolTip_pg1.SetToolTip(gB_MtA_temp, "驱动电机温度");
			toolTip_pg1.SetToolTip(gB_MCUB_Curr, "油泵电机电流");
			toolTip_pg1.SetToolTip(gB_MCUB_Spd, "油泵电机转速");
			toolTip_pg1.SetToolTip(gB_MCUB_Temp, "驱动控制单元温度");
			toolTip_pg1.SetToolTip(gB_MtB_temp, "油泵电机温度");
			gB_VCU_FCD.Text = "整车故障码";
			gB_VCU_Vspd.Text = "车速";
			rB_LftSw.Text = "举升开关";
			rB_HdBrkSw.Text = "手刹标志";
			rB_AccSw.Text = "加速器开关";
			rB_BrkSw.Text = "刹车标志";
			rB_StSw.Text = "座椅开关";
			rB_TiltSw.Text = "倾斜开关";
			rB_Vl3Sw.Text = "阀三开关";
			rB_Vl4Sw.Text = "阀四开关";
			rB_Vl5Sw.Text = "阀五开关";
			label_Accle.Text = "加速器开度";
			label_LiftAd.Text = "举升开度";
			label_TrAngAd.Text = "转向角度";
			label_EWP.Text = "水泵";
			label_FanL.Text = "风扇一档";
			label_FanH.Text = "风扇二档";
			label_Test.Text = "测试";
			gB_DCDC_FCD.Text = "DC故障码";
			gB_DCDC_LoVlt.Text = "蓄电池电压";
			label_cnnt.Text = "Step1 - 连接ECU";
			label_loadFile.Text = "Step2 - 加载文件";
			label_program.Text = "Step3 - 烧写";
			label_run.Text = "Step4 - 启动程序";
			tp1.Text = "车辆状态";
			tp2.Text = "参数调试";
			tp3.Text = "软件升级";
			tp4.Text = "电池详情";
		}

		private void chToEnn()
		{
			gB_BMS_FCD.Text = "BMS FCD";
			gB_BMS_HiVlt.Text = "HvBatVolt";
			gB_BMS_ISR.Text = "InsRes";
			gB_BMS_SOC.Text = "SOC";
			gB_BMS_Temp.Text = "HvBatTemp";
			gB_DCDC_FCD.Text = "DC FCD";
			gB_DCDC_LoVlt.Text = "LvBatVolt";
			gB_MCUA_Curr.Text = "MCU curr1";
			gB_MCUA_FCD.Text = "Drv FCD";
			gB_MCUA_Spd.Text = "MotorSpd1";
			gB_MCUA_Temp.Text = "MCU Temp1";
			gB_MCUB_Curr.Text = "MCU curr2";
			gB_MCUB_FCD.Text = "Hyd FCD";
			gB_MCUB_Spd.Text = "MotorSpd2";
			gB_MCUB_Temp.Text = "MCU Temp2";
			gB_MtA_temp.Text = "MotorTemp1";
			gB_MtB_temp.Text = "MotorTemp2";
			gB_VCU_FCD.Text = "VCU FCD";
			gB_VCU_Vspd.Text = "VehicleSpd";
			toolTip_pg1.SetToolTip(gB_MCUA_Curr, "DrivMotorCurrent");
			toolTip_pg1.SetToolTip(gB_MCUA_Spd, "DrivMotorSpeed");
			toolTip_pg1.SetToolTip(gB_MCUA_Temp, "DrivControlUnitTemperature");
			toolTip_pg1.SetToolTip(gB_MtA_temp, "DriverMotorTemperature");
			toolTip_pg1.SetToolTip(gB_MCUB_Curr, "OilPumpMotorCurrent");
			toolTip_pg1.SetToolTip(gB_MCUB_Spd, "OilPumpMotorSpeed");
			toolTip_pg1.SetToolTip(gB_MCUB_Temp, "OilPumpControlUnitTemperature");
			toolTip_pg1.SetToolTip(gB_MtB_temp, "OilPumpMotorTemperature");
			rB_AccSw.Text = "Accelerate Sw";
			rB_BrkSw.Text = "Brake Sw";
			rB_ChgRly.Text = "Charge Rly";
			rB_ChgSt.Text = "Charge Flag";
			rB_HdBrkSw.Text = "Handbrake Sw";
			rB_ITLK.Text = "INTL Sw";
			rB_LftSw.Text = "Lift up Sw";
			rB_MnRly.Text = "Main Rly";
			rB_NgRly.Text = "Negative Rly";
			rB_StSw.Text = "Seat Sw";
			rB_TiltSw.Text = "Tilt Sw";
			rB_Vl3Sw.Text = "Value 3 Sw";
			rB_Vl4Sw.Text = "Value 4 Sw";
			rB_Vl5Sw.Text = "Value 5 Sw";
			label_Accle.Text = "Accelerator";
			label_cnnt.Text = "Step1 - Connect ECU";
			label_EWP.Text = "WatorP";
			label_FanH.Text = "Fan Lv1";
			label_FanL.Text = "Fan Lv2";
			label_LiftAd.Text = "Lift up";
			label_Test.Text = "Test";
			label_loadFile.Text = "Step2 - Load file";
			label_program.Text = "Step3 - Reprogram";
			label_run.Text = "Step4 - Run program";
			label_TrAngAd.Text = "SteeringAngle";
			tp1.Text = "VehicleData";
			tp2.Text = "Debugging";
			tp3.Text = "Renew APP";
			tp4.Text = "BatteryInfo";
		}

		private void ChTOCH(object sender, EventArgs e)
		{
			f_lng = false;
			chToChn();
			cHToolStripMenuItem.Checked = true;
			eNToolStripMenuItem.Checked = false;
			SetLgSet(".\\Config.ini");
			listRenew(CAL1.paraList);
		}

		private void ChTOEN(object sender, EventArgs e)
		{
			f_lng = true;
			chToEnn();
			cHToolStripMenuItem.Checked = false;
			eNToolStripMenuItem.Checked = true;
			SetLgSet(".\\Config.ini");
			listRenew(CAL1.paraList);
		}

		private void SetLgSet(string filepath)
		{
			FileStream fileStream = new FileStream(filepath, FileMode.OpenOrCreate);
			fileStream.Close();
			StreamReader streamReader = new StreamReader(filepath, Encoding.UTF8);
			string text = "Language:" + (f_lng ? "1" : "0");
			string text2 = null;
			string value = "Language".ToUpperInvariant();
			string text3 = null;
			while (0 <= streamReader.Peek())
			{
				text2 = streamReader.ReadLine();
				text3 = text2.ToUpperInvariant();
				if (0 > text3.IndexOf(value))
				{
					text = text + "\r\n" + text2;
				}
			}
			streamReader.Close();
			File.WriteAllText(filepath, text, Encoding.UTF8);
		}

		private void bkWk_CANRs_DoWork(object sender, DoWorkEventArgs e)
		{
			TPCANMsg MessageBuffer = default(TPCANMsg);
			TPCANStatus tPCANStatus = TPCANStatus.PCAN_ERROR_OK;
			while (true)
			{
				bool flag = true;
				if (f_cnnt)
				{
					if (f_RsSw)
					{
						tPCANStatus = PCANBasic.Read(canDev.m_Channel, out MessageBuffer);
						if (TPCANStatus.PCAN_ERROR_OK == tPCANStatus)
						{
							switch (MessageBuffer.ID)
							{
							case 402772647u:
								VCU1.msg1Decode(MessageBuffer.DATA);
								break;
							case 402838183u:
								VCU1.msg2Decode(MessageBuffer.DATA);
								break;
							case 402784244u:
								BMS1.msg1Decode(MessageBuffer.DATA);
								break;
							case 402915316u:
								BMS1.msg3Decode(MessageBuffer.DATA);
								break;
							case 402980852u:
								BMS1.msg4Decode(MessageBuffer.DATA);
								break;
							case 403177460u:
								BMS1.msg7Decode(MessageBuffer.DATA);
								break;
							case 201893872u:
								MCUA.msg1Decode(MessageBuffer.DATA);
								break;
							case 201959408u:
								MCUA.msg2Decode(MessageBuffer.DATA);
								break;
							case 201893873u:
								MCUB.msg1Decode(MessageBuffer.DATA);
								break;
							case 201959409u:
								MCUB.msg2Decode(MessageBuffer.DATA);
								break;
							case 419373318u:
								DC1.msg1Decode(MessageBuffer.DATA);
								break;
							case 402827218u:
								f_Test = false;
								tester1.ini();
								break;
							case 402772724u:
								BMS1.set_vlt_c(0, MessageBuffer.DATA);
								break;
							case 402838260u:
								BMS1.set_vlt_c(4, MessageBuffer.DATA);
								break;
							case 402903796u:
								BMS1.set_vlt_c(8, MessageBuffer.DATA);
								break;
							case 402969332u:
								BMS1.set_vlt_c(12, MessageBuffer.DATA);
								break;
							case 403034868u:
								BMS1.set_vlt_c(16, MessageBuffer.DATA);
								break;
							case 403100404u:
								BMS1.set_vlt_c(20, MessageBuffer.DATA);
								break;
							case 403165940u:
								BMS1.set_vlt_c(24, MessageBuffer.DATA);
								break;
							case 403231476u:
								BMS1.set_vlt_c(28, MessageBuffer.DATA);
								break;
							case 403297012u:
								BMS1.set_vlt_c(32, MessageBuffer.DATA);
								break;
							case 403362548u:
								BMS1.set_vlt_c(36, MessageBuffer.DATA);
								break;
							case 403428084u:
								BMS1.set_vlt_c(40, MessageBuffer.DATA);
								break;
							case 403493620u:
								BMS1.set_vlt_c(44, MessageBuffer.DATA);
								break;
							case 407950068u:
								BMS1.set_temp_m(0, MessageBuffer.DATA);
								break;
							case 408015604u:
								BMS1.set_temp_m(8, MessageBuffer.DATA);
								break;
							}
						}
						else if (TPCANStatus.PCAN_ERROR_NODRIVER == tPCANStatus || TPCANStatus.PCAN_ERROR_REGTEST == tPCANStatus || TPCANStatus.PCAN_ERROR_ILLHW == tPCANStatus)
						{
							f_cnnt = false;
							f_RsSw = false;
						}
						else
						{
							Thread.Sleep(1);
						}
					}
					f_RsSt = f_RsSw;
					if (!f_RsSt)
					{
						Thread.Sleep(1);
					}
				}
				else
				{
					Thread.Sleep(1);
				}
			}
		}

		private bool GetCANSrc()
		{
			uint num = 10u;
			bool result = false;
			if (f_cnnt)
			{
				uint num2 = iTmCnt;
				if (f_RsSw)
				{
					f_RsSw = false;
					while (f_RsSt && iTmCnt - num2 <= num)
					{
					}
					result = !f_RsSt;
				}
			}
			return result;
		}

		private void RelsCANSrc()
		{
			if (f_cnnt)
			{
				f_RsSt = true;
				f_RsSw = f_RsSt;
			}
		}

		private void Form1Renew()
		{
			button_BMS_FCD.Text = BMS1.rdFCD();
			button_BMS_HiVlt.Text = BMS1.rdHvVlt();
			button_BMS_ISR.Text = BMS1.rdISR();
			button_BMS_SOC.Text = BMS1.rdSOC();
			button_BMS_Temp.Text = BMS1.rdTemp();
			button_DCDC_FCD.Text = DC1.rdFCD();
			if ((double)DC1.volt > 0.1)
			{
				button_DCDC_LoVlt.Text = DC1.rdLvVolt();
			}
			else
			{
				button_DCDC_LoVlt.Text = VCU1.rdLvVolt();
			}
			button_MCUA_Curr.Text = MCUA.rdCurrMc();
			button_MCUA_FCD.Text = MCUA.rdFCD();
			button_MCUA_Spd.Text = MCUA.rdSpd();
			button_MCUA_Temp.Text = MCUA.rdTempMc();
			button_MtA_temp.Text = MCUA.rdTempMt();
			button_MCUB_Curr.Text = MCUB.rdCurrMc();
			button_MCUB_FCD.Text = MCUB.rdFCD();
			button_MCUB_Spd.Text = MCUB.rdSpd();
			button_MCUB_Temp.Text = MCUB.rdTempMc();
			button_MtB_temp.Text = MCUB.rdTempMt();
			button_VCU_FCD.Text = VCU1.rdFCD();
			button_VCU_Vspd.Text = VCU1.rdSpd();
			rB_AccSw.Checked = VCU1.acclSw;
			rB_BrkSw.Checked = VCU1.brkSw;
			rB_ChgRly.Checked = BMS1.cRly;
			rB_ChgSt.Checked = BMS1.fChg;
			rB_HdBrkSw.Checked = VCU1.hdbrkSw;
			rB_ITLK.Checked = BMS1.fITLK;
			rB_LftSw.Checked = VCU1.liftSw;
			rB_MnRly.Checked = BMS1.mRly;
			rB_NgRly.Checked = BMS1.nRly;
			rB_StSw.Checked = VCU1.seatSw;
			rB_TiltSw.Checked = VCU1.tiltSw;
			rB_Vl3Sw.Checked = VCU1.vl3Sw;
			rB_Vl4Sw.Checked = VCU1.vl4Sw;
			rB_Vl5Sw.Checked = VCU1.vl5Sw;
			progressBar_AccleAd.Value = VCU1.acclAd;
			progressBar_LiftAd.Value = VCU1.liftAd;
			progressBar_TrAngAd.Value = VCU1.strAd;
			lab_accl.Text = Convert.ToString(VCU1.acclAd_i);
			lab_lft.Text = Convert.ToString(VCU1.liftAd_i);
			lab_strad.Text = Convert.ToString(VCU1.strAd_i);
			label_ECUver.Text = VCU1.getver();
		}

		private void tab2ForCALini()
		{
			button_parPrgm.Enabled = false;
			button_parRd.Enabled = true;
			button_parSd.Enabled = false;
		}

		private void listRenew(Parameters[] pList)
		{
			dataGridView1.RowCount = pList.Length;
			int num = 0;
			for (int i = 0; i < pList.Length; i++)
			{
				Parameters parameters = pList[i];
				dataGridView1[0, num].Value = num + 1;
				dataGridView1[1, num].Value = (f_lng ? parameters.NameEN : parameters.NameCH);
				dataGridView1[2, num].Value = parameters.pVal;
				if (null == dataGridView1[3, num].Value)
				{
					dataGridView1[3, num].Value = parameters.pPv;
				}
				dataGridView1[4, num].Value = parameters.pMin;
				dataGridView1[5, num].Value = parameters.pMax;
				num++;
			}
		}

		private void button_parRd_Click(object sender, EventArgs e)
		{
			bool flag = false;
			if (!verCheck(CAL1.ver) || !hwCheck())
			{
				label_verChk.Text = "No Good";
				label_verChk.BackColor = Color.Red;
				return;
			}
			label_verChk.Text = "Pass";
			label_verChk.BackColor = Color.Green;
			if (f_cnnt && !ccphandle(false))
			{
				MessageBox.Show("Connect failed", "Error");
			}
			if (canDev.f_cnnt)
			{
				flag = rdDataFromECU(CAL1.paraList);
				listRenew(CAL1.paraList);
				if (!flag)
				{
					MessageBox.Show("Read failed", "Error");
				}
				else
				{
					button_parSd.Enabled = true;
					label_ECUver.Visible = true;
					label_PCver.Visible = true;
				}
				ccprelease();
			}
		}

		private bool rdDataFromECU(Parameters[] paraL)
		{
			bool flag = false;
			byte[] data = new byte[4];
			for (int i = 0; i < paraL.Length; i++)
			{
				uint addr = MtrlFmtToItelFmt(paraL[i].Addr, CAL1.addOffset2 - CAL1.addOffset1);
				flag = canDev.rdData(addr, paraL[i].Lenth, out data);
				if (!flag)
				{
					break;
				}
				paraL[i].pVal = datToStr(data, paraL[i].Fmt);
			}
			return flag;
		}

		private string datToStr(byte[] data, string fmt)
		{
			byte[] array = new byte[4];
			string result = null;
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = data[3 - i];
			}
			switch (fmt)
			{
			case "UBYTE":
				result = data[0].ToString();
				break;
			case "UWORD":
				result = ((ushort)(data[0] * 256 + data[1])).ToString();
				break;
			case "SWORD":
				result = ((short)(data[0] * 256 + data[1])).ToString();
				break;
			case "FLOAT32":
				result = BitConverter.ToSingle(array, 0).ToString();
				break;
			}
			return result;
		}

		private void strToData(string str1, string fmt, out byte[] data)
		{
			data = new byte[4];
			switch (fmt)
			{
			case "UBYTE":
				try
				{
					byte value4 = Convert.ToByte(str1);
					data = BitConverter.GetBytes(value4);
					break;
				}
				catch
				{
					break;
				}
			case "UWORD":
				try
				{
					ushort value3 = Convert.ToUInt16(str1);
					byte[] bytes = BitConverter.GetBytes(value3);
					data[0] = bytes[1];
					data[1] = bytes[0];
					break;
				}
				catch
				{
					break;
				}
			case "SWORD":
				try
				{
					short value2 = Convert.ToInt16(str1);
					byte[] bytes = BitConverter.GetBytes(value2);
					data[0] = bytes[1];
					data[1] = bytes[0];
					break;
				}
				catch
				{
					break;
				}
			case "FLOAT32":
				try
				{
					float value = Convert.ToSingle(str1);
					byte[] bytes = BitConverter.GetBytes(value);
					data[0] = bytes[3];
					data[1] = bytes[2];
					data[2] = bytes[1];
					data[3] = bytes[0];
					break;
				}
				catch
				{
					break;
				}
			}
		}

		private uint MtrlFmtToItelFmt(uint data, uint offset)
		{
			uint num = data + offset;
			byte[] array = new byte[4];
			for (byte b = 0; b < 4; b++)
			{
				array[b] = (byte)(num % 256);
				num /= 256;
			}
			return (uint)(array[0] * 256 * 256 * 256 + array[1] * 256 * 256 + array[2] * 256 + array[3]);
		}

		private bool ccphandle(bool f_rprg)
		{
			if (!canDev.f_cnnt && GetCANSrc())
			{
				canDev.canUini();
				if (canDev.ccpIni(0) && canDev.cnntCALCANB(f_rprg))
				{
					return true;
				}
			}
			return false;
		}

		private void ccprelease()
		{
			canDev.ccpUini();
			canDev.canIni(0);
			canDev.f_cnnt = false;
			RelsCANSrc();
		}

		private void button_parSd_Click(object sender, EventArgs e)
		{
			if (!verCheck(CAL1.ver) || !hwCheck())
			{
				label_verChk.Text = "No Good";
				label_verChk.BackColor = Color.Red;
				return;
			}
			label_verChk.Text = "Pass";
			label_verChk.BackColor = Color.Green;
			if (f_cnnt && renewPList(CAL1.paraList))
			{
				if (ccphandle(false) && offset1Check() && sdDataToECU(CAL1.paraList))
				{
					button_parPrgm.Enabled = true;
				}
				listRenew(CAL1.paraList);
				if (canDev.f_cnnt)
				{
					ccprelease();
				}
			}
		}

		private bool renewPList(Parameters[] paraL)
		{
			bool result = false;
			string text = null;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < dataGridView1.RowCount; i++)
			{
				try
				{
					text = dataGridView1[3, i].Value.ToString();
					num = Convert.ToSingle(text);
					num2 = Convert.ToSingle(paraL[i].pMin);
					num3 = Convert.ToSingle(paraL[i].pMax);
					if (num <= num3 && num >= num2)
					{
						paraL[i].pPv = text;
						result = true;
						dataGridView1[3, i].Value = null;
					}
				}
				catch
				{
					text = null;
				}
			}
			return result;
		}

		private bool sdDataToECU(Parameters[] paraL)
		{
			bool flag = false;
			bool result = false;
			byte[] data = new byte[4];
			for (int i = 0; i < paraL.Length; i++)
			{
				if (null != paraL[i].pPv)
				{
					strToData(paraL[i].pPv, paraL[i].Fmt, out data);
					uint addr = MtrlFmtToItelFmt(paraL[i].Addr, CAL1.addOffset2 - CAL1.addOffset1);
					flag = canDev.sdData(addr, paraL[i].Lenth, data);
					paraL[i].pVal = paraL[i].pPv;
					paraL[i].pPv = null;
					if (flag)
					{
						result = true;
					}
				}
			}
			return result;
		}

		private void button_parPrgm_Click(object sender, EventArgs e)
		{
			button_parPrgm.Enabled = false;
			bool flag = false;
			if (!verCheck(CAL1.ver) || !hwCheck())
			{
				label_verChk.Text = "No Good";
				label_verChk.BackColor = Color.Red;
				return;
			}
			label_verChk.Text = "Pass";
			label_verChk.BackColor = Color.Green;
			if (!f_cnnt)
			{
				return;
			}
			if (ccphandle(false) && offset2Check())
			{
				uint num = CAL1.addrOff + 8 - CAL1.addOffset1;
				uint lenth = MtrlFmtToItelFmt(num, 0u);
				uint addr = MtrlFmtToItelFmt(CAL1.addOffset2, 0u);
				uint addr2 = MtrlFmtToItelFmt(CAL1.addOffset1, 0u);
				byte[] data;
				if (canDev.rdDataBlock(addr, num, out data) && canDev.erData(addr2, lenth))
				{
					flag = canDev.wtDataBlock(addr2, num, data);
				}
			}
			if (!flag)
			{
				MessageBox.Show("Program failed", "Error");
			}
			else
			{
				MessageBox.Show("Please restart the key!", "Tip");
			}
			listRenew(CAL1.paraList);
			if (canDev.f_cnnt)
			{
				ccprelease();
			}
		}

		private void button_cnnect_Click(object sender, EventArgs e)
		{
			button_loadFile.Enabled = false;
			button_program.Enabled = false;
			f_pro_ok = false;
			if (f_cnnt)
			{
				bool f_rprg = false;
				if (2 == usLvl)
				{
					f_rprg = true;
				}
				if (!ccphandle(f_rprg))
				{
					MessageBox.Show("Connect failed", "Error");
					return;
				}
				button_loadFile.Enabled = true;
				label_ECUver.Text = VCU1.getver();
			}
		}

		private void button_loadFile_Click(object sender, EventArgs e)
		{
			f_dcd = false;
			f_pro_ok = false;
			string path = ".\\data.bin";
			string text = null;
			button_program.Enabled = false;
			switch (usLvl)
			{
			case 1:
				if (File.Exists(path))
				{
					text = Path.GetFullPath(path);
				}
				break;
			case 2:
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Multiselect = false;
				openFileDialog.Title = "请选择文件";
				openFileDialog.Filter = "SourceFile(mot)|*.mot|SourceFile(jwd)|*.jwd";
				openFileDialog.RestoreDirectory = false;
				openFileDialog.FilterIndex = 0;
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					text = openFileDialog.FileName;
					textBox_filePath.Text = text;
				}
				break;
			}
			default:
				usLvl = 0;
				f_log = false;
				break;
			}
			if (null == text)
			{
				return;
			}
			if (2 == usLvl)
			{
				if (".jwd" == Path.GetExtension(text))
				{
					f_dcd = true;
				}
				if (file_decode(text, out addrPro, out dtLen, out dtPro))
				{
					button_program.Enabled = true;
				}
			}
			else if (DecodeFile(text, out addrForPro, out dataForPro))
			{
				button_program.Enabled = true;
			}
		}

		private bool file_decode(string filepath, out uint[] addr, out int[] datalenth, out byte[] data)
		{
			bool result = false;
			addr = null;
			datalenth = null;
			data = null;
			uint[] array = new uint[15];
			int[] array2 = new int[15];
			byte[] array3 = new byte[1500001];
			int num = 0;
			int num2 = 0;
			byte b = 0;
			string strI = string.Empty;
			CmdRpro cmd = CmdRpro.Rpro_No_action;
			uint addr2 = 0u;
			byte[] data2 = new byte[80];
			int dtLenth = 0;
			int linNm = 0;
			if (File.Exists(filepath))
			{
				StreamReader streamReader = new StreamReader(filepath, Encoding.Default);
				string strR = null;
				short[] array4 = new short[6];
				short[] key = array4;
				bool flag = false;
				while (streamReader.Peek() >= 0 || !string.IsNullOrEmpty(strI))
				{
					flag = false;
					if (f_dcd)
					{
						if (strI == null || "" == strI)
						{
							strI = streamReader.ReadLine();
						}
						while (strI != null && "" != strI && (!DCD.lToL_seed_rvs(ref strI, ref strR, ref linNm, key) || null == strR))
						{
						}
						flag = line_decode(strR, 80, out cmd, out addr2, out data2, out dtLenth);
					}
					else
					{
						strI = streamReader.ReadLine();
						linNm++;
						flag = line_decode(strI, 80, out cmd, out addr2, out data2, out dtLenth);
						strI = null;
					}
					if (flag)
					{
						if (cmd == CmdRpro.Rpro_Start)
						{
							if (0 == b)
							{
								b = 1;
							}
							else if (b > 1)
							{
								MessageBox.Show("Start Error" + Convert.ToString(b), "Error");
								break;
							}
						}
						if (cmd == CmdRpro.Rpro_End)
						{
							if (2 != b)
							{
								MessageBox.Show("End Error" + Convert.ToString(b), "Error");
								break;
							}
							b = 3;
						}
						if (cmd == CmdRpro.Rpro_Pro)
						{
							if (1 != b && 2 != b)
							{
								MessageBox.Show("Data Error" + Convert.ToString(b), "Error");
								break;
							}
							b = 2;
							if (0 == num)
							{
								array[num] = addr2;
								num++;
							}
							else if (1 == num)
							{
								if (addr2 != array[num - 1] + array2[num - 1])
								{
									array[num] = addr2;
									num++;
								}
							}
							else if (addr2 != array[num - 1] + array2[num - 1] - array2[num - 2])
							{
								array[num] = addr2;
								num++;
							}
							if (num > 15)
							{
								MessageBox.Show("Address Overflow", "Error");
								break;
							}
							if (num2 + dtLenth >= 1500000)
							{
								MessageBox.Show("Data Overflow", "Error");
								break;
							}
							for (int i = 0; i < dtLenth; i++)
							{
								array3[num2] = data2[i];
								num2++;
							}
							array2[num - 1] = num2;
						}
					}
					if (linNm > 80000)
					{
						MessageBox.Show("Line Overflow", "Error");
						break;
					}
				}
				streamReader.Close();
			}
			if (b == 3)
			{
				addr = new uint[num];
				datalenth = new int[num];
				data = new byte[num2];
				for (int i = 0; i < num; i++)
				{
					addr[i] = array[i];
					datalenth[i] = array2[i];
				}
				for (int i = 0; i < num2; i++)
				{
					data[i] = array3[i];
				}
				result = true;
			}
			return result;
		}

		private bool line_decode(string strLine, int arrLth, out CmdRpro cmd, out uint addr, out byte[] data, out int dtLenth)
		{
			cmd = CmdRpro.Rpro_No_action;
			addr = 0u;
			data = new byte[arrLth];
			bool result = false;
			dtLenth = 0;
			if (null == strLine)
			{
				return false;
			}
			int num = strLine.Length / 2;
			if (1 >= num)
			{
				return false;
			}
			string text = strLine.ToUpper();
			string[] array = new string[num];
			int num2 = 0;
			for (num2 = 0; num2 < num; num2++)
			{
				array[num2] = text.Substring(num2 * 2, 2);
			}
			switch (array[0])
			{
			case "S0":
			{
				string text2 = "S013000045434F5F4843555F464C532E73726563B7";
				string text3 = "S0030000FC";
				if (text == text2 || text == text3)
				{
					result = true;
					cmd = CmdRpro.Rpro_Start;
				}
				break;
			}
			case "S3":
			{
				if (num <= 7)
				{
					result = false;
					break;
				}
				int num3 = num - 1;
				byte[] array2 = new byte[num3];
				try
				{
					for (int i = 0; i < num3; i++)
					{
						array2[i] = Convert.ToByte("0x" + array[i + 1], 16);
					}
				}
				catch
				{
					result = false;
					break;
				}
				if (array2[num3 - 1] != check_sum(array2, num3 - 1) && num != array2[0] + 2)
				{
					result = false;
					break;
				}
				try
				{
					addr = Convert.ToUInt32("0x" + array[2] + array[3] + array[4] + array[5], 16);
				}
				catch
				{
					result = false;
					break;
				}
				int num4 = num3 - 6;
				if (arrLth >= num4)
				{
					dtLenth = num4;
					for (int i = 0; i < num4; i++)
					{
						data[i] = array2[i + 5];
					}
					cmd = CmdRpro.Rpro_Pro;
					result = true;
				}
				break;
			}
			case "S7":
				if (array.Length <= 4)
				{
					break;
				}
				try
				{
					for (int i = 2; i < 6; i++)
					{
						msgBuff.DATA[i + 1] = Convert.ToByte("0x" + array[i], 16);
					}
					result = true;
					cmd = CmdRpro.Rpro_End;
				}
				catch
				{
				}
				break;
			}
			return result;
		}

		private bool DecodeFile(string filepath, out uint addr, out byte[] data)
		{
			bool flag = false;
			addr = 0u;
			byte[] array = new byte[4096];
			int num = 0;
			StreamReader streamReader = new StreamReader(filepath, Encoding.Default);
			uint num2 = CAL1.addOffset1;
			byte[] array2 = new byte[4];
			byte b = 0;
			if (streamReader.Peek() >= 0)
			{
				string text = streamReader.ReadLine();
				string[] array3 = text.Split(',', ';', ':', '\t', '，', '。', '：', '\n', '\r', ' ');
				bool flag2 = false;
				if (array3.Length > 1 && array3[0] == "ver")
				{
					string text2 = array3[1].Substring(0, array3[1].Length - 2);
					label_PCver.Text = "FPC:" + text2 + CAL1.hwNm.ToString("D2");
					flag2 = verCheck(array3[1]) && hwCheck();
				}
				if (!flag2)
				{
					label_verChk.Text = "No Good";
					label_verChk.BackColor = Color.Red;
					data = null;
					return flag2;
				}
				label_verChk.Text = "Pass";
				label_verChk.BackColor = Color.Green;
			}
			while (streamReader.Peek() >= 0)
			{
				string text = streamReader.ReadLine();
				int num3 = text.Length / 2;
				string[] array4 = new string[num3];
				for (int i = 0; i < num3; i++)
				{
					array4[i] = text.Substring(i * 2, 2);
				}
				byte[] array5 = new byte[num3 - 1];
				try
				{
					int num4 = 0;
					for (int j = 0; j < num3 - 1; j++)
					{
						array5[j] = Convert.ToByte("0x" + array4[j], 16);
						num4 += array5[j];
						array[num] = array5[j];
						num++;
					}
					b = Convert.ToByte("0x" + array4[num3 - 1], 16);
					array2 = BitConverter.GetBytes(num2);
					num4 = num4 + array2[0] + array2[1] + array2[2] + array2[3] + num3 + 4;
					num4 %= 256;
					b = (byte)(b + num4);
					num2 = (uint)(num2 + num3 - 1);
					if (byte.MaxValue != b)
					{
						flag = false;
						break;
					}
				}
				catch
				{
					flag = false;
					break;
				}
			}
			if (streamReader.Peek() < 0 && num2 == CAL1.addrOff + 8)
			{
				flag = true;
			}
			streamReader.Close();
			if (flag)
			{
				data = new byte[num];
				for (int j = 0; j < num; j++)
				{
					data[j] = array[j];
				}
				addr = num2;
			}
			else
			{
				data = null;
			}
			return flag;
		}

		private byte check_sum(byte[] data, int lenth)
		{
			int num = 0;
			for (int i = 0; i < lenth; i++)
			{
				num += data[i];
			}
			num %= 256;
			num = 255 - num;
			return (byte)num;
		}

		private void button_program_Click(object sender, EventArgs e)
		{
			bool flag = false;
			button_loadFile.Enabled = false;
			button_program.Enabled = false;
			m_PccpHandle = canDev.m_PccpHandle;
			if (!f_cnnt || !canDev.f_cnnt)
			{
				return;
			}
			if (usLvl < 2)
			{
				if (offset1Check() && offset2Check())
				{
					uint lenth = MtrlFmtToItelFmt((uint)dataForPro.Length, 0u);
					uint addr = MtrlFmtToItelFmt(CAL1.addOffset1, 0u);
					if (canDev.erData(addr, lenth))
					{
						flag = canDev.wtDataBlock(addr, (uint)dataForPro.Length, dataForPro);
					}
					if (!flag)
					{
						MessageBox.Show("Program failed", "Error");
					}
					else
					{
						button_run.Visible = true;
					}
					if (canDev.f_cnnt)
					{
						ccprelease();
					}
				}
				return;
			}
			TCCPResourceMask tCCPResourceMask = TCCPResourceMask.CCP_RSM_MEMORY_PROGRAMMING;
			int num = 0;
			bool flag2 = false;
			bool flag3 = false;
			for (num = 0; num < 2; num++)
			{
				if (CCPApi.ExchangeId(m_PccpHandle, ref m_ExchangeData, 100) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
				{
					flag2 = (m_ExchangeData.AvailabilityMask & tCCPResourceMask) == tCCPResourceMask;
					flag3 = (m_ExchangeData.ProtectionMask & tCCPResourceMask) == tCCPResourceMask;
					flag2 = true;
					flag3 = true;
					break;
				}
			}
			if (num >= 2)
			{
				MessageBox.Show("ExID Error", "Error");
				return;
			}
			if (!flag2)
			{
				MessageBox.Show("No availability", "Error");
				return;
			}
			bool CurrentStatus = true;
			byte[] seed = new byte[4];
			if (flag3)
			{
				for (num = 0; num < 2; num++)
				{
					if (CCPApi.GetSeed(m_PccpHandle, (byte)tCCPResourceMask, ref CurrentStatus, seed, 100) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
					{
						break;
					}
				}
				if (num >= 2)
				{
					MessageBox.Show("Get Seed Error", "Error");
					return;
				}
				CurrentStatus = true;
			}
			else
			{
				CurrentStatus = false;
			}
			if (CurrentStatus)
			{
				byte[] array = new byte[4];
				if (!KeyFromSeed.getKeyFromSeed(seed, 4, array, 4, null))
				{
					MessageBox.Show("Get key error", "Error");
					return;
				}
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
					return;
				}
				Privileges = TCCPResourceMask.CCP_RSM_MEMORY_PROGRAMMING;
				if (TCCPResourceMask.CCP_RSM_MEMORY_PROGRAMMING != Privileges)
				{
					MessageBox.Show("Unlock fail 2", "Error");
					return;
				}
			}
			fProgramRight = true;
			bkgrd_program.RunWorkerAsync();
		}

		private bool verCheck(string ver)
		{
			string text = null;
			if (!CAL1.iniSucc)
			{
				MessageBox.Show("Confige error", "Error");
				return false;
			}
			try
			{
				text = VCU1.getver().Substring(4);
				text = text.Substring(0, text.Length - 3);
				ver = ver.Substring(0, ver.Length - 3);
			}
			catch
			{
				MessageBox.Show("Version mismatch", "Error");
				return false;
			}
			if (text != ver)
			{
				MessageBox.Show("Version mismatch", "Error");
				return false;
			}
			return true;
		}

		private bool hwCheck()
		{
			if (CAL1.hwNm == VCU1.sevNm[0])
			{
				return true;
			}
			return false;
		}

		private bool offset1Check()
		{
			uint addr = MtrlFmtToItelFmt(CAL1.addrOff, CAL1.addOffset2 - CAL1.addOffset1);
			byte[] data = new byte[4];
			bool flag = canDev.rdData(addr, 4, out data);
			if (flag && (data[0] != 17 || data[1] != 34 || data[2] != 51 || data[3] != 68))
			{
				flag = false;
			}
			return flag;
		}

		private bool offset2Check()
		{
			uint addr = MtrlFmtToItelFmt(CAL1.addrOff, 2u);
			byte[] data = new byte[4];
			bool flag = canDev.rdData(addr, 4, out data);
			if (flag && (data[0] != 51 || data[1] != 68 || (data[2] != 85 && data[2] != byte.MaxValue) || (data[3] != 102 && data[3] != byte.MaxValue)))
			{
				flag = false;
			}
			return flag;
		}

		private void button_Log_Click(object sender, EventArgs e)
		{
			log();
		}

		private void textBox_skey_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r' && !f_log)
			{
				log();
			}
		}

		private void log()
		{
			if (f_log)
			{
				f_log = false;
				panel_prgMsk.Visible = true;
				textBox_filePath.Visible = false;
				button_Log.Text = "Login";
				textBox_skey.Text = "";
				if (canDev.f_cnnt)
				{
					ccprelease();
				}
				return;
			}
			usLvl = 0;
			if (textBox_skey.Text == "601")
			{
				usLvl = 1;
			}
			if (textBox_skey.Text == "606")
			{
				usLvl = 2;
			}
			if (usLvl > 0)
			{
				f_log = true;
				button_Log.Text = "Logout";
				panel_prgMsk.Visible = false;
				button_cnnect.Enabled = true;
				button_loadFile.Enabled = false;
				button_program.Enabled = false;
				textBox_filePath.Text = "";
			}
			switch (usLvl)
			{
			case 1:
				textBox_filePath.Visible = false;
				progressBar_program.Visible = false;
				return;
			case 2:
				textBox_filePath.Visible = true;
				progressBar_program.Visible = true;
				return;
			}
			usLvl = 0;
			f_log = false;
			panel_prgMsk.Visible = true;
			textBox_filePath.Visible = false;
			button_Log.Text = "Login";
			textBox_skey.Text = "";
			if (canDev.f_cnnt)
			{
				ccprelease();
			}
		}

		private void dataGridBackCororSet()
		{
			dataGridView1.Columns[0].DefaultCellStyle.BackColor = Color.Gainsboro;
			dataGridView1.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
			dataGridView1.Columns[2].DefaultCellStyle.BackColor = Color.WhiteSmoke;
			dataGridView1.Columns[3].DefaultCellStyle.BackColor = Color.Ivory;
			dataGridView1.Columns[4].DefaultCellStyle.BackColor = Color.WhiteSmoke;
			dataGridView1.Columns[5].DefaultCellStyle.BackColor = Color.WhiteSmoke;
		}

		private void dataGridBackCororSet1()
		{
			dataGridView2.Columns[0].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView2.Columns[2].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView2.Columns[4].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView2.Columns[6].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView2.Columns[8].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView2.Columns[10].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView3.Columns[0].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView3.Columns[2].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView3.Columns[4].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView3.Columns[6].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView3.Columns[8].DefaultCellStyle.BackColor = Color.LightBlue;
			dataGridView3.Columns[10].DefaultCellStyle.BackColor = Color.LightBlue;
		}

		private void bat_info_list_ini()
		{
			dataGridView2.RowCount = 8;
			dataGridView2.ColumnCount = 12;
			dataGridView3.RowCount = 3;
			dataGridView3.ColumnCount = 12;
			dataGridBackCororSet1();
		}

		private void bat_vlt_list_renew(int[] vla)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (num = 0; num < 8; num++)
			{
				for (num2 = 0; num2 < 6; num2++)
				{
					num3 = num + num2 * 8 + 1;
					dataGridView2[2 * num2, num].Value = num3;
					dataGridView2[2 * num2 + 1, num].Value = vla[num3 - 1];
				}
			}
		}

		private void bat_tmp_list_renew(int[] tla)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (num = 0; num < 3; num++)
			{
				for (num2 = 0; num2 < 6; num2++)
				{
					num3 = num + num2 * 3 + 1;
					dataGridView3[2 * num2, num].Value = num3;
					if (num3 < 16)
					{
						dataGridView3[2 * num2 + 1, num].Value = tla[num3 - 1];
					}
					else
					{
						dataGridView3[2 * num2 + 1, num].Value = 0;
					}
				}
			}
		}

		private void bkgrd_program_DoWork(object sender, DoWorkEventArgs e)
		{
			m_PccpHandle = canDev.m_PccpHandle;
			if (!fProgramRight)
			{
				return;
			}
			if (addrPro == null)
			{
				fProgramRight = false;
				return;
			}
			if (dtLen == null)
			{
				fProgramRight = false;
				return;
			}
			if (dtPro == null)
			{
				fProgramRight = false;
				return;
			}
			TCCPResult tCCPResult = TCCPResult.CCP_ERROR_UNKNOWN_COMMAND;
			int num = 0;
			int num2 = addrPro.Length;
			if (num2 != dtLen.Length)
			{
				fProgramRight = false;
				MessageBox.Show("Program cmd error 1", "Error");
				return;
			}
			uint num3 = 0u;
			uint num4 = 0u;
			for (num = 0; num < num2; num++)
			{
				num4 = MtrlFmtToItelFmt(addrPro[num]);
				if (num == 0)
				{
					num3 = MtrlFmtToItelFmt((uint)dtLen[num]);
				}
				else
				{
					int num5 = dtLen[num] - dtLen[num - 1];
					if (num5 < 0)
					{
						MessageBox.Show("Data length error1," + num, "Error");
						break;
					}
					num3 = MtrlFmtToItelFmt((uint)num5);
				}
				int num6 = 0;
				for (num6 = 0; num6 < 2; num6++)
				{
					if (CCPApi.SetMemoryTransferAddress(m_PccpHandle, 0, 0, num4, 100) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
					{
						break;
					}
				}
				if (num6 >= 2)
				{
					MessageBox.Show("Set MTA Error1," + num, "Error");
					break;
				}
				for (num6 = 0; num6 < 2; num6++)
				{
					if (CCPApi.ClearMemory(m_PccpHandle, num3, 30000) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
					{
						break;
					}
				}
				if (num6 >= 2)
				{
					MessageBox.Show("Clear Error" + num, "Error");
					break;
				}
			}
			if (num < num2)
			{
				fProgramRight = false;
				return;
			}
			int num7 = -1;
			uint num8 = 0u;
			num2 = addrPro.Length;
			int num9 = 0;
			for (num = 0; num < num2; num++)
			{
				num8 = MtrlFmtToItelFmt(addrPro[num]);
				if (num == 0)
				{
					num7 = dtLen[num];
					num9 = 0;
				}
				else
				{
					num7 = dtLen[num] - dtLen[num - 1];
					num9 = dtLen[num - 1];
					if (num7 < 0)
					{
						MessageBox.Show("Data length error2," + num, "Error");
						break;
					}
				}
				int num6 = 0;
				for (num6 = 0; num6 < 2; num6++)
				{
					if (CCPApi.SetMemoryTransferAddress(m_PccpHandle, 0, 0, num8, 100) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
					{
						break;
					}
				}
				if (num6 >= 2)
				{
					MessageBox.Show("Set MTA Error2," + num, "Error");
					break;
				}
				byte b = 0;
				byte[] array = new byte[6];
				byte MTA0Ext = 0;
				uint MTA0Addr = num8;
				while (num7 > 0)
				{
					if (num7 > 5)
					{
						b = 5;
						num7 -= 5;
					}
					else
					{
						b = (byte)num7;
						num7 = 0;
					}
					for (int i = 0; i < b; i++)
					{
						array[i] = dtPro[num9 + i];
					}
					for (num6 = 0; num6 < 2; num6++)
					{
						if (CCPApi.Program(m_PccpHandle, array, b, out MTA0Ext, out MTA0Addr, 150) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
						{
							num9 += b;
							break;
						}
					}
					if (num6 >= 2)
					{
						MessageBox.Show("Program Error," + MTA0Addr, "Error");
						break;
					}
					iProBar = num9 * 100 / dtPro.Length;
				}
				if (0 != num7)
				{
					break;
				}
			}
			if (num < num2)
			{
				fProgramRight = false;
				return;
			}
			int num10 = 0;
			uint MTA0Addr2 = 0u;
			for (num10 = 0; num10 < 2; num10++)
			{
				byte MTA0Ext2;
				if (CCPApi.Program(m_PccpHandle, null, 0, out MTA0Ext2, out MTA0Addr2, 300) == TCCPResult.CCP_ERROR_ACKNOWLEDGE_OK)
				{
					break;
				}
			}
			if (num10 >= 2)
			{
				MessageBox.Show("Program end error," + MTA0Addr2, "Error");
				fProgramRight = false;
			}
			else
			{
				fProgramRight = false;
				iProBar = 100;
				f_pro_ok = true;
			}
		}

		public uint MtrlFmtToItelFmt(uint data)
		{
			uint num = data;
			byte[] array = new byte[4];
			for (byte b = 0; b < 4; b++)
			{
				array[b] = (byte)(num % 256);
				num /= 256;
			}
			return (uint)(array[0] * 256 * 256 * 256 + array[1] * 256 * 256 + array[2] * 256 + array[3]);
		}

		private void mForm1_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				canDev.ccpUini();
				canDev.canUini();
			}
			catch
			{
			}
		}

		private void button_run_Click(object sender, EventArgs e)
		{
			if (f_pro_ok)
			{
				canDev.ccpUini();
				if (canDev.canIni(0) && PCANBasic.Write(canDev.m_Channel, ref msgBuff) == TPCANStatus.PCAN_ERROR_OK)
				{
					ccprelease();
					f_pro_ok = false;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WDPower.mForm1));
			this.rB_MnRly = new System.Windows.Forms.RadioButton();
			this.rB_NgRly = new System.Windows.Forms.RadioButton();
			this.rB_ChgRly = new System.Windows.Forms.RadioButton();
			this.rB_ChgSt = new System.Windows.Forms.RadioButton();
			this.rB_ITLK = new System.Windows.Forms.RadioButton();
			this.rB_AccSw = new System.Windows.Forms.RadioButton();
			this.rB_HdBrkSw = new System.Windows.Forms.RadioButton();
			this.rB_StSw = new System.Windows.Forms.RadioButton();
			this.rB_BrkSw = new System.Windows.Forms.RadioButton();
			this.rB_LftSw = new System.Windows.Forms.RadioButton();
			this.rB_TiltSw = new System.Windows.Forms.RadioButton();
			this.rB_Vl3Sw = new System.Windows.Forms.RadioButton();
			this.rB_Vl4Sw = new System.Windows.Forms.RadioButton();
			this.rB_Vl5Sw = new System.Windows.Forms.RadioButton();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tp1 = new System.Windows.Forms.TabPage();
			this.gB_MCUA_FCD = new System.Windows.Forms.GroupBox();
			this.button_MCUA_FCD = new System.Windows.Forms.Button();
			this.gB_DCDC_FCD = new System.Windows.Forms.GroupBox();
			this.button_DCDC_FCD = new System.Windows.Forms.Button();
			this.gB_MCUB_Spd = new System.Windows.Forms.GroupBox();
			this.button_MCUB_Spd = new System.Windows.Forms.Button();
			this.gB_VCU_FCD = new System.Windows.Forms.GroupBox();
			this.button_VCU_FCD = new System.Windows.Forms.Button();
			this.gB_MCUB_FCD = new System.Windows.Forms.GroupBox();
			this.button_MCUB_FCD = new System.Windows.Forms.Button();
			this.gB_MCUA_Spd = new System.Windows.Forms.GroupBox();
			this.button_MCUA_Spd = new System.Windows.Forms.Button();
			this.gB_BMS_FCD = new System.Windows.Forms.GroupBox();
			this.button_BMS_FCD = new System.Windows.Forms.Button();
			this.gB_MCUB_Curr = new System.Windows.Forms.GroupBox();
			this.button_MCUB_Curr = new System.Windows.Forms.Button();
			this.gB_MCUA_Curr = new System.Windows.Forms.GroupBox();
			this.button_MCUA_Curr = new System.Windows.Forms.Button();
			this.gB_BMS_Temp = new System.Windows.Forms.GroupBox();
			this.button_BMS_Temp = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button_FanH = new System.Windows.Forms.Button();
			this.label_FanH = new System.Windows.Forms.Label();
			this.button_FanL = new System.Windows.Forms.Button();
			this.label_FanL = new System.Windows.Forms.Label();
			this.button_EWP = new System.Windows.Forms.Button();
			this.label_EWP = new System.Windows.Forms.Label();
			this.label_Test = new System.Windows.Forms.Label();
			this.button_Test = new System.Windows.Forms.Button();
			this.gB_progressBar = new System.Windows.Forms.GroupBox();
			this.lab_strad = new System.Windows.Forms.Label();
			this.lab_lft = new System.Windows.Forms.Label();
			this.lab_accl = new System.Windows.Forms.Label();
			this.progressBar_TrAngAd = new System.Windows.Forms.ProgressBar();
			this.label_TrAngAd = new System.Windows.Forms.Label();
			this.label_LiftAd = new System.Windows.Forms.Label();
			this.progressBar_LiftAd = new System.Windows.Forms.ProgressBar();
			this.label_Accle = new System.Windows.Forms.Label();
			this.progressBar_AccleAd = new System.Windows.Forms.ProgressBar();
			this.gB_flags = new System.Windows.Forms.GroupBox();
			this.gB_BMS_ISR = new System.Windows.Forms.GroupBox();
			this.button_BMS_ISR = new System.Windows.Forms.Button();
			this.gB_MtB_temp = new System.Windows.Forms.GroupBox();
			this.button_MtB_temp = new System.Windows.Forms.Button();
			this.gB_MtA_temp = new System.Windows.Forms.GroupBox();
			this.button_MtA_temp = new System.Windows.Forms.Button();
			this.gB_MCUB_Temp = new System.Windows.Forms.GroupBox();
			this.button_MCUB_Temp = new System.Windows.Forms.Button();
			this.gB_MCUA_Temp = new System.Windows.Forms.GroupBox();
			this.button_MCUA_Temp = new System.Windows.Forms.Button();
			this.gB_BMS_SOC = new System.Windows.Forms.GroupBox();
			this.button_BMS_SOC = new System.Windows.Forms.Button();
			this.gB_BMS_HiVlt = new System.Windows.Forms.GroupBox();
			this.button_BMS_HiVlt = new System.Windows.Forms.Button();
			this.gB_DCDC_LoVlt = new System.Windows.Forms.GroupBox();
			this.button_DCDC_LoVlt = new System.Windows.Forms.Button();
			this.gB_VCU_Vspd = new System.Windows.Forms.GroupBox();
			this.button_VCU_Vspd = new System.Windows.Forms.Button();
			this.tp2 = new System.Windows.Forms.TabPage();
			this.button_parPrgm = new System.Windows.Forms.Button();
			this.button_parSd = new System.Windows.Forms.Button();
			this.button_parRd = new System.Windows.Forms.Button();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tp3 = new System.Windows.Forms.TabPage();
			this.panel_prgMsk = new System.Windows.Forms.Panel();
			this.button_run = new System.Windows.Forms.Button();
			this.panel16 = new System.Windows.Forms.Panel();
			this.label_verChk = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label_PCver = new System.Windows.Forms.Label();
			this.label_ECUver = new System.Windows.Forms.Label();
			this.button_Log = new System.Windows.Forms.Button();
			this.textBox_skey = new System.Windows.Forms.TextBox();
			this.textBox_filePath = new System.Windows.Forms.TextBox();
			this.progressBar_program = new System.Windows.Forms.ProgressBar();
			this.button_loadFile = new System.Windows.Forms.Button();
			this.button_cnnect = new System.Windows.Forms.Button();
			this.label_run = new System.Windows.Forms.Label();
			this.label_program = new System.Windows.Forms.Label();
			this.label_cnnt = new System.Windows.Forms.Label();
			this.label_loadFile = new System.Windows.Forms.Label();
			this.button_program = new System.Windows.Forms.Button();
			this.tp4 = new System.Windows.Forms.TabPage();
			this.dataGridView3 = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridView2 = new System.Windows.Forms.DataGridView();
			this.C1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.C12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.statusStrip_lang = new System.Windows.Forms.StatusStrip();
			this.toolStripDrpDwBtt_lang = new System.Windows.Forms.ToolStripDropDownButton();
			this.eNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.bkWk_main = new System.ComponentModel.BackgroundWorker();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.bkWk_CANRs = new System.ComponentModel.BackgroundWorker();
			this.toolTip_pg1 = new System.Windows.Forms.ToolTip(this.components);
			this.bkgrd_program = new System.ComponentModel.BackgroundWorker();
			System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel2 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel3 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel4 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel5 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel6 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel7 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel8 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel9 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel10 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel11 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel12 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel13 = new System.Windows.Forms.Panel();
			System.Windows.Forms.Panel panel14 = new System.Windows.Forms.Panel();
			panel.SuspendLayout();
			panel2.SuspendLayout();
			panel3.SuspendLayout();
			panel4.SuspendLayout();
			panel5.SuspendLayout();
			panel6.SuspendLayout();
			panel7.SuspendLayout();
			panel8.SuspendLayout();
			panel9.SuspendLayout();
			panel10.SuspendLayout();
			panel11.SuspendLayout();
			panel12.SuspendLayout();
			panel13.SuspendLayout();
			panel14.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tp1.SuspendLayout();
			this.gB_MCUA_FCD.SuspendLayout();
			this.gB_DCDC_FCD.SuspendLayout();
			this.gB_MCUB_Spd.SuspendLayout();
			this.gB_VCU_FCD.SuspendLayout();
			this.gB_MCUB_FCD.SuspendLayout();
			this.gB_MCUA_Spd.SuspendLayout();
			this.gB_BMS_FCD.SuspendLayout();
			this.gB_MCUB_Curr.SuspendLayout();
			this.gB_MCUA_Curr.SuspendLayout();
			this.gB_BMS_Temp.SuspendLayout();
			this.panel1.SuspendLayout();
			this.gB_progressBar.SuspendLayout();
			this.gB_flags.SuspendLayout();
			this.gB_BMS_ISR.SuspendLayout();
			this.gB_MtB_temp.SuspendLayout();
			this.gB_MtA_temp.SuspendLayout();
			this.gB_MCUB_Temp.SuspendLayout();
			this.gB_MCUA_Temp.SuspendLayout();
			this.gB_BMS_SOC.SuspendLayout();
			this.gB_BMS_HiVlt.SuspendLayout();
			this.gB_DCDC_LoVlt.SuspendLayout();
			this.gB_VCU_Vspd.SuspendLayout();
			this.tp2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			this.tp3.SuspendLayout();
			this.panel16.SuspendLayout();
			this.tp4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dataGridView3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dataGridView2).BeginInit();
			this.statusStrip_lang.SuspendLayout();
			base.SuspendLayout();
			panel.Controls.Add(this.rB_MnRly);
			panel.Location = new System.Drawing.Point(0, 10);
			panel.Name = "panel2";
			panel.Size = new System.Drawing.Size(155, 37);
			panel.TabIndex = 30;
			this.rB_MnRly.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_MnRly.AutoCheck = false;
			this.rB_MnRly.AutoSize = true;
			this.rB_MnRly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_MnRly.Location = new System.Drawing.Point(46, 6);
			this.rB_MnRly.Name = "rB_MnRly";
			this.rB_MnRly.Size = new System.Drawing.Size(106, 23);
			this.rB_MnRly.TabIndex = 0;
			this.rB_MnRly.TabStop = true;
			this.rB_MnRly.Text = "主继电器";
			this.rB_MnRly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_MnRly.UseVisualStyleBackColor = true;
			panel2.Controls.Add(this.rB_NgRly);
			panel2.Location = new System.Drawing.Point(0, 48);
			panel2.Name = "panel3";
			panel2.Size = new System.Drawing.Size(155, 37);
			panel2.TabIndex = 31;
			this.rB_NgRly.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_NgRly.AutoCheck = false;
			this.rB_NgRly.AutoSize = true;
			this.rB_NgRly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_NgRly.Location = new System.Drawing.Point(46, 6);
			this.rB_NgRly.Name = "rB_NgRly";
			this.rB_NgRly.Size = new System.Drawing.Size(106, 23);
			this.rB_NgRly.TabIndex = 1;
			this.rB_NgRly.TabStop = true;
			this.rB_NgRly.Text = "负继电器";
			this.rB_NgRly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_NgRly.UseVisualStyleBackColor = true;
			panel3.Controls.Add(this.rB_ChgRly);
			panel3.Location = new System.Drawing.Point(0, 86);
			panel3.Name = "panel4";
			panel3.Size = new System.Drawing.Size(155, 37);
			panel3.TabIndex = 32;
			this.rB_ChgRly.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_ChgRly.AutoCheck = false;
			this.rB_ChgRly.AutoSize = true;
			this.rB_ChgRly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_ChgRly.Location = new System.Drawing.Point(27, 6);
			this.rB_ChgRly.Name = "rB_ChgRly";
			this.rB_ChgRly.Size = new System.Drawing.Size(125, 23);
			this.rB_ChgRly.TabIndex = 2;
			this.rB_ChgRly.TabStop = true;
			this.rB_ChgRly.Text = "充电继电器";
			this.rB_ChgRly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_ChgRly.UseVisualStyleBackColor = true;
			panel4.Controls.Add(this.rB_ChgSt);
			panel4.Location = new System.Drawing.Point(0, 124);
			panel4.Name = "panel5";
			panel4.Size = new System.Drawing.Size(155, 37);
			panel4.TabIndex = 33;
			this.rB_ChgSt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_ChgSt.AutoCheck = false;
			this.rB_ChgSt.AutoSize = true;
			this.rB_ChgSt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_ChgSt.Location = new System.Drawing.Point(46, 6);
			this.rB_ChgSt.Name = "rB_ChgSt";
			this.rB_ChgSt.Size = new System.Drawing.Size(106, 23);
			this.rB_ChgSt.TabIndex = 3;
			this.rB_ChgSt.TabStop = true;
			this.rB_ChgSt.Text = "充电标志";
			this.rB_ChgSt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_ChgSt.UseVisualStyleBackColor = true;
			panel5.Controls.Add(this.rB_ITLK);
			panel5.Location = new System.Drawing.Point(0, 162);
			panel5.Name = "panel6";
			panel5.Size = new System.Drawing.Size(155, 37);
			panel5.TabIndex = 34;
			this.rB_ITLK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_ITLK.AutoCheck = false;
			this.rB_ITLK.AutoSize = true;
			this.rB_ITLK.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_ITLK.Location = new System.Drawing.Point(46, 6);
			this.rB_ITLK.Name = "rB_ITLK";
			this.rB_ITLK.Size = new System.Drawing.Size(106, 23);
			this.rB_ITLK.TabIndex = 4;
			this.rB_ITLK.TabStop = true;
			this.rB_ITLK.Text = "高压互锁";
			this.rB_ITLK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_ITLK.UseVisualStyleBackColor = true;
			panel6.Controls.Add(this.rB_AccSw);
			panel6.Location = new System.Drawing.Point(0, 200);
			panel6.Name = "panel7";
			panel6.Size = new System.Drawing.Size(155, 37);
			panel6.TabIndex = 35;
			this.rB_AccSw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_AccSw.AutoCheck = false;
			this.rB_AccSw.AutoSize = true;
			this.rB_AccSw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_AccSw.Location = new System.Drawing.Point(27, 6);
			this.rB_AccSw.Name = "rB_AccSw";
			this.rB_AccSw.Size = new System.Drawing.Size(125, 23);
			this.rB_AccSw.TabIndex = 10;
			this.rB_AccSw.TabStop = true;
			this.rB_AccSw.Text = "加速器开关";
			this.rB_AccSw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_AccSw.UseVisualStyleBackColor = true;
			panel7.Controls.Add(this.rB_HdBrkSw);
			panel7.Location = new System.Drawing.Point(0, 238);
			panel7.Name = "panel8";
			panel7.Size = new System.Drawing.Size(155, 37);
			panel7.TabIndex = 36;
			this.rB_HdBrkSw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_HdBrkSw.AutoCheck = false;
			this.rB_HdBrkSw.AutoSize = true;
			this.rB_HdBrkSw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_HdBrkSw.Location = new System.Drawing.Point(46, 6);
			this.rB_HdBrkSw.Name = "rB_HdBrkSw";
			this.rB_HdBrkSw.Size = new System.Drawing.Size(106, 23);
			this.rB_HdBrkSw.TabIndex = 6;
			this.rB_HdBrkSw.TabStop = true;
			this.rB_HdBrkSw.Text = "手刹标志";
			this.rB_HdBrkSw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_HdBrkSw.UseVisualStyleBackColor = true;
			panel8.Controls.Add(this.rB_StSw);
			panel8.Location = new System.Drawing.Point(155, 10);
			panel8.Name = "panel9";
			panel8.Size = new System.Drawing.Size(155, 37);
			panel8.TabIndex = 37;
			this.rB_StSw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_StSw.AutoCheck = false;
			this.rB_StSw.AutoSize = true;
			this.rB_StSw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_StSw.Location = new System.Drawing.Point(46, 6);
			this.rB_StSw.Name = "rB_StSw";
			this.rB_StSw.Size = new System.Drawing.Size(106, 23);
			this.rB_StSw.TabIndex = 13;
			this.rB_StSw.TabStop = true;
			this.rB_StSw.Text = "座椅开关";
			this.rB_StSw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_StSw.UseVisualStyleBackColor = true;
			panel9.Controls.Add(this.rB_BrkSw);
			panel9.Location = new System.Drawing.Point(155, 48);
			panel9.Name = "panel10";
			panel9.Size = new System.Drawing.Size(155, 37);
			panel9.TabIndex = 38;
			this.rB_BrkSw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_BrkSw.AutoCheck = false;
			this.rB_BrkSw.AutoSize = true;
			this.rB_BrkSw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_BrkSw.Location = new System.Drawing.Point(46, 6);
			this.rB_BrkSw.Name = "rB_BrkSw";
			this.rB_BrkSw.Size = new System.Drawing.Size(106, 23);
			this.rB_BrkSw.TabIndex = 5;
			this.rB_BrkSw.TabStop = true;
			this.rB_BrkSw.Text = "刹车标志";
			this.rB_BrkSw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_BrkSw.UseVisualStyleBackColor = true;
			panel10.Controls.Add(this.rB_LftSw);
			panel10.Location = new System.Drawing.Point(155, 86);
			panel10.Name = "panel11";
			panel10.Size = new System.Drawing.Size(155, 37);
			panel10.TabIndex = 39;
			this.rB_LftSw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_LftSw.AutoCheck = false;
			this.rB_LftSw.AutoSize = true;
			this.rB_LftSw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_LftSw.Location = new System.Drawing.Point(46, 6);
			this.rB_LftSw.Name = "rB_LftSw";
			this.rB_LftSw.Size = new System.Drawing.Size(106, 23);
			this.rB_LftSw.TabIndex = 11;
			this.rB_LftSw.TabStop = true;
			this.rB_LftSw.Text = "举升开关";
			this.rB_LftSw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_LftSw.UseVisualStyleBackColor = true;
			panel11.Controls.Add(this.rB_TiltSw);
			panel11.Location = new System.Drawing.Point(155, 124);
			panel11.Name = "panel12";
			panel11.Size = new System.Drawing.Size(155, 37);
			panel11.TabIndex = 40;
			this.rB_TiltSw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_TiltSw.AutoCheck = false;
			this.rB_TiltSw.AutoSize = true;
			this.rB_TiltSw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_TiltSw.Location = new System.Drawing.Point(46, 6);
			this.rB_TiltSw.Name = "rB_TiltSw";
			this.rB_TiltSw.Size = new System.Drawing.Size(106, 23);
			this.rB_TiltSw.TabIndex = 7;
			this.rB_TiltSw.TabStop = true;
			this.rB_TiltSw.Text = "倾斜开关";
			this.rB_TiltSw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_TiltSw.UseVisualStyleBackColor = true;
			panel12.Controls.Add(this.rB_Vl3Sw);
			panel12.Location = new System.Drawing.Point(155, 162);
			panel12.Name = "panel13";
			panel12.Size = new System.Drawing.Size(155, 37);
			panel12.TabIndex = 41;
			this.rB_Vl3Sw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_Vl3Sw.AutoCheck = false;
			this.rB_Vl3Sw.AutoSize = true;
			this.rB_Vl3Sw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_Vl3Sw.Location = new System.Drawing.Point(46, 6);
			this.rB_Vl3Sw.Name = "rB_Vl3Sw";
			this.rB_Vl3Sw.Size = new System.Drawing.Size(106, 23);
			this.rB_Vl3Sw.TabIndex = 8;
			this.rB_Vl3Sw.TabStop = true;
			this.rB_Vl3Sw.Text = "阀三开关";
			this.rB_Vl3Sw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_Vl3Sw.UseVisualStyleBackColor = true;
			panel13.Controls.Add(this.rB_Vl4Sw);
			panel13.Location = new System.Drawing.Point(155, 200);
			panel13.Name = "panel14";
			panel13.Size = new System.Drawing.Size(155, 37);
			panel13.TabIndex = 42;
			this.rB_Vl4Sw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_Vl4Sw.AutoCheck = false;
			this.rB_Vl4Sw.AutoSize = true;
			this.rB_Vl4Sw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_Vl4Sw.Location = new System.Drawing.Point(46, 6);
			this.rB_Vl4Sw.Name = "rB_Vl4Sw";
			this.rB_Vl4Sw.Size = new System.Drawing.Size(106, 23);
			this.rB_Vl4Sw.TabIndex = 9;
			this.rB_Vl4Sw.TabStop = true;
			this.rB_Vl4Sw.Text = "阀四开关";
			this.rB_Vl4Sw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_Vl4Sw.UseVisualStyleBackColor = true;
			panel14.Controls.Add(this.rB_Vl5Sw);
			panel14.Location = new System.Drawing.Point(155, 238);
			panel14.Name = "panel15";
			panel14.Size = new System.Drawing.Size(155, 37);
			panel14.TabIndex = 43;
			this.rB_Vl5Sw.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.rB_Vl5Sw.AutoCheck = false;
			this.rB_Vl5Sw.AutoSize = true;
			this.rB_Vl5Sw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_Vl5Sw.Location = new System.Drawing.Point(46, 6);
			this.rB_Vl5Sw.Name = "rB_Vl5Sw";
			this.rB_Vl5Sw.Size = new System.Drawing.Size(106, 23);
			this.rB_Vl5Sw.TabIndex = 12;
			this.rB_Vl5Sw.TabStop = true;
			this.rB_Vl5Sw.Text = "阀五开关";
			this.rB_Vl5Sw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rB_Vl5Sw.UseVisualStyleBackColor = true;
			this.tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.tabControl1.Controls.Add(this.tp1);
			this.tabControl1.Controls.Add(this.tp2);
			this.tabControl1.Controls.Add(this.tp3);
			this.tabControl1.Controls.Add(this.tp4);
			this.tabControl1.Font = new System.Drawing.Font("宋体", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.tabControl1.ItemSize = new System.Drawing.Size(160, 30);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(3, 3);
			this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(783, 613);
			this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.tabControl1.TabIndex = 0;
			this.tp1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.tp1.Controls.Add(this.gB_MCUA_FCD);
			this.tp1.Controls.Add(this.gB_DCDC_FCD);
			this.tp1.Controls.Add(this.gB_MCUB_Spd);
			this.tp1.Controls.Add(this.gB_VCU_FCD);
			this.tp1.Controls.Add(this.gB_MCUB_FCD);
			this.tp1.Controls.Add(this.gB_MCUA_Spd);
			this.tp1.Controls.Add(this.gB_BMS_FCD);
			this.tp1.Controls.Add(this.gB_MCUB_Curr);
			this.tp1.Controls.Add(this.gB_MCUA_Curr);
			this.tp1.Controls.Add(this.gB_BMS_Temp);
			this.tp1.Controls.Add(this.panel1);
			this.tp1.Controls.Add(this.gB_progressBar);
			this.tp1.Controls.Add(this.gB_flags);
			this.tp1.Controls.Add(this.gB_BMS_ISR);
			this.tp1.Controls.Add(this.gB_MtB_temp);
			this.tp1.Controls.Add(this.gB_MtA_temp);
			this.tp1.Controls.Add(this.gB_MCUB_Temp);
			this.tp1.Controls.Add(this.gB_MCUA_Temp);
			this.tp1.Controls.Add(this.gB_BMS_SOC);
			this.tp1.Controls.Add(this.gB_BMS_HiVlt);
			this.tp1.Controls.Add(this.gB_DCDC_LoVlt);
			this.tp1.Controls.Add(this.gB_VCU_Vspd);
			this.tp1.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 120);
			this.tp1.Location = new System.Drawing.Point(4, 34);
			this.tp1.Margin = new System.Windows.Forms.Padding(1);
			this.tp1.Name = "tp1";
			this.tp1.Padding = new System.Windows.Forms.Padding(1);
			this.tp1.Size = new System.Drawing.Size(775, 575);
			this.tp1.TabIndex = 0;
			this.tp1.Text = "车辆状态";
			this.tp1.UseVisualStyleBackColor = true;
			this.gB_MCUA_FCD.Controls.Add(this.button_MCUA_FCD);
			this.gB_MCUA_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MCUA_FCD.ForeColor = System.Drawing.Color.FromArgb(192, 64, 0);
			this.gB_MCUA_FCD.Location = new System.Drawing.Point(480, 490);
			this.gB_MCUA_FCD.Name = "gB_MCUA_FCD";
			this.gB_MCUA_FCD.Size = new System.Drawing.Size(130, 73);
			this.gB_MCUA_FCD.TabIndex = 27;
			this.gB_MCUA_FCD.TabStop = false;
			this.gB_MCUA_FCD.Text = "行走故障码";
			this.button_MCUA_FCD.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MCUA_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MCUA_FCD.Location = new System.Drawing.Point(3, 24);
			this.button_MCUA_FCD.Name = "button_MCUA_FCD";
			this.button_MCUA_FCD.Size = new System.Drawing.Size(124, 46);
			this.button_MCUA_FCD.TabIndex = 1;
			this.button_MCUA_FCD.Text = "0 000";
			this.button_MCUA_FCD.UseVisualStyleBackColor = true;
			this.gB_DCDC_FCD.Controls.Add(this.button_DCDC_FCD);
			this.gB_DCDC_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_DCDC_FCD.ForeColor = System.Drawing.Color.FromArgb(192, 64, 0);
			this.gB_DCDC_FCD.Location = new System.Drawing.Point(637, 490);
			this.gB_DCDC_FCD.Name = "gB_DCDC_FCD";
			this.gB_DCDC_FCD.Size = new System.Drawing.Size(130, 73);
			this.gB_DCDC_FCD.TabIndex = 29;
			this.gB_DCDC_FCD.TabStop = false;
			this.gB_DCDC_FCD.Text = "DC故障码";
			this.button_DCDC_FCD.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_DCDC_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_DCDC_FCD.Location = new System.Drawing.Point(3, 24);
			this.button_DCDC_FCD.Name = "button_DCDC_FCD";
			this.button_DCDC_FCD.Size = new System.Drawing.Size(124, 46);
			this.button_DCDC_FCD.TabIndex = 1;
			this.button_DCDC_FCD.Text = "0 000";
			this.button_DCDC_FCD.UseVisualStyleBackColor = true;
			this.gB_MCUB_Spd.Controls.Add(this.button_MCUB_Spd);
			this.gB_MCUB_Spd.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MCUB_Spd.Location = new System.Drawing.Point(324, 298);
			this.gB_MCUB_Spd.Name = "gB_MCUB_Spd";
			this.gB_MCUB_Spd.Size = new System.Drawing.Size(130, 73);
			this.gB_MCUB_Spd.TabIndex = 24;
			this.gB_MCUB_Spd.TabStop = false;
			this.gB_MCUB_Spd.Text = "电机转速2";
			this.button_MCUB_Spd.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MCUB_Spd.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MCUB_Spd.Location = new System.Drawing.Point(3, 24);
			this.button_MCUB_Spd.Name = "button_MCUB_Spd";
			this.button_MCUB_Spd.Size = new System.Drawing.Size(124, 46);
			this.button_MCUB_Spd.TabIndex = 2;
			this.button_MCUB_Spd.Text = " 0000 Rpm";
			this.button_MCUB_Spd.UseVisualStyleBackColor = true;
			this.gB_VCU_FCD.Controls.Add(this.button_VCU_FCD);
			this.gB_VCU_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_VCU_FCD.ForeColor = System.Drawing.Color.FromArgb(192, 64, 0);
			this.gB_VCU_FCD.Location = new System.Drawing.Point(13, 490);
			this.gB_VCU_FCD.Name = "gB_VCU_FCD";
			this.gB_VCU_FCD.Size = new System.Drawing.Size(130, 73);
			this.gB_VCU_FCD.TabIndex = 26;
			this.gB_VCU_FCD.TabStop = false;
			this.gB_VCU_FCD.Text = "整车故障码";
			this.button_VCU_FCD.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_VCU_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_VCU_FCD.Location = new System.Drawing.Point(3, 24);
			this.button_VCU_FCD.Name = "button_VCU_FCD";
			this.button_VCU_FCD.Size = new System.Drawing.Size(124, 46);
			this.button_VCU_FCD.TabIndex = 1;
			this.button_VCU_FCD.Text = "0 000";
			this.button_VCU_FCD.UseVisualStyleBackColor = true;
			this.gB_MCUB_FCD.Controls.Add(this.button_MCUB_FCD);
			this.gB_MCUB_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MCUB_FCD.ForeColor = System.Drawing.Color.FromArgb(192, 64, 0);
			this.gB_MCUB_FCD.Location = new System.Drawing.Point(324, 490);
			this.gB_MCUB_FCD.Name = "gB_MCUB_FCD";
			this.gB_MCUB_FCD.Size = new System.Drawing.Size(130, 73);
			this.gB_MCUB_FCD.TabIndex = 28;
			this.gB_MCUB_FCD.TabStop = false;
			this.gB_MCUB_FCD.Text = "油泵故障码";
			this.button_MCUB_FCD.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MCUB_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MCUB_FCD.Location = new System.Drawing.Point(3, 24);
			this.button_MCUB_FCD.Name = "button_MCUB_FCD";
			this.button_MCUB_FCD.Size = new System.Drawing.Size(124, 46);
			this.button_MCUB_FCD.TabIndex = 1;
			this.button_MCUB_FCD.Text = "0 000";
			this.button_MCUB_FCD.UseVisualStyleBackColor = true;
			this.gB_MCUA_Spd.Controls.Add(this.button_MCUA_Spd);
			this.gB_MCUA_Spd.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MCUA_Spd.Location = new System.Drawing.Point(168, 298);
			this.gB_MCUA_Spd.Name = "gB_MCUA_Spd";
			this.gB_MCUA_Spd.Size = new System.Drawing.Size(130, 73);
			this.gB_MCUA_Spd.TabIndex = 23;
			this.gB_MCUA_Spd.TabStop = false;
			this.gB_MCUA_Spd.Text = "电机转速1";
			this.button_MCUA_Spd.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MCUA_Spd.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MCUA_Spd.Location = new System.Drawing.Point(3, 24);
			this.button_MCUA_Spd.Name = "button_MCUA_Spd";
			this.button_MCUA_Spd.Size = new System.Drawing.Size(124, 46);
			this.button_MCUA_Spd.TabIndex = 1;
			this.button_MCUA_Spd.Text = " 0000 Rpm";
			this.button_MCUA_Spd.UseVisualStyleBackColor = true;
			this.gB_BMS_FCD.Controls.Add(this.button_BMS_FCD);
			this.gB_BMS_FCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.gB_BMS_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_BMS_FCD.ForeColor = System.Drawing.Color.FromArgb(192, 64, 0);
			this.gB_BMS_FCD.Location = new System.Drawing.Point(168, 490);
			this.gB_BMS_FCD.Name = "gB_BMS_FCD";
			this.gB_BMS_FCD.Size = new System.Drawing.Size(130, 73);
			this.gB_BMS_FCD.TabIndex = 25;
			this.gB_BMS_FCD.TabStop = false;
			this.gB_BMS_FCD.Text = "电池故障码";
			this.button_BMS_FCD.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_BMS_FCD.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_BMS_FCD.Location = new System.Drawing.Point(3, 24);
			this.button_BMS_FCD.Name = "button_BMS_FCD";
			this.button_BMS_FCD.Size = new System.Drawing.Size(124, 46);
			this.button_BMS_FCD.TabIndex = 1;
			this.button_BMS_FCD.Text = "0 000";
			this.button_BMS_FCD.UseVisualStyleBackColor = true;
			this.gB_MCUB_Curr.Controls.Add(this.button_MCUB_Curr);
			this.gB_MCUB_Curr.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MCUB_Curr.Location = new System.Drawing.Point(324, 106);
			this.gB_MCUB_Curr.Name = "gB_MCUB_Curr";
			this.gB_MCUB_Curr.Size = new System.Drawing.Size(130, 73);
			this.gB_MCUB_Curr.TabIndex = 22;
			this.gB_MCUB_Curr.TabStop = false;
			this.gB_MCUB_Curr.Text = "电控电流2";
			this.button_MCUB_Curr.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MCUB_Curr.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MCUB_Curr.Location = new System.Drawing.Point(3, 24);
			this.button_MCUB_Curr.Name = "button_MCUB_Curr";
			this.button_MCUB_Curr.Size = new System.Drawing.Size(124, 46);
			this.button_MCUB_Curr.TabIndex = 1;
			this.button_MCUB_Curr.Text = "000 A";
			this.button_MCUB_Curr.UseVisualStyleBackColor = true;
			this.gB_MCUA_Curr.Controls.Add(this.button_MCUA_Curr);
			this.gB_MCUA_Curr.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MCUA_Curr.Location = new System.Drawing.Point(168, 106);
			this.gB_MCUA_Curr.Name = "gB_MCUA_Curr";
			this.gB_MCUA_Curr.Size = new System.Drawing.Size(130, 73);
			this.gB_MCUA_Curr.TabIndex = 21;
			this.gB_MCUA_Curr.TabStop = false;
			this.gB_MCUA_Curr.Text = "电控电流1";
			this.button_MCUA_Curr.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MCUA_Curr.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MCUA_Curr.Location = new System.Drawing.Point(3, 24);
			this.button_MCUA_Curr.Name = "button_MCUA_Curr";
			this.button_MCUA_Curr.Size = new System.Drawing.Size(124, 46);
			this.button_MCUA_Curr.TabIndex = 1;
			this.button_MCUA_Curr.Text = "000 A";
			this.button_MCUA_Curr.UseVisualStyleBackColor = true;
			this.gB_BMS_Temp.Controls.Add(this.button_BMS_Temp);
			this.gB_BMS_Temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_BMS_Temp.Location = new System.Drawing.Point(13, 298);
			this.gB_BMS_Temp.Name = "gB_BMS_Temp";
			this.gB_BMS_Temp.Size = new System.Drawing.Size(130, 73);
			this.gB_BMS_Temp.TabIndex = 20;
			this.gB_BMS_Temp.TabStop = false;
			this.gB_BMS_Temp.Text = "电池温度";
			this.button_BMS_Temp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_BMS_Temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_BMS_Temp.Location = new System.Drawing.Point(3, 24);
			this.button_BMS_Temp.Name = "button_BMS_Temp";
			this.button_BMS_Temp.Size = new System.Drawing.Size(124, 46);
			this.button_BMS_Temp.TabIndex = 1;
			this.button_BMS_Temp.Text = "000 ℃";
			this.button_BMS_Temp.UseVisualStyleBackColor = true;
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.button_FanH);
			this.panel1.Controls.Add(this.label_FanH);
			this.panel1.Controls.Add(this.button_FanL);
			this.panel1.Controls.Add(this.label_FanL);
			this.panel1.Controls.Add(this.button_EWP);
			this.panel1.Controls.Add(this.label_EWP);
			this.panel1.Controls.Add(this.label_Test);
			this.panel1.Controls.Add(this.button_Test);
			this.panel1.Location = new System.Drawing.Point(460, 382);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(310, 94);
			this.panel1.TabIndex = 19;
			this.button_FanH.BackgroundImage = WDPower.Properties.Resources.Off_D;
			this.button_FanH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button_FanH.FlatAppearance.BorderSize = 0;
			this.button_FanH.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.button_FanH.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.button_FanH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_FanH.Location = new System.Drawing.Point(241, 55);
			this.button_FanH.Name = "button_FanH";
			this.button_FanH.Size = new System.Drawing.Size(60, 27);
			this.button_FanH.TabIndex = 11;
			this.button_FanH.UseVisualStyleBackColor = true;
			this.button_FanH.Click += new System.EventHandler(button_FanH_Click);
			this.label_FanH.AutoSize = true;
			this.label_FanH.Location = new System.Drawing.Point(155, 63);
			this.label_FanH.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.label_FanH.Name = "label_FanH";
			this.label_FanH.Size = new System.Drawing.Size(85, 19);
			this.label_FanH.TabIndex = 10;
			this.label_FanH.Text = "风扇二挡";
			this.label_FanH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button_FanL.BackgroundImage = WDPower.Properties.Resources.Off_D;
			this.button_FanL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button_FanL.FlatAppearance.BorderSize = 0;
			this.button_FanL.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.button_FanL.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.button_FanL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_FanL.Location = new System.Drawing.Point(241, 14);
			this.button_FanL.Name = "button_FanL";
			this.button_FanL.Size = new System.Drawing.Size(60, 27);
			this.button_FanL.TabIndex = 9;
			this.button_FanL.UseVisualStyleBackColor = true;
			this.button_FanL.Click += new System.EventHandler(button_FanL_Click);
			this.label_FanL.AutoSize = true;
			this.label_FanL.Location = new System.Drawing.Point(155, 18);
			this.label_FanL.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.label_FanL.Name = "label_FanL";
			this.label_FanL.Size = new System.Drawing.Size(85, 19);
			this.label_FanL.TabIndex = 8;
			this.label_FanL.Text = "风扇一档";
			this.label_FanL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button_EWP.BackgroundImage = WDPower.Properties.Resources.Off_D;
			this.button_EWP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button_EWP.FlatAppearance.BorderSize = 0;
			this.button_EWP.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.button_EWP.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.button_EWP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_EWP.Location = new System.Drawing.Point(74, 55);
			this.button_EWP.Name = "button_EWP";
			this.button_EWP.Size = new System.Drawing.Size(60, 27);
			this.button_EWP.TabIndex = 7;
			this.button_EWP.UseVisualStyleBackColor = true;
			this.button_EWP.Click += new System.EventHandler(button_EWP_Click);
			this.label_EWP.AutoSize = true;
			this.label_EWP.Location = new System.Drawing.Point(0, 59);
			this.label_EWP.Margin = new System.Windows.Forms.Padding(0);
			this.label_EWP.Name = "label_EWP";
			this.label_EWP.Size = new System.Drawing.Size(47, 19);
			this.label_EWP.TabIndex = 6;
			this.label_EWP.Text = "水泵";
			this.label_Test.AutoSize = true;
			this.label_Test.Location = new System.Drawing.Point(0, 14);
			this.label_Test.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.label_Test.Name = "label_Test";
			this.label_Test.Size = new System.Drawing.Size(47, 19);
			this.label_Test.TabIndex = 5;
			this.label_Test.Text = "测试";
			this.label_Test.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.button_Test.BackgroundImage = (System.Drawing.Image)Resources.button_Test_BackgroundImage;
			this.button_Test.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button_Test.FlatAppearance.BorderSize = 0;
			this.button_Test.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.button_Test.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.button_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_Test.Location = new System.Drawing.Point(74, 10);
			this.button_Test.Name = "button_Test";
			this.button_Test.Size = new System.Drawing.Size(60, 27);
			this.button_Test.TabIndex = 0;
			this.button_Test.UseVisualStyleBackColor = true;
			this.button_Test.Click += new System.EventHandler(button_Test_Click);
			this.gB_progressBar.Controls.Add(this.lab_strad);
			this.gB_progressBar.Controls.Add(this.lab_lft);
			this.gB_progressBar.Controls.Add(this.lab_accl);
			this.gB_progressBar.Controls.Add(this.progressBar_TrAngAd);
			this.gB_progressBar.Controls.Add(this.label_TrAngAd);
			this.gB_progressBar.Controls.Add(this.label_LiftAd);
			this.gB_progressBar.Controls.Add(this.progressBar_LiftAd);
			this.gB_progressBar.Controls.Add(this.label_Accle);
			this.gB_progressBar.Controls.Add(this.progressBar_AccleAd);
			this.gB_progressBar.Location = new System.Drawing.Point(460, 270);
			this.gB_progressBar.Name = "gB_progressBar";
			this.gB_progressBar.Size = new System.Drawing.Size(310, 114);
			this.gB_progressBar.TabIndex = 18;
			this.gB_progressBar.TabStop = false;
			this.lab_strad.Location = new System.Drawing.Point(246, 82);
			this.lab_strad.Name = "lab_strad";
			this.lab_strad.Size = new System.Drawing.Size(50, 20);
			this.lab_strad.TabIndex = 31;
			this.lab_strad.Text = "100";
			this.lab_lft.Location = new System.Drawing.Point(246, 52);
			this.lab_lft.Name = "lab_lft";
			this.lab_lft.Size = new System.Drawing.Size(50, 20);
			this.lab_lft.TabIndex = 31;
			this.lab_lft.Text = "100";
			this.lab_accl.Location = new System.Drawing.Point(246, 23);
			this.lab_accl.Name = "lab_accl";
			this.lab_accl.Size = new System.Drawing.Size(50, 20);
			this.lab_accl.TabIndex = 31;
			this.lab_accl.Text = "100";
			this.progressBar_TrAngAd.Location = new System.Drawing.Point(111, 82);
			this.progressBar_TrAngAd.Name = "progressBar_TrAngAd";
			this.progressBar_TrAngAd.Size = new System.Drawing.Size(120, 20);
			this.progressBar_TrAngAd.TabIndex = 6;
			this.label_TrAngAd.AutoSize = true;
			this.label_TrAngAd.Location = new System.Drawing.Point(5, 82);
			this.label_TrAngAd.Name = "label_TrAngAd";
			this.label_TrAngAd.Size = new System.Drawing.Size(85, 19);
			this.label_TrAngAd.TabIndex = 5;
			this.label_TrAngAd.Text = "转向角度";
			this.label_LiftAd.AutoSize = true;
			this.label_LiftAd.Location = new System.Drawing.Point(5, 53);
			this.label_LiftAd.Name = "label_LiftAd";
			this.label_LiftAd.Size = new System.Drawing.Size(85, 19);
			this.label_LiftAd.TabIndex = 4;
			this.label_LiftAd.Text = "举升开度";
			this.progressBar_LiftAd.Location = new System.Drawing.Point(111, 52);
			this.progressBar_LiftAd.Name = "progressBar_LiftAd";
			this.progressBar_LiftAd.Size = new System.Drawing.Size(120, 20);
			this.progressBar_LiftAd.TabIndex = 3;
			this.label_Accle.AutoSize = true;
			this.label_Accle.Location = new System.Drawing.Point(5, 24);
			this.label_Accle.Name = "label_Accle";
			this.label_Accle.Size = new System.Drawing.Size(104, 19);
			this.label_Accle.TabIndex = 2;
			this.label_Accle.Text = "加速器开度";
			this.progressBar_AccleAd.Location = new System.Drawing.Point(111, 23);
			this.progressBar_AccleAd.Name = "progressBar_AccleAd";
			this.progressBar_AccleAd.Size = new System.Drawing.Size(120, 20);
			this.progressBar_AccleAd.TabIndex = 0;
			this.gB_flags.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.gB_flags.Controls.Add(panel14);
			this.gB_flags.Controls.Add(panel7);
			this.gB_flags.Controls.Add(panel13);
			this.gB_flags.Controls.Add(panel6);
			this.gB_flags.Controls.Add(panel12);
			this.gB_flags.Controls.Add(panel5);
			this.gB_flags.Controls.Add(panel11);
			this.gB_flags.Controls.Add(panel4);
			this.gB_flags.Controls.Add(panel10);
			this.gB_flags.Controls.Add(panel3);
			this.gB_flags.Controls.Add(panel9);
			this.gB_flags.Controls.Add(panel2);
			this.gB_flags.Controls.Add(panel8);
			this.gB_flags.Controls.Add(panel);
			this.gB_flags.Location = new System.Drawing.Point(458, -2);
			this.gB_flags.Name = "gB_flags";
			this.gB_flags.Size = new System.Drawing.Size(314, 284);
			this.gB_flags.TabIndex = 17;
			this.gB_flags.TabStop = false;
			this.gB_BMS_ISR.Controls.Add(this.button_BMS_ISR);
			this.gB_BMS_ISR.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_BMS_ISR.Location = new System.Drawing.Point(13, 394);
			this.gB_BMS_ISR.Name = "gB_BMS_ISR";
			this.gB_BMS_ISR.Size = new System.Drawing.Size(130, 73);
			this.gB_BMS_ISR.TabIndex = 15;
			this.gB_BMS_ISR.TabStop = false;
			this.gB_BMS_ISR.Text = "绝缘电阻";
			this.button_BMS_ISR.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_BMS_ISR.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_BMS_ISR.Location = new System.Drawing.Point(3, 24);
			this.button_BMS_ISR.Name = "button_BMS_ISR";
			this.button_BMS_ISR.Size = new System.Drawing.Size(124, 46);
			this.button_BMS_ISR.TabIndex = 1;
			this.button_BMS_ISR.Text = "00 MΩ";
			this.button_BMS_ISR.UseVisualStyleBackColor = true;
			this.gB_MtB_temp.Controls.Add(this.button_MtB_temp);
			this.gB_MtB_temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MtB_temp.Location = new System.Drawing.Point(324, 202);
			this.gB_MtB_temp.Name = "gB_MtB_temp";
			this.gB_MtB_temp.Size = new System.Drawing.Size(130, 73);
			this.gB_MtB_temp.TabIndex = 14;
			this.gB_MtB_temp.TabStop = false;
			this.gB_MtB_temp.Text = "电机温度2";
			this.button_MtB_temp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MtB_temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MtB_temp.Location = new System.Drawing.Point(3, 24);
			this.button_MtB_temp.Name = "button_MtB_temp";
			this.button_MtB_temp.Size = new System.Drawing.Size(124, 46);
			this.button_MtB_temp.TabIndex = 1;
			this.button_MtB_temp.Text = "000 ℃";
			this.button_MtB_temp.UseVisualStyleBackColor = true;
			this.gB_MtA_temp.Controls.Add(this.button_MtA_temp);
			this.gB_MtA_temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MtA_temp.Location = new System.Drawing.Point(168, 202);
			this.gB_MtA_temp.Name = "gB_MtA_temp";
			this.gB_MtA_temp.Size = new System.Drawing.Size(130, 73);
			this.gB_MtA_temp.TabIndex = 13;
			this.gB_MtA_temp.TabStop = false;
			this.gB_MtA_temp.Text = "电机温度1";
			this.button_MtA_temp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MtA_temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MtA_temp.Location = new System.Drawing.Point(3, 24);
			this.button_MtA_temp.Name = "button_MtA_temp";
			this.button_MtA_temp.Size = new System.Drawing.Size(124, 46);
			this.button_MtA_temp.TabIndex = 1;
			this.button_MtA_temp.Text = "000 ℃";
			this.button_MtA_temp.UseVisualStyleBackColor = true;
			this.gB_MCUB_Temp.Controls.Add(this.button_MCUB_Temp);
			this.gB_MCUB_Temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MCUB_Temp.Location = new System.Drawing.Point(324, 10);
			this.gB_MCUB_Temp.Name = "gB_MCUB_Temp";
			this.gB_MCUB_Temp.Size = new System.Drawing.Size(130, 73);
			this.gB_MCUB_Temp.TabIndex = 12;
			this.gB_MCUB_Temp.TabStop = false;
			this.gB_MCUB_Temp.Text = "电控温度2";
			this.button_MCUB_Temp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MCUB_Temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MCUB_Temp.Location = new System.Drawing.Point(3, 24);
			this.button_MCUB_Temp.Name = "button_MCUB_Temp";
			this.button_MCUB_Temp.Size = new System.Drawing.Size(124, 46);
			this.button_MCUB_Temp.TabIndex = 1;
			this.button_MCUB_Temp.Text = "000 ℃";
			this.button_MCUB_Temp.UseVisualStyleBackColor = true;
			this.gB_MCUA_Temp.Controls.Add(this.button_MCUA_Temp);
			this.gB_MCUA_Temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_MCUA_Temp.Location = new System.Drawing.Point(168, 10);
			this.gB_MCUA_Temp.Name = "gB_MCUA_Temp";
			this.gB_MCUA_Temp.Size = new System.Drawing.Size(130, 73);
			this.gB_MCUA_Temp.TabIndex = 11;
			this.gB_MCUA_Temp.TabStop = false;
			this.gB_MCUA_Temp.Text = "电控温度1";
			this.button_MCUA_Temp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_MCUA_Temp.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_MCUA_Temp.Location = new System.Drawing.Point(3, 24);
			this.button_MCUA_Temp.Name = "button_MCUA_Temp";
			this.button_MCUA_Temp.Size = new System.Drawing.Size(124, 46);
			this.button_MCUA_Temp.TabIndex = 1;
			this.button_MCUA_Temp.Text = "000 ℃";
			this.button_MCUA_Temp.UseVisualStyleBackColor = true;
			this.gB_BMS_SOC.Controls.Add(this.button_BMS_SOC);
			this.gB_BMS_SOC.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_BMS_SOC.Location = new System.Drawing.Point(13, 106);
			this.gB_BMS_SOC.Name = "gB_BMS_SOC";
			this.gB_BMS_SOC.Size = new System.Drawing.Size(130, 73);
			this.gB_BMS_SOC.TabIndex = 6;
			this.gB_BMS_SOC.TabStop = false;
			this.gB_BMS_SOC.Text = "电量";
			this.button_BMS_SOC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_BMS_SOC.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_BMS_SOC.Location = new System.Drawing.Point(3, 24);
			this.button_BMS_SOC.Name = "button_BMS_SOC";
			this.button_BMS_SOC.Size = new System.Drawing.Size(124, 46);
			this.button_BMS_SOC.TabIndex = 1;
			this.button_BMS_SOC.Text = "000 %";
			this.button_BMS_SOC.UseVisualStyleBackColor = true;
			this.gB_BMS_HiVlt.Controls.Add(this.button_BMS_HiVlt);
			this.gB_BMS_HiVlt.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_BMS_HiVlt.Location = new System.Drawing.Point(13, 202);
			this.gB_BMS_HiVlt.Name = "gB_BMS_HiVlt";
			this.gB_BMS_HiVlt.Size = new System.Drawing.Size(130, 73);
			this.gB_BMS_HiVlt.TabIndex = 5;
			this.gB_BMS_HiVlt.TabStop = false;
			this.gB_BMS_HiVlt.Text = "锂电池电压";
			this.button_BMS_HiVlt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_BMS_HiVlt.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_BMS_HiVlt.Location = new System.Drawing.Point(3, 24);
			this.button_BMS_HiVlt.Name = "button_BMS_HiVlt";
			this.button_BMS_HiVlt.Size = new System.Drawing.Size(124, 46);
			this.button_BMS_HiVlt.TabIndex = 1;
			this.button_BMS_HiVlt.Text = "000.0 V";
			this.button_BMS_HiVlt.UseVisualStyleBackColor = true;
			this.gB_DCDC_LoVlt.Controls.Add(this.button_DCDC_LoVlt);
			this.gB_DCDC_LoVlt.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_DCDC_LoVlt.Location = new System.Drawing.Point(168, 394);
			this.gB_DCDC_LoVlt.Name = "gB_DCDC_LoVlt";
			this.gB_DCDC_LoVlt.Size = new System.Drawing.Size(130, 73);
			this.gB_DCDC_LoVlt.TabIndex = 4;
			this.gB_DCDC_LoVlt.TabStop = false;
			this.gB_DCDC_LoVlt.Text = "蓄电池电压";
			this.button_DCDC_LoVlt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_DCDC_LoVlt.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_DCDC_LoVlt.Location = new System.Drawing.Point(3, 24);
			this.button_DCDC_LoVlt.Name = "button_DCDC_LoVlt";
			this.button_DCDC_LoVlt.Size = new System.Drawing.Size(124, 46);
			this.button_DCDC_LoVlt.TabIndex = 1;
			this.button_DCDC_LoVlt.Text = "00.0 V";
			this.button_DCDC_LoVlt.UseVisualStyleBackColor = true;
			this.gB_VCU_Vspd.Controls.Add(this.button_VCU_Vspd);
			this.gB_VCU_Vspd.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.gB_VCU_Vspd.Location = new System.Drawing.Point(13, 10);
			this.gB_VCU_Vspd.Name = "gB_VCU_Vspd";
			this.gB_VCU_Vspd.Size = new System.Drawing.Size(130, 73);
			this.gB_VCU_Vspd.TabIndex = 0;
			this.gB_VCU_Vspd.TabStop = false;
			this.gB_VCU_Vspd.Text = "车速";
			this.button_VCU_Vspd.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button_VCU_Vspd.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_VCU_Vspd.Location = new System.Drawing.Point(3, 24);
			this.button_VCU_Vspd.Name = "button_VCU_Vspd";
			this.button_VCU_Vspd.Size = new System.Drawing.Size(124, 46);
			this.button_VCU_Vspd.TabIndex = 1;
			this.button_VCU_Vspd.Text = "00.0 km/h";
			this.button_VCU_Vspd.UseVisualStyleBackColor = true;
			this.tp2.Controls.Add(this.button_parPrgm);
			this.tp2.Controls.Add(this.button_parSd);
			this.tp2.Controls.Add(this.button_parRd);
			this.tp2.Controls.Add(this.dataGridView1);
			this.tp2.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.tp2.Location = new System.Drawing.Point(4, 34);
			this.tp2.Margin = new System.Windows.Forms.Padding(1);
			this.tp2.Name = "tp2";
			this.tp2.Padding = new System.Windows.Forms.Padding(1);
			this.tp2.Size = new System.Drawing.Size(775, 575);
			this.tp2.TabIndex = 1;
			this.tp2.Text = "参数调试";
			this.tp2.UseVisualStyleBackColor = true;
			this.button_parPrgm.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_parPrgm.Location = new System.Drawing.Point(633, 6);
			this.button_parPrgm.Name = "button_parPrgm";
			this.button_parPrgm.Size = new System.Drawing.Size(94, 33);
			this.button_parPrgm.TabIndex = 6;
			this.button_parPrgm.Text = "Program";
			this.button_parPrgm.UseVisualStyleBackColor = true;
			this.button_parPrgm.Click += new System.EventHandler(button_parPrgm_Click);
			this.button_parSd.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_parSd.Location = new System.Drawing.Point(220, 6);
			this.button_parSd.Name = "button_parSd";
			this.button_parSd.Size = new System.Drawing.Size(131, 33);
			this.button_parSd.TabIndex = 5;
			this.button_parSd.Text = "Send";
			this.button_parSd.UseVisualStyleBackColor = true;
			this.button_parSd.Click += new System.EventHandler(button_parSd_Click);
			this.button_parRd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_parRd.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_parRd.Location = new System.Drawing.Point(8, 6);
			this.button_parRd.Name = "button_parRd";
			this.button_parRd.Size = new System.Drawing.Size(131, 33);
			this.button_parRd.TabIndex = 4;
			this.button_parRd.Text = "Read";
			this.button_parRd.UseVisualStyleBackColor = true;
			this.button_parRd.Click += new System.EventHandler(button_parRd_Click);
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToResizeColumns = false;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(this.Column1, this.Column2, this.Column3, this.Column4, this.Column5, this.Column6);
			this.dataGridView1.Location = new System.Drawing.Point(3, 45);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowHeadersWidth = 51;
			this.dataGridView1.RowTemplate.Height = 27;
			this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.Size = new System.Drawing.Size(764, 524);
			this.dataGridView1.TabIndex = 2;
			this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.Column1.DividerWidth = 1;
			this.Column1.FillWeight = 1f;
			this.Column1.HeaderText = "Nm";
			this.Column1.MaxInputLength = 2;
			this.Column1.MinimumWidth = 6;
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column1.ToolTipText = "Number";
			this.Column1.Width = 38;
			this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column2.DividerWidth = 1;
			this.Column2.HeaderText = "Name";
			this.Column2.MinimumWidth = 6;
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column2.ToolTipText = "VariableName";
			this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.Column3.DividerWidth = 1;
			this.Column3.FillWeight = 1f;
			this.Column3.HeaderText = "CurVal";
			this.Column3.MaxInputLength = 6;
			this.Column3.MinimumWidth = 6;
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column3.ToolTipText = "CurrentValue";
			this.Column3.Width = 76;
			this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.Column4.DividerWidth = 1;
			this.Column4.FillWeight = 1f;
			this.Column4.HeaderText = "SetVal";
			this.Column4.MaxInputLength = 6;
			this.Column4.MinimumWidth = 6;
			this.Column4.Name = "Column4";
			this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column4.ToolTipText = "SetValue";
			this.Column4.Width = 76;
			this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.Column5.DividerWidth = 1;
			this.Column5.FillWeight = 1f;
			this.Column5.HeaderText = "  Min  ";
			this.Column5.MaxInputLength = 6;
			this.Column5.MinimumWidth = 6;
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column5.ToolTipText = "Minimum limit";
			this.Column5.Width = 78;
			this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.Column6.DividerWidth = 1;
			this.Column6.FillWeight = 1f;
			this.Column6.HeaderText = "  Max  ";
			this.Column6.MaxInputLength = 6;
			this.Column6.MinimumWidth = 6;
			this.Column6.Name = "Column6";
			this.Column6.ReadOnly = true;
			this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column6.ToolTipText = "Maximum limit";
			this.Column6.Width = 78;
			this.tp3.Controls.Add(this.panel_prgMsk);
			this.tp3.Controls.Add(this.button_run);
			this.tp3.Controls.Add(this.panel16);
			this.tp3.Controls.Add(this.button_Log);
			this.tp3.Controls.Add(this.textBox_skey);
			this.tp3.Controls.Add(this.textBox_filePath);
			this.tp3.Controls.Add(this.progressBar_program);
			this.tp3.Controls.Add(this.button_loadFile);
			this.tp3.Controls.Add(this.button_cnnect);
			this.tp3.Controls.Add(this.label_run);
			this.tp3.Controls.Add(this.label_program);
			this.tp3.Controls.Add(this.label_cnnt);
			this.tp3.Controls.Add(this.label_loadFile);
			this.tp3.Controls.Add(this.button_program);
			this.tp3.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.tp3.Location = new System.Drawing.Point(4, 34);
			this.tp3.Margin = new System.Windows.Forms.Padding(1);
			this.tp3.Name = "tp3";
			this.tp3.Padding = new System.Windows.Forms.Padding(1);
			this.tp3.Size = new System.Drawing.Size(775, 575);
			this.tp3.TabIndex = 2;
			this.tp3.Text = "软件升级";
			this.tp3.UseVisualStyleBackColor = true;
			this.panel_prgMsk.Location = new System.Drawing.Point(35, 61);
			this.panel_prgMsk.Name = "panel_prgMsk";
			this.panel_prgMsk.Size = new System.Drawing.Size(638, 490);
			this.panel_prgMsk.TabIndex = 17;
			this.button_run.AutoSize = true;
			this.button_run.BackColor = System.Drawing.Color.Transparent;
			this.button_run.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_run.Location = new System.Drawing.Point(92, 426);
			this.button_run.Name = "button_run";
			this.button_run.Size = new System.Drawing.Size(250, 31);
			this.button_run.TabIndex = 10;
			this.button_run.Text = "Restart key,please!";
			this.button_run.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button_run.UseVisualStyleBackColor = true;
			this.button_run.Visible = false;
			this.button_run.Click += new System.EventHandler(button_run_Click);
			this.panel16.Controls.Add(this.label_verChk);
			this.panel16.Controls.Add(this.label3);
			this.panel16.Controls.Add(this.label_PCver);
			this.panel16.Controls.Add(this.label_ECUver);
			this.panel16.Location = new System.Drawing.Point(52, 83);
			this.panel16.Name = "panel16";
			this.panel16.Size = new System.Drawing.Size(427, 86);
			this.panel16.TabIndex = 19;
			this.label_verChk.AutoSize = true;
			this.label_verChk.BackColor = System.Drawing.Color.Transparent;
			this.label_verChk.Location = new System.Drawing.Point(276, 46);
			this.label_verChk.Name = "label_verChk";
			this.label_verChk.Size = new System.Drawing.Size(89, 19);
			this.label_verChk.TabIndex = 0;
			this.label_verChk.Text = "VerCheck";
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.Location = new System.Drawing.Point(13, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(150, 20);
			this.label3.TabIndex = 18;
			this.label3.Text = "SevExt:030301";
			this.label_PCver.BackColor = System.Drawing.Color.Transparent;
			this.label_PCver.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label_PCver.Location = new System.Drawing.Point(232, 7);
			this.label_PCver.Name = "label_PCver";
			this.label_PCver.Size = new System.Drawing.Size(190, 20);
			this.label_PCver.TabIndex = 18;
			this.label_PCver.Text = "FPC:3007163101";
			this.label_ECUver.BackColor = System.Drawing.Color.Transparent;
			this.label_ECUver.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label_ECUver.Location = new System.Drawing.Point(13, 7);
			this.label_ECUver.Name = "label_ECUver";
			this.label_ECUver.Size = new System.Drawing.Size(190, 20);
			this.label_ECUver.TabIndex = 0;
			this.label_ECUver.Text = "ECU:3007163101";
			this.button_Log.Location = new System.Drawing.Point(332, 27);
			this.button_Log.Name = "button_Log";
			this.button_Log.Size = new System.Drawing.Size(87, 28);
			this.button_Log.TabIndex = 16;
			this.button_Log.Text = "Login";
			this.button_Log.UseVisualStyleBackColor = true;
			this.button_Log.Click += new System.EventHandler(button_Log_Click);
			this.textBox_skey.Location = new System.Drawing.Point(92, 27);
			this.textBox_skey.Name = "textBox_skey";
			this.textBox_skey.PasswordChar = '*';
			this.textBox_skey.Size = new System.Drawing.Size(197, 28);
			this.textBox_skey.TabIndex = 15;
			this.textBox_skey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox_skey_KeyPress);
			this.textBox_filePath.Location = new System.Drawing.Point(189, 283);
			this.textBox_filePath.Name = "textBox_filePath";
			this.textBox_filePath.ReadOnly = true;
			this.textBox_filePath.Size = new System.Drawing.Size(245, 28);
			this.textBox_filePath.TabIndex = 14;
			this.textBox_filePath.Visible = false;
			this.progressBar_program.Location = new System.Drawing.Point(189, 359);
			this.progressBar_program.Name = "progressBar_program";
			this.progressBar_program.Size = new System.Drawing.Size(245, 23);
			this.progressBar_program.TabIndex = 13;
			this.button_loadFile.Location = new System.Drawing.Point(92, 283);
			this.button_loadFile.Name = "button_loadFile";
			this.button_loadFile.Size = new System.Drawing.Size(90, 29);
			this.button_loadFile.TabIndex = 12;
			this.button_loadFile.Text = "Load";
			this.button_loadFile.UseVisualStyleBackColor = true;
			this.button_loadFile.Click += new System.EventHandler(button_loadFile_Click);
			this.button_cnnect.Location = new System.Drawing.Point(92, 212);
			this.button_cnnect.Name = "button_cnnect";
			this.button_cnnect.Size = new System.Drawing.Size(90, 30);
			this.button_cnnect.TabIndex = 9;
			this.button_cnnect.Text = "Connect";
			this.button_cnnect.UseVisualStyleBackColor = true;
			this.button_cnnect.Click += new System.EventHandler(button_cnnect_Click);
			this.label_run.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.label_run.Location = new System.Drawing.Point(48, 403);
			this.label_run.Name = "label_run";
			this.label_run.Size = new System.Drawing.Size(239, 20);
			this.label_run.TabIndex = 5;
			this.label_run.Text = "Step4 - 启动程序";
			this.label_program.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.label_program.Location = new System.Drawing.Point(48, 330);
			this.label_program.Name = "label_program";
			this.label_program.Size = new System.Drawing.Size(239, 20);
			this.label_program.TabIndex = 6;
			this.label_program.Text = "Step3 - 烧写";
			this.label_cnnt.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.label_cnnt.Location = new System.Drawing.Point(48, 181);
			this.label_cnnt.Name = "label_cnnt";
			this.label_cnnt.Size = new System.Drawing.Size(239, 28);
			this.label_cnnt.TabIndex = 7;
			this.label_cnnt.Text = "Step1 - 连接ECU";
			this.label_loadFile.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.label_loadFile.Location = new System.Drawing.Point(48, 257);
			this.label_loadFile.Name = "label_loadFile";
			this.label_loadFile.Size = new System.Drawing.Size(239, 28);
			this.label_loadFile.TabIndex = 7;
			this.label_loadFile.Text = "Step2 - 加载文件";
			this.button_program.Location = new System.Drawing.Point(92, 359);
			this.button_program.Name = "button_program";
			this.button_program.Size = new System.Drawing.Size(90, 28);
			this.button_program.TabIndex = 11;
			this.button_program.Text = "Program";
			this.button_program.UseVisualStyleBackColor = true;
			this.button_program.Click += new System.EventHandler(button_program_Click);
			this.tp4.Controls.Add(this.dataGridView3);
			this.tp4.Controls.Add(this.dataGridView2);
			this.tp4.Location = new System.Drawing.Point(4, 34);
			this.tp4.Margin = new System.Windows.Forms.Padding(1);
			this.tp4.Name = "tp4";
			this.tp4.Padding = new System.Windows.Forms.Padding(1);
			this.tp4.Size = new System.Drawing.Size(775, 575);
			this.tp4.TabIndex = 3;
			this.tp4.Text = "电池详情";
			this.tp4.UseVisualStyleBackColor = true;
			this.dataGridView3.AllowUserToAddRows = false;
			this.dataGridView3.AllowUserToDeleteRows = false;
			this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView3.Columns.AddRange(this.dataGridViewTextBoxColumn1, this.dataGridViewTextBoxColumn2, this.dataGridViewTextBoxColumn3, this.dataGridViewTextBoxColumn4, this.dataGridViewTextBoxColumn5, this.dataGridViewTextBoxColumn6, this.dataGridViewTextBoxColumn7, this.dataGridViewTextBoxColumn8, this.dataGridViewTextBoxColumn9, this.dataGridViewTextBoxColumn10, this.dataGridViewTextBoxColumn11, this.dataGridViewTextBoxColumn12);
			this.dataGridView3.Location = new System.Drawing.Point(2, 367);
			this.dataGridView3.MultiSelect = false;
			this.dataGridView3.Name = "dataGridView3";
			this.dataGridView3.ReadOnly = true;
			this.dataGridView3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.dataGridView3.RowHeadersVisible = false;
			this.dataGridView3.RowHeadersWidth = 51;
			this.dataGridView3.RowTemplate.Height = 27;
			this.dataGridView3.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dataGridView3.Size = new System.Drawing.Size(769, 156);
			this.dataGridView3.TabIndex = 1;
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn1.HeaderText = "Nm";
			this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 35;
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.HeaderText = "T ℃";
			this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn3.HeaderText = "Nm";
			this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 35;
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn4.HeaderText = "T ℃";
			this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn5.HeaderText = "Nm";
			this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.Width = 35;
			this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn6.HeaderText = "T ℃";
			this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn7.HeaderText = "Nm";
			this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			this.dataGridViewTextBoxColumn7.Width = 35;
			this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn8.HeaderText = "T ℃";
			this.dataGridViewTextBoxColumn8.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn9.HeaderText = "Nm";
			this.dataGridViewTextBoxColumn9.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			this.dataGridViewTextBoxColumn9.Width = 35;
			this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn10.HeaderText = "T ℃";
			this.dataGridViewTextBoxColumn10.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.ReadOnly = true;
			this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn11.HeaderText = "Nm";
			this.dataGridViewTextBoxColumn11.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			this.dataGridViewTextBoxColumn11.Width = 35;
			this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn12.HeaderText = "T ℃";
			this.dataGridViewTextBoxColumn12.MinimumWidth = 6;
			this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			this.dataGridViewTextBoxColumn12.ReadOnly = true;
			this.dataGridView2.AllowUserToAddRows = false;
			this.dataGridView2.AllowUserToDeleteRows = false;
			this.dataGridView2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView2.Columns.AddRange(this.C1, this.C2, this.C3, this.C4, this.C5, this.C6, this.C7, this.C8, this.C9, this.C10, this.C11, this.C12);
			this.dataGridView2.Location = new System.Drawing.Point(0, 0);
			this.dataGridView2.Margin = new System.Windows.Forms.Padding(1);
			this.dataGridView2.MultiSelect = false;
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.ReadOnly = true;
			this.dataGridView2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.dataGridView2.RowHeadersVisible = false;
			this.dataGridView2.RowHeadersWidth = 51;
			this.dataGridView2.RowTemplate.Height = 27;
			this.dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dataGridView2.Size = new System.Drawing.Size(770, 325);
			this.dataGridView2.StandardTab = true;
			this.dataGridView2.TabIndex = 0;
			this.dataGridView2.TabStop = false;
			this.C1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.C1.HeaderText = "Nm";
			this.C1.MinimumWidth = 6;
			this.C1.Name = "C1";
			this.C1.ReadOnly = true;
			this.C1.Width = 35;
			this.C2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.C2.HeaderText = "Vlt ";
			this.C2.MinimumWidth = 6;
			this.C2.Name = "C2";
			this.C2.ReadOnly = true;
			this.C2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.C3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.C3.HeaderText = "Nm";
			this.C3.MinimumWidth = 6;
			this.C3.Name = "C3";
			this.C3.ReadOnly = true;
			this.C3.Width = 35;
			this.C4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.C4.HeaderText = "Vlt ";
			this.C4.MinimumWidth = 6;
			this.C4.Name = "C4";
			this.C4.ReadOnly = true;
			this.C5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.C5.HeaderText = "Nm";
			this.C5.MinimumWidth = 6;
			this.C5.Name = "C5";
			this.C5.ReadOnly = true;
			this.C5.Width = 35;
			this.C6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.C6.HeaderText = "Vlt ";
			this.C6.MinimumWidth = 6;
			this.C6.Name = "C6";
			this.C6.ReadOnly = true;
			this.C7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.C7.HeaderText = "Nm";
			this.C7.MinimumWidth = 6;
			this.C7.Name = "C7";
			this.C7.ReadOnly = true;
			this.C7.Width = 35;
			this.C8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.C8.HeaderText = "Vlt ";
			this.C8.MinimumWidth = 6;
			this.C8.Name = "C8";
			this.C8.ReadOnly = true;
			this.C9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.C9.HeaderText = "Nm";
			this.C9.MinimumWidth = 6;
			this.C9.Name = "C9";
			this.C9.ReadOnly = true;
			this.C9.Width = 35;
			this.C10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.C10.HeaderText = "Vlt ";
			this.C10.MinimumWidth = 6;
			this.C10.Name = "C10";
			this.C10.ReadOnly = true;
			this.C11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.C11.HeaderText = "Nm";
			this.C11.MinimumWidth = 6;
			this.C11.Name = "C11";
			this.C11.ReadOnly = true;
			this.C11.Width = 35;
			this.C12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.C12.HeaderText = "Vlt ";
			this.C12.MinimumWidth = 6;
			this.C12.Name = "C12";
			this.C12.ReadOnly = true;
			this.statusStrip_lang.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip_lang.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripDrpDwBtt_lang, this.toolStripStatusLabel1 });
			this.statusStrip_lang.Location = new System.Drawing.Point(0, 615);
			this.statusStrip_lang.Name = "statusStrip_lang";
			this.statusStrip_lang.Size = new System.Drawing.Size(783, 26);
			this.statusStrip_lang.TabIndex = 21;
			this.statusStrip_lang.Text = "Language";
			this.toolStripDrpDwBtt_lang.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDrpDwBtt_lang.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.eNToolStripMenuItem, this.cHToolStripMenuItem });
			this.toolStripDrpDwBtt_lang.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDrpDwBtt_lang.Name = "toolStripDrpDwBtt_lang";
			this.toolStripDrpDwBtt_lang.Size = new System.Drawing.Size(94, 24);
			this.toolStripDrpDwBtt_lang.Text = "Language";
			this.toolStripDrpDwBtt_lang.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.eNToolStripMenuItem.Name = "eNToolStripMenuItem";
			this.eNToolStripMenuItem.Size = new System.Drawing.Size(106, 26);
			this.eNToolStripMenuItem.Text = "EN";
			this.eNToolStripMenuItem.Click += new System.EventHandler(ChTOEN);
			this.cHToolStripMenuItem.Name = "cHToolStripMenuItem";
			this.cHToolStripMenuItem.Size = new System.Drawing.Size(106, 26);
			this.cHToolStripMenuItem.Text = "CH";
			this.cHToolStripMenuItem.Click += new System.EventHandler(ChTOCH);
			this.toolStripStatusLabel1.AutoSize = false;
			this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(0);
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(575, 26);
			this.toolStripStatusLabel1.Text = "Ver 1.2.0";
			this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bkWk_main.DoWork += new System.ComponentModel.DoWorkEventHandler(bkWk_main_DoWork);
			this.timer1.Tick += new System.EventHandler(timer1_Tick);
			this.bkWk_CANRs.DoWork += new System.ComponentModel.DoWorkEventHandler(bkWk_CANRs_DoWork);
			this.bkgrd_program.DoWork += new System.ComponentModel.DoWorkEventHandler(bkgrd_program_DoWork);
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 15f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(783, 641);
			base.Controls.Add(this.statusStrip_lang);
			base.Controls.Add(this.tabControl1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.HelpButton = true;
			base.MaximizeBox = false;
			base.Name = "mForm1";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WD E-Power";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(mForm1_FormClosing);
			base.Load += new System.EventHandler(mForm_Load);
			panel.ResumeLayout(false);
			panel.PerformLayout();
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			panel4.ResumeLayout(false);
			panel4.PerformLayout();
			panel5.ResumeLayout(false);
			panel5.PerformLayout();
			panel6.ResumeLayout(false);
			panel6.PerformLayout();
			panel7.ResumeLayout(false);
			panel7.PerformLayout();
			panel8.ResumeLayout(false);
			panel8.PerformLayout();
			panel9.ResumeLayout(false);
			panel9.PerformLayout();
			panel10.ResumeLayout(false);
			panel10.PerformLayout();
			panel11.ResumeLayout(false);
			panel11.PerformLayout();
			panel12.ResumeLayout(false);
			panel12.PerformLayout();
			panel13.ResumeLayout(false);
			panel13.PerformLayout();
			panel14.ResumeLayout(false);
			panel14.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tp1.ResumeLayout(false);
			this.gB_MCUA_FCD.ResumeLayout(false);
			this.gB_DCDC_FCD.ResumeLayout(false);
			this.gB_MCUB_Spd.ResumeLayout(false);
			this.gB_VCU_FCD.ResumeLayout(false);
			this.gB_MCUB_FCD.ResumeLayout(false);
			this.gB_MCUA_Spd.ResumeLayout(false);
			this.gB_BMS_FCD.ResumeLayout(false);
			this.gB_MCUB_Curr.ResumeLayout(false);
			this.gB_MCUA_Curr.ResumeLayout(false);
			this.gB_BMS_Temp.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.gB_progressBar.ResumeLayout(false);
			this.gB_progressBar.PerformLayout();
			this.gB_flags.ResumeLayout(false);
			this.gB_BMS_ISR.ResumeLayout(false);
			this.gB_MtB_temp.ResumeLayout(false);
			this.gB_MtA_temp.ResumeLayout(false);
			this.gB_MCUB_Temp.ResumeLayout(false);
			this.gB_MCUA_Temp.ResumeLayout(false);
			this.gB_BMS_SOC.ResumeLayout(false);
			this.gB_BMS_HiVlt.ResumeLayout(false);
			this.gB_DCDC_LoVlt.ResumeLayout(false);
			this.gB_VCU_Vspd.ResumeLayout(false);
			this.tp2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			this.tp3.ResumeLayout(false);
			this.tp3.PerformLayout();
			this.panel16.ResumeLayout(false);
			this.panel16.PerformLayout();
			this.tp4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dataGridView3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dataGridView2).EndInit();
			this.statusStrip_lang.ResumeLayout(false);
			this.statusStrip_lang.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}

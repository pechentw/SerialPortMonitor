using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerialPortMonitor
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        private int lineCount = 0;
        //private string currentLogFile = "";
        private List<string> buffer = new List<string>();
        private List<byte> dataBuffer = new List<byte>();
        private bool startFound = false;
        private int missedHeaderCount = 0;  // 找不到標頭的次數
        private const int HEADER_BYTE = 255;
        private const int INTEGER_COUNT = 4;
        private const int MIN_BUFFER_SIZE = 200;  // 最小緩衝區大小（考慮20組數據）
        private const int BYTES_PER_GROUP = 8;   // 每組數據的位元組數（4個16位元整數）
        private const int GROUPS_TO_PROCESS = 20;  // 一次處理的組數
        private Task saveTask = null;
        private bool isSaving = false;
        private object saveLock = new object();

        public Form1()
        {
            InitializeComponent();
            InitializeSerialPort();
        }

        private void InitializeSerialPort()
        {
            serialPort = new SerialPort();
            // 設定通訊協定參數
            serialPort.DataBits = 8;                    // 8位元資料
            serialPort.StopBits = StopBits.One;        // 1個停止位元
            serialPort.Parity = Parity.None;           // 無同位檢查
            serialPort.Handshake = Handshake.None;     // 無流量控制
            serialPort.ReadTimeout = 1000;             // 設定讀取超時時間
            serialPort.WriteTimeout = 1000;            // 設定寫入超時時間
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 填充串列埠列表
            string[] ports = SerialPort.GetPortNames();
            comboBoxPorts.Items.AddRange(ports);
            
            // 尋找並選擇 COM3
            int com3Index = Array.FindIndex(ports, p => p.Equals("COM3", StringComparison.OrdinalIgnoreCase));
            if (com3Index >= 0)
            {
                comboBoxPorts.SelectedIndex = com3Index;
            }
            else if (ports.Length > 0)
            {
                comboBoxPorts.SelectedIndex = 0;
            }

            // 填充鮑率選項
            comboBoxBaudRate.Items.AddRange(new object[] { "115200", "9600", "19200", "38400", "57600" });
            comboBoxBaudRate.SelectedIndex = 0;  // 預設選擇 115200

            // 設定存檔筆數的預設值
            textBoxSaveCount.Text = "10000";
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    // 先暫停接收數據
                    serialPort.DataReceived -= SerialPort_DataReceived;
                    
                    // 清空接收緩衝區
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    
                    // 使用 Task 來非同步關閉串列埠
                    Task.Run(() =>
                    {
                        try
                        {
                            // 等待一小段時間確保沒有正在處理的數據
                            System.Threading.Thread.Sleep(100);
                            
                            // 關閉串列埠
                            serialPort.Close();
                            
                            // 在 UI 執行緒中更新介面
                            this.Invoke(new Action(() =>
                            {
                                UpdateUIAfterDisconnect();
                            }));
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show("斷開連接時發生錯誤: " + ex.Message);
                            }));
                        }
                    });
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show("斷開連接時發生錯誤: " + ex.Message);
                    }));
                }
            }
            else
            {
                try
                {
                    // 重新初始化串列埠
                    InitializeSerialPort();
                    
                    serialPort.PortName = comboBoxPorts.SelectedItem.ToString();
                    serialPort.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
                    
                    // 嘗試打開串列埠
                    serialPort.Open();
                    
                    // 等待一小段時間確保連接穩定
                    System.Threading.Thread.Sleep(500);
                    
                    // 在 UI 執行緒中更新介面
                    this.Invoke(new Action(() =>
                    {
                        UpdateUIAfterConnect();
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show("連接失敗: " + ex.Message);
                    }));
                }
            }
        }

        private void UpdateUIAfterDisconnect()
        {
            buttonConnect.Text = "連接";
            textBoxLog.AppendText("已斷開連接\r\n");
            startFound = false;
            dataBuffer.Clear();
            textBoxBufferSize.Text = "0";
            textBoxBytesToRead.Text = "0";
            missedHeaderCount = 0;
            textBoxMissedHeaders.Text = "0";
        }

        private void UpdateUIAfterConnect()
        {
            buttonConnect.Text = "斷開";
            textBoxLog.AppendText("已連接至 " + serialPort.PortName + "\r\n");
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen) return;

                // 更新 BytesToRead 顯示
                this.Invoke(new Action(() =>
                {
                    textBoxBytesToRead.Text = serialPort.BytesToRead.ToString();
                    textBoxBufferSize.Text = dataBuffer.Count.ToString();
                }));

                // 讀取所有可用的數據
                byte[] newData = new byte[serialPort.BytesToRead];
                serialPort.Read(newData, 0, newData.Length);
                dataBuffer.AddRange(newData);

                // 如果累積的數據超過最小緩衝區大小，則處理數據
                if (dataBuffer.Count >= MIN_BUFFER_SIZE)
                {
                    ProcessBuffer();
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    textBoxLog.AppendText($"錯誤: {ex.Message}\r\n");
                }));
            }
        }

        private short ConvertToInt16(byte high, byte low)
        {
            // 高位元組在前，低位元組在後
            return (short)((high << 8) | low);
        }

        private void ProcessBuffer()
        {
            // 如果數據不足，直接返回等待更多數據
            if (dataBuffer.Count < 2)
            {
                return;
            }

            if (!startFound)
            {
                // 尋找連續兩個255
                if (dataBuffer[0] == HEADER_BYTE && dataBuffer[1] == HEADER_BYTE)
                {
                    startFound = true;
                    dataBuffer.RemoveRange(0, 2); // 移除標頭位元組
                }
                else
                {
                    missedHeaderCount++;  // 增加找不到標頭的計數
                    this.Invoke(new Action(() =>
                    {
                        textBoxMissedHeaders.Text = missedHeaderCount.ToString();
                    }));
                    dataBuffer.RemoveAt(0); // 移除第一個位元組
                }
            }
            else
            {
                // 檢查是否有足夠的數據處理一組
                if (dataBuffer.Count < BYTES_PER_GROUP)
                {
                    return; // 數據不足，等待更多數據
                }

                StringBuilder dataStr = new StringBuilder();
                int groupsProcessed = 0;

                while (groupsProcessed < GROUPS_TO_PROCESS)
                {
                    // 檢查是否有足夠的數據處理下一組
                    if (dataBuffer.Count < BYTES_PER_GROUP)
                    {
                        break; // 數據不足，等待更多數據
                    }

                    int[] values = new int[INTEGER_COUNT];
                    for (int i = 0; i < INTEGER_COUNT; i++)
                    {
                        // 讀取高位元組和低位元組
                        byte highByte = dataBuffer[i * 2];
                        byte lowByte = dataBuffer[i * 2 + 1];
                        values[i] = ConvertToInt16(highByte, lowByte);
                    }
                    //dataStr.AppendFormat("第{0}組數據: {1}, {2}, {3}, {4}\r\n", 
                    //    groupsProcessed + 1, values[0], values[1], values[2], values[3]);
                    dataStr.AppendFormat("{0},{1},{2},{3}", 
                    values[0], values[1], values[2], values[3]);

                    // 如果不是最後一組，添加換行
                    if (groupsProcessed < GROUPS_TO_PROCESS - 1)
                    {
                        dataStr.Append("\r\n");
                    }

                    // 移除已處理的數據
                    dataBuffer.RemoveRange(0, BYTES_PER_GROUP);
                    groupsProcessed++;

                    // 如果還要處理更多組，檢查下一組的標頭
                    if (groupsProcessed < GROUPS_TO_PROCESS)
                    {
                        if (dataBuffer.Count >= 2)
                        {
                            if (dataBuffer[0] == HEADER_BYTE && dataBuffer[1] == HEADER_BYTE)
                            {
                                dataBuffer.RemoveRange(0, 2); // 移除標頭位元組
                            }
                            else
                            {
                                missedHeaderCount++;  // 增加找不到標頭的計數
                                this.Invoke(new Action(() =>
                                {
                                    textBoxMissedHeaders.Text = missedHeaderCount.ToString();
                                }));
                                break; // 沒有找到下一組的標頭，等待更多數據
                            }
                        }
                        else
                        {
                            break; // 數據不足，等待更多數據
                        }
                    }
                }

                // 如果有處理到數據才顯示
                if (groupsProcessed > 0)
                {
                    this.Invoke(new Action(() =>
                    {
                        string finalStr = dataStr.ToString();
                        if (checkBoxShowData.Checked)
                        {
                            textBoxLog.AppendText(finalStr);
                            textBoxLog.AppendText("\r\n"); // for UI display
                            // 檢查 textBoxLog 的記憶體使用量
                            if (Encoding.UTF8.GetByteCount(textBoxLog.Text) > 1024 * 100) // 100KB
                            {
                                textBoxLog.Clear();
                                textBoxLog.AppendText("已清除日誌（記憶體使用量超過 100KB）\r\n");
                            }
                        }
                        buffer.Add(finalStr);
                        lineCount += groupsProcessed;

                        // 使用設定的存檔筆數
                        int saveCount;
                        if (int.TryParse(textBoxSaveCount.Text, out saveCount) && saveCount > 0)
                        {
                            if (lineCount >= saveCount && !isSaving)
                            {
                                StartSaveTask();
                                lineCount = 0; // 重置計數器
                            }
                        }
                    }));
                }

                startFound = false; // 重置標記，開始尋找下一組數據
            }

            // 在處理完數據後更新 dataBuffer 大小顯示
            this.Invoke(new Action(() =>
            {
                textBoxBufferSize.Text = dataBuffer.Count.ToString();
            }));
        }

        private void StartSaveTask()
        {
            if (saveTask == null || saveTask.IsCompleted)
            {
                isSaving = true;
                saveTask = Task.Run(() =>
                {
                    try
                    {
                        SaveLogFile();
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            textBoxLog.AppendText($"存檔時發生錯誤: {ex.Message}\r\n");
                        }));
                    }
                    finally
                    {
                        isSaving = false;
                    }
                });
            }
        }

        private void SaveLogFile()
        {
            List<string> dataToSave;
            lock (saveLock)
            {
                if (buffer.Count == 0)
                {
                    return;
                }
                dataToSave = new List<string>(buffer);
                buffer.Clear();
            }

            string fileName = $"SerialLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            File.WriteAllLines(fileName, dataToSave);
            
            this.Invoke(new Action(() =>
            {
                textBoxLog.AppendText($"已保存日誌文件: {fileName}\r\n");
            }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort != null)
            {
                try
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.DataReceived -= SerialPort_DataReceived;
                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();
                        System.Threading.Thread.Sleep(100);
                        serialPort.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("關閉串列埠時發生錯誤: " + ex.Message);
                }
            }

            // 等待存檔任務完成
            if (saveTask != null && !saveTask.IsCompleted)
            {
                saveTask.Wait();
            }
        }
    }
} 
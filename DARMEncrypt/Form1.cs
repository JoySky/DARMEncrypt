using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace DARMEncrypt
{
    public partial class Form1 : Form
    {
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private string encryptStr;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            int temp = listBoxEncryption.Items.Count;
            if (temp!=0)
            {
                if (DialogResult.Yes == MessageBox.Show("您是否要放弃该文件？", "警告", MessageBoxButtons.YesNo))
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.Close();
            }
            
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string strCode = txtMachineCode.Text;
            string strKey = "DARMYFZX";
            if (strCode==String.Empty)
            {
                MessageBox.Show("请填写机器码！", "警告");
                return;
            }
            encryptStr=EncryptDES(strCode,strKey);
            listBoxEncryption.Items.Add(encryptStr);
        }
        private void btnAutoEncrypt_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            if (path == string.Empty)
            {
                return;
            }
            List<string> lstCode = new List<string>();
            StreamReader objStreamReader = new StreamReader(path, Encoding.Default);
            string str = objStreamReader.ReadLine();

            while (str != null)
            {
                lstCode.Add(str);
                str = objStreamReader.ReadLine();
            }
            objStreamReader.Close();
            string strKey = "DARMYFZX";
            List<string> lstEncry = new List<string>();
            for (int i = 0; i < lstCode.Count;i++ )
            {
                lstEncry.Add(EncryptDES(lstCode[i], strKey));
                listBoxEncryption.Items.Add(lstEncry[i]);
            }

        }
        private void btnBuild_Click(object sender, EventArgs e)
        {
            int temp = listBoxEncryption.Items.Count;
            if (temp == 0)
            {
                MessageBox.Show("没有生成注册码，不能为您生成注册文件！","警告");
                return;
            }
            saveFileDialog1.ShowDialog();
            string path = saveFileDialog1.FileName;
            if (path==string.Empty)
            {
                return;
            }
            StreamWriter objStreamWriter = new StreamWriter(path, false);
            
            for (int i = 0; i < listBoxEncryption.Items.Count;i++ )
            {
                objStreamWriter.WriteLine(listBoxEncryption.Items[i].ToString());
            }
            
            objStreamWriter.Close();
            MessageBox.Show("文件生成完毕，请替换原有配置文件！","提示");

        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            listBoxEncryption.Items.Clear();
            txtMachineCode.Text = "";
        }
        /// <summary>
        /// DES加密方法
        /// </summary>
        /// <param name="encryptString">待加密字符串</param>
        /// <param name="encryptKey">加密密钥，要求为8位</param>
        /// <returns>加密成功则返回加密后的字符串，否则返回原字符串</returns>
        public string EncryptDES(string encryptString,string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider desCSP=new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, desCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch 
            {
                return encryptString;
            }
        }

        

        
    }
}

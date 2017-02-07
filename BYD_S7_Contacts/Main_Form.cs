using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BYD_S7_Contacts
{
    public partial class Main_Form : Form
    {
        public Main_Form()
        {
            InitializeComponent();
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
            this.txt_说明.Text = @"
工具使用方法：
    1、手机安装【QQ同步助手】
    2、登录并同步通讯录
    3、打开【http://ic.qq.com】并登录
    4、找到：更多操作 -> 导出联系人到本地 -> CSV -> 导出
    5、使用该工具生成数据库
    6、把目录下的【GPS_BYD】目录复制到TF卡中
    7、将TF卡插入S7的MAP插槽，按【导航】即可完成同步


蓝牙名称 查看方法：
    Android查看方法：设置 -> 蓝牙 -> 开启蓝牙 -> 名称
    iPhone查看方法：设置 -> 通用 -> 关于本机 -> 名称

    如不能正常显示，请查看车机中蓝牙设备名称。


分组 说明：
    QQ同步中可以建立分组：
    1、不常用联系人   该分组的联系人不同步
    2、常用联系人     该分组的联系人名称补充0（优先显示）
".Trim();
            this.cbo_NameMerger.SelectedIndex = 0;
        }

        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            string fileName;
            using (var of = new OpenFileDialog())
            {
                of.Filter = "QQ同步文件|*.csv";
                of.AddExtension = true;
                if (of.ShowDialog() == DialogResult.OK)
                {
                    fileName = of.FileName;
                }
                else
                {
                    return;
                }
            }

            if (!File.Exists(fileName))
            {
                MessageBox.Show("文件不存在");
                return;
            }
            this.txt_CSVFile.Text = fileName;
        }

        private void btn_Generate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_CSVFile.Text))
            {
                MessageBox.Show("请先选择文件");
                return;
            }
            if (string.IsNullOrEmpty(this.txt_BluetoothName.Text))
            {
                MessageBox.Show("蓝牙名称不能为空");
                return;
            }
            if (!File.Exists(this.txt_CSVFile.Text))
            {
                MessageBox.Show("文件不存在");
                return;
            }

            this.btn_Generate.Enabled = false;
            var sql = GenerateDatabase(this.txt_CSVFile.Text, this.txt_BluetoothName.Text, this.cbo_NameMerger.SelectedIndex);

            try
            {
                var dir_path = Path.Combine(Application.StartupPath, "GPS_BYD");
                if (Directory.Exists(dir_path))
                {
                    try
                    {
                        Directory.Delete(dir_path, true);
                        Thread.Sleep(500);
                        Directory.CreateDirectory(dir_path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除文件夹失败" + ex.Message);
                        this.btn_Generate.Enabled = true;
                        return;
                    }
                }
                else
                {
                    Directory.CreateDirectory(dir_path);
                }

                var db_path = Path.Combine(dir_path, "BT.db");
                GenerateFile(db_path, "BYD_S7_Contacts.GPS_BYD.BT.db");

                var myconn = new SQLiteConnection($"Data Source={db_path};;Pooling=true;FailIfMissing=false");
                myconn.Open();
                var com = new SQLiteCommand(sql, myconn);
                com.ExecuteNonQuery();
                myconn.Close();
                myconn.Dispose();

                SQLiteConnection.ClearAllPools();

                GC.Collect();
                GC.WaitForPendingFinalizers();


                var path = Path.Combine(dir_path, @"MortScript.exe");
                GenerateFile(path, "BYD_S7_Contacts.GPS_BYD.MortScript.exe");

                path = Path.Combine(dir_path, @"StartUp.exe");
                GenerateFile(path, "BYD_S7_Contacts.GPS_BYD.StartUp.exe");

                path = Path.Combine(dir_path, @"StartUp.mscr");
                GenerateFile(path, "BYD_S7_Contacts.GPS_BYD.StartUp.mscr");

                MessageBox.Show("生成数据成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建数据库失败：" + ex.Message);
            }
            this.btn_Generate.Enabled = true;
        }

        private void GenerateFile(string path, string name)
        {
            if (!File.Exists(path))
            {
                using (var sTream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
                {
                    if (sTream != null)
                    {
                        var bytes = new byte[sTream.Length];
                        sTream.Read(bytes, 0, bytes.Length);
                        using (var fs = new FileStream(path, FileMode.CreateNew))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                            fs.Close();
                        }
                    }
                }
            }
        }

        private string GenerateDatabase(string filename, string devicename, int nametype)
        {
            List<List<string>> rows = CsvFile.Reader(filename, Encoding.Default);
            if (rows.Count == 0)
            {
                MessageBox.Show("没有有效数据");
                return null;
            }

            List<ContactInfo> contacts = null;
            if (rows[0][0] == "Family Name")
            {
                contacts = csv_google(rows);
            }
            else if (rows[0][0] == "姓")
            {
                contacts = csv_outlook(rows);
            }

            if (contacts == null)
            {
                MessageBox.Show("没有有效数据");
                return null;
            }


            var sql = new StringBuilder();

            /*
            sql.AppendLine("DROP TABLE IF EXISTS CallLog;");
            sql.AppendLine("CREATE TABLE CallLog(ID  integer,DeviceName  varchar(32),PhoneNum  varchar(24),Name  varchar(24),TelType  varchar(24));");
            sql.AppendLine();

            sql.AppendLine("DROP TABLE IF EXISTS Device;");
            sql.AppendLine("CREATE TABLE Device(ID  integer,LocalName  varchar(24),LocalPin  varchar(24),IsAutoConnect  varchar(24),IsAutoAnser  varchar(24),IsSound  varchar(24),IsAutoHide  varchar(24),RingVolume  varchar(24),AddSearch  varchar(24));");
            sql.AppendLine("INSERT INTO Device VALUES (1, 'BYD', '0000', 'YES', 'NO', 'YES', 'NO', 0, 'YES');");
            sql.AppendLine();

            sql.AppendLine("DROP TABLE IF EXISTS Paired;");
            sql.AppendLine("CREATE TABLE Paired(ID  integer,PairedDev1  varchar(32),PairedDev2  varchar(32),PairedDev3  varchar(32),PairedDev4  varchar(32),PairedDev5  varchar(32),PairedDev6  varchar(32),PairedDev7  varchar(32),PairedDev8  varchar(32),PairedDev9  varchar(32),PairedDev10  varchar(32),PairedDev11  varchar(32),PairedDev12  varchar(32));");
            sql.AppendLine();
            */

            //sql.AppendLine("UPDATE Paired SET PairedDev1='" + devicename + "' WHERE ID=1;");

            sql.AppendLine("DROP TABLE IF EXISTS Contact;");
            sql.AppendLine("CREATE TABLE Contact(ID  integer,DeviceName  varchar(32),PhoneNum  varchar(24),Name  varchar(24));");
            sql.AppendLine();

            string name, lastname = null;
            int z = 0;
            foreach (var contact in contacts)
            {
                if (contact.Group.IndexOf("不常用联系人", StringComparison.Ordinal) != -1)
                {
                    continue;
                }
                switch (nametype)
                {
                    case 0: //姓氏 + 名字
                        name = contact.FamilyName + contact.GivenName;
                        break;
                    case 1: // 名字 + 姓氏
                        name = contact.GivenName + contact.FamilyName;
                        break;
                    case 2: // 仅姓氏
                        name = contact.FamilyName;
                        break;
                    case 3: //仅名字
                        name = contact.GivenName;
                        break;
                    default:
                        name = contact.FamilyName + contact.GivenName;
                        break;
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = contact.FamilyName + contact.GivenName;
                }

                if (contact.Group.IndexOf("常用联系人", StringComparison.Ordinal) != -1)
                {
                    if (lastname != name)
                    {
                        lastname = name;
                        name = "0" + name;
                    }
                }

                sql.AppendLine("INSERT INTO Contact VALUES (" + (++z) + ", '" + devicename + "', '" + contact.PhoneValue + "', '" + name + "');");
            }

            return sql.ToString();
        }

        private List<ContactInfo> csv_google(List<List<string>> rows)
        {
            var row = rows[0];
            var dic = new Dictionary<string, int>();

            for (int i = 0; i < row.Count; i++)
            {
                switch (row[i])
                {
                    case "Family Name":
                    case "Given Name":
                    case "Nickname":
                        dic.Add(row[i], i);
                        break;
                    case "Group Membership":
                        dic.Add("分组", i);
                        break;
                    default:
                        if (row[i].StartsWith("Phone "))
                        {
                            dic.Add(row[i], i);
                        }
                        break;
                }
            }

            if (!dic.ContainsKey("Family Name") || !dic.ContainsKey("Given Name") || !dic.ContainsKey("Phone 1 - Type"))
            {
                MessageBox.Show("没有必要字段");
                return null;
            }

            var contacts = new List<ContactInfo>();
            ContactInfo contact;
            for (var i = 1; i < rows.Count; i++)
            {
                string type, value;
                var index = (dic.Count - 3) / 2;
                for (int j = 1; j <= index; j++)
                {
                    if (dic.ContainsKey("Phone " + j + " - Type") && dic.ContainsKey("Phone " + j + " - Value"))
                    {
                        type = rows[i][dic["Phone " + j + " - Type"]].Trim();
                        value = rows[i][dic["Phone " + j + " - Value"]].Trim();

                        if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
                        {
                            contact = new ContactInfo();
                            contact.FamilyName = rows[i][dic["Family Name"]].Trim();
                            contact.GivenName = rows[i][dic["Given Name"]].Trim();
                            contact.PhoneType = type;
                            contact.PhoneValue = value;
                            if (dic.ContainsKey("分组"))
                            {
                                contact.Group = rows[i][dic["分组"]];
                            }
                            contacts.Add(contact);
                        }
                    }
                }
            }
            return contacts;
        }

        private List<ContactInfo> csv_outlook(List<List<string>> rows)
        {
            var row = rows[0];
            var dic = new Dictionary<string, int>();

            for (int i = 0; i < row.Count; i++)
            {
                switch (row[i])
                {
                    case "姓":
                    case "名":
                        dic.Add(row[i], i);
                        break;
                    case "类别":
                    case "qq同步助手 分组":
                        dic.Add("分组", i);
                        break;
                    default:
                        if (row[i].IndexOf("电话", StringComparison.Ordinal) != -1)
                        {
                            dic.Add(row[i], i);
                        }
                        break;
                }
            }

            if (!dic.ContainsKey("姓") || !dic.ContainsKey("名"))
            {
                MessageBox.Show("没有必要字段");
                return null;
            }


            var contacts = new List<ContactInfo>();
            ContactInfo contact;

            for (var i = 1; i < rows.Count; i++)
            {
                foreach (var kv in dic)
                {
                    if (kv.Key.IndexOf("电话", StringComparison.Ordinal) != -1)
                    {
                        string value = rows[i][kv.Value].Trim();
                        if (!string.IsNullOrEmpty(value))
                        {
                            contact = new ContactInfo();
                            contact.FamilyName = rows[i][dic["姓"]].Trim();
                            contact.GivenName = rows[i][dic["名"]].Trim();
                            contact.PhoneType = "";
                            contact.PhoneValue = value;
                            if (dic.ContainsKey("分组"))
                            {
                                contact.Group = rows[i][dic["分组"]];
                            }
                            contacts.Add(contact);
                        }
                    }
                }
            }

            return contacts;
        }
    }
}

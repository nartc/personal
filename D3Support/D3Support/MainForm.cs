using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
namespace D3Support
{
    public partial class MainForm : Form
    {
        private ControlMouse cm;
        private bool left = true;
        private bool right = true;
        private bool skill = true;
        private bool hiddenStart = true;
        private Thread thread1;
        private Thread thread2;
        private Thread thread3;
        private Thread thread4;

        private Keys keyS1;
        private Keys keyS2;
        private Keys keyS3;
        private Keys keyS4;
        private Keys keyHps;

        private String sKeyS1 = "1";
        private String sKeyS2 = "2";
        private String sKeyS3 = "3";
        private String sKeyS4 = "4";
        private String sKeyHps = "Q";

        private const int WM_HOTKEY = 0x312;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int DEFAULT_SKILL_TIME = 9;
        private const string DEFAULT_SKILL_TIME_STRING = "9";

        private IntPtr windowToFind;
        private IntPtr param;
        private KeysConverter kc;

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hwnd, int id);
        [DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        public MainForm()
        {
            InitializeComponent();
            cm = new ControlMouse();
            param = new IntPtr(0);
            kc = new KeysConverter();
            RegisterHotKey(Handle, 712, 0, (int)Keys.F1);
            RegisterHotKey(Handle, 713, 0, (int)Keys.F2);
            RegisterHotKey(Handle, 714, 0, (int)Keys.F3);
            //RegisterHotKey(Handle, 715, 0, (int)Keys.F4);
            loadConfig();
        }

        public void sendKeystroke(ushort k)
        {
            SendMessage(windowToFind, WM_KEYDOWN, ((IntPtr)k), param);
            SendMessage(windowToFind, WM_KEYUP, ((IntPtr)k), param);
        }
        public void changeLeft()
        {
            bool e = left;
            ls.Enabled = e;
            lss.Enabled = e;
            lsss.Enabled = e; lssss.Enabled = e;
        }
        public void changeRight()
        {
            bool e = right;
            rs.Enabled = e;
            rss.Enabled = e;
            rsss.Enabled = e;
            rssss.Enabled = e;
        }
        public void changeSkill()
        {
            bool e = skill;

            key1.Enabled = e;
            key2.Enabled = e;
            key3.Enabled = e;
            key4.Enabled = e;
            keyHp.Enabled = e;

            time1.Enabled = e;
            time2.Enabled = e;
            time3.Enabled = e;
            time4.Enabled = e;
            timeHp.Enabled = e;

            s1.Enabled = e;
            s2.Enabled = e;
            s3.Enabled = e;
            s4.Enabled = e;
            hp.Enabled = e;
        }
        public int getLeftTime()
        {
            return (int)(lssss.Value + lsss.Value * 10 + lss.Value * 100 + ls.Value * 1000);
        }
        public int getRightTime()
        {
            return (int)(rssss.Value + rsss.Value * 10 + rss.Value * 100 + rs.Value * 1000);
        }
        public int getSkill1Time()
        {
            return getInt(time1.Text, DEFAULT_SKILL_TIME);
        }
        public int getSkill2Time()
        {
            return getInt(time2.Text, DEFAULT_SKILL_TIME);
        }
        public int getSkill3Time()
        {
            return getInt(time3.Text, DEFAULT_SKILL_TIME);
        }
        public int getSkill4Time()
        {
            return getInt(time4.Text, DEFAULT_SKILL_TIME);
        }
        public int getHPTime()
        {
            return getInt(hp.Text, DEFAULT_SKILL_TIME);
        }

        private void clickLeft()
        {
            int time = getLeftTime();
            if (time > 0)
            {
                while (!left)
                {
                    cm.LeftClick(Cursor.Position.X, Cursor.Position.Y, time);
                }
            }
        }
        private void holdLeft()
        {
            int time = getLeftTime();
            if (time > 0)
            {
                while (!left)
                {
                    cm.HoldLeft(Cursor.Position.X, Cursor.Position.Y, time);
                }
            }
        }
        private void clickRight()
        {
            int time = getRightTime();
            if (time > 0)
            {
                while (!right)
                {
                    cm.RightClick(Cursor.Position.X, Cursor.Position.Y, time);
                }
            }
        }
        private void leftMouse()
        {

            int time = getLeftTime();
            if (time > 0)
            {
                left = !left;
                changeLeft();
                Thread thread = new Thread(new ThreadStart(() => clickLeft()));
                thread.Start();
            }
        }
        private void rightMouse()
        {
            int time = getRightTime();
            if (time > 0)
            {
                right = !right;
                changeRight();
                Thread thread = new Thread(new ThreadStart(() => clickRight()));
                thread.Start();
            }
        }


        private void skillSpam()
        {
            windowToFind = FindWindow(null, "Diablo III");
            skill = !skill;
            changeSkill();

            if (s1.Checked && getSkill1Time() > 0)
            {
                thread1 = new Thread(new ThreadStart(() => skill1()));
                thread1.Start();
            }
            if (s2.Checked && getSkill2Time() > 0)
            {
                thread2 = new Thread(new ThreadStart(() => skill2()));
                thread2.Start();
            }
            if (s3.Checked && getSkill3Time() > 0)
            {
                thread3 = new Thread(new ThreadStart(() => skill3()));
                thread3.Start();
            }
            if (s4.Checked && getSkill4Time() > 0)
            {
                thread4 = new Thread(new ThreadStart(() => skill4()));
                thread4.Start();
            }
            if (hp.Checked && getHPTime() > 0)
            {
                thread4 = new Thread(new ThreadStart(() => HP()));
                thread4.Start();
            }
        }
        private void skill1()
        {
            int time = getSkill1Time();
            if (time > 0)
            {
                while (!skill)
                {
                    sendKeystroke((ushort)keyS1);
                    Thread.Sleep(time);
                }
            }
        }
        private void skill2()
        {
            int time = getSkill2Time();
            if (time > 0)
            {
                while (!skill)
                {
                    sendKeystroke((ushort)keyS2);
                    Thread.Sleep(time);
                }
            }
        }
        private void skill3()
        {
            int time = getSkill3Time();
            if (time > 0)
            {
                while (!skill)
                {
                    sendKeystroke((ushort)keyS3);
                    Thread.Sleep(time);
                }
            }
        }
        private void skill4()
        {
            int time = getSkill4Time();
            if (time > 0)
            {
                while (!skill)
                {
                    sendKeystroke((ushort)keyS4);
                    Thread.Sleep(time);
                }
            }
        }


        private void HP()
        {
            int time = getHPTime();
            if (time > 0)
            {
                while (!skill)
                {
                    sendKeystroke((ushort)keyHps);
                    Thread.Sleep(time);
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_HOTKEY)
            {
                Keys vk = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                if (vk == Keys.F1)
                {
                    leftMouse();
                }
                else if (vk == Keys.F2)
                {
                    skillSpam();
                }
                else if (vk == Keys.F3)
                {
                    rightMouse();
                }

            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(Handle, 712);
            UnregisterHotKey(Handle, 713);
            UnregisterHotKey(Handle, 714);
            UnregisterHotKey(Handle, 715);
            saveConfig();
        }
        private void saveConfig()
        {
            String config = getLeftTime() + "|" +
                getRightTime() + "|" +
                (s1.Checked ? 1 : 0) + "|" +
                key1.Text + "|" +
                time1.Text + "|" +
                (s2.Checked ? 1 : 0) + "|" +
                key2.Text + "|" +
                time2.Text + "|" +

                (s3.Checked ? 1 : 0) + "|" +
                key3.Text + "|" +
                time3.Text + "|" +

                (s4.Checked ? 1 : 0) + "|" +
                key4.Text + "|" +
                time4.Text + "|" +

                (hp.Checked ? 1 : 0) + "|" +
               keyHp.Text + "|" +
                timeHp.Text;

            StreamWriter sw = new StreamWriter("settings.ini");
            sw.WriteLine(config);
            sw.Flush();
            sw.Close();
        }
        private void loadConfig()
        {

            time1.Text = DEFAULT_SKILL_TIME_STRING;
            time2.Text = DEFAULT_SKILL_TIME_STRING;
            time3.Text = DEFAULT_SKILL_TIME_STRING;
            time4.Text = DEFAULT_SKILL_TIME_STRING;
            timeHp.Text = DEFAULT_SKILL_TIME_STRING;


            string config = "";
            StreamReader reader = null;
            try
            {
                reader = new StreamReader("settings.ini");
            }
            catch (FileNotFoundException) { }
            if (reader != null)
            {
                while ((config = reader.ReadLine()) != null)
                {
                    break;
                }
                reader.Close();
            }

            int s1Time = DEFAULT_SKILL_TIME;
            int s2Time = DEFAULT_SKILL_TIME;
            int s3Time = DEFAULT_SKILL_TIME;
            int s4Time = DEFAULT_SKILL_TIME;
            int hpTime = DEFAULT_SKILL_TIME;

            bool s1Enable = false;
            bool s2Enable = false;
            bool s3Enable = false;
            bool s4Enable = false;
            bool hpEnable = false;
            if (config.Length > 0)
            {
                string[] configs = config.Split('|');
                int timeLeft = getInt(configs, 0, 0);
                int timeRight = getInt(configs, 1, 0);
                s1Enable = getBoolean(configs, 2);
                keyS1 = getKeys(configs, 3, Keys.D1);
                sKeyS1 = configs[3];
                s1Time = getInt(configs, 4, DEFAULT_SKILL_TIME);

                s2Enable = getBoolean(configs, 5);
                keyS2 = getKeys(configs, 6, Keys.D2);
                sKeyS2 = configs[6];
                s2Time = getInt(configs, 7, DEFAULT_SKILL_TIME);

                s3Enable = getBoolean(configs, 8);
                keyS3 = getKeys(configs, 9, Keys.D3);
                sKeyS3 = configs[9];
                s3Time = getInt(configs, 10, DEFAULT_SKILL_TIME);

                s4Enable = getBoolean(configs, 11);
                keyS4 = getKeys(configs, 12, Keys.D4);
                sKeyS4 = configs[12];
                s4Time = getInt(configs, 13, DEFAULT_SKILL_TIME);

                hpEnable = getBoolean(configs, 14);

                keyHps = getKeys(configs, 15, Keys.Q);
                sKeyHps = configs[15];
                hpTime = getInt(configs, 16, DEFAULT_SKILL_TIME);





                if (timeLeft < 10000)
                {

                    string time = "" + timeLeft;
                    int length = time.Length;
                    if (timeLeft >= 1000)
                    {
                        ls.Value = (int)timeLeft / 1000;
                    }
                    if (timeLeft >= 100)
                    {
                        lss.Value = getInt(time[length - 3]);
                    }
                    if (timeLeft >= 10)
                    {
                        lsss.Value = getInt(time[length - 2]);
                    }
                    if (timeLeft > 0)
                    {
                        lssss.Value = getInt(time[length - 1]);
                    }
                }
                if (timeRight < 10000)
                {
                    string time = "" + timeRight;
                    int length = time.Length;
                    if (timeRight > 1000)
                    {
                        rs.Value = (int)timeRight / 1000;
                    }
                    if (timeRight > 100)
                    {
                        rss.Value = getInt(time[length - 3]);
                    }
                    if (timeRight >= 10)
                    {
                        rsss.Value = getInt(time[length - 2]);
                    }
                    if (timeRight > 0)
                    {
                        rssss.Value = getInt(time[length - 1]);
                    }
                }
            }
            s1.Checked = s1Enable;
            s2.Checked = s2Enable;
            s3.Checked = s3Enable;
            s4.Checked = s4Enable;
            hp.Checked = hpEnable;


            key1.Text = kc.ConvertToString(sKeyS1);
            key2.Text = kc.ConvertToString(sKeyS2);
            key3.Text = kc.ConvertToString(sKeyS3);
            key4.Text = kc.ConvertToString(sKeyS4);
            keyHp.Text = kc.ConvertToString(sKeyHps);

            time1.Text = s1Time + "";
            time2.Text = s2Time + "";
            time3.Text = s3Time + "";
            time4.Text = s4Time + "";
            timeHp.Text = hpTime + "";
        }



        private void systemTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            systemTray.Visible = false;
            this.Activate();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();
                systemTray.Visible = true;
            }
        }
        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (hiddenStart)
            {
                hiddenStart = false;
                Hide();
                systemTray.Visible = true;
            }
        }

        private void tngyeu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.facebook.com/groups/552186094890694/"));
        }
        private int getInt(String[] arr, int index, int defaultValue)
        {
            int i = 0;
            try
            {
                i = Int32.Parse(arr[index]);
            }
            catch (Exception) { }
            return i < defaultValue ? defaultValue : i;
        }
        private Keys getKeys(String[] arr, int index, Keys defaultKeys)
        {
            return getKeys(arr[index], defaultKeys);
        }

        private bool getBoolean(String[] arr, int index)
        {
            try
            {
                return arr[index] == "1";
            }
            catch (Exception) { }
            return false;
        }
        private int getInt(char c)
        {
            return getInt(c.ToString(), 0);
        }
        private int getInt(string s, int defaultValue)
        {
            try
            {
                return Int32.Parse(s);
            }
            catch (Exception) { }
            return defaultValue;
        }

        private void key1_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            keyS1 = getKey(t, Keys.D1);
            if (keyS1 == Keys.D1)
            {
                t.Text = "1";
            }

        }

        private void key2_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            keyS2 = getKey(t, Keys.D2);
            if (keyS2 == Keys.D2)
            {
                t.Text = "2";
            }
        }

        private void key3_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            keyS3 = getKey(t, Keys.D3);
            if (keyS3 == Keys.D3)
            {
                t.Text = "3";
            }
        }

        private void key4_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            keyS4 = getKey(t, Keys.D4);
            if (keyS4 == Keys.D4)
            {
                t.Text = "4";
            }
        }

        private void keyHp_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            keyHps = getKey(t, Keys.Q);
            if (keyHps == Keys.Q)
            {
                t.Text = "Q";
            }
        }

        private void time1_TextChanged(object sender, EventArgs e)
        {
            validateInt((TextBox)sender, DEFAULT_SKILL_TIME_STRING);
        }

        private void time2_TextChanged(object sender, EventArgs e)
        {
            validateInt((TextBox)sender, DEFAULT_SKILL_TIME_STRING);
        }

        private void time3_TextChanged(object sender, EventArgs e)
        {
            validateInt((TextBox)sender, DEFAULT_SKILL_TIME_STRING);
        }

        private void time4_TextChanged(object sender, EventArgs e)
        {
            validateInt((TextBox)sender, DEFAULT_SKILL_TIME_STRING);
        }

        private void timeHp_TextChanged(object sender, EventArgs e)
        {
            validateInt((TextBox)sender, DEFAULT_SKILL_TIME_STRING);
        }


        private void validateInt(TextBox t, string defaultValue)
        {
            try { Int32.Parse(t.Text); }
            catch (Exception) { t.Text = defaultValue; }
        }

        private Keys getKey(TextBox t, Keys defaultKey)
        {
            string text = t.Text.ToUpper();
            t.Text = text;
            try
            {
                int i = Int32.Parse(text);
                switch (i)
                {
                    case 0: return Keys.D0;
                    case 1: return Keys.D1;
                    case 2: return Keys.D2;
                    case 3: return Keys.D3;
                    case 4: return Keys.D4;
                    case 5: return Keys.D5;
                    case 6: return Keys.D6;
                    case 7: return Keys.D7;
                    case 8: return Keys.D8;
                    case 9: return Keys.D9;
                }
            }
            catch (Exception) { }
            return getKeys(text, defaultKey);
        }
        private Keys getKeys(String s, Keys defaultKeys)
        {
            Keys k = Keys.None;
            try
            {
                k = (Keys)System.Enum.Parse(typeof(Keys), s);
            }
            catch (Exception) { }
            return k == Keys.None ? defaultKeys : k;
        }

        private void key_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            t.SelectAll();
        }

        private void key_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox t = (TextBox)sender;
            t.SelectAll();
        }
    }
}

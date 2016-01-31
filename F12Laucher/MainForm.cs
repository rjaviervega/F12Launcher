using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowScrape.Constants;
using WindowScrape.Types;
using WindowScrape;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

namespace F12Laucher
{

    public partial class MainForm : Form
    {

        #region lib
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hwnd, bool b);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetKeyState(int virtualKeyCode);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr GetWindow(IntPtr hwnd, int wFlag);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        const UInt32 WS_EX_APPWINDOW = 0x40000;

        const int WM_COMMAND = 0x111;
        const int MIN_ALL = 419;
        const int MIN_ALL_UNDO = 416;
        #endregion

        // Keyboard Hook
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static int execution_loop = 0;

        private delegate int LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        bool expand = false;
        static public List<F12LauncherSet> ListAppSet;


        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static int HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int state = Convert.ToInt32(GetAsyncKeyState(Keys.LShiftKey).ToString());

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN && (Keys)Marshal.ReadInt32(lParam) == Keys.F12 ) 
            {
                Program.PrgForm.listBox1.Items.Add("Pressed F12" + " " + state.ToString());
                if (state == -32767 || state == -32768 )
                {
                    Program.PrgForm.listBox1.Items.Add("Switching OK");
                    int vkCode = Marshal.ReadInt32(lParam);
                    PositionAndLaunchWindows();
                    return 1;
                }
            }


            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN && (Keys)Marshal.ReadInt32(lParam) == Keys.Right)
            {
                Program.PrgForm.listBox1.Items.Add("Pressed Right" + " " + state.ToString());
                if (state == -32767 || state == -32768)
                {
                    Program.PrgForm.listBox1.Items.Add("Switching Right + Shift OK");
                    int vkCode = Marshal.ReadInt32(lParam);
                    PositionAndLaunchWindows();
                    return 1;
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        public MainForm()
        {
            _hookID = SetHook(_proc);
            InitializeComponent();
            ListAppSet = new List<F12LauncherSet>();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }




        private void CmdstreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!expand)
                e.Cancel = true;
        }



        private void CmdstreeView1_Click(object sender, EventArgs e)
        {

        }

        private void CmdstreeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (!expand)
                e.Cancel = true;
        }

        private void CmdstreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        private void CmdstreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((e as MouseEventArgs).X > 18)
                expand = false;
            else
                expand = true;

            if (expand)
            {
                if (CmdstreeView1.Nodes[e.Node.Index].IsExpanded)
                {
                    CmdstreeView1.Nodes[e.Node.Index].Collapse();
                    expand = false;
                }
                else
                {
                    CmdstreeView1.Nodes[e.Node.Index].Expand();
                    expand = false;
                }
            }
        }

        private void AddBnt_Click(object sender, EventArgs e)
        {
            F12LauncherSet set = new F12LauncherSet();
            AddForm add = new AddForm();
            add.LocalSet = set;
            add.ShowDialog();
            if (set.AppsCount > 0)
                ListAppSet.Add(set);
            PopulateTreeView();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            linkLabel1.Text = "http://www.rjaviervega.com";
            linkLabel1.Links.Add(0, 26, "http://www.rjaviervega.com");
        }


        private static void PositionAndLaunchWindows()
        {
            if (ListAppSet.Count == 0) return;

            if (execution_loop >= ListAppSet.Count)
                execution_loop = 0;

            // Hightlight TreevIew
            Program.PrgForm.CmdstreeView1.SelectedNode = Program.PrgForm.CmdstreeView1.Nodes[execution_loop];

            // Get First Set and Attempt to Position Windows
            F12LauncherSet item = ListAppSet[execution_loop];

            Type t = typeof(System.Windows.Forms.SystemInformation);
            PropertyInfo[] pi = t.GetProperties();
            PropertyInfo prop = null;
            for (int i = 0; i < pi.Length; i++)
                if (pi[i].Name == "WorkingArea")
                {
                    prop = pi[i];
                    break;
                }
            object propval = prop.GetValue(null, null);
            Rectangle r = (Rectangle)propval;
            int w = r.Width / item.AppsCount;
            IntPtr p = new IntPtr(0);

            int i2 = 0;

            //To better display just the ones that we are working with,
            // minimize all windows expect for the ones in this set
            //
            List<HwndObject> x = new List<HwndObject>();
            x = HwndObject.GetWindows();
            ImageList img = new ImageList();
            for (int i = 0; i < x.Count; i++)
            {
                if (IsWindowVisible(x[i].Hwnd) && x[i].Title.ToString() != "")
                {                    
                    var win = HwndObject.GetWindowByTitle(x[i].Title.ToString());
                    uint lpdwProcessId;

                    GetWindowThreadProcessId(win.Hwnd, out lpdwProcessId);

                    Process p2 = Process.GetProcessById((int)lpdwProcessId);
                    // Check that the process is not of the windows that we want to show
                    bool skip = false;
                    foreach (F12LauncherApplication item_app in item.Apps)
                    {
                        if (item_app.Handle == p2.MainWindowHandle) skip = true;
                    }
                    if (skip == false) continue;
                    try
                    {
                        if (p2.MainWindowHandle != IntPtr.Zero && p2.MainWindowHandle != Program.PrgForm.Handle && IsWindowVisible(p2.MainWindowHandle))
                        {
                            Icon ico = Icon.ExtractAssociatedIcon(p2.MainModule.FileName);
                            ShowWindow(p2.MainWindowHandle, 6);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            // Done Minimization

            foreach (F12LauncherApplication item_app in item.Apps)
            {
                int l = i2 * w;

                if (!IsWindow(item_app.Handle))
                {
                    Program.PrgForm.listBox1.Items.Add("Handle not found: " + item_app.AppFileName);
                    Program.PrgForm.listBox1.Items.Add("Running: " + item_app.AppFileName);

                    Process proc = new Process();
                    proc.StartInfo.FileName = item_app.AppFileName;
                    proc.Start();
                    proc.WaitForInputIdle();
                    int waiting = 0;
                    // Process Will exit if there is a constrain that says that there can only be one instance of the process
                    // in case that the process can not be launch we skip it (scenario with Thunderbird)
                    // Maybe we can try to get the instance of a process with the same name and copy its handler
                    // ... TBD
                    bool process_ok = false;
                    if (!proc.HasExited)
                    {
                        while (proc.MainWindowHandle == IntPtr.Zero)
                        {
                            proc.Refresh();
                            System.Threading.Thread.Sleep(50);
                            waiting += 50; // add the 50 milliseconds 

                            // if an app doesn't get main window handler over 10 seconds, break
                            if (waiting == 10000)
                                break;
                        }

                        item_app.Handle = proc.MainWindowHandle;
                        Program.PrgForm.listBox1.Items.Add("Set Handle: " + item_app.Handle.ToString());
                        Program.PrgForm.listBox1.Items.Add("Launch Time: " + waiting.ToString() + "ms.");
                        process_ok = true;
                    }
                    else
                    {
                        // Check if there is already another process running with the same Filename and get its handler
                        x = HwndObject.GetWindows();
                        for (int i = 0; i < x.Count; i++)
                        {
                            if (IsWindowVisible(x[i].Hwnd) && x[i].Title.ToString() != "")
                            {
                                var win = HwndObject.GetWindowByTitle(x[i].Title.ToString());
                                uint lpdwProcessId;
                                GetWindowThreadProcessId(win.Hwnd, out lpdwProcessId);
                                Process p2 = Process.GetProcessById((int)lpdwProcessId);
                                // Check that the process is not of the windows that we want to show
                                //bool skip = false;
                                foreach (F12LauncherApplication item_app2 in item.Apps)
                                {
                                    // if matching file name of exec process
                                    // get its handler
                                    if (item_app2.AppFileName == p2.MainModule.FileName)
                                    {
                                        Program.PrgForm.listBox1.Items.Add("PROCESS FOUND: " + p2.MainModule.FileName);
                                        item_app.Handle = p2.MainWindowHandle;
                                        process_ok = true;
                                        break;
                                    }
                                }
                                if (process_ok) break;
                            }
                        }

                        if (!process_ok)
                        {
                            execution_loop++;
                            // Here we should probably call each other to get the next, but this is risky if we use
                            // recurtion because we can end up with a never ending loop.
                            // This should be solve if we scan all the running windows and get the MainWindowHandler of the
                            // process with the same file name as the one that exits.
                            //
                            return;
                        }
                    }
                }

                
                ShowWindow(item_app.Handle, 9);
                ShowWindow(item_app.Handle, 5);
                BringWindowToTop(item_app.Handle);
                SetWindowPos(item_app.Handle, p, l, 0, w, r.Height, (uint)(0x0040));
                SwitchToThisWindow(item_app.Handle, true);
                i2++;
            }

            execution_loop++;

            //Program.PrgForm.listBox1.Items.Add("Apps Switched");
        }



        private void PopulateTreeView()
        {
            CmdstreeView1.Nodes.Clear();

            ImageList images = new ImageList();

            CmdstreeView1.ImageList = images;
            images.Images.Add(this.Icon.ToBitmap());

            int img_count = 1;

            int i = 0;
            //Read List and Create Nodes in TreeView
            foreach (F12LauncherSet item in ListAppSet)
            {
                // Merget Exe Names
                String NodeTitle = "";
                foreach (F12LauncherApplication item_app in item.Apps)
                {
                    NodeTitle += " " + item_app.AppFileName.Split('\\')[item_app.AppFileName.Split('\\').Length - 1].ToLower() + " +";
                }
                if (NodeTitle.Length > 2)
                    CmdstreeView1.Nodes.Add(NodeTitle.Substring(0, NodeTitle.Length - 2));
                CmdstreeView1.Nodes[i].ImageIndex = img_count;
                CmdstreeView1.Nodes[i].SelectedImageIndex = img_count;


                int i2 = 0;
                foreach (F12LauncherApplication item_app in item.Apps)
                {
                    CmdstreeView1.Nodes[i].Nodes.Add(item_app.AppWindowName);
                    Icon ico = Icon.ExtractAssociatedIcon(item_app.AppFileName);
                    images.Images.Add(ico.ToBitmap());
                    CmdstreeView1.Nodes[i].Nodes[i2].ImageIndex = img_count;
                    CmdstreeView1.Nodes[i].Nodes[i2].SelectedImageIndex = img_count;
                    img_count++;
                    i2++;
                }
                i++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PositionAndLaunchWindows();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        public void MinimizeAll()
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
            System.Threading.Thread.Sleep(2000);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL_UNDO, IntPtr.Zero);

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox1.Items.Count - 1; 
        }



        //Switching Windows
        private void NextPairs()
        {

            Type t = typeof(System.Windows.Forms.SystemInformation);
            PropertyInfo[] pi = t.GetProperties();
            PropertyInfo prop = null;
            for (int i = 0; i < pi.Length; i++)
                if (pi[i].Name == "WorkingArea")
                {
                    prop = pi[i];
                    break;
                }
            object propval = prop.GetValue(null, null);
            Rectangle r = (Rectangle)propval;
            int w = r.Width / 2;
            IntPtr p = new IntPtr(0);



            // Get top most window and next one, and split them vertically
            List<HwndObject> enumWindows = new List<HwndObject>();
            enumWindows = HwndObject.GetWindows();
            ImageList img = new ImageList();

            int d = 0;

            for (int i = 0; i < enumWindows.Count; i++)
            {
                if (
                        IsWindowVisible(enumWindows[i].Hwnd) && 
                        enumWindows[i].Title.ToString() != "" && 
                        GetParent(enumWindows[i].Hwnd) == IntPtr.Zero &&
                        GetWindow(enumWindows[i].Hwnd, 4) == IntPtr.Zero
                    )
                {
                    uint lpdwProcessId;

                    GetWindowThreadProcessId(enumWindows[i].Hwnd, out lpdwProcessId);

                    Process p2 = Process.GetProcessById((int)lpdwProcessId);                     
                    
                    try
                    {
                        if (p2.MainWindowHandle != IntPtr.Zero && p2.MainWindowHandle != Program.PrgForm.Handle && IsWindowVisible(p2.MainWindowHandle))
                        {
                            // Skip first one to move 2 and 3 positions
                            if (d == 0)
                            {
                                d++;
                                continue;
                            }
                            IntPtr ptr = GetWindowLongPtr(enumWindows[i].Hwnd, -20);
                            if (ptr.ToInt32() > 128)
                            {                                
                                if (d >= 3) break;
                                listBox1.Items.Add(ptr.ToString() + " --- " + enumWindows[i].Title);
                                ShowWindow(enumWindows[i].Hwnd, 9);
                                ShowWindow(enumWindows[i].Hwnd, 5);
                                BringWindowToTop(enumWindows[i].Hwnd);
                                SetWindowPos(enumWindows[i].Hwnd, p, (d-1) * w, 0, w, r.Height, (uint)(0x0040));
                                SwitchToThisWindow(enumWindows[i].Hwnd, true);

                                d++;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }


                    /*
                    
                     */
                }
            }


        }





        private void button3_Click(object sender, EventArgs e)
        {
            NextPairs();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm AboutFormDialog;
            AboutFormDialog = new AboutForm();
            AboutFormDialog.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }






    }


    public class F12LauncherApplication
    {
        public IntPtr Handle;
        public String AppFileName;
        public String AppWindowName;
    }

    public class F12LauncherSet
    {
        public List<F12LauncherApplication> Apps;
        public int AppsCount;
        public String Title;

        //Construnctors
        public F12LauncherSet()
        {
            Apps = new List<F12LauncherApplication>();
        }

        public void AddApp(IntPtr WinHandle, String FileName, String WinName)
        {
            F12LauncherApplication app = new F12LauncherApplication();
            app.Handle = WinHandle;
            app.AppFileName = FileName;
            app.AppWindowName = WinName;

            Apps.Add(app);
            AppsCount++;
        }
    }

    public class F12LauncherAllSets
    {
        private List<F12LauncherSet> _Set;

        public List<F12LauncherSet> Set
        {
            get { return this._Set; }
            set { this._Set = value; }
        }


        public F12LauncherAllSets()
        {
        }



    }



}

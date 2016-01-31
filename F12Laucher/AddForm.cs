using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using WindowScrape.Constants;
using WindowScrape.Types;
using WindowScrape;

namespace F12Laucher
{






    public partial class AddForm : Form
    {
        public F12LauncherSet LocalSet;

        public List<F12LauncherApplication> ListApps;

        List<HwndObject> x = new List<HwndObject>();

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

        #endregion





        public AddForm()
        {
            InitializeComponent();
            ListApps = new List<F12LauncherApplication>();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            // Initialize




            x = HwndObject.GetWindows();
            ImageList img = new ImageList();
            int i2 = 0;

            for (int i = 0; i < x.Count; i++)
            {
                if (IsWindowVisible(x[i].Hwnd) && x[i].Title.ToString() != "")
                {
                    var win = HwndObject.GetWindowByTitle(x[i].Title.ToString());
                    uint lpdwProcessId;

                    GetWindowThreadProcessId(win.Hwnd, out lpdwProcessId);

                    Process p = Process.GetProcessById((int)lpdwProcessId);
                    try
                    {
                        if (p.MainWindowHandle != IntPtr.Zero && p.MainWindowHandle != this.Handle && IsWindowVisible(p.MainWindowHandle) )
                        {
                            Icon ico = Icon.ExtractAssociatedIcon(p.MainModule.FileName);                                                       

                            string s = p.MainModule.FileName;
                            string[] parts = s.Split('\\');  
                          
                            listView1.Items.Add(x[i].Title + " (" + parts[parts.Length -1].ToString()  + ")" );
                            img.Images.Add(ico.ToBitmap()); 
                            listView1.SmallImageList = img;
                            listView1.LargeImageList = img;
                            listView1.Items[i2].ImageIndex = i2;
                            i2++;
                            // Add to List
                            F12LauncherApplication app = new F12LauncherApplication();
                            app.Handle = p.MainWindowHandle;
                            app.AppFileName = p.MainModule.FileName;
                            app.AppWindowName = x[i].Title.ToString();
                            ListApps.Add(app);
                                                       
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());

                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check Checked Windows
            foreach (ListViewItem item in listView1.CheckedItems)
            {                
                LocalSet.AddApp(ListApps[item.Index].Handle, ListApps[item.Index].AppFileName, ListApps[item.Index].AppWindowName);    
            }
            
            Close();
        }
    }
}

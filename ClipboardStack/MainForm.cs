using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace ClipboardStack
{
    public partial class MainForm : Form
    {
        private List<String> stack = new List<String>();
        public MainForm()
        {
            InitializeComponent();
            this.registerHotkeys();
            this.Icon = ClipboardStack.Properties.Resources.icon;
            this.TrayIcon.Icon = ClipboardStack.Properties.Resources.icon;
        }


        private void registerHotkeys()
        {
            Hotkeys.RegisterHotKey(this.Handle, 1, Hotkeys.MOD_CTRL, (int)Keys.X);
            Hotkeys.RegisterHotKey(this.Handle, 2, Hotkeys.MOD_CTRL, (int)Keys.C);
            Hotkeys.RegisterHotKey(this.Handle, 3, Hotkeys.MOD_WIN, (int)Keys.V);
            Hotkeys.RegisterHotKey(this.Handle, 4, Hotkeys.MOD_CTRL, (int)Keys.V);
        }

        private void unregisterHotkeys()
        {
            Hotkeys.UnregisterHotKey(this.Handle, 1);
            Hotkeys.UnregisterHotKey(this.Handle, 2);
            Hotkeys.UnregisterHotKey(this.Handle, 3);
            Hotkeys.UnregisterHotKey(this.Handle, 4);
        }


        private Boolean deleteStackOnNextCopy = false;
        protected override void WndProc(ref Message m)
        {
            // Cut / Copy
            if (m.Msg == Hotkeys.WM_HOTKEY && ((int)m.WParam == 1 || (int)m.WParam == 2))
            {
                if (this.deleteStackOnNextCopy)
                {
                    this.stack.Clear();
                    this.deleteStackOnNextCopy = false;
                }
                this.unregisterHotkeys();
                if ((int)m.WParam == 1) SendKeys.SendWait("^x");
                else if ((int)m.WParam == 2) SendKeys.SendWait("^c");
                this.registerHotkeys();
                if (Clipboard.ContainsText())
                    this.stack.Add(Clipboard.GetText());
                else this.stack.Clear();
            }

            // Stacked Paste
            if (m.Msg == Hotkeys.WM_HOTKEY && (int)m.WParam == 3)
            {
                this.deleteStackOnNextCopy = true;
                String newText = "";
                // Only paste if stack is valid
                if (this.stack.Count > 0 && Clipboard.ContainsText())
                {
                    newText = this.stack[0];
                    this.stack.RemoveAt(0);
                    Clipboard.SetText(newText);

                    // Use Built-In Paste-Action
                    this.unregisterHotkeys();
                    SendKeys.SendWait("^v");
                    this.registerHotkeys();
                }
            }

            // Default Paste
            if (m.Msg == Hotkeys.WM_HOTKEY && (int)m.WParam == 4)
            {
                this.deleteStackOnNextCopy = true;
                this.unregisterHotkeys();
                SendKeys.SendWait("^v");
                this.registerHotkeys();
            }



            base.WndProc(ref m);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                Application.Exit();

            this.Visible = !this.Visible;
            if (this.Visible)
            {
                this.TopMost = true;
                this.TopMost = false;
                this.ShowInTaskbar = true;
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
            else
            {
                this.ShowInTaskbar = false;
                this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            }
        }


    }
}

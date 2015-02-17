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
            this.TrayIcon.Icon = ClipboardStack.Properties.Resources.icon;
            this.TrayIcon.Click += new System.EventHandler(TrayIcon_Click);
            this.TrayIcon.Visible = true;
        }

        private void TrayIcon_Click(object sender, System.EventArgs e)
        {
            this.Visible = !this.Visible;
            if (this.Visible) this.TopMost = true;
            this.TopMost = false;
        }

        private void registerHotkeys()
        {
            Hotkeys.RegisterHotKey(this.Handle, 1, Hotkeys.MOD_CONTROL, (int)Keys.X);
            Hotkeys.RegisterHotKey(this.Handle, 2, Hotkeys.MOD_CONTROL, (int)Keys.C);
            Hotkeys.RegisterHotKey(this.Handle, 3, Hotkeys.MOD_CONTROL + Hotkeys.MOD_SHIFT, (int)Keys.V);
        }

        private void unregisterHotkeys()
        {
            Hotkeys.UnregisterHotKey(this.Handle, 1);
            Hotkeys.UnregisterHotKey(this.Handle, 2);
            Hotkeys.UnregisterHotKey(this.Handle, 3);
        }

   

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Hotkeys.WM_HOTKEY && ((int)m.WParam == 1 || (int)m.WParam == 2))
            {
                this.unregisterHotkeys();
                if ((int)m.WParam == 1) SendKeys.Send("^x");
                else if ((int)m.WParam == 2) SendKeys.Send("^c");
                this.registerHotkeys();
                if (Clipboard.ContainsText())
                    this.stack.Add(Clipboard.GetText());
                else this.stack.Clear();
            }
            if (m.Msg == Hotkeys.WM_HOTKEY && (int)m.WParam == 3)
            {
                String text = "";
                if (this.stack.Count > 0 && Clipboard.ContainsText())
                {
                    text = this.stack[this.stack.Count - 1];
                    this.stack.RemoveAt(this.stack.Count - 1);
                    Clipboard.SetText(text);
                }
                
                this.unregisterHotkeys();
                SendKeys.Send("^v");
                this.registerHotkeys();
            }
            base.WndProc(ref m);
        }


    }
}

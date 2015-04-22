﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sdm.Client
{
    internal partial class MainDialog : Form
    {
        public MainDialog()
        {
            InitializeComponent();
        }

        private void TrySendMessage()
        {
            if (tbNewMsg.TextLength == 0)
                return;
            // XXX: send message
            tbNewMsg.Clear();
        }

        private void btnSend_Click(object sender, EventArgs e)
        { TrySendMessage(); }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            // XXX: show OpenFile dialog
        }

        private void btnSrv_Click(object sender, EventArgs e)
        {
            // XXX: show context menu (connect/disconnect)
        }

        private void lvUsers_DoubleClick(object sender, EventArgs e)
        {
            // XXX: get selected user and show conversation tab
        }

        private void tbNewMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                ; // XXX: send message
        }
    }
}

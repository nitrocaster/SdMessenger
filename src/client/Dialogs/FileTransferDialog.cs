﻿using System;
using System.Windows.Forms;
using Sdm.Client.Controls;

namespace Sdm.Client
{
    internal partial class FileTransferDialog : Form
    {
        public FileTransferDialog()
        { InitializeComponent(); }

        public FileTransferView View { get { return ftv; } }
    }
}

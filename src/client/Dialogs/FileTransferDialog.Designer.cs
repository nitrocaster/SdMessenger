﻿namespace Sdm.Client
{
    partial class FileTransferDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ftv = new Sdm.Client.Controls.FileTransferView();
            this.SuspendLayout();
            // 
            // ftv
            // 
            this.ftv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ftv.Location = new System.Drawing.Point(0, 0);
            this.ftv.Name = "ftv";
            this.ftv.Size = new System.Drawing.Size(404, 165);
            this.ftv.TabIndex = 0;
            // 
            // FileTransferDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 165);
            this.Controls.Add(this.ftv);
            this.Icon = Properties.Resources.Icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 200);
            this.Name = "FileTransferDialog";
            this.Text = "File transfer";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.FileTransferView ftv;
    }
}

namespace _27_Task
{
    partial class EnterForm
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
            this.enterButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // enterButton
            // 
            this.enterButton.Location = new System.Drawing.Point(62, 41);
            this.enterButton.Name = "enterButton";
            this.enterButton.Size = new System.Drawing.Size(165, 48);
            this.enterButton.TabIndex = 0;
            this.enterButton.Text = "Вход в систему.";
            this.enterButton.UseVisualStyleBackColor = true;
            this.enterButton.Click += new System.EventHandler(this.enterButton_Click);
            // 
            // EnterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 133);
            this.Controls.Add(this.enterButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnterForm";
            this.Text = "Система проверки доступа к голосованию";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button enterButton;
    }
}
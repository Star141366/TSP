namespace CTSP路由调度
{
    partial class Form2
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.旅行商信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.客户信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.旅行商调度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(846, 497);
            this.panel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.旅行商信息ToolStripMenuItem,
            this.客户信息ToolStripMenuItem,
            this.旅行商调度ToolStripMenuItem,
            this.退出系统ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(846, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 旅行商信息ToolStripMenuItem
            // 
            this.旅行商信息ToolStripMenuItem.Name = "旅行商信息ToolStripMenuItem";
            this.旅行商信息ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.旅行商信息ToolStripMenuItem.Text = "旅行商信息";
            this.旅行商信息ToolStripMenuItem.Click += new System.EventHandler(this.旅行商信息ToolStripMenuItem_Click);
            // 
            // 客户信息ToolStripMenuItem
            // 
            this.客户信息ToolStripMenuItem.Name = "客户信息ToolStripMenuItem";
            this.客户信息ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.客户信息ToolStripMenuItem.Text = "客户信息";
            this.客户信息ToolStripMenuItem.Click += new System.EventHandler(this.客户信息ToolStripMenuItem_Click);
            // 
            // 旅行商调度ToolStripMenuItem
            // 
            this.旅行商调度ToolStripMenuItem.Name = "旅行商调度ToolStripMenuItem";
            this.旅行商调度ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.旅行商调度ToolStripMenuItem.Text = "旅行商调度";
            this.旅行商调度ToolStripMenuItem.Click += new System.EventHandler(this.旅行商调度ToolStripMenuItem_Click);
            // 
            // 退出系统ToolStripMenuItem
            // 
            this.退出系统ToolStripMenuItem.Name = "退出系统ToolStripMenuItem";
            this.退出系统ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.退出系统ToolStripMenuItem.Text = "退出系统";
            this.退出系统ToolStripMenuItem.Click += new System.EventHandler(this.退出系统ToolStripMenuItem_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 497);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form2";
            this.Text = "CTSP路由调度管理界面";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 旅行商信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 客户信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 旅行商调度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出系统ToolStripMenuItem;
    }
}
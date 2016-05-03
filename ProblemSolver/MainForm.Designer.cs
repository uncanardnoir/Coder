namespace ProblemSolver
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.problemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.wbProblemDisplay = new System.Windows.Forms.WebBrowser();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnClearSolution = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRunOnSample = new System.Windows.Forms.Button();
            this.txtCodeEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.rtbOutputDisplay = new System.Windows.Forms.RichTextBox();
            this.msMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMenu
            // 
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.problemsToolStripMenuItem,
            this.progressToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Size = new System.Drawing.Size(999, 24);
            this.msMenu.TabIndex = 5;
            this.msMenu.Text = "menuStrip1";
            // 
            // problemsToolStripMenuItem
            // 
            this.problemsToolStripMenuItem.Name = "problemsToolStripMenuItem";
            this.problemsToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.problemsToolStripMenuItem.Text = "&Problems";
            // 
            // progressToolStripMenuItem
            // 
            this.progressToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewSolutionToolStripMenuItem,
            this.progressToolStripMenuItem1});
            this.progressToolStripMenuItem.Name = "progressToolStripMenuItem";
            this.progressToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.progressToolStripMenuItem.Text = "Progress";
            // 
            // viewSolutionToolStripMenuItem
            // 
            this.viewSolutionToolStripMenuItem.Enabled = false;
            this.viewSolutionToolStripMenuItem.Name = "viewSolutionToolStripMenuItem";
            this.viewSolutionToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.viewSolutionToolStripMenuItem.Text = "View Solution";
            this.viewSolutionToolStripMenuItem.Click += new System.EventHandler(this.viewSolutionToolStripMenuItem_Click);
            // 
            // progressToolStripMenuItem1
            // 
            this.progressToolStripMenuItem1.Name = "progressToolStripMenuItem1";
            this.progressToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.progressToolStripMenuItem1.Text = "Progress";
            this.progressToolStripMenuItem1.Click += new System.EventHandler(this.progressToolStripMenuItem1_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(13, 27);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.rtbOutputDisplay);
            this.splitContainer2.Size = new System.Drawing.Size(974, 687);
            this.splitContainer2.SplitterDistance = 434;
            this.splitContainer2.TabIndex = 10;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.wbProblemDisplay);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnSubmit);
            this.splitContainer1.Panel2.Controls.Add(this.btnClearSolution);
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Panel2.Controls.Add(this.btnRunOnSample);
            this.splitContainer1.Panel2.Controls.Add(this.txtCodeEditor);
            this.splitContainer1.Size = new System.Drawing.Size(974, 434);
            this.splitContainer1.SplitterDistance = 214;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 10;
            // 
            // wbProblemDisplay
            // 
            this.wbProblemDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbProblemDisplay.Location = new System.Drawing.Point(0, 0);
            this.wbProblemDisplay.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbProblemDisplay.Name = "wbProblemDisplay";
            this.wbProblemDisplay.Size = new System.Drawing.Size(974, 214);
            this.wbProblemDisplay.TabIndex = 0;
            this.wbProblemDisplay.WebBrowserShortcutsEnabled = false;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Location = new System.Drawing.Point(807, 0);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(168, 23);
            this.btnSubmit.TabIndex = 9;
            this.btnSubmit.Text = "S&ubmit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnClearSolution
            // 
            this.btnClearSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSolution.Location = new System.Drawing.Point(633, 0);
            this.btnClearSolution.Name = "btnClearSolution";
            this.btnClearSolution.Size = new System.Drawing.Size(168, 23);
            this.btnClearSolution.TabIndex = 8;
            this.btnClearSolution.Text = "Clear Solution";
            this.btnClearSolution.UseVisualStyleBackColor = true;
            this.btnClearSolution.Click += new System.EventHandler(this.btnClearSolution_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(173, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(168, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save (CTRL+S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRunOnSample
            // 
            this.btnRunOnSample.Location = new System.Drawing.Point(-1, 0);
            this.btnRunOnSample.Name = "btnRunOnSample";
            this.btnRunOnSample.Size = new System.Drawing.Size(168, 23);
            this.btnRunOnSample.TabIndex = 2;
            this.btnRunOnSample.Text = "&Run against sample case (F5)";
            this.btnRunOnSample.UseVisualStyleBackColor = true;
            this.btnRunOnSample.Click += new System.EventHandler(this.btnRunOnSample_Click);
            // 
            // txtCodeEditor
            // 
            this.txtCodeEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCodeEditor.IsReadOnly = false;
            this.txtCodeEditor.Location = new System.Drawing.Point(0, 29);
            this.txtCodeEditor.Name = "txtCodeEditor";
            this.txtCodeEditor.ShowSpaces = true;
            this.txtCodeEditor.ShowTabs = true;
            this.txtCodeEditor.Size = new System.Drawing.Size(975, 170);
            this.txtCodeEditor.TabIndex = 1;
            // 
            // rtbOutputDisplay
            // 
            this.rtbOutputDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOutputDisplay.Location = new System.Drawing.Point(0, 0);
            this.rtbOutputDisplay.Name = "rtbOutputDisplay";
            this.rtbOutputDisplay.ReadOnly = true;
            this.rtbOutputDisplay.Size = new System.Drawing.Size(974, 249);
            this.rtbOutputDisplay.TabIndex = 3;
            this.rtbOutputDisplay.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 726);
            this.Controls.Add(this.msMenu);
            this.Controls.Add(this.splitContainer2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.msMenu;
            this.Name = "MainForm";
            this.Text = "Coder";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem problemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem progressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewSolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem progressToolStripMenuItem1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.WebBrowser wbProblemDisplay;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnClearSolution;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRunOnSample;
        private ICSharpCode.TextEditor.TextEditorControl txtCodeEditor;
        private System.Windows.Forms.RichTextBox rtbOutputDisplay;
    }
}


namespace ExpressionEvaluatorTest
{
    partial class EvaluatorTest
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
            this.tbExpression = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEvaluate = new System.Windows.Forms.Button();
            this.lvTokens = new System.Windows.Forms.ListView();
            this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTokenDetail = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lvOperands = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvOperators = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvIdentifiers = new System.Windows.Forms.ListView();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label7 = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbExpression
            // 
            this.tbExpression.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbExpression.Location = new System.Drawing.Point(116, 27);
            this.tbExpression.Margin = new System.Windows.Forms.Padding(4);
            this.tbExpression.Name = "tbExpression";
            this.tbExpression.Size = new System.Drawing.Size(952, 30);
            this.tbExpression.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Expression";
            // 
            // btnEvaluate
            // 
            this.btnEvaluate.Location = new System.Drawing.Point(1090, 29);
            this.btnEvaluate.Margin = new System.Windows.Forms.Padding(4);
            this.btnEvaluate.Name = "btnEvaluate";
            this.btnEvaluate.Size = new System.Drawing.Size(100, 28);
            this.btnEvaluate.TabIndex = 2;
            this.btnEvaluate.Text = "Evaluate";
            this.btnEvaluate.UseVisualStyleBackColor = true;
            this.btnEvaluate.Click += new System.EventHandler(this.btnEvaluate_Click);
            // 
            // lvTokens
            // 
            this.lvTokens.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2});
            this.lvTokens.FullRowSelect = true;
            this.lvTokens.GridLines = true;
            this.lvTokens.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTokens.Location = new System.Drawing.Point(46, 90);
            this.lvTokens.Margin = new System.Windows.Forms.Padding(4);
            this.lvTokens.MultiSelect = false;
            this.lvTokens.Name = "lvTokens";
            this.lvTokens.Size = new System.Drawing.Size(291, 627);
            this.lvTokens.TabIndex = 3;
            this.lvTokens.UseCompatibleStateImageBehavior = false;
            this.lvTokens.View = System.Windows.Forms.View.Details;
            this.lvTokens.SelectedIndexChanged += new System.EventHandler(this.lvTokens_SelectedIndexChanged);
            // 
            // columnHeader0
            // 
            this.columnHeader0.Text = "#";
            this.columnHeader0.Width = 23;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "TokenType";
            this.columnHeader1.Width = 152;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "TokenValue";
            this.columnHeader2.Width = 88;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(43, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tokens";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(357, 69);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Detail";
            // 
            // lblTokenDetail
            // 
            this.lblTokenDetail.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTokenDetail.Location = new System.Drawing.Point(360, 90);
            this.lblTokenDetail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTokenDetail.Name = "lblTokenDetail";
            this.lblTokenDetail.Size = new System.Drawing.Size(416, 149);
            this.lblTokenDetail.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(357, 249);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Results/Errors";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(357, 459);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Operand Stack";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(799, 459);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Operator Stack";
            // 
            // lvOperands
            // 
            this.lvOperands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvOperands.FullRowSelect = true;
            this.lvOperands.GridLines = true;
            this.lvOperands.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvOperands.Location = new System.Drawing.Point(360, 489);
            this.lvOperands.Margin = new System.Windows.Forms.Padding(4);
            this.lvOperands.MultiSelect = false;
            this.lvOperands.Name = "lvOperands";
            this.lvOperands.Size = new System.Drawing.Size(416, 228);
            this.lvOperands.TabIndex = 11;
            this.lvOperands.UseCompatibleStateImageBehavior = false;
            this.lvOperands.View = System.Windows.Forms.View.Details;
            this.lvOperands.SelectedIndexChanged += new System.EventHandler(this.lvOperands_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "#";
            this.columnHeader3.Width = 23;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Type";
            this.columnHeader4.Width = 152;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Value";
            this.columnHeader5.Width = 88;
            // 
            // lvOperators
            // 
            this.lvOperators.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader12});
            this.lvOperators.FullRowSelect = true;
            this.lvOperators.GridLines = true;
            this.lvOperators.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvOperators.Location = new System.Drawing.Point(802, 489);
            this.lvOperators.Margin = new System.Windows.Forms.Padding(4);
            this.lvOperators.MultiSelect = false;
            this.lvOperators.Name = "lvOperators";
            this.lvOperators.Size = new System.Drawing.Size(388, 228);
            this.lvOperators.TabIndex = 12;
            this.lvOperators.UseCompatibleStateImageBehavior = false;
            this.lvOperators.View = System.Windows.Forms.View.Details;
            this.lvOperators.SelectedIndexChanged += new System.EventHandler(this.lvOperators_SelectedIndexChanged);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "#";
            this.columnHeader6.Width = 23;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Symbol";
            this.columnHeader7.Width = 83;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Name";
            this.columnHeader8.Width = 205;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Priority";
            // 
            // lvIdentifiers
            // 
            this.lvIdentifiers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader13,
            this.columnHeader11});
            this.lvIdentifiers.FullRowSelect = true;
            this.lvIdentifiers.GridLines = true;
            this.lvIdentifiers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvIdentifiers.Location = new System.Drawing.Point(802, 113);
            this.lvIdentifiers.Margin = new System.Windows.Forms.Padding(4);
            this.lvIdentifiers.MultiSelect = false;
            this.lvIdentifiers.Name = "lvIdentifiers";
            this.lvIdentifiers.Size = new System.Drawing.Size(388, 342);
            this.lvIdentifiers.TabIndex = 13;
            this.lvIdentifiers.UseCompatibleStateImageBehavior = false;
            this.lvIdentifiers.View = System.Windows.Forms.View.Details;
            this.lvIdentifiers.SelectedIndexChanged += new System.EventHandler(this.lvIdentifiers_SelectedIndexChanged);
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "#";
            this.columnHeader9.Width = 23;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Identifier Type";
            this.columnHeader10.Width = 100;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Name";
            this.columnHeader13.Width = 130;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Value";
            this.columnHeader11.Width = 115;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(799, 81);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Identifiers";
            // 
            // lblResults
            // 
            this.lblResults.AcceptsReturn = true;
            this.lblResults.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.Location = new System.Drawing.Point(360, 270);
            this.lblResults.Multiline = true;
            this.lblResults.Name = "lblResults";
            this.lblResults.ReadOnly = true;
            this.lblResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lblResults.Size = new System.Drawing.Size(416, 186);
            this.lblResults.TabIndex = 15;
            // 
            // EvaluatorTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1216, 735);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lvIdentifiers);
            this.Controls.Add(this.lvOperators);
            this.Controls.Add(this.lvOperands);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblTokenDetail);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lvTokens);
            this.Controls.Add(this.btnEvaluate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbExpression);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EvaluatorTest";
            this.Text = "WESNet Designs C# Expression Evaluator Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbExpression;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEvaluate;
        private System.Windows.Forms.ListView lvTokens;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTokenDetail;
        private System.Windows.Forms.ColumnHeader columnHeader0;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListView lvOperands;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ListView lvOperators;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ListView lvIdentifiers;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox lblResults;
    }
}



namespace Presentacion.Core.Deposito
{
    partial class _00056_TransferenciaDeposito
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
            this.txtArticulo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDepositoOrigen = new System.Windows.Forms.ComboBox();
            this.cmbDepositoDestino = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudOrigen = new System.Windows.Forms.NumericUpDown();
            this.nudDestino = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrigen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDestino)).BeginInit();
            this.SuspendLayout();
            // 
            // txtArticulo
            // 
            this.txtArticulo.Location = new System.Drawing.Point(107, 85);
            this.txtArticulo.Name = "txtArticulo";
            this.txtArticulo.ReadOnly = true;
            this.txtArticulo.Size = new System.Drawing.Size(338, 20);
            this.txtArticulo.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Articulo:";
            // 
            // cmbDepositoOrigen
            // 
            this.cmbDepositoOrigen.FormattingEnabled = true;
            this.cmbDepositoOrigen.Location = new System.Drawing.Point(107, 111);
            this.cmbDepositoOrigen.Name = "cmbDepositoOrigen";
            this.cmbDepositoOrigen.Size = new System.Drawing.Size(260, 21);
            this.cmbDepositoOrigen.TabIndex = 6;
            this.cmbDepositoOrigen.SelectionChangeCommitted += new System.EventHandler(this.cmbDepositoOrigen_SelectionChangeCommitted);
            // 
            // cmbDepositoDestino
            // 
            this.cmbDepositoDestino.FormattingEnabled = true;
            this.cmbDepositoDestino.Location = new System.Drawing.Point(107, 138);
            this.cmbDepositoDestino.Name = "cmbDepositoDestino";
            this.cmbDepositoDestino.Size = new System.Drawing.Size(260, 21);
            this.cmbDepositoDestino.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Deposito origen:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Deposito destino:";
            // 
            // nudOrigen
            // 
            this.nudOrigen.Enabled = false;
            this.nudOrigen.Location = new System.Drawing.Point(373, 112);
            this.nudOrigen.Name = "nudOrigen";
            this.nudOrigen.ReadOnly = true;
            this.nudOrigen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudOrigen.Size = new System.Drawing.Size(72, 20);
            this.nudOrigen.TabIndex = 10;
            this.nudOrigen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudDestino
            // 
            this.nudDestino.Location = new System.Drawing.Point(373, 139);
            this.nudDestino.Name = "nudDestino";
            this.nudDestino.Size = new System.Drawing.Size(72, 20);
            this.nudDestino.TabIndex = 11;
            this.nudDestino.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _00056_TransferenciaDeposito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 183);
            this.Controls.Add(this.nudDestino);
            this.Controls.Add(this.nudOrigen);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbDepositoDestino);
            this.Controls.Add(this.cmbDepositoOrigen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtArticulo);
            this.MaximumSize = new System.Drawing.Size(478, 222);
            this.MinimumSize = new System.Drawing.Size(478, 222);
            this.Name = "_00056_TransferenciaDeposito";
            this.Text = "Transferencia entre depositos";
            this.Controls.SetChildIndex(this.txtArticulo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbDepositoOrigen, 0);
            this.Controls.SetChildIndex(this.cmbDepositoDestino, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.nudOrigen, 0);
            this.Controls.SetChildIndex(this.nudDestino, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudOrigen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDestino)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtArticulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDepositoOrigen;
        private System.Windows.Forms.ComboBox cmbDepositoDestino;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudOrigen;
        private System.Windows.Forms.NumericUpDown nudDestino;
    }
}
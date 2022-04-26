using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoControl
{
    public partial class SelectURL : Form
    {
        public SelectURL(string url)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            txtURL.Text = url;
        }

        public string URL
        {
            set
            {
                txtURL.Text = value;
            }
            get
            {
                return txtURL.Text;
            }
        }            

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

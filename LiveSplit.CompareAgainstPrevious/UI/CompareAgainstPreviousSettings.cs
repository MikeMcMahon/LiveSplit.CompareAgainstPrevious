using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.UI;

namespace LiveSplit.CompareAgainstPrevious.UI
{
    public partial class CompareAgainstPreviousSettings : UserControl
    {
        public CompareAgainstPreviousSettings()
        {
            InitializeComponent();
            chkResetRuns.DataBindings.Add("Checked", this, "UseResetRuns", false, DataSourceUpdateMode.OnPropertyChanged);
            resetRunAmount.DataBindings.Add("Value", this, "UseResetRunPercent", false, DataSourceUpdateMode.OnPropertyChanged);
            txtComparisonName.DataBindings.Add("Text", this, "ComparisonName");
            resetRunAmount_ValueChanged(null, null);
        }

        private string _ComparisonName = CompareAgainstPreviousComparisonGenerator.ComparisonName;
        private int _UseResetRunPercent = 50;

        public bool UseResetRuns { get; set; }
        public string ComparisonName
        {
            get { return _ComparisonName; }
            set
            {
                _ComparisonName = value;
                ComparisonNameChanged?.Invoke(this, null);
            }
        }
        public int UseResetRunPercent { get { return _UseResetRunPercent; } set { _UseResetRunPercent = value; } }


        public event EventHandler ComparisonNameChanged;

        private void chkResetRuns_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;
            UseResetRuns = SettingsHelper.ParseBool(element["UseResetRuns"]);
            ComparisonName = SettingsHelper.ParseString(element["ComparisonName"]);
            UseResetRunPercent = SettingsHelper.ParseInt(element["UseResetRunPercent"]);
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");
            SettingsHelper.CreateSetting(document, parent, "Version", "1.0");
            SettingsHelper.CreateSetting(document, parent, "UseResetRuns", UseResetRuns);
            SettingsHelper.CreateSetting(document, parent, "UseResetRunPercent", UseResetRunPercent);
            SettingsHelper.CreateSetting(document, parent, "ComparisonName", ComparisonName);

            return parent;
        }

        private void resetRunAmount_ValueChanged(object sender, EventArgs e)
        {
            txtResetRunPct.Text = string.Format("{0} %", resetRunAmount.Value);
        }
    }
}

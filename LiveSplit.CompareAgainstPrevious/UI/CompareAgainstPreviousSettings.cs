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
        }

        public bool UseResetRuns { get; set; }
        public string ComparisonName { get; set; }

        private void chkResetRuns_CheckedChanged(object sender, EventArgs e)
        {
            chkResetRuns.DataBindings.Add("Checked", this, "UseResetRuns", false, DataSourceUpdateMode.OnPropertyChanged);
            txtComparisonName.DataBindings.Add("Text", this, "ComparisonName");
        }

        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;
            UseResetRuns = SettingsHelper.ParseBool(element["UseResetRuns"));
            ComparisonName = element["ComparisonName"].Value;
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");
            SettingsHelper.CreateSetting(document, parent, "Version", "1.0");
            SettingsHelper.CreateSetting(document, parent, "UseResetRuns", UseResetRuns);
            SettingsHelper.CreateSetting(document, parent, "ComparisonName", ComparisonName);

            return parent;
        }


    }
}

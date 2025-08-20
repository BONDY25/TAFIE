using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace TAFIE
{
    public partial class xmlView : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        public string xmlString { get; set; }

        public xmlView()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void xmlView_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[xmlView]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - TAFIE XML Viewer";
            DisplayPrettyXml(xmlString, rtbxXML);
        }


        // Format XML data ------------------------------------------------------------------------------------------------------------------
        public void DisplayPrettyXml(string xmlData, RichTextBox richTextBox)
        {
            try
            {
                // Load the XML data into an XmlDocument
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);

                // Create a StringWriter to capture the formatted output
                using (StringWriter stringWriter = new StringWriter())
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                {
                    // Configure the XmlTextWriter to format the XML
                    xmlTextWriter.Formatting = Formatting.Indented;
                    xmlDoc.WriteTo(xmlTextWriter);

                    // Set the formatted XML to the RichTextBox
                    richTextBox.Text = stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                richTextBox.Text = $"Error formatting XML: {ex.Message}";
            }
        }

        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnClose);
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnClose);
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //=============================================================================================================================================================================================
        //-- Key Down Events --//
        //=============================================================================================================================================================================================
        private void xmlView_KeyDown(object sender, KeyEventArgs e)
        {
            // Esc
            if (e.KeyCode == Keys.Escape)
            {
                btnClose_Click(sender, e);
            }
        }
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using System.Collections;
using Pelesys.Scheduling;
using Pelesys.Data;
using Pelesys.Administration;
using eForm.Controls;


namespace eForm
{
    public class FormDesign
    {
        public static String cStyle = "style";
        public static String ccsTop = "Top:";
        public static String ccsLeft = "Left:";
        public static String ccsRelative = "position:absolute;"; //position:relative; z-index:0;
        public static String ccsPX = "px;";
        public static string SessionImageList = "SessionImageList";
        public static string SessionImageValueList = "SessionImageValueList";
        public static string SessionTextBoxList = "SessionTextBoxList";
        public static string SessionCurrentOrg = "SessionCurrentOrg";
        public static string SessionFormDesignControlList = "SessionFormDesignControlList";
        public const string BuildInChangeButtonControl = "ChangeButton";
        public const string BuildInChangeLoginNameButtonControl = "ChangeUserNameButton";
        public static string ctrFormDesignImageControl = "Control/ucImageControl.ascx";
        private Boolean IsNotDefaultSeasonID = false;
        public const string MaxLenghtOptionKey = "MaxLenghtOptionKey";
        public const string ImageList = "UploadImageList";

        public enum ValidateControlType
        {
            text = 1,
            dropdownlist = 2,
            number = 3,
            date = 4,
            RegExp = 5
        }

        public enum ControlType
        {
            DrowDownList = 1,
            CheckBox = 2,
            TextBox = 3,
            ListBox = 4,
            TextArea = 5,
            Literal = 6,
            Button = 7,
            Image = 8,
            DateTime = 9,
            Money = 10,
            HyperLink = 11

        }

        public enum DataType
        {
            String,
            Number,
            Bit,
            Date
        }

        public enum SaveResultType
        {
            Mandatory,
            Warning,
            Question,
            Successfull
        }

        public enum TransactionProcessType
        {
            MemberSaveProcess,
            ScheduleSaveProcess

        }


        public enum FormDesignValidateType
        {
            SysTextBoxValidateMemberDesignRule,
            SysTextBoxValidateLeagueDesignRule,
            SysTextBoxValidateDivisionDesignRule,
            SysTextBoxValidateTeamDesignRule

        }

        public enum FormDesignOptionType
        {
            Money,
            Mandatory,
            Date
        }


        public Page oPage;

        private int _eFormID = 0;
        public int eFormID
        {
            get
            {
                return _eFormID;
            }

            set
            {

                _eFormID = value;
            }


        }


        private Hashtable oImageList = new Hashtable();
        private Hashtable oTextBoxList = new Hashtable();
        public static string MemberTabDesignCSSClass = "Design_tab_body";
        private const string ButtonSkinID = "AppButton";
        private const string LieralSkinID = "sLieralList";
        private const string DropDownListSkinID = "sDropDownList";
        private const string CheckBoxSkinID = "DesignCheckBox";
        private const string TextAreaSkinID = "defaultTextAreaOnDesign";
        private const string TextBoxSkinID = "TabTextBox";
        private const string HyperlinkSkinID = "Hyperlink";

        private const string InvalidMessage = "This Date Time is invalid! Please enter it again.";
        private const string InvalidMoney = "The Money is invalid! Please enter it again.";

        public Hashtable ValidateMandatoryFields(int EformID, Panel oPanel)
        {
            Hashtable oHT = new Hashtable();
            List<DesignFormField> oList = DesignFormField.GetMandatoryDataBy(EformID);

           
                foreach (System.Web.UI.Control oControl in oPanel.Controls)
                {
                    foreach (DesignFormField ofield in oList)
                    {
                        string ControlName = "txtEform" + ofield.Name;
                        TextBox oText = oControl.FindControl(ControlName) as TextBox;
                        if (oText != null)
                        {
                            if (ofield.IsMandatory == true && oText.Text == string.Empty)
                            {
                                oHT.Add("ClientID", oText.ClientID);
                                oHT.Add("Message", ofield.Message);
                                oHT.Add("TabID", ofield.TabID);
                                return oHT;
                            }

                            if (oText.Text != string.Empty && ofield.ControlType == Convert.ToInt16(FormDesign.ControlType.DateTime))
                            {
                                DateTime date;
                                if (!(DateTime.TryParse(oText.Text, out date)))
                                {
                                    oHT.Add("ClientID", oText.ClientID);
                                    oHT.Add("Message", InvalidMessage);
                                    oHT.Add("TabID", ofield.TabID);
                                    return oHT;
                                }

                            }
                            if (oText.Text != string.Empty && ofield.ControlType == Convert.ToInt16(FormDesign.ControlType.Money))
                            {
                                double value;
                                if (!double.TryParse(oText.Text, out value)) // Returns bool
                                {
                                    oHT.Add("ClientID", oText.ClientID);
                                    oHT.Add("Message", InvalidMoney);
                                    oHT.Add("TabID", ofield.TabID);
                                    return oHT;
                                }


                            }
                        }


                    

                }
            }

            return null;
        }

        public Hashtable ValidateMandatoryFields(int EformID, TabContainer oTabs)
        {
            Hashtable oHT = new Hashtable();
            List<DesignFormField> oList = DesignFormField.GetMandatoryDataBy(EformID);

            foreach (TabPanel oPanel in oTabs.Tabs)
            {
                foreach (System.Web.UI.Control oControl in oPanel.Controls)
                {
                    foreach (DesignFormField ofield in oList)
                    {
                        string ControlName = "txtEform" + ofield.Name;
                        TextBox oText = oControl.FindControl(ControlName) as TextBox;
                        if (oText != null)
                        {
                            if (ofield.IsMandatory == true && oText.Text == string.Empty)
                            {
                                oHT.Add("ClientID", oText.ClientID);
                                oHT.Add("Message", ofield.Message);
                                oHT.Add("TabID", ofield.TabID);
                                return oHT;
                            }

                            if (oText.Text != string.Empty && ofield.ControlType == Convert.ToInt16(FormDesign.ControlType.DateTime))
                            {
                                DateTime date;
                                if (!(DateTime.TryParse(oText.Text, out date)))
                                {
                                    oHT.Add("ClientID", oText.ClientID);
                                    oHT.Add("Message", InvalidMessage);
                                    oHT.Add("TabID", ofield.TabID);
                                    return oHT;
                                }

                            }
                            if (oText.Text != string.Empty && ofield.ControlType == Convert.ToInt16(FormDesign.ControlType.Money))
                            {
                                double value;
                                if (!double.TryParse(oText.Text, out value)) // Returns bool
                                {
                                    oHT.Add("ClientID", oText.ClientID);
                                    oHT.Add("Message", InvalidMoney);
                                    oHT.Add("TabID", ofield.TabID);
                                    return oHT;
                                }


                            }
                        }


                    }

                }
            }

            return null;
        }

        public void SaveTabsData(Int64 IDValue, TabContainer oTab, string DBTable, int eFromID, string IDColumnName)
        {
            DataTable oDt = DesignFormField.GetDataTableStructure(DBTable, IDColumnName, IDValue);
            DataTable oSaveDT = new DataTable();
            foreach (TabPanel oPanel in oTab.Tabs)
            {

                oSaveDT = SetTabValueIntoDataSet(oDt, false, oTab, "", DBTable);
               
            }
            string sql = CreateQuerStatement(IDValue, oSaveDT, DBTable, IDColumnName);
            DBManager odbm = new DBManager();
            odbm.ExecuteNonQuery(sql);

        }

        public void SaveTabsData(Int64 IDValue, Panel oPanel, string DBTable, int eFromID, string IDColumnName)
        {
            DataTable oDt = DesignFormField.GetDataTableStructure(DBTable, IDColumnName, IDValue);
            DataTable oSaveDT = new DataTable();

           

            FormDesign oDesign = new FormDesign();
            oSaveDT = SetTabValueIntoDataSet(oDt, false, oPanel, "", DBTable);

            string sql = CreateQuerStatement(IDValue, oSaveDT, DBTable, IDColumnName);
            DBManager odbm = new DBManager();
            odbm.ExecuteNonQuery(sql);

        }

        public void SetControlLabel(Panel oPanel, DesignFormField oItem, int eFormID)
        {
            Label oLabel = new Label();
            if (oItem.Label == null)
            {
                return;
            }


            string LID = oItem.Label.Replace(" ", string.Empty);

            LID = LID.Replace(":", string.Empty);
            oLabel.ID = "lblEform" + oItem.Name; //+ oItem.Label
            oLabel.SkinID = "Controllabel";
            oLabel.Text = oItem.Label;
            oLabel.CssClass = "editText";
            int num;

            string cssStyle = string.Empty;
            bool IsTop = int.TryParse(oItem.LabelTop.ToString(), out num);
            if (IsTop == true)
            {
                cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;

            }
            bool IsLeft = int.TryParse(oItem.LabelLeft.ToString(), out num);
            if (IsLeft == true)
            {
                cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
            }
            LiteralControl oLiteral = new LiteralControl();
            oLiteral.Text = "<Div id=\"LABELDIV" + oLabel.ID + "\" Style='" + cssStyle + "' >";

            oPanel.Controls.Add(oLiteral);
            oPanel.Controls.Add(oLabel);
            LiteralControl oEndLiteral = new LiteralControl();
            if (oItem.IsMandatory == true)
            {
                LiteralControl oAddStar = new LiteralControl();
                oAddStar.Text = "<span style='color: red;'>*</span>";
                oPanel.Controls.Add(oAddStar);
            }
            oEndLiteral.Text = "</Div>";
            oPanel.Controls.Add(oEndLiteral);
        }


        public void SetControlLabel(Control oPanel, DesignFormField oItem, int eFormID)
        {
            Label oLabel = new Label();
            if (oItem.Label == null)
            {
                return;
            }


            string LID = oItem.Label.Replace(" ", string.Empty);

            LID = LID.Replace(":", string.Empty);
            oLabel.ID = "lblEform" + oItem.Name; //+ oItem.Label
            oLabel.SkinID = "Controllabel";
            oLabel.Text = oItem.Label;
            oLabel.CssClass = "editText";
            int num;

            string cssStyle = string.Empty;
            bool IsTop = int.TryParse(oItem.LabelTop.ToString(), out num);
            if (IsTop == true)
            {
                cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;

            }
            bool IsLeft = int.TryParse(oItem.LabelLeft.ToString(), out num);
            if (IsLeft == true)
            {
                cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
            }
            LiteralControl oLiteral = new LiteralControl();
            oLiteral.Text = "<Div id=\"LABELDIV" + oLabel.ID + "\" Style='" + cssStyle + "' >";

            oPanel.Controls.Add(oLiteral);
            oPanel.Controls.Add(oLabel);
            LiteralControl oEndLiteral = new LiteralControl();
            if (oItem.IsMandatory == true)
            {
                LiteralControl oAddStar = new LiteralControl();
                oAddStar.Text = "<span style='color: red;'>*</span>";
                oPanel.Controls.Add(oAddStar);
            }
            oEndLiteral.Text = "</Div>";
            oPanel.Controls.Add(oEndLiteral);
        }

        public void SetDataToDB(TabPanel oPanel, DataTable oDT, String DBTable)
        {
           


            foreach (System.Web.UI.Control oControl in oPanel.Controls)
            {
                String ControlName = oControl.ID;
                if (ControlName != null)
                {
                    ControlName = ControlName.Replace("txtEform", string.Empty);
                }
                if (oControl is TextBox)
                {
                    TextBox oTextBox = (TextBox)oControl;

                    //if (oSMMember.Tables[0].Rows[0][ControlName].ToString() != null)
                    {
                        if (oTextBox.Text != string.Empty)
                        {
                            String sText = oTextBox.Text;
                            try
                            {
                                oDT.Rows[0][ControlName] = HttpUtility.HtmlEncode(sText);
                            }
                            catch (Exception ex)
                            {
                                oDT.Rows[0]["[" + ControlName + "]"] = HttpUtility.HtmlEncode(sText);
                            }
                        }
                    }

                }


                else if (oControl is CheckBox)
                {
                    CheckBox oCk = (CheckBox)oControl;

                    try
                    {
                        oDT.Rows[0][ControlName] = oCk.Checked;
                    }
                    catch (Exception ex)
                    {
                        oDT.Rows[0]["[" + ControlName + "]"] = oCk.Checked; ;
                    }




                }

                else if (oControl is DropDownList)
                {
                    DropDownList oDDL = (DropDownList)oControl;
                    try
                    {
                        oDT.Rows[0][ControlName] = oDDL.SelectedValue;
                    }
                    catch (Exception ex)
                    {
                        oDT.Rows[0]["[" + ControlName + "]"] = oDDL.SelectedValue;
                    }


                }
                else if (oControl is UserControl)
                {

                   Hashtable oImageValue = (Hashtable)HttpContext.Current.Session[ImageList];
                    if (HttpContext.Current.Session[ImageList] != null)
                    {
                        if (oImageValue.ContainsKey(oControl.ClientID))
                        {

                            oDT.Rows[0][ControlName] = oImageValue[oControl.ClientID].ToString();
                        }
                    }

                }

            }



        }



        public void SetDataToDB(Panel oPanel, DataTable oDT, String DBTable)
        {



            foreach (System.Web.UI.Control oControl in oPanel.Controls)
            {
                String ControlName = oControl.ID;
                if (ControlName != null)
                {
                    ControlName = ControlName.Replace("txtEform", string.Empty);
                }
                if (oControl is TextBox)
                {
                    TextBox oTextBox = (TextBox)oControl;

                    //if (oSMMember.Tables[0].Rows[0][ControlName].ToString() != null)
                    {
                        if (oTextBox.Text != string.Empty)
                        {
                            String sText = oTextBox.Text;
                            try
                            {
                                oDT.Rows[0][ControlName] = HttpUtility.HtmlEncode(sText);
                            }
                            catch (Exception ex)
                            {
                                oDT.Rows[0]["[" + ControlName + "]"] = HttpUtility.HtmlEncode(sText);
                            }
                        }
                    }

                }


                else if (oControl is CheckBox)
                {
                    CheckBox oCk = (CheckBox)oControl;

                    try
                    {
                        oDT.Rows[0][ControlName] = oCk.Checked;
                    }
                    catch (Exception ex)
                    {
                        oDT.Rows[0]["[" + ControlName + "]"] = oCk.Checked; ;
                    }




                }

                else if (oControl is DropDownList)
                {
                    DropDownList oDDL = (DropDownList)oControl;
                    try
                    {
                        oDT.Rows[0][ControlName] = oDDL.SelectedValue;
                    }
                    catch (Exception ex)
                    {
                        oDT.Rows[0]["[" + ControlName + "]"] = oDDL.SelectedValue;
                    }


                }
                else if (oControl is UserControl)
                {

                    Hashtable oImageValue = (Hashtable)HttpContext.Current.Session[ImageList];
                    if (HttpContext.Current.Session[ImageList] != null)
                    {
                        if (oImageValue.ContainsKey(oControl.ClientID))
                        {

                            oDT.Rows[0][ControlName] = oImageValue[oControl.ClientID].ToString();
                        }
                    }

                }

            }



        }


        // Clear Panel controls
        public void ClearPanelControls(TabContainer oTabCon)
        {
            foreach (TabPanel oPanel in oTabCon.Tabs)
            {
                foreach (System.Web.UI.Control oControl in oPanel.Controls)
                {
                    if (oControl is TextBox)
                    {
                        TextBox oTextBox = (TextBox)oControl;
                        oTextBox.Text = string.Empty;
                    }
                    else if (oControl is DropDownList)
                    {
                        DropDownList oDDL = (DropDownList)oControl;
                        oDDL.SelectedIndex = -1;
                    }
                    else if (oControl is CheckBox)
                    {
                        CheckBox oCk = (CheckBox)oControl;
                        oCk.Checked = false;
                    }
                    else if (oControl is Button)
                    {
                        Button oButton = (Button)oControl;
                        oButton.Enabled = false;
                    }


                }


            }


        }


        //Dataset value to each control on panel, a part of form design
        public void SetPanelControlValue(TabPanel oPanel, DataSet oDSFields)
        {


            if (oDSFields.Tables.Count > 0)
            {
                if (oDSFields.Tables[0].Rows.Count > 0)
                {
                    for (int n = 0; n <= oDSFields.Tables[0].Columns.Count - 1; n++)
                    {

                        String ControlName = oDSFields.Tables[0].Columns[n].ToString();
                        System.Web.UI.Control oCnt = oPanel.FindControl(ControlName);


                        if (oCnt != null)
                        {

                            if (oCnt is TextBox)
                            {
                                TextBox oTextBox = (TextBox)oCnt;

                                //Check Read only
                                if (oTextBoxList != null)
                                {

                                    if (IsNotDefaultSeasonID)
                                    {
                                        oTextBox.Enabled = false;
                                    }
                                    if (oDSFields.Tables[0].Rows[0][ControlName].ToString() != null)
                                    {
                                        if (oDSFields.Tables[0].Rows[0][ControlName].ToString() != string.Empty)
                                        {
                                            String sText = oDSFields.Tables[0].Rows[0][ControlName].ToString();

                                            oTextBox.Text = HttpUtility.HtmlDecode(sText);
                                        }
                                        else
                                        {

                                            oTextBox.Text = string.Empty;

                                        }
                                    }
                                }

                            }
                            else if (oCnt is DropDownList)
                            {
                                DropDownList oDDL = (DropDownList)oCnt;
                                //if (IsNotDefaultSeasonID)
                                //{
                                //    oDDL.Enabled = false;
                                //}
                                for (int i = 0; i <= oDDL.Items.Count - 1; i++)
                                    if (oDSFields.Tables[0].Rows[0][ControlName] != null)
                                    {
                                        if (oDSFields.Tables[0].Rows[0][ControlName].ToString() != string.Empty)
                                        {
                                            if (oDDL.Items[i].Value == oDSFields.Tables[0].Rows[0][ControlName].ToString())
                                            {
                                                oDDL.SelectedIndex = i;
                                                break;
                                            }
                                        }
                                    }
                            }
                            else if (oCnt is CheckBox)
                            {
                                CheckBox oCk = (CheckBox)oCnt;
                                if (IsNotDefaultSeasonID)
                                {
                                    oCk.Enabled = false;
                                }
                                if (oDSFields.Tables[0].Rows[0][ControlName] != null)
                                {
                                    if (oDSFields.Tables[0].Rows[0][ControlName].ToString() != string.Empty)
                                    {
                                        if (Convert.ToBoolean(oDSFields.Tables[0].Rows[0][ControlName]) == true)
                                        {
                                            oCk.Checked = true;
                                        }
                                        else
                                        {
                                            oCk.Checked = false;
                                        }
                                    }
                                }
                                //oCk.Checked = true;

                            }

                        }
                    }
                }
            }
        }


        public static string FilterSpecialChar(string SQL)
        {

            SQL = SQL.Replace("'", "''");


            return SQL;
        }

        public string GetDataType(DataColumn oDC, DataRow oRow)
        {
            if (oDC.DataType == System.Type.GetType("System.String"))
            {

                return "'" + FilterSpecialChar(oRow[oDC.ColumnName].ToString()) + "'";
            }
            else if (oDC.DataType == System.Type.GetType("System.Int32"))
            {

                if (oRow[oDC.ColumnName].ToString() == string.Empty)
                {
                    return "0";

                }
                else
                {
                    return FilterSpecialChar(oRow[oDC.ColumnName].ToString());
                }
            }

            else if (oDC.DataType == System.Type.GetType("System.Int64"))
            {
                if (oRow[oDC.ColumnName].ToString() == string.Empty)
                {
                    return "0";

                }
                else
                {
                    return oRow[oDC.ColumnName].ToString();
                }
            }

            else if (oDC.DataType == System.Type.GetType("System.Int16"))
            {
                if (oRow[oDC.ColumnName].ToString() == string.Empty)
                {
                    return "0";

                }
                else
                {
                    return oRow[oDC.ColumnName].ToString();
                }


            }


            else if (oDC.DataType == System.Type.GetType("System.Boolean"))
            {
                if (oRow[oDC.ColumnName].ToString() == "False")
                {
                    return "0";

                }
                else
                {
                    return "1";
                }
            }

            else if (oDC.DataType == System.Type.GetType("System.Decimal"))
            {
                if (oRow[oDC.ColumnName].ToString() == string.Empty)
                {
                    return "0";

                }
                else
                {
                    return oRow[oDC.ColumnName].ToString();
                }

            }

            else if (oDC.DataType == System.Type.GetType("System.DateTime"))
            { return "'" + oRow[oDC.ColumnName].ToString() + "'"; }



            return "'" + oRow[oDC.ColumnName].ToString() + "'";
        }







        //Render Control to Panel from database design table 
        public void SetPanelControl(Panel oPanel, List<DesignFormField> oControlList)
        {
          

            // DataMappingDataContext db = new DataMappingDataContext(AppModuleManager.ConnectionString);
            // hash table to hold all build in control
            Hashtable BuildInControlList = new Hashtable();
            if (HttpContext.Current.Session[SessionFormDesignControlList] != null)
            {
                BuildInControlList = (Hashtable)HttpContext.Current.Session[SessionFormDesignControlList];
            }

            //Session[AppConstManager.SessionFormDesignControlList] = string.Empty ;
            string cssStyle = string.Empty;
            int num;



            foreach (var oItem in oControlList)
            {

                // Button
                if (oItem.ControlType == Convert.ToInt32(ControlType.Button))
                {
                    Button oButton = new Button();
                    oButton.ID = oItem.Name;
                    // add some buid in control to session
                    if ((oButton.ID == BuildInChangeButtonControl)
                        || (oButton.ID == BuildInChangeLoginNameButtonControl))
                    {
                        if (!BuildInControlList.ContainsKey(oButton.ID))
                        {
                            BuildInControlList.Add(oButton.ID, oButton);
                        }
                    }

                    if (oButton.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oButton.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oButton.SkinID = ButtonSkinID;
                        }
                    }
                    else
                    {
                        oButton.SkinID = ButtonSkinID;
                    }

                    string cStyle = string.Empty;
                    bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                    if (IsTop == true)
                    {
                        cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;

                    }
                    cStyle = cssStyle;
                    bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                    if (IsLeft == true)
                    {
                        cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;

                    }

                    LiteralControl oLiteral = new LiteralControl();
                    oLiteral.Text = "<Div id=\"DIV" + oButton.ID + "\" Style='" + cssStyle + "'>";
                    // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                    oPanel.Controls.Add(oLiteral);
                    oButton.Text = oItem.Label;
                    oButton.ToolTip = oItem.Label;
                    oPanel.Controls.Add(oButton);
                    LiteralControl oEndLiteral = new LiteralControl();
                    oEndLiteral.Text = "</Div>";

                    //  Button oExist = (Button)oPanel.FindControl(oButton.ID);
                    //if (oExist == null)
                    {
                        oPanel.Controls.Add(oEndLiteral);
                    }
                    //oButton.Attributes.Add(AppConstManager.cStyle, cssStyle);

                }
                //Literal
                if (oItem.ControlType == Convert.ToInt32(ControlType.Literal))
                {
                    Literal oLiteral = new Literal();
                    oLiteral.ID = oItem.Name;

                    if (oLiteral.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oLiteral.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oLiteral.SkinID = LieralSkinID;
                        }
                    }
                    else
                    {
                        oLiteral.SkinID = LieralSkinID;
                    }

                    string cStyle = string.Empty;
                    bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                    if (IsTop == true)
                    {
                        cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;

                    }
                    cStyle = cssStyle;
                    bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                    if (IsLeft == true)
                    {
                        cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;

                    }
                    cStyle = cStyle + "; " + cssStyle;
                    String oHTML = oItem.Label.Replace("[position]", cStyle);
                    oLiteral.Text = oHTML;
                    oPanel.Controls.Add(oLiteral);
                }
                else if (oItem.ControlType == Convert.ToInt32(ControlType.DrowDownList))
                {
                    DropDownList oDDL = new DropDownList();
                    oDDL.ID = oItem.Name; // +oItem.ControlType;
                    oDDL.ToolTip = oItem.Name;


                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oDDL.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oDDL.SkinID = DropDownListSkinID;
                        }
                    }
                    else
                    {
                        oDDL.SkinID = DropDownListSkinID;
                    }
                    //oDDL.SkinID = "sDropDownList";
                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oDDL.Enabled = false;
                        }




                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;

                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        // oDDL.Attributes.Add(AppConstManager.cStyle, cssStyle);

                        //Get data source

                        if (oItem.QueryStatement != null)
                        {
                            if (oItem.QueryStatement != string.Empty)
                            {
                                DBManager oDB = new DBManager();
                                DataTable oDS = oDB.GetDataTableFromSQL(oItem.QueryStatement);



                                string DisplayValue = oDS.Columns[0].ColumnName;
                                string IDValue = oDS.Columns[1].ColumnName;
                                SetListToDropDownList(oDS, oDDL, DisplayValue, IDValue);
                                // oDDLManager.SetDatasetToDropDownList(oDS, oDDL, DisplayValue, IDValue, true);
                            }

                        }
                        else
                        {
                            if (oItem.DataListTypeID != null )
                            {

                                List<DesignFormDataList> oDDLList = DesignFormDataList.GetDataListByType(oItem.DataListType);

                                oDDL.DataSource = oDDLList;
                                oDDL.DataTextField = "DisplayField";
                                oDDL.DataValueField = "ValueField";
                                oDDL.DataBind();
                            }
                        }

                        //set label
                        if (oItem.IsReadOnly == true)
                        {
                            oDDL.Enabled = false;
                        }


                        SetControlLabel(oPanel, oItem, eFormID);

                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oDDL.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oDDL);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);


                    }


                }

                else if (oItem.ControlType == Convert.ToInt32(ControlType.CheckBox))
                {
                    CheckBox oChk = new CheckBox();
                    oChk.ID = oItem.Name; //+ oItem.ControlType;
                    oChk.ToolTip = oItem.Name;
                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oChk.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oChk.SkinID = CheckBoxSkinID;
                        }
                    }
                    else
                    {
                        oChk.SkinID = CheckBoxSkinID;
                    }

                    if (oItem.HasDefault == true)
                    {
                        oChk.Checked = true;

                    }
                    else
                    {
                        oChk.Checked = false;

                    }

                    //oChk.SkinID = "DesignCheckBox";
                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oChk.Enabled = false;
                        }

                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }
                        Boolean IsSetTrue = false;
                        //oChk.Attributes.Add(AppConstManager.cStyle, cssStyle);


                        //set label
                        SetControlLabel(oPanel, oItem, eFormID);
                        if (IsSetTrue)
                        {

                            oChk.Checked = true;
                        }


                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oChk.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oChk);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                        oChk = null;
                    }

                }
                //Text area
                else if (oItem.ControlType == Convert.ToInt32(ControlType.TextArea))
                {
                    TextBox oTextArea = new TextBox();
                    oTextArea.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
                    oTextArea.ID = oItem.Name;// +oItem.ControlType;
                    oTextArea.ToolTip = oItem.Name;
                    oTextArea.Width = Unit.Pixel(Convert.ToInt16(oItem.Width));
                    oTextArea.Height = Unit.Pixel(Convert.ToInt16(oItem.Height));
                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oTextArea.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oTextArea.SkinID = TextAreaSkinID;
                        }
                    }
                    else
                    {
                        oTextArea.SkinID = TextAreaSkinID;
                    }

                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oTextArea.Enabled = false;
                        }

                        if (oItem.IsReadOnly == true)
                        {
                            oTextArea.ReadOnly = true;
                        }

                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        //oTextArea.Attributes.Add(AppConstManager.cStyle, cssStyle);


                        //set label

                        SetControlLabel(oPanel, oItem, eFormID);
                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oTextArea.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oTextArea);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                        //oPanel.Controls.Add(oTextArea);
                    }
                }
                //Hyperlink
                else if (oItem.ControlType == Convert.ToInt32(ControlType.HyperLink))
                {
                    HyperLink oHyperlink = new HyperLink();

                    oHyperlink.ID = oItem.Name;// +oItem.ControlType;
                    oHyperlink.ToolTip = oItem.Message;
                    oHyperlink.Text = oItem.Message;

                    if (oItem.NewWindow == true)
                    {
                        //  "popOpenWindow('FileUpload.aspx', 'FileUpload', 
                        // 'ImageClientID=ImageLoad<%= this.ClientID %>&ImageHiddenPath=<%=this.HidImagePath.ClientID %>', 450, 120, 'No', false)" 
                        oHyperlink.NavigateUrl = @"Javascript: popOpenWindow('" + oItem.URL + "', 'URLNewWindow', '', 900, 600, 'Yes', true)";
                    }
                    else
                    {
                        oHyperlink.NavigateUrl = oItem.URL;
                    }
                    if (oItem.NewWindow == true)
                    {

                    }

                    if (oItem.IsMandatory == true)
                    {
                        oHyperlink.Target = "_blank";
                    }
                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oHyperlink.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oHyperlink.SkinID = HyperlinkSkinID;
                        }
                    }
                    else
                    {
                        oHyperlink.SkinID = HyperlinkSkinID;
                    }

                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oHyperlink.Enabled = false;
                        }



                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        //oTextArea.Attributes.Add(AppConstManager.cStyle, cssStyle);

                        LiteralControl oLiteral = new LiteralControl();
                        //set label
                        if (oItem.WithLabel == true)
                        {
                            SetControlLabel(oPanel, oItem, eFormID);
                            // oLiteral.Text = "<Div id=\"DIV" + oHyperlink.ID + "\" Style='" + cssStyle + "'>";
                        }


                        oLiteral.Text = "<Div id=\"DIV" + oHyperlink.ID + "\" Style='" + cssStyle + "'>";

                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oHyperlink);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                        //oPanel.Controls.Add(oTextArea);
                    }
                }
                //Image 
                else if (oItem.ControlType == Convert.ToInt32(ControlType.Image))
                {

                    //UserControl oImageControl = (UserControl)oPage.LoadControl("~/" + AppConstManager.ctrFormDesignImageControl);
                    ////Set this into a hash table and session
                  //  DesignFormField oImageDesign = (DesignFormField)oItem;

                    if (HttpContext.Current.Session[SessionImageList] != null)
                    {
                        oImageList = (Hashtable)HttpContext.Current.Session[SessionImageList];
                    }

                    if (!oImageList.ContainsKey(oItem.Name))
                    {

                        oImageList.Add(oItem.Name, oItem);
                        HttpContext.Current.Session[SessionImageList] = oImageList;
                    }


                    //DesignFormField oTextBoxField = (DesignFormField)oItem;

                    //if (HttpContext.Current.Session[SessionTextBoxList] != null)
                    //{
                    //    oTextBoxList = (Hashtable)HttpContext.Current.Session[SessionTextBoxList];
                    //}

                    //if (!oTextBoxList.ContainsKey(oItem.Name))
                    //{

                    //    oTextBoxList.Add(oItem.Name, oTextBoxField);
                    //    HttpContext.Current.Session[SessionTextBoxList] = oTextBoxList;
                    //}


                }
                //datetime
                else if (oItem.ControlType == Convert.ToInt32(ControlType.DateTime))
                {
                    TextBox oDateTime = new TextBox();
                    oDateTime.Width = Unit.Pixel(Convert.ToInt16(oItem.Width));
                    oDateTime.Height = Unit.Pixel(Convert.ToInt16(oItem.Height));

                    DesignFormField oTextBoxdesign = (DesignFormField)oItem;

                    if (HttpContext.Current.Session[SessionTextBoxList] != null)
                    {
                        oTextBoxList = (Hashtable)HttpContext.Current.Session[SessionTextBoxList];
                    }

                    if (!oTextBoxList.ContainsKey(oItem.Name))
                    {

                        oTextBoxList.Add(oItem.Name, oTextBoxdesign);
                        HttpContext.Current.Session[SessionTextBoxList] = oTextBoxList;
                    }

                    oDateTime.ID = oItem.Name;// +oItem.ControlType;
                    oDateTime.ToolTip = oItem.Name;
                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oDateTime.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oDateTime.SkinID = TextBoxSkinID;
                        }
                    }
                    else
                    {
                        oDateTime.SkinID = TextBoxSkinID;
                    }

                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oDateTime.Enabled = false;
                        }

                        //if (oItem.IsReadOnly == true)
                        //{
                        //    oDateTime.ReadOnly = true;
                        //}

                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        // oTextBox.Attributes.Add(AppConstManager.cStyle, cssStyle);

                        oDateTime.MaxLength = 40;
                        //set label


                        SetControlLabel(oPanel, oItem, eFormID);
                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oDateTime.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oDateTime);
                        //Add calendar
                        if (oItem.IsCalendar == true)
                        {
                            DateFormat oDF = DateFormat.Load<DateFormat>(oItem.DateFormat);
                            FormDesign.AddCalendar(oDateTime.ID, oPanel, oDF.Format);
                        }
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                    }

                }

               //Money box

                //text box
                else if (oItem.ControlType == Convert.ToInt32(ControlType.TextBox) ||
                    oItem.ControlType == Convert.ToInt32(ControlType.Money))
                {
                    TextBox oTextBox = new TextBox();

                    oTextBox.ID = "txtEform" + oItem.Name;// +oItem.ControlType;
                    oTextBox.ToolTip = oItem.Name;
                    oTextBox.Width = Unit.Pixel(Convert.ToInt16(oItem.Width));
                    oTextBox.Height = Unit.Pixel(Convert.ToInt16(oItem.Height));

                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oTextBox.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oTextBox.SkinID = TextBoxSkinID;
                        }
                    }
                    else
                    {
                        oTextBox.SkinID = TextBoxSkinID;
                    }

                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oTextBox.Enabled = false;
                        }
                        if (oItem.ControlType == Convert.ToInt32(ControlType.TextBox))
                        {
                            if (oItem.IsReadOnly == true)
                            {
                                oTextBox.ReadOnly = true;
                                DesignFormField oDesign = (DesignFormField)oItem;

                                if (HttpContext.Current.Session[SessionTextBoxList] != null)
                                {
                                    oTextBoxList = (Hashtable)HttpContext.Current.Session[SessionTextBoxList];
                                }

                                if (!oTextBoxList.ContainsKey(oItem.Name))
                                {

                                    oTextBoxList.Add(oItem.Name, oDesign);
                                    HttpContext.Current.Session[SessionTextBoxList] = oTextBoxList;
                                }

                            }
                        }
                        if (oItem.ControlType == Convert.ToInt32(ControlType.Money))
                        {
                            DesignFormField oDesign = (DesignFormField)oItem;

                            if (HttpContext.Current.Session[SessionTextBoxList] != null)
                            {
                                oTextBoxList = (Hashtable)HttpContext.Current.Session[SessionTextBoxList];
                            }

                            if (!oTextBoxList.ContainsKey(oItem.Name))
                            {

                                oTextBoxList.Add(oItem.Name, oDesign);
                                HttpContext.Current.Session[SessionTextBoxList] = oTextBoxList;
                            }

                        }
                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        // oTextBox.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        string MaxLength = Option.GetOptionByKey(MaxLenghtOptionKey);
                        if (MaxLength == string.Empty)
                        {
                            MaxLength = "100";
                        }
                        oTextBox.MaxLength = Convert.ToInt32(MaxLength);
                        //set label


                        SetControlLabel(oPanel, oItem, eFormID);
                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oTextBox.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oTextBox);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                        //oPanel.Controls.Add(oTextBox);
                    }



                }

            }
            HttpContext.Current.Session[SessionFormDesignControlList] = BuildInControlList;


        }


        public void SetPanelControl(TabPanel oPanel, List<DesignFormField> oControlList)
        {
          
            // DataMappingDataContext db = new DataMappingDataContext(AppModuleManager.ConnectionString);
            // hash table to hold all build in control
            Hashtable BuildInControlList = new Hashtable();
            if (HttpContext.Current.Session[SessionFormDesignControlList] != null)
            {
                BuildInControlList = (Hashtable)HttpContext.Current.Session[SessionFormDesignControlList];
            }

            //Session[AppConstManager.SessionFormDesignControlList] = string.Empty ;
            string cssStyle = string.Empty;
            int num;



            foreach (var oItem in oControlList)
            {

                // Button
                if (oItem.ControlType == Convert.ToInt32(ControlType.Button))
                {
                    Button oButton = new Button();
                    oButton.ID = oItem.Name;
                    // add some buid in control to session
                    if ((oButton.ID == BuildInChangeButtonControl)
                        || (oButton.ID == BuildInChangeLoginNameButtonControl))
                    {
                        if (!BuildInControlList.ContainsKey(oButton.ID))
                        {
                            BuildInControlList.Add(oButton.ID, oButton);
                        }
                    }

                    if (oButton.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oButton.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oButton.SkinID = ButtonSkinID;
                        }
                    }
                    else
                    {
                        oButton.SkinID = ButtonSkinID;
                    }

                    string cStyle = string.Empty;
                    bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                    if (IsTop == true)
                    {
                        cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;

                    }
                    cStyle = cssStyle;
                    bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                    if (IsLeft == true)
                    {
                        cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;

                    }

                    LiteralControl oLiteral = new LiteralControl();
                    oLiteral.Text = "<Div id=\"DIV" + oButton.ID + "\" Style='" + cssStyle + "'>";
                    // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                    oPanel.Controls.Add(oLiteral);
                    oButton.Text = oItem.Label;
                    oButton.ToolTip = oItem.Label;
                    oPanel.Controls.Add(oButton);
                    LiteralControl oEndLiteral = new LiteralControl();
                    oEndLiteral.Text = "</Div>";

                    //  Button oExist = (Button)oPanel.FindControl(oButton.ID);
                    //if (oExist == null)
                    {
                        oPanel.Controls.Add(oEndLiteral);
                    }
                    //oButton.Attributes.Add(AppConstManager.cStyle, cssStyle);

                }
                //Literal
                if (oItem.ControlType == Convert.ToInt32(ControlType.Literal))
                {
                    Literal oLiteral = new Literal();
                    oLiteral.ID = oItem.Name;

                    if (oLiteral.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oLiteral.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oLiteral.SkinID = LieralSkinID;
                        }
                    }
                    else
                    {
                        oLiteral.SkinID = LieralSkinID;
                    }

                    string cStyle = string.Empty;
                    bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                    if (IsTop == true)
                    {
                        cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;

                    }
                    cStyle = cssStyle;
                    bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                    if (IsLeft == true)
                    {
                        cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;

                    }
                    cStyle = cStyle + "; " + cssStyle;
                    String oHTML = oItem.Label.Replace("[position]", cStyle);
                    oLiteral.Text = oHTML;
                    oPanel.Controls.Add(oLiteral);
                }
                else if (oItem.ControlType == Convert.ToInt32(ControlType.DrowDownList))
                {
                    DropDownList oDDL = new DropDownList();
                    oDDL.ID = oItem.Name; // +oItem.ControlType;
                    oDDL.ToolTip = oItem.Name;


                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oDDL.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oDDL.SkinID = DropDownListSkinID;
                        }
                    }
                    else
                    {
                        oDDL.SkinID = DropDownListSkinID;
                    }
                    //oDDL.SkinID = "sDropDownList";
                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oDDL.Enabled = false;
                        }




                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;

                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        // oDDL.Attributes.Add(AppConstManager.cStyle, cssStyle);

                        //Get data source

                        if (oItem.QueryStatement != null)
                        {
                            if (oItem.QueryStatement != string.Empty)
                            {
                                DBManager oDB = new DBManager();
                                DataTable oDS = oDB.GetDataTableFromSQL(oItem.QueryStatement);



                                string DisplayValue = oDS.Columns[0].ColumnName;
                                string IDValue = oDS.Columns[1].ColumnName;
                                SetListToDropDownList(oDS, oDDL, DisplayValue, IDValue);
                                // oDDLManager.SetDatasetToDropDownList(oDS, oDDL, DisplayValue, IDValue, true);
                            }

                        }
                        else
                        {
                            if (oItem.DataListType != null && oItem.DataListType != string.Empty)
                            {

                                List<DesignFormDataList> oDDLList = DesignFormDataList.GetDataListByType(oItem.DataListType);

                                oDDL.DataSource = oDDLList;
                                oDDL.DataTextField = "DisplayField";
                                oDDL.DataValueField = "ValueField";
                                oDDL.DataBind();
                            }
                        }

                        //set label
                        if (oItem.IsReadOnly == true)
                        {
                            oDDL.Enabled = false;
                        }


                        SetControlLabel(oPanel, oItem, eFormID);

                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oDDL.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oDDL);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);


                    }


                }

                else if (oItem.ControlType == Convert.ToInt32(ControlType.CheckBox))
                {
                    CheckBox oChk = new CheckBox();
                    oChk.ID = oItem.Name; //+ oItem.ControlType;
                    oChk.ToolTip = oItem.Name;
                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oChk.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oChk.SkinID = CheckBoxSkinID;
                        }
                    }
                    else
                    {
                        oChk.SkinID = CheckBoxSkinID;
                    }

                    if (oItem.HasDefault== true)
                    {
                        oChk.Checked = true;

                    }
                    else
                    {
                        oChk.Checked = false;

                    }

                    //oChk.SkinID = "DesignCheckBox";
                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oChk.Enabled = false;
                        }

                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }
                        Boolean IsSetTrue = false;
                        //oChk.Attributes.Add(AppConstManager.cStyle, cssStyle);


                        //set label
                        SetControlLabel(oPanel, oItem, eFormID);
                        if (IsSetTrue)
                        {

                            oChk.Checked = true;
                        }


                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oChk.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oChk);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                        oChk = null;
                    }

                }
                //Text area
                else if (oItem.ControlType == Convert.ToInt32(ControlType.TextArea))
                {
                    TextBox oTextArea = new TextBox();
                    oTextArea.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
                    oTextArea.ID = oItem.Name;// +oItem.ControlType;
                    oTextArea.ToolTip = oItem.Name;
                    oTextArea.Width = Unit.Pixel(Convert.ToInt16(oItem.Width));
                    oTextArea.Height = Unit.Pixel(Convert.ToInt16(oItem.Height));
                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oTextArea.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oTextArea.SkinID = TextAreaSkinID;
                        }
                    }
                    else
                    {
                        oTextArea.SkinID = TextAreaSkinID;
                    }

                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oTextArea.Enabled = false;
                        }

                        if (oItem.IsReadOnly == true)
                        {
                            oTextArea.ReadOnly = true;
                        }

                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        //oTextArea.Attributes.Add(AppConstManager.cStyle, cssStyle);


                        //set label

                        SetControlLabel(oPanel, oItem, eFormID);
                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oTextArea.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oTextArea);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                        //oPanel.Controls.Add(oTextArea);
                    }
                }
                //Hyperlink
                else if (oItem.ControlType == Convert.ToInt32(ControlType.HyperLink))
                {
                    HyperLink oHyperlink = new HyperLink();

                    oHyperlink.ID = oItem.Name;// +oItem.ControlType;
                    oHyperlink.ToolTip = oItem.Message;
                    oHyperlink.Text = oItem.Message;

                    if (oItem.NewWindow == true)
                    {
                        //  "popOpenWindow('FileUpload.aspx', 'FileUpload', 
                        // 'ImageClientID=ImageLoad<%= this.ClientID %>&ImageHiddenPath=<%=this.HidImagePath.ClientID %>', 450, 120, 'No', false)" 
                        oHyperlink.NavigateUrl = @"Javascript: popOpenWindow('" + oItem.URL + "', 'URLNewWindow', '', 900, 600, 'Yes', true)";
                    }
                    else
                    {
                        oHyperlink.NavigateUrl = oItem.URL;
                    }
                    if (oItem.NewWindow == true)
                    {

                    }

                    if (oItem.IsMandatory == true)
                    {
                        oHyperlink.Target = "_blank";
                    }
                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oHyperlink.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oHyperlink.SkinID = HyperlinkSkinID;
                        }
                    }
                    else
                    {
                        oHyperlink.SkinID = HyperlinkSkinID;
                    }

                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oHyperlink.Enabled = false;
                        }



                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        //oTextArea.Attributes.Add(AppConstManager.cStyle, cssStyle);

                        LiteralControl oLiteral = new LiteralControl();
                        //set label
                        if (oItem.WithLabel == true)
                        {
                            SetControlLabel(oPanel, oItem, eFormID);
                            // oLiteral.Text = "<Div id=\"DIV" + oHyperlink.ID + "\" Style='" + cssStyle + "'>";
                        }


                        oLiteral.Text = "<Div id=\"DIV" + oHyperlink.ID + "\" Style='" + cssStyle + "'>";

                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oHyperlink);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                        //oPanel.Controls.Add(oTextArea);
                    }
                }
                //Image 
                else if (oItem.ControlType == Convert.ToInt32(ControlType.Image))
                {

                    //UserControl oImageControl = (UserControl)oPage.LoadControl("~/" + AppConstManager.ctrFormDesignImageControl);
                    ////Set this into a hash table and session
                    //  DesignFormField oImageDesign = (DesignFormField)oItem;

                    if (HttpContext.Current.Session[SessionImageList] != null)
                    {
                        oImageList = (Hashtable)HttpContext.Current.Session[SessionImageList];
                    }

                    if (!oImageList.ContainsKey(oItem.Name))
                    {

                        oImageList.Add(oItem.Name, oItem);
                        HttpContext.Current.Session[SessionImageList] = oImageList;
                    }


                    //DesignFormField oTextBoxField = (DesignFormField)oItem;

                    //if (HttpContext.Current.Session[SessionTextBoxList] != null)
                    //{
                    //    oTextBoxList = (Hashtable)HttpContext.Current.Session[SessionTextBoxList];
                    //}

                    //if (!oTextBoxList.ContainsKey(oItem.Name))
                    //{

                    //    oTextBoxList.Add(oItem.Name, oTextBoxField);
                    //    HttpContext.Current.Session[SessionTextBoxList] = oTextBoxList;
                    //}


                }
                //datetime
                else if (oItem.ControlType == Convert.ToInt32(ControlType.DateTime))
                {
                    TextBox oDateTime = new TextBox();
                    oDateTime.Width = Unit.Pixel(Convert.ToInt16(oItem.Width));
                    oDateTime.Height = Unit.Pixel(Convert.ToInt16(oItem.Height));

                    DesignFormField oTextBoxdesign = (DesignFormField)oItem;

                    if (HttpContext.Current.Session[SessionTextBoxList] != null)
                    {
                        oTextBoxList = (Hashtable)HttpContext.Current.Session[SessionTextBoxList];
                    }

                    if (!oTextBoxList.ContainsKey(oItem.Name))
                    {

                        oTextBoxList.Add(oItem.Name, oTextBoxdesign);
                        HttpContext.Current.Session[SessionTextBoxList] = oTextBoxList;
                    }

                    oDateTime.ID = oItem.Name;// +oItem.ControlType;
                    oDateTime.ToolTip = oItem.Name;
                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oDateTime.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oDateTime.SkinID = TextBoxSkinID;
                        }
                    }
                    else
                    {
                        oDateTime.SkinID = TextBoxSkinID;
                    }

                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oDateTime.Enabled = false;
                        }

                        //if (oItem.IsReadOnly == true)
                        //{
                        //    oDateTime.ReadOnly = true;
                        //}

                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        // oTextBox.Attributes.Add(AppConstManager.cStyle, cssStyle);

                        oDateTime.MaxLength = 40;
                        //set label


                        SetControlLabel(oPanel, oItem, eFormID);
                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oDateTime.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oDateTime);
                        //Add calendar
                        if (oItem.IsCalendar == true)
                        {
                            DateFormat oDF = DateFormat.Load<DateFormat>(oItem.DateFormat);
                            FormDesign.AddCalendar(oDateTime.ID, oPanel, oDF.Format);
                        }
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                    }

                }

               //Money box

                //text box
                else if (oItem.ControlType == Convert.ToInt32(ControlType.TextBox) ||
                    oItem.ControlType == Convert.ToInt32(ControlType.Money))
                {
                    TextBox oTextBox = new TextBox();

                    oTextBox.ID = "txtEform" + oItem.Name;// +oItem.ControlType;
                    oTextBox.ToolTip = oItem.Name;
                    oTextBox.Width = Unit.Pixel(Convert.ToInt16(oItem.Width));
                    oTextBox.Height = Unit.Pixel(Convert.ToInt16(oItem.Height));

                    if (oItem.SkinID != null)
                    {
                        if (oItem.SkinID != string.Empty)
                        {
                            oTextBox.SkinID = oItem.SkinID;
                        }
                        else
                        {
                            oTextBox.SkinID = TextBoxSkinID;
                        }
                    }
                    else
                    {
                        oTextBox.SkinID = TextBoxSkinID;
                    }

                    if (oItem.IsVisible == true)
                    {
                        if (oItem.IsEnabled == false)
                        {
                            oTextBox.Enabled = false;
                        }
                        if (oItem.ControlType == Convert.ToInt32(ControlType.TextBox))
                        {
                            if (oItem.IsReadOnly == true)
                            {
                                oTextBox.ReadOnly = true;
                                DesignFormField oDesign = (DesignFormField)oItem;

                                if (HttpContext.Current.Session[SessionTextBoxList] != null)
                                {
                                    oTextBoxList = (Hashtable)HttpContext.Current.Session[SessionTextBoxList];
                                }

                                if (!oTextBoxList.ContainsKey(oItem.Name))
                                {

                                    oTextBoxList.Add(oItem.Name, oDesign);
                                    HttpContext.Current.Session[SessionTextBoxList] = oTextBoxList;
                                }

                            }
                        }
                        if (oItem.ControlType == Convert.ToInt32(ControlType.Money))
                        {
                            DesignFormField oDesign = (DesignFormField)oItem;

                            if (HttpContext.Current.Session[SessionTextBoxList] != null)
                            {
                                oTextBoxList = (Hashtable)HttpContext.Current.Session[SessionTextBoxList];
                            }

                            if (!oTextBoxList.ContainsKey(oItem.Name))
                            {

                                oTextBoxList.Add(oItem.Name, oDesign);
                                HttpContext.Current.Session[SessionTextBoxList] = oTextBoxList;
                            }

                        }
                        bool IsTop = int.TryParse(oItem.ControlTop.ToString(), out num);
                        if (IsTop == true)
                        {
                            cssStyle = ccsRelative + ccsTop + num.ToString() + ccsPX;
                            // oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsRelative);
                            // oDDL.Attributes.Add(AppConstManager.cStyle, num.ToString() + AppConstManager.ccsPX);
                        }
                        bool IsLeft = int.TryParse(oItem.ControlLeft.ToString(), out num);
                        if (IsLeft == true)
                        {
                            cssStyle = cssStyle + ccsLeft + num.ToString() + ccsPX;
                            //oDDL.Attributes.Add(AppConstManager.cStyle, AppConstManager.ccsLeft + num.ToString() + AppConstManager.ccsPX);
                        }

                        // oTextBox.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        string MaxLength = Option.GetOptionByKey(MaxLenghtOptionKey);
                        if (MaxLength == string.Empty)
                        {
                            MaxLength = "100";
                        }
                        oTextBox.MaxLength = Convert.ToInt32(MaxLength);
                        //set label


                        SetControlLabel(oPanel, oItem, eFormID);
                        LiteralControl oLiteral = new LiteralControl();
                        oLiteral.Text = "<Div id=\"DIV" + oTextBox.ID + "\" Style='" + cssStyle + "'>";
                        // oLabel.Attributes.Add(AppConstManager.cStyle, cssStyle);
                        oPanel.Controls.Add(oLiteral);

                        oPanel.Controls.Add(oTextBox);
                        LiteralControl oEndLiteral = new LiteralControl();
                        oEndLiteral.Text = "</Div>";
                        oPanel.Controls.Add(oEndLiteral);
                        //oPanel.Controls.Add(oTextBox);
                    }



                }

            }
            HttpContext.Current.Session[SessionFormDesignControlList] = BuildInControlList;


        }

        public static void SetActiveIndex(TabContainer oTC, String TabName, String TabIdentity)
        {
            for (int n = 0; n <= oTC.Tabs.Count - 1; n++)
            {
                if (oTC.Tabs[n].HeaderText.ToUpper() == TabName.ToUpper())
                {
                    oTC.ActiveTabIndex = n;
                    break;
                }
                if ( (oTC.Tabs[n].HeaderText == string.Empty ) &&  (oTC.Tabs[n].ID == TabIdentity))
                {
                    oTC.ActiveTabIndex = n;
                    break;
                }
            }

        }



        public static void SetListToDropDownList(DataTable oDS, DropDownList oDDL, String TextField, String IDField)
        {
            oDDL.DataSource = oDS;
            oDDL.DataTextField = TextField;
            oDDL.DataValueField = IDField;
            oDDL.DataBind();

        }






        public static string AddNewFieldToTabContainer(TabContainer oTC, String FieldName,
            String TabName, WebControl oControl, Boolean IsLabel)
        {


            for (int n = 0; n <= oTC.Tabs.Count - 1; n++)
            {
                if (TabName.ToUpper() == oTC.Tabs[n].HeaderText.ToUpper())
                {
                    if (IsLabel)
                    {
                        Label oLabel = new Label();
                        oTC.Tabs[n].Controls.Add(oLabel);
                    }
                    oTC.Tabs[n].Controls.Add(oControl);


                }

            }


            return string.Empty;

        }



        public static void AddCalendar(String ID, Control oActiveTab, string DateFormat)
        {
            CalendarExtender oCalendar = new CalendarExtender();
            oCalendar.TargetControlID = ID;
            oCalendar.Format = DateFormat;
            oCalendar.CssClass = "calendar";
            oActiveTab.Controls.Add(oCalendar);

        }


        public string CreateQuerStatement(Int64 ID, DataTable oDS, String TableName, string IDColumn)
        {
            string UnUpdateedID = IDColumn;

            Hashtable oImageValue = (Hashtable)HttpContext.Current.Session[FormDesign.SessionImageList];

           
            StringBuilder sQuery = new StringBuilder();
            DataRow DTRow = oDS.Rows[0];

            if (ID == 0)
            {


                Int32 nCount = 0;
                sQuery.Append("Insert into " + TableName + " ( ");
                for (int n = 0; n <= oDS.Columns.Count - 1; n++)
                {
                    if (oDS.Columns[n].ColumnName.ToString() != UnUpdateedID)
                    {
                        if (nCount == 0)
                        {
                            sQuery.Append(oDS.Columns[n].ColumnName);
                        }
                        else
                        {
                            sQuery.Append(", " + oDS.Columns[n].ColumnName);
                        }
                        nCount++;
                    }

                }



                nCount = 0;
                sQuery.Append(" ) Select ");
                for (int n = 0; n <= oDS.Columns.Count - 1; n++)
                {

                    if (oDS.Columns[n].ColumnName.ToString() != UnUpdateedID)
                    {
                        if (nCount == 0)
                        {
                            if (oImageValue != null)
                            {
                                if (oImageValue.Contains(oDS.Columns[n].ColumnName))
                                {
                                    sQuery.Append("'" + oImageValue[oDS.Columns[n].ColumnName].ToString() + "'");
                                }
                                else
                                {
                                    DataColumn oDTColumn = oDS.Columns[n];
                                    sQuery.Append(GetDataType(oDTColumn, DTRow));
                                }
                            
                            }
                            else
                            {
                               
                             
                                    DataColumn oDTColumn = oDS.Columns[n];
                                    sQuery.Append(GetDataType(oDTColumn, DTRow));
                               
                            }


                        }
                        else
                        {

                            if (oImageValue != null)
                            {
                                if (oImageValue.Contains(oDS.Columns[n].ColumnName))
                                {
                                    sQuery.Append(", '" + oImageValue[oDS.Columns[n].ColumnName].ToString() + "'");
                                }
                                else
                                {
                                    DataColumn oDTColumn = oDS.Columns[n];
                                    sQuery.Append(", " + GetDataType(oDTColumn, DTRow));
                                }

                            }
                            else
                            {

                                DataColumn oDTColumn = oDS.Columns[n];
                                sQuery.Append(", " + GetDataType(oDTColumn, DTRow));
                            }
                           
                        }
                        nCount++;
                    }



                }

                //oDS.Tables[0].Columns[0].DataType 

            }
            else
            {
                sQuery.Append("Update " + TableName + " Set ");
                Int32 nCount = 0;
                for (int n = 0; n <= oDS.Columns.Count - 1; n++)
                {
                    if (oDS.Columns[n].ColumnName.ToString() != UnUpdateedID)
                    {


                        DataColumn oDTColumn = oDS.Columns[n];

                        if (nCount == 0)
                        {
                            if (oImageValue != null)
                            {
                                if (oImageValue.Contains(oDS.Columns[n].ColumnName))
                                {
                                    sQuery.Append("," + oDS.Columns[n].ColumnName + "=");

                                    sQuery.Append(" '" + oImageValue[oDS.Columns[n].ColumnName].ToString() + "'");
                                }
                                else
                                {
                                    sQuery.Append(oDS.Columns[n].ColumnName + "=");
                                    sQuery.Append(GetDataType(oDTColumn, DTRow));
                                }
                            }
                            else
                            {
                                sQuery.Append(oDS.Columns[n].ColumnName + "=");
                                sQuery.Append(GetDataType(oDTColumn, DTRow));
                            
                            }
                        }

                        else
                        {
                            if (oImageValue != null)
                            {
                                if (oImageValue.Contains(oDS.Columns[n].ColumnName))
                                {
                                    sQuery.Append("," + oDS.Columns[n].ColumnName + "=");

                                    sQuery.Append(" '" + oImageValue[oDS.Columns[n].ColumnName].ToString() + "'");
                                }
                                else
                                {
                                    sQuery.Append("," + oDS.Columns[n].ColumnName + "=");
                                    sQuery.Append(GetDataType(oDTColumn, DTRow));
                                }
                            }
                            else
                            {
                                sQuery.Append("," + oDS.Columns[n].ColumnName + "=");
                                sQuery.Append(GetDataType(oDTColumn, DTRow));
                            }
                        }
                        nCount++;
                    }

                
                }
                sQuery.Append(" Where " + IDColumn + " =" + ID.ToString());
            }

            return sQuery.ToString();
        }


        public static DataTable SetTabValueIntoDataSet(DataTable oTabDataInfo, Boolean IsGeneralTable,
       TabContainer tabContainer, String GeneralTable, string DBTable)
        {
            // UsersManager oUserManager = new UsersManager();


            foreach (TabPanel panel in tabContainer.Tabs)
            {
                FormDesign oDesign = new FormDesign();
                oDesign.SetDataToDB(panel, oTabDataInfo, DBTable);

            }

            return oTabDataInfo;
        }



        public static DataTable SetTabValueIntoDataSet(DataTable oTabDataInfo, Boolean IsGeneralTable,
       Panel  oPanel, String GeneralTable, string DBTable)
        {
            // UsersManager oUserManager = new UsersManager();


         
                FormDesign oDesign = new FormDesign();
                oDesign.SetDataToDB(oPanel, oTabDataInfo, DBTable);


            return oTabDataInfo;
        }


        public static void SetDataToControls(TabContainer oTab, string DBTable, Int64 IDValue, string IDColumnName, int eFormID)
        {
            if (eFormID > 0 && IDValue > 0)
            {
                DataTable oDT = DesignFormField.GetDataTableStructure(DBTable, IDColumnName, IDValue);
                if (oDT.Rows.Count > 0)
                {
                    foreach (TabPanel panel in oTab.Tabs)
                    {
                        FormDesign oDesign = new FormDesign();
                        SetDataToControls(panel, oDT, eFormID);

                    }
                }
            }
        
        }


        public static void SetDataToControls(Panel panel, string DBTable, Int64 IDValue, string IDColumnName, int eFormID)
        {

            DataTable oDT = DesignFormField.GetDataTableStructure(DBTable, IDColumnName, IDValue);
            if (oDT.Rows.Count > 0)
            {
              
                    FormDesign oDesign = new FormDesign();
                    SetDataToControlsOnPanel(panel, oDT, eFormID);

               
            }

        }


        public static void SetDataToControlsOnPanel(Panel oPanel, DataTable oDT, int eFormID)
        {



            foreach (System.Web.UI.Control oControl in oPanel.Controls)
            {
                for (int n = 0; n <= oDT.Columns.Count - 1; n++)
                {
                    string ColumnName = oDT.Columns[n].ColumnName;
                    Control oWControl = oPanel.FindControl("txtEform" + ColumnName) as Control;
                    if (oWControl != null)
                    {
                        TextBox oText = oWControl as TextBox;
                        oText.Text = oDT.Rows[0][ColumnName].ToString();
                    }
                    Control oOtherControl = oPanel.FindControl(ColumnName) as Control;
                    if (oOtherControl != null)
                    {

                        if (oOtherControl is TextBox)
                        {
                            TextBox oText = oOtherControl as TextBox;
                            oText.Text = oDT.Rows[0][ColumnName].ToString();
                        }
                        else if (oOtherControl is DropDownList)
                        {
                            DropDownList oDDL = oOtherControl as DropDownList;
                            oDDL.SelectedValue = oDT.Rows[0][ColumnName].ToString();
                        }
                        else if (oOtherControl is CheckBox)
                        {
                            CheckBox oChk = oOtherControl as CheckBox;
                            if (oDT.Rows[0][ColumnName].ToString() != string.Empty)
                            {
                                if (Convert.ToBoolean(oDT.Rows[0][ColumnName]) == false)
                                {
                                    oChk.Checked = false;
                                }
                                else
                                {
                                    oChk.Checked = true;
                                }
                            }
                        }
                        else if (oOtherControl is UserControl)
                        {

                           

                            if (oDT.Rows[0][ColumnName] != null)
                            {

                                if (oDT.Rows[0][ColumnName].ToString() != string.Empty)
                                {
                                    if (HttpContext.Current.Session[ImageList] == null)
                                    {
                                        Hashtable oImageValue = new Hashtable();
                                        oImageValue.Add(oOtherControl.ClientID, oDT.Rows[0][ColumnName].ToString());
                                        HttpContext.Current.Session[ImageList] = oImageValue;
                                        HttpContext.Current.Session[FormDesign.SessionImageList] = oImageValue;
                                        ucImageControl oImage = oOtherControl as ucImageControl;
                                          
                                        oImage.SetImagePath();

                                    }
                                    else
                                    {
                                        if (oDT.Rows[0][ColumnName] != null)
                                        {
                                            Hashtable oImageValue = HttpContext.Current.Session[ImageList] as Hashtable;
                                            if (!oImageValue.ContainsKey(oOtherControl.ClientID))
                                            {

                                                oImageValue.Add(oOtherControl.ClientID, oDT.Rows[0][ColumnName].ToString());
                                                HttpContext.Current.Session[ImageList] = oImageValue;
                                                HttpContext.Current.Session[FormDesign.SessionImageList] = oImageValue;
                                                ucImageControl oImage = oOtherControl as ucImageControl;
                                                oImage.SetImagePath();

                                            }

                                        }

                                    }


                                }


                            }



                        }

                    }
                }

            }
        }

        public static void SetDataToControls(TabPanel oPanel, DataTable oDT, int eFormID)
        {

          

            foreach (System.Web.UI.Control oControl in oPanel.Controls)
            {
                for (int n = 0; n <= oDT.Columns.Count - 1; n++)
                {
                    string ColumnName = oDT.Columns[n].ColumnName;
                    Control oWControl = oPanel.FindControl("txtEform" + ColumnName )as Control;
                    if (oWControl != null)
                    {
                        TextBox oText = oWControl as TextBox;
                        oText.Text = oDT.Rows[0][ColumnName].ToString();
                    }
                    Control oOtherControl = oPanel.FindControl( ColumnName) as Control;
                    if (oOtherControl != null)
                    {

                        if (oOtherControl is TextBox)
                        {
                            TextBox oText = oOtherControl as TextBox;
                            oText.Text = oDT.Rows[0][ColumnName].ToString();
                        }
                        else if (oOtherControl is DropDownList)
                        {
                            DropDownList oDDL = oOtherControl as DropDownList;
                            oDDL.SelectedValue = oDT.Rows[0][ColumnName].ToString();
                        }
                        else if (oOtherControl is CheckBox)
                        {
                            CheckBox oChk = oOtherControl as CheckBox;
                            if (oDT.Rows[0][ColumnName].ToString() != string.Empty )
                            {
                                if (Convert.ToBoolean(oDT.Rows[0][ColumnName]) == false)
                                {
                                    oChk.Checked = false;
                                }
                                else
                                {
                                    oChk.Checked = true; 
                                }
                            }
                        }
                        else if (oOtherControl is UserControl )
                        {
                            
                              
                                
                            if (oDT.Rows[0][ColumnName] != null)
                            {
                              
                                    if (oDT.Rows[0][ColumnName].ToString() != string.Empty)
                                    {
                                         if (HttpContext.Current.Session[ImageList] == null)
                                         {
                                             Hashtable oImageValue =  new Hashtable();
                                             oImageValue.Add(oOtherControl.ClientID, oDT.Rows[0][ColumnName].ToString());
                                             HttpContext.Current.Session[ImageList]  = oImageValue;
                                             ucImageControl oImage = oOtherControl as ucImageControl;
                                             HttpContext.Current.Session[FormDesign.SessionImageList] = oImageValue;
                                             oImage.SetImagePath();
                                                         
                                         }
                                         else
                                         {
                                             if (oDT.Rows[0][ColumnName] != null)
                                             {
                                                 Hashtable oImageValue = HttpContext.Current.Session[ImageList] as Hashtable;
                                                 if (!oImageValue.ContainsKey(oOtherControl.ClientID))
                                                 {
                                                     
                                                         oImageValue.Add(oOtherControl.ClientID, oDT.Rows[0][ColumnName].ToString());
                                                         HttpContext.Current.Session[ImageList] = oImageValue;
                                                         HttpContext.Current.Session[FormDesign.SessionImageList] = oImageValue;
                                                         ucImageControl oImage = oOtherControl as  ucImageControl ;
                                                         oImage.SetImagePath();
                                                         
                                                 }

                                             }

                                         }
                                       
                                       
                                    }
                                

                            }
                            
                           
                                
                        }
                    
                    }
                }

            }
        
        }
    }


    public class TabHeaderTemplate : ITemplate
    {
        public string HeaderText = string.Empty;
        public string ImagePath = string.Empty;
        private string clientid = string.Empty;
        private string Tabidentify = string.Empty;

        public TabHeaderTemplate(string Text, string Path, string HidClientID, string TabID)
        {
            HeaderText = Text;
            ImagePath = Path;
            clientid = HidClientID;
            Tabidentify = TabID;
        }

        public void InstantiateIn(Control container)
        {

            if (ImagePath != string.Empty)
            { 
                Image oImage = new Image();
                oImage.ImageUrl = ImagePath;
                container.Controls.Add(oImage);
            }

            Label lbl = new Label();
            lbl.Attributes.Add("onclick", "SetHidValue('"+ clientid   + "','"+   Tabidentify + "');");
            lbl.Text = HeaderText;

            // Lastly, add the controls to the container...
            container.Controls.Add(lbl);
        }
    }



}
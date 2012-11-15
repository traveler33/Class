using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using Pelesys.Scheduling;

namespace eForm.Class
{
    public class FileUploadManager : System.Web.UI.UserControl
    {

        public static string ImageUploadFolder = "Images";
        public static string AttachmentUploadFolder = "EmailAttachment";
        public static string TempUploadFolder = "Temp";
        public static String ErrorMsg = string.Empty;
        public static String FullDownPath = string.Empty;

        public static string ErrorMSGForDefaultUploadFolder = "Default upload folder does not exist. Plase ask administrator to create DocLibrary folder.";


        public static string SessionAttachmentDS = "SessionAttachmentDS";
        public static string SessionIDType = "SessionIDType";
        public static string SessionCurrentID = "SessionCurrentID";
        public static string SepcialFileName = string.Empty;
        public static string sNormalFileName = string.Empty;
        public static String sFile = "Document";

        public static Boolean IsAttachedBefore = false;
        private static string substringDirectory;



        //***************************************************
        //Get all files from the server

        #region static function: Get tree view from directory
        public static Int32 GetFileCount(string sFilePath, Page oPage)
        {
            string fileFullPath = oPage.Server.MapPath("~/" + sFilePath);
            string[] fileEntries = Directory.GetFiles(fileFullPath, "*.*");
            return fileEntries.Length;
        }

        public static void PopulateTreeView(string directoryValue, System.Web.UI.WebControls.TreeView oTree, Page oPage, Hashtable oAttachedList)
        {

            string fileFullPath = oPage.Server.MapPath("~/" + directoryValue);
            string[] fileEntries = Directory.GetFiles(fileFullPath, "*.*");

            foreach (string fileName in fileEntries)
            {

                string sDocName = fileName.Substring(
                        fileName.LastIndexOf('\\') + 1);
                if (oAttachedList != null)
                {
                    if (!oAttachedList.ContainsKey(sDocName))
                    {

                        TreeNode myNode = new TreeNode(sDocName, "0", string.Empty, string.Empty, string.Empty);
                        SetTreeViewNode(myNode, sFile, sDocName, fileName);
                        oTree.Nodes.Add(myNode);
                    }
                }
                else
                {
                    TreeNode myNode = new TreeNode(sDocName, "0", string.Empty, string.Empty, string.Empty);
                    SetTreeViewNode(myNode, sFile, sDocName, fileName);
                    oTree.Nodes.Add(myNode);

                }
            }




        }

        public static void SetTreeViewNode(TreeNode oNode, string sFile, string sDocName, string fileName)
        {

            oNode.ToolTip = sFile;
            oNode.Text = sDocName;
            oNode.Value = fileName;
            oNode.Checked = false;
            oNode.ShowCheckBox = true;



        }


        #endregion

        #region static function: Get all files under a folder
        public static void GetAllFileAndAddNode(String sFilePath, TreeNode parentNode, Page oPage, DataTable oDT)
        {
            string fileFullPath = oPage.Server.MapPath("~/" + sFilePath);
            string[] fileEntries = Directory.GetFiles(fileFullPath, "*.*");


            foreach (string fileName in fileEntries)
            {
                string sDocName = fileName.Substring(
                        fileName.LastIndexOf('\\') + 1);


                TreeNode myNode = new TreeNode(sDocName, "0", string.Empty, string.Empty, string.Empty);
                myNode.ToolTip = sFile;
                //   myNode.Tag = fileName.ToString() + "-" + sFile;
                //  myNode. = "DocTypeInDocTree";
                parentNode.ChildNodes.Add(myNode);
                // }

            }





        }
        #endregion



















        //**************************************************



        public static string UploadThisFile(System.Web.UI.WebControls.FileUpload Fupload, bool IsOverwriteFile, Page oPage,
                    bool IsSpecialFileName, String FileUploadFolder)
        {



            if (Fupload.HasFile)
            {
                string fullPath = oPage.Server.MapPath("~/" + FileUploadFolder);
                if ( !System.IO.Directory.Exists ( fullPath ))
                {
                    System.IO.Directory.CreateDirectory(fullPath);
                }
                string theFileName = Path.Combine(oPage.Server.MapPath("~/" + FileUploadFolder), Fupload.FileName);
                if (IsSpecialFileName == true)
                {

                    sNormalFileName = theFileName;
                    theFileName = GetASpecialFileName(theFileName);
                    SepcialFileName = theFileName;

                }
                if (IsOverwriteFile)
                {
                    if (File.Exists(theFileName))
                    {
                        File.Delete(theFileName);
                    }
                }
                Fupload.SaveAs(theFileName);
                return theFileName;
            }
            return string.Empty;

        }


        public static string GetASpecialFileName(String FileName)
        {
            String OldName = string.Empty;
            String FileExt = string.Empty;
            String WholeFileName = string.Empty;
            int ExtPos = FileName.LastIndexOf(".");
            if (ExtPos > 0)
            {
                FileExt = FileName.Substring(ExtPos);
                FileExt = FileExt.Replace(".", string.Empty);

            }

            ExtPos = FileName.LastIndexOf("\\");
            if (ExtPos > 0)
            {
                WholeFileName = FileName.Substring(ExtPos);
                OldName = WholeFileName;
                WholeFileName = WholeFileName.Replace("." + FileExt, string.Empty);
                WholeFileName = WholeFileName + DateTime.Now.ToString().Replace("/", string.Empty);
                WholeFileName = WholeFileName.Replace(":", string.Empty);
                WholeFileName = WholeFileName.Replace(" ", string.Empty);
            }


            FileName = FileName.Replace(OldName, WholeFileName);
            return FileName;
        }

        public static void DeleteExistsFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }

        public static String GetServerPathByLocalFilePath(String FilePath, Page oPage, String FileUploadFolder)
        {
            return Path.Combine(oPage.Server.MapPath("~/" + FileUploadFolder), FilePath);
        }

        public static void CopyFileFromTempFolder(String OldFilePath, String NewFileFolder)
        {
            if (File.Exists(OldFilePath))
            {
                if (File.Exists(NewFileFolder))
                {
                    File.Delete(NewFileFolder);
                }
                File.Copy(OldFilePath, NewFileFolder);

            }

        }

        public static void RenameFileName(String NewFileName, String OldFileName)
        {

            if (File.Exists(OldFileName))
            {
                File.Copy(OldFileName, NewFileName);

            }

        }


        #region static function: Get file full path
        public static string GetFileFullPath(String FilePath, String UploadFolder)
        {
            int FilePathPos = FilePath.LastIndexOf(UploadFolder);
            if (FilePathPos > 0)
            {
                FilePath = FilePath.Substring(FilePathPos);


            }

            return FilePath;
        }
        #endregion

        #region static function: Get File extension

        public static String GetFileNameFromFileFullPath(String FullFilePath)
        {
            int ExtPos = FullFilePath.LastIndexOf("\\");
            if (ExtPos > 0)
            {
                String FileExt = FullFilePath.Substring(ExtPos);
                FileExt = FileExt.Replace("\\", string.Empty);
                return FileExt;
            }
            return string.Empty;

        }

        public static String GetFileExtension(String FileName)
        {
            int ExtPos = FileName.LastIndexOf(".");
            if (ExtPos > 0)
            {
                String FileExt = FileName.Substring(ExtPos);
                FileExt = FileExt.Replace(".", string.Empty);
                return FileExt;
            }
            return string.Empty;
        }

        #endregion

        #region static function: Check file extension

        public static bool CheckFileSize(Int64 FileSize)
        {

            String MaxFize =   Option.GetOptionByKey("SysUploadFileMaxFileSizeKey");;
            if (FileSize > Convert.ToInt64(MaxFize))
            {
                return false;
            }
            return true;
        }


        public static bool IsRightFileExtensionToUpload(String filePath, String IsRightType)
        {
            ArrayList FileExtension = new ArrayList();

            List<Setting> oList =  Setting.GetDicByCategory(IsRightType);
            foreach (Setting oItem in oList)
            {
                FileExtension.Add(oItem.Name);
            }
            if (oList.Count == 0)
            { 
                
                
            }



            String FileExt = GetFileExtension(filePath);

            if (FileExt != string.Empty)
            {
                for (int i = 0; i <= FileExtension.Count - 1; i++)
                {
                    if (FileExt.ToLower() == FileExtension[i].ToString().ToLower())
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        #endregion

        #region static function: Get file in all sub directory
        public static void GetThisFileFromSubDirectory(String FileName, String directoryValue, Page oPage)
        {


            string fileFullPath = directoryValue + "\\" + FileName;
            if (IsThisExistInThisDirectory(fileFullPath))
            {
                FullDownPath = fileFullPath;
                return;
            }



            string[] directoryArray =
             Directory.GetDirectories(directoryValue);
            try
            {
                if (directoryArray.Length != 0)
                {
                    foreach (string directory in directoryArray)
                    {

                        fileFullPath = directory + "\\" + FileName;
                        if (IsThisExistInThisDirectory(fileFullPath))
                        {
                            FullDownPath = fileFullPath;
                            return;

                        }
                        GetThisFileFromSubDirectory(FileName, directory, oPage);

                    }
                }


            }
            catch (UnauthorizedAccessException)
            {

            } // end ca    






        }

        public static Boolean IsThisExistInThisDirectory(String fileFullPath)
        {
            FileInfo file = new FileInfo(fileFullPath);

            // Checking if file exists
            if (file.Exists)
            { return true; }
            return false;

        }

        #endregion





        #region static function: Download file
        public static void DownFile(string sFilePath, string sFileName, Page oPage, String UploadFilePath)
        {
            FullDownPath = oPage.Server.MapPath("~/" + UploadFilePath); ;
            GetThisFileFromSubDirectory(sFileName, FullDownPath, oPage);

            string fileFullPath = FullDownPath;
            // if (fileFullPath == string.Empty)
            // { fileFullPath = FullDownPath; }
            FileInfo file = new FileInfo(fileFullPath);

            // Checking if file exists
            if (file.Exists)
            {
                // Clear the content of the response
                oPage.Response.ClearContent();

                // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                oPage.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                // Add the file size into the response header
                oPage.Response.AddHeader("Content-Length", file.Length.ToString());

                // Set the ContentType
                oPage.Response.ContentType = ReturnExtension(file.Extension.ToLower());

                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                oPage.Response.TransmitFile(file.FullName);

                // End the response
                oPage.Response.End();



            }
            else
            {
                //show error message 
                ErrorMsg = "The file can not be found. It may be deleted or renamed or moved to the other folder.";


            }
        }


        public static string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }

        }
        #endregion


        static public Boolean IsThisFileExist(String FilePath, Page oPage)
        {


            if (File.Exists(oPage.Server.MapPath(FilePath)))
            {
                return true;
            }

            return false;
        }





    }


    public class ImageList
    {
        public string ImagePath = string.Empty;
        public string ImageClientID = string.Empty;
    
    }

}
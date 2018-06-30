using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BEB_csharp05
{
    class Testftp
    {
        void Useftp()
        {
            /* Create Object Instance */
            Ftp ftpClient = new Ftp(@"ftp://10.10.10.10/", "user", "password");

            /* Upload a File */
            ftpClient.Upload("etc/test.txt", @"C:\Users\metastruct\Desktop\test.txt");

            /* Download a File */
            ftpClient.Download("etc/test.txt", @"C:\Users\metastruct\Desktop\test.txt");

            /* Delete a File */
            ftpClient.Delete("etc/test.txt");

            /* Rename a File */
            ftpClient.Rename("etc/test.txt", "test2.txt");

            /* Create a New Directory */
            ftpClient.CreateDirectory("etc/test");

            /* Get the Date/Time a File was Created */
            string fileDateTime = ftpClient.GetFileCreatedDateTime("etc/test.txt");
            //Console.WriteLine(fileDateTime);

            /* Get the Size of a File */
            string fileSize = ftpClient.GetFileSize("etc/test.txt");
            //Console.WriteLine(fileSize);

            /* Get Contents of a Directory (Names Only) */
            string[] simpleDirectoryListing = ftpClient.DirectoryListSimple("/etc");
            for (int i = 0; i < simpleDirectoryListing.Count(); i++) { Console.WriteLine(simpleDirectoryListing[i]); }

            /* Get Contents of a Directory with Detailed File/Directory Info */
            string[] detailDirectoryListing = ftpClient.DirectoryListDetailed("/etc");
            for (int i = 0; i < detailDirectoryListing.Count(); i++) { Console.WriteLine(detailDirectoryListing[i]); }

            /* Release Resources */
            ftpClient = null;
        }
    }


    class Ftp
    {
        private readonly string host = null;
        private readonly string user = null;
        private readonly string pass = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        private readonly int bufferSize = 2048;

        /* Construct Object */
        public Ftp(string hostIP, string userName, string password) { host = hostIP; user = userName; pass = password; }

        /* Download File */
        public void Download(string remoteFile, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                ftpStream = ftpResponse.GetResponseStream();
                /* Open a File Stream to Write the Downloaded File */
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { IO.Log(2018061802, "ftp, download  | " + ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { IO.Log(2018061803, "ftp, download  | " + ex.ToString()); }
            return;
        }

        /* Upload File */
        public void Upload(string remoteFile, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Delete File */
        public void Delete(string deleteFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Rename File */
        public void Rename(string currentFileNameAndPath, string newFileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + currentFileNameAndPath);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                /* Rename the File */
                ftpRequest.RenameTo = newFileName;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Create a New Directory on the FTP Server */
        public void CreateDirectory(string newDirectory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + newDirectory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Get the Date/Time a File was Created */
        public string GetFileCreatedDateTime(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { fileInfo = ftpReader.ReadToEnd(); }
                catch (Exception ex) { IO.Log(2018061804, "ftp, getFileCreatedDateTime  | " + ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Created Date Time */
                return fileInfo;
            }
            catch (Exception ex) { IO.Log(2018061805, "ftp, getFileCreatedDateTime  | " + ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* Get the Size of a File */
        public string GetFileSize(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
                catch (Exception ex) { IO.Log(2018061806, "ftp, getFileSize  | "+ ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Size */
                return fileInfo;
            }
            catch (Exception ex) { IO.Log(2018061807, "ftp, getFileSize  | "+ ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* List Directory Contents File/Folder Name Only */
        public string[] DirectoryListSimple(string directory)
        {
            try
            {
                /* Create an FTP Request */
                //ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                IO.Log(2018062803, "Liste FTP-Ordner " + Path.Combine(host, directory) + @"/");
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Path.Combine(host,directory) + @"/");
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { IO.Log(2018061808, "ftp, directoryListSimple  | " + ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { IO.Log(2018061809, "ftp, directoryListSimple  | " + ex.ToString()); }
            }
            catch (Exception ex) { IO.Log(2018061810, "ftp, directoryListSimple  | " + ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }

        /* List Directory Contents in Detail (Name, Size, Created, etc.) */
        public string[] DirectoryListDetailed(string directory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(Path.Combine(host, directory) + @"/");
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { IO.Log(2018061811, "ftp, directoryListDetailed  | " + ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { IO.Log(2018061812, "ftp, directoryListDetailed | " + ex.ToString()); }
            }
            catch (Exception ex) { IO.Log(2018061813, "ftp, directoryListSimple | " + ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }
      
        /// <summary>
        /// Prüft ein FTP-Verzeichnis und gibt Grün wieder, wenn vorhadnen, sonst rot.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static System.Windows.Media.Brush DoesFtpDirectoryExist(string dirPath)
        {
            try
            {  
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
                request.Credentials = new NetworkCredential(Var.FtpUsername1, Var.FtpPassword1);
                request.Timeout = 4000;
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
 
                return Brushes.Green;
            }
            catch (WebException ex)
            {
                IO.Log(2018061814,  "DoesFtpDirectoryExist() | '" + dirPath + "' existiert nicht. | " + ex.Message);
                return Brushes.Red;
            }
            catch (Exception ex)
            {
                IO.Log(2018062003, "DoesFtpDirectoryExist() | '" + dirPath + "' existiert nicht. | " + ex.GetType() + " | " + ex.Message);
                return Brushes.Red;
            }
        }

        //Calling the method:
        //string ftpDirectory = "ftp://ftpserver.com/rootdir/test_if_exist_directory/"; //Note: backslash at the last position of the path.
        //bool dirExists = DoesFtpDirectoryExist(ftpDirectory);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // von http://csharp-blog.de/2015/11/c-asyncawait-fortschritts-benachrichtigung-und-abbruch-moeglichkeit/

        public static async Task DownloadFileAsync(Uri ftpSite, string targetPath, IProgress<int> onProgressChanged, CancellationToken token)
        {
            try
            {
                //File.Create(Path.Combine(targetPath));

                int totalBytes = 0;
                long fileSize = 0;

                FtpWebRequest requestS = (FtpWebRequest)WebRequest.Create(ftpSite);
                requestS.Credentials = new NetworkCredential(Var.FtpUsername1, Var.FtpPassword1);
                requestS.Method = WebRequestMethods.Ftp.GetFileSize;

                using (FtpWebResponse response = (FtpWebResponse)await requestS.GetResponseAsync())
                {
                    fileSize = response.ContentLength;
                    response.Close();
                }

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpSite);
                request.Credentials = new NetworkCredential(Var.FtpUsername1, Var.FtpPassword1);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                   
                    Stream data = response.GetResponseStream();

                    //Console.WriteLine($"Downloading {ftpSite.AbsoluteUri} to {targetPath}...");
                    IO.Log(2018061904, $"Starte Download von {ftpSite.AbsoluteUri} nach {targetPath}");

                    byte[] byteBuffer = new byte[4096];
                    using (FileStream output = new FileStream(targetPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 4096, useAsync: true))
                    {
                        int bytesRead = 1;
                        while (bytesRead > 0)
                        {
                            bytesRead = await data.ReadAsync(byteBuffer, 0, byteBuffer.Length);
                            if (bytesRead > 0)
                            {
                                totalBytes += bytesRead;

                                // Report progress
                                int locProgress = Convert.ToInt32(totalBytes * 100 / fileSize);
                                onProgressChanged?.Report(locProgress);
                                // Upload
                                await output.WriteAsync(byteBuffer, 0, bytesRead, token);     
                            }
                        } 
                    }

                    //Console.WriteLine($"Downloaded {ftpSite.AbsoluteUri} to {targetPath}");
                    IO.Log(2018061905, $"Download von {ftpSite.AbsoluteUri} nach {targetPath} war erfolgreich.");
                }
            }
            catch(OperationCanceledException)
            {
                //Console.Write("Download cancelled!");
                IO.Log(2018061906, "Download abgebrochen.");
            }
            catch (WebException e)
            {
                //Console.WriteLine($"Failed to download {ftpSite.AbsoluteUri} to {targetPath}");
                //Console.WriteLine(e);
                IO.Log(2018061907, $"Download fehlgeschlagen: {ftpSite.AbsoluteUri} nach {targetPath} : " + e.Message);
                throw;
            }
        }


        public static async void LoadFtpFiles(IEnumerable<string> filesToLoad, string ftpDirSource, string locDirTarget, IProgress<int> progress1, IProgress<int> progress2, IProgress<int> progressIndex)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            int i = 0;
            int maxi = filesToLoad.Count();

            foreach (string fileName in filesToLoad)
            {
                //Fortschrittsanzeige
                if (progress1 != null) progress1.Report(++i * 100 / maxi);
                if (progressIndex != null) progressIndex.Report(i);

                //Wenn fileName leer, dann Schleife überspringen.
                if (fileName.Length == 0) continue;

                Thread.Sleep(Var.SleepingMilliSec); // CPU-bound work

                // Start asyn operation
                Uri ftpSite = new Uri(Path.Combine(ftpDirSource, fileName));
                string locFileTarget = Path.Combine(locDirTarget, fileName);
                try
                {
                    await Ftp.DownloadFileAsync(ftpSite, locFileTarget, progress2, cts.Token);
                }
                catch (Exception ex)
                {
                    IO.Log(2018062005, ftpSite + " => " + locFileTarget + " | " + ex.Message);
                }


            }
        }


    }





}
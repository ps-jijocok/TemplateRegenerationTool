using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using WinSCP;
using System.Reflection;
//using System.Threading;
namespace TemplateRegenerationTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
           /* folderBrowserDialog1.ShowDialog();
            string foldername = folderBrowserDialog1.SelectedPath;

            string rootDirectory = foldername;
           
           var foundFiles = Directory.EnumerateFiles(rootDirectory, "widget.json", SearchOption.AllDirectories);

            List<String> directoryNames = new List<String>();
            foreach (var file in foundFiles)
            {
                //Console.WriteLine(System.IO.Path.GetDirectoryName(file));
                directoryNames.Add(System.IO.Path.GetDirectoryName(file));
            }
              foreach (string drct in directoryNames)
                {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                // Code new start
                string strToProcess = drct.ToString();
                string newStr = strToProcess.Substring(0, 2);
                //Code new end
                string strCmnd = "cd " + drct.ToString();
                process.StandardInput.WriteLine(newStr);
                process.StandardInput.WriteLine(strCmnd);
                process.StandardInput.WriteLine("yo widget");
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();
                string result = process.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }
            */
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void FolderBrowserDialog2_HelpRequest(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //Code to browse the template path
            folderBrowserDialog2.ShowDialog();
            string foldername = folderBrowserDialog2.SelectedPath;

            string rootDirectory = foldername;

            var foundFiles = Directory.EnumerateFiles(rootDirectory, "widget.json", SearchOption.AllDirectories);
            listBox1.Items.Clear();
            List<String> directoryNames = new List<String>();
            foreach (var file in foundFiles)
            {
                //Console.WriteLine(System.IO.Path.GetDirectoryName(file));
                directoryNames.Add(System.IO.Path.GetDirectoryName(file));
            }

            //Code to regenerate template
            foreach (string drct in directoryNames)
            {
                label1.Text = "Regenerating templates in progress...";
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                // Code new start
                string strToProcess = drct.ToString();
                string newStr = strToProcess.Substring(0, 2);
                //Code new end
                string strCmnd = "cd " + drct.ToString();
                process.StandardInput.WriteLine(newStr);
                process.StandardInput.WriteLine(strCmnd);
                process.StandardInput.WriteLine("yo widget --force");
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();
                string result = process.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }
            label1.Text = "Upload in Progress...";
            //Code to upload start
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = "demo.pricespider.com",
                    UserName = "ubuntu",
                    Password = "mypassword",
                    SshHostKeyFingerprint = "ssh-ed25519 256 b3:e0:71:85:65:5e:59:b2:90:e4:23:ac:e6:53:49:2c"
                };

                string path = Directory.GetCurrentDirectory();
                path = path + "\\demo_jijo.ppk";
                sessionOptions.SshPrivateKeyPath = path;

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                   // transferOptions.OverwriteMode = OverwriteMode.Overwrite;
                    TransferOperationResult transferResult;
                    foreach (string drct in directoryNames)
                    {
                        //Upload Index.html file
                        string sourcePath = drct.ToString() + "\\" + "index.html";
                        string destpath = drct.ToString();
                        destpath = destpath.Replace("\\","/");
                        string toBeSearched = "DemoTest";
                        string destpathremain = destpath.Substring(destpath.IndexOf(toBeSearched) + toBeSearched.Length);
                        string newDestPath = "/var/www/html/wtb/Demos" + destpathremain;
                        newDestPath = newDestPath + "/";
                        transferResult =
                            session.PutFiles(sourcePath, newDestPath, false, transferOptions);
                           
                        // Throw on any error
                        transferResult.Check();

                        // Print results
                        /*
                        foreach (TransferEventArgs transfer in transferResult.Transfers)
                        {
                            Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                        }
                        */
                        sourcePath = drct.ToString() + "\\" + "src\\*";
                        newDestPath = newDestPath + "src" + "/";
                        transferResult =
                            session.PutFiles(sourcePath, newDestPath, false, transferOptions);

                        // Throw on any error
                         transferResult.Check();

                        // Print results
                       /* foreach (TransferEventArgs transfer in transferResult.Transfers)
                        {
                            Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                        }*/
                    }
                }

            }
            catch (Exception ce)
            {
                Console.WriteLine("Error: {0}", ce);
            }
            //Code to upload end
            foreach (string drct in directoryNames)
            {
                listBox1.Items.Add(drct.ToString());
            }
                label1.Text = "Upload Completed...";
        }
    }
}


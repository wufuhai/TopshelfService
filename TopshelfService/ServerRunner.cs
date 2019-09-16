using System;
using System.Diagnostics;
using System.IO;

namespace TopshelfService
{
    public class ServerRunner
    {
        private readonly string _fileName;
        private readonly string _args;
        private Process _process;

        public ServerRunner(string fileName, string args)
        {
            _fileName = fileName;
            _args = args;
        }
        private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        public void Start()
        {
            this.Stop();
            string path = _fileName;
            if (!File.Exists(path))
            {
                Console.WriteLine(path + " does not exist!");
            }
            else
            {
                Process process = new Process
                {
                    StartInfo =
                    {
                        FileName = path, UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        Arguments = _args
                    },
                    EnableRaisingEvents = true
                };
                process.StartInfo.CreateNoWindow = true;
                process.ErrorDataReceived += Proc_ErrorDataReceived;
                process.OutputDataReceived += Proc_OutputDataReceived;
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit();
                this._process = process;
            }

        }

        public void Stop()
        {
            if (this._process != null)
            {
                this._process.Close();
                this._process.Dispose();
                this._process = null;
            }
        }
    }
}


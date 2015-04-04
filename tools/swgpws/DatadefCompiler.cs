using System;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace SWGPacket_Workshop
{
    class DatadefCompiler
    {
        protected System.Reflection.Assembly _assembly;
        protected System.Threading.Mutex _mutex = new System.Threading.Mutex();
        protected System.IO.FileSystemWatcher _fsw;
        protected System.Windows.Forms.TextBox _output;
        protected Boolean _update = false;

        public DatadefCompiler(System.Windows.Forms.TextBox outputWindow)
        {
            _output = outputWindow;
        }
        public String Compile()
        {
            String output = "";
            _mutex.WaitOne();

            try
            {
                String[] scriptFiles = System.IO.Directory.GetFiles("datadefs", "*.cs", System.IO.SearchOption.AllDirectories);

                Dictionary<String, String> provOpt = new Dictionary<String, String>();
                provOpt.Add("CompilerVersion", "v3.5");

                CSharpCodeProvider provider = new CSharpCodeProvider(provOpt);

                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;
                parameters.ReferencedAssemblies.Add("system.core.dll");
                parameters.ReferencedAssemblies.Add(System.Reflection.Assembly.GetExecutingAssembly().Location);

                CompilerResults results = provider.CompileAssemblyFromFile(parameters, scriptFiles);

                if (results.Errors.HasErrors)
                    for (Int32 i = 2; i < results.Output.Count; output += results.Output[i++] + "\r\n") ;
                else
                {
                    if (results.Errors.HasWarnings)
                        for (Int32 i = 2; i < results.Output.Count; output += results.Output[i++] + "\r\n") ;

                    _assembly = results.CompiledAssembly;

                    if (_fsw == null)
                    {
                        _fsw = new System.IO.FileSystemWatcher("datadefs", "*.cs");
                        _fsw.Changed += new System.IO.FileSystemEventHandler(OnChanged);
                        _fsw.Created += new System.IO.FileSystemEventHandler(OnChanged);
                        _fsw.Deleted += new System.IO.FileSystemEventHandler(OnChanged);
                        _fsw.NotifyFilter = System.IO.NotifyFilters.LastWrite;
                        _fsw.EnableRaisingEvents = true;
                    }

                    if (output == "") output = String.Format("Successfully compiled assembly {0}\r\n\r\n", _assembly);
                }
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                output = ex.Message + "\r\n\r\n";
            }
            catch (System.InvalidOperationException ex)
            {
                output = ex.Message + "\r\n\r\n";
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

            _output.Invoke((System.Windows.Forms.MethodInvoker)delegate { _output.Text += output; });

            return output;
        }
        /// <summary>
        /// Returns the proper SOE message object
        /// </summary>
        /// <param name="OpCode">OpCode of the message</param>
        /// <param name="data">Full UDP segment, or for Frag types a reassembled message</param>
        /// <returns>Object representing SOE message</returns>
        public Object soeFactory(Byte[] data)
        {
            if (_assembly == null) return null;

            _mutex.WaitOne();

            Object obj = null;

            try
            {
                UInt16 OpCode = BitConverter.ToUInt16(data, 0);
                foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
                    if (type.Namespace == "PacketData.SOE" && type.GetField("OpCode") != null && (UInt16)type.GetField("OpCode").GetValue(null) == OpCode)
                        obj = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(
                            type.FullName, false, System.Reflection.BindingFlags.CreateInstance, null, new Object[] { data }, null, new object[] { });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _mutex.ReleaseMutex();

            return obj;
        }
        /// <summary>
        /// Returns a proper SWG message object
        /// </summary>
        /// <param name="OpCode">OpCode of the message</param>
        /// <param name="data">SWG packet data immediately following the OpCode</param>
        /// <returns>Object representing SWG message</returns>
        public Object swgFactory(Byte[] data)
        {
            if (_assembly == null) return null;

            _mutex.WaitOne();

            Object obj = null;

            try
            {
                if (data.Length < 4) return null;
                UInt32 OpCode = BitConverter.ToUInt32(data, 0);
                foreach (Type type in _assembly.GetTypes())
                    if (type.Namespace == "PacketData.SOE.SWG" && type.GetField("OpCode") != null && (UInt32)(type.GetField("OpCode").GetValue(null)) == OpCode)
                        obj = _assembly.CreateInstance(
                            type.FullName, false, System.Reflection.BindingFlags.CreateInstance, null, new Object[] { data }, null, new object[] { });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _mutex.ReleaseMutex();

            return obj;
        }

        protected void OnChanged(Object source, System.IO.FileSystemEventArgs e)
        {
            _fsw.EnableRaisingEvents = false;
            System.Threading.Thread.Sleep(250);
            Compile();
            _fsw.EnableRaisingEvents = true;
        }
    }
}
using System;
using System.Linq;

namespace SWGPacket_Workshop
{
    class CRCLibraryMgr
    {
        protected String _objectLibrary = "crc_objects";
        protected String _commandLibrary = "crc_commands";
        protected System.Collections.Generic.Dictionary<String, String> _objectDict = new System.Collections.Generic.Dictionary<string, string>();
        protected System.Collections.Generic.Dictionary<String, String> _commandDict = new System.Collections.Generic.Dictionary<string, string>();
        public CRCLibraryMgr()
        {
            loadLibraries();
        }

        public String GetObjectName(String objectCRC)
        {
            try
            {
                return _objectDict[objectCRC.ToUpper()];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public String GetCommandName(String commandCRC)
        {
            try
            {
                return _commandDict[commandCRC];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private Boolean loadLibraries()
        {
            try
            {
                String[] libraries = new String[] { _objectLibrary, _commandLibrary };
                foreach (String library in libraries)
                    if (System.IO.File.Exists(library + ".txt"))
                        pakLibrary(library);
                unpakLibrary(_objectLibrary, _objectDict);
                unpakLibrary(_commandLibrary, _commandDict);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private Boolean pakLibrary(String file)
        {
            try
            {
                if (System.IO.File.Exists(file + ".txt.bak")) System.IO.File.Delete(file + ".txt.bak");
                Byte[] uncompressed = System.IO.File.ReadAllBytes(file + ".txt");
                using (System.IO.MemoryStream compressed = new System.IO.MemoryStream())
                {
                    using (System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(compressed, System.IO.Compression.CompressionMode.Compress))
                        zip.Write(uncompressed, 0, uncompressed.Length);
                    System.IO.File.WriteAllBytes(file + ".pak", compressed.ToArray());
                }
                System.IO.File.Delete(file + ".txt");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private Boolean unpakLibrary(String file, System.Collections.Generic.Dictionary<String, String> dictionary)
        {
            try
            {
                using (System.IO.FileStream compressed = new System.IO.FileStream(file + ".pak", System.IO.FileMode.Open))
                {
                    using (System.IO.MemoryStream uncompressed = new System.IO.MemoryStream())
                    {
                        using (System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(compressed, System.IO.Compression.CompressionMode.Decompress))
                        {
                            Int32 b;
                            Byte[] buffer = new Byte[4096];
                            while ((b = zip.Read(buffer, 0, buffer.Length)) > 0)
                                uncompressed.Write(buffer, 0, b);
                        }
                        uncompressed.Position = 0;
                        using (System.IO.StringReader sr = new System.IO.StringReader(System.Text.Encoding.ASCII.GetString(uncompressed.ToArray())))
                        {
                            String line = "";
                            while ((line = sr.ReadLine()) != null && line.IndexOf('\t') != -1)
                            {
                                if (dictionary.ContainsKey(line.Split('\t')[1]))
                                {
                                    if (dictionary[line.Split('\t')[1]] != line.Split('\t')[0]) 
                                        Console.WriteLine("CRC {0} item {1} already exists for item {2}", line.Split('\t')[1], line.Split('\t')[0], dictionary[line.Split('\t')[1]]);
                                }
                                else
                                    dictionary.Add(line.Split('\t')[1], line.Split('\t')[0]);
                            }
                        }
                    }
                    System.Collections.Generic.KeyValuePair<String, String> temp = dictionary.First<System.Collections.Generic.KeyValuePair<String, String>>(x => x.Value == "LoginConnectionFailed");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}

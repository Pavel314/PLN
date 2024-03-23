using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PLNCompiler.Compile.PLNReflection.AssemblyLoader
{

    public class GACRunner
    {
        public Func<string> ReadNext { get; private set; }
        public Func<bool> IsEnd { get; private set; }
        public Action OnEnd { get; private set; }

        public GACRunner(Func<string> readNext, Func<bool> isEnd,Action onEnd)
        {
            ReadNext = readNext;
            IsEnd = isEnd;
            OnEnd = onEnd;
        }
        public static GACRunner CreateGACUtil(string gacUtilPath)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = gacUtilPath,
                    Arguments = "-nologo -l",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            //try { proc.Kill(); } catch { } 
            return new GACRunner(() => proc.StandardOutput.ReadLine(), () => proc.StandardOutput.EndOfStream,null);
        }

#if DEBUG
        public static GACRunner CreateGACUtil()
        {
            return CreateGACUtil(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\gacutil.exe");
        }
#endif


        }


    public static  class GACGenerator
    {
        public const string FILE_NAME = "libs.xml";
        public const string PAIR = "Pair";
        public const string SHORT_NAME = "name";
        public const string LONG_NAME = "longName";
        public const string ROOT_NAME = "Assemblies";

        public static readonly StringComparer FileSystemComparer = StringComparer.OrdinalIgnoreCase;

        public static void GenerateGACDocument(GACRunner runner)
        {
            var dict = new SortedDictionary<string, AssemblyName>(StringComparer.OrdinalIgnoreCase);
            var AssemblyDomain = AppDomain.CreateDomain("AssemblyTestDomian");
            while (!runner.IsEnd())
            {
                var longName = runner.ReadNext();
             
                AssemblyName assemblyName = null;

                try
                {
                    assemblyName = new AssemblyName(longName);
                    AssemblyDomain.Load(assemblyName);
                }
                catch (Exception)
                {
                    assemblyName = null;
                }
              
                if (assemblyName != null)
                {
                    AssemblyName outname;
                    if (dict.TryGetValue(assemblyName.Name, out outname))
                    {
                        if (assemblyName.Version > outname.Version)
                        {
                            dict[assemblyName.Name] = assemblyName;
                        }
                    }
                    else
                        dict.Add(assemblyName.Name, assemblyName);
                }
            }
            AppDomain.Unload(AssemblyDomain);
            runner.OnEnd?.Invoke();

            XmlSerializer serializer = new XmlSerializer(typeof(Pair[]), new XmlRootAttribute() { ElementName = ROOT_NAME });

            using (var stream = new FileStream(FILE_NAME, FileMode.Create))
            {
                var assemblyNames = new Pair[dict.Count];
                int i = 0;
                foreach (var item in dict)
                {
                    assemblyNames[i++] = new Pair(item.Key, item.Value.ToString());
                }
                serializer.Serialize(stream, assemblyNames);
            }
        }


        public class Pair
        {
            [XmlAttribute]
            public string name;
            [XmlAttribute]
            public string longName;

            public Pair(string name_, string longName_)
            {
                name = name_;
                longName = longName_;
            }

            private Pair() { }

        }
    }
}

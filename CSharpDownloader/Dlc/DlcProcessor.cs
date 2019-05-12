using CSharpDownloader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDownloader.Dlc
{
    public static class DlcProcessor
    {
        public enum Mode : int
        {
            RSDF,
            CCF,
            DLC,
            Check
        }

        public const string ENDPOINT = "https://cable.ayra.ch/decrypt/decrypt.php?mode={0}&name={1}";

        public async static Task<DlcDecryptResult> Handle(string fullPath)
        {
            var downloadLinks = new List<string>();

            var result = await Decrypt(File.ReadAllBytes(fullPath), Path.GetFileNameWithoutExtension(fullPath), ModeFromFileName(fullPath));

            return result;
        }

        public static Mode ModeFromFileName(string NameOrExtension)
        {
            string ext;
            switch (ext = NameOrExtension.Split('.').Last().ToLower())
            {
                case "rsdf":
                    return Mode.RSDF;
                case "ccf":
                    return Mode.CCF;
                case "dlc":
                    return Mode.DLC;
                default:
                    throw new Exception($"Can't convert {ext} to a supported value");
            }
        }

        private static HttpWebRequest GetRequest(string URL)
        {
            var Req = WebRequest.CreateHttp(URL);
            Req.UserAgent = "AyrA-Decryptor/" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " +https://github.com/AyrA/Decrypter";
            return Req;
        }

        public static async Task<DlcDecryptResult> Decrypt(byte[] Content, string Name, Mode FileType)
        {
            if (FileType == Mode.Check)
            {
                throw new ArgumentException("FileType can't be 'Check'");
            }
            var Req = GetRequest(string.Format(ENDPOINT, FileType.ToString().ToLower(), Name));

            Req.Method = "POST";

            using (var S = await Req.GetRequestStreamAsync())
            {
                S.Write(Content, 0, Content.Length);
            }
            using (var Res = await Req.GetResponseAsync())
            {
                var Response = "";
                try
                {
                    using (var S = Res.GetResponseStream())
                    {
                        using (var SR = new StreamReader(S))
                        {
                            return JsonConvert.DeserializeObject<DlcDecryptResult>(Response = await SR.ReadToEndAsync());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public class IO
    {
        public static void Initialize(Runtime runtime)
        {
            runtime.RegisterCommand("files_list", (func, runtime) => {
                var files = Directory.GetFiles(Directory.GetCurrentDirectory());

                runtime.PushValue(files.Select(x => new SmolString(x)).ToArray());
            });
            runtime.RegisterCommand("directories_list", (func, runtime) =>
            {
                var files = Directory.GetDirectories(Directory.GetCurrentDirectory());

                runtime.PushValue(files.Select(x => new SmolString(x)).ToArray());
            });

            
            runtime.RegisterCommand("file_read", (func, runtime) => {
                var value = runtime.Pop().AsString();

                runtime.PushValue(File.ReadAllText(value));
            });
            runtime.RegisterCommand("file_write", (func, runtime) => {
                var value = runtime.Pop().AsString();
                var path = runtime.Pop().AsString();

                File.WriteAllText(path, value);
            });


            runtime.RegisterCommand("http_get", (func, runtime) => {
                var url = runtime.Pop().AsString();

                using (var client = new WebClient())
                {
                    var result = client.DownloadString(url);

                    runtime.PushValue(result);
                }
            });
        }
    }
}

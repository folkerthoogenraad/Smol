using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Smol.Values;

namespace Smol.Modules
{
    public class IO
    {
        public static void Initialize(SmolContext context)
        {
            context.RegisterCommand("files_list", (func, context) => {
                var files = Directory.GetFiles(Directory.GetCurrentDirectory());

                context.PushValue(files.Select(x => new SmolString(x)).ToArray());
            });
            context.RegisterCommand("directories_list", (func, context) =>
            {
                var files = Directory.GetDirectories(Directory.GetCurrentDirectory());

                context.PushValue(files.Select(x => new SmolString(x)).ToArray());
            });


            context.RegisterCommand("file_name", (func, context) => {
                var value = context.Pop().AsString();

                context.PushValue(Path.GetFileName(value));
            });

            context.RegisterCommand("file_read", (func, context) => {
                var value = context.Pop().AsString();

                context.PushValue(File.ReadAllText(value));
            });
            context.RegisterCommand("file_write", (func, context) => {
                var value = context.Pop().AsString();
                var path = context.Pop().AsString();

                File.WriteAllText(path, value);
            });
            context.RegisterCommand("file_delete", (func, context) => {
                var path = context.Pop().AsString();

                File.Delete(path);
            });


            context.RegisterCommand("http_get", (func, context) => {
                var url = context.Pop().AsString();

                using (var client = new WebClient())
                {
                    var result = client.DownloadString(url);

                    context.PushValue(result);
                }
            });
        }
    }
}

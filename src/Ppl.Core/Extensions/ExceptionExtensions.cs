using System;
using System.Text;

namespace Ppl.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetFullMessage(this Exception ex)
        {
            var strb = new StringBuilder();
            strb.AppendLine("=======================================================");
            strb.AppendLine("=Exception has occurred !                             =");
            strb.AppendLine("=======================================================");
            strb.AppendLine("= Last Exception is                                   =");
            strb.AppendLine($"{ex.Message}\r\n{ex.StackTrace}");
            LogInnerException(ex, strb);
            strb.AppendLine("=======================================================");
            return ex.ToString();
        }

        private static void LogInnerException(Exception e, StringBuilder strb)
        {
            if (e.InnerException != null)
            {
                strb.AppendLine("=======================================================");
                strb.AppendLine("= Next Exception is                                   =");
                var ex = e.InnerException;
                strb.AppendLine($"\r\n{ex.Message}\r\n{ex.StackTrace}");
                LogInnerException(ex, strb);
            }
        }
    }
}
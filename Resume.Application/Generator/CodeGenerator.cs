using System;

namespace Resume.Application.Generator
{
    public class CodeGenerator
    {
        public static string GenerateUniqCode()
        {
            return Guid.NewGuid().ToString("N");
        }

    }
}

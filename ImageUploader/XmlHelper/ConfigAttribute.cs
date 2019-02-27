using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUploader
{
    public class ConfigAttribute : Attribute
    {
        public string[] Names { get; private set; }

        public ConfigAttribute()
        {
            Names = new string[0];
        }

        public ConfigAttribute(params string[] name)
        {
            Names = name;
        }
    }
}

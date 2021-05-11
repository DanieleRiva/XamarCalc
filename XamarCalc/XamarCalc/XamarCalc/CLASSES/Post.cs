using System;
using System.Collections.Generic;
using System.Text;

namespace XamarCalc.CLASSES
{
    public class Post
    {
        public string expr { get; set; }
        public int precision { get; set; }

        public override string ToString()
        {
            return "{\"expr\": \"" + expr + "\", \"precision\": " + precision + "} ";
        }
    }
}

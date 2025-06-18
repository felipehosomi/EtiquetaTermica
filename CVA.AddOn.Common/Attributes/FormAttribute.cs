using System;

namespace CVA.AddOn.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FormAttribute : Attribute
    {
        public int FormId { get; set; }

        public FormAttribute(int formId)
        {
            this.FormId = formId;
        }
    }
}

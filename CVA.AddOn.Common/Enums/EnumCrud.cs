using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVA.AddOn.Common.Enums
{
    public enum EnumModelTableType
    { 
        UserTable,
        SBOTable
    }

    public enum EnumModelFieldType
    {
        UserField,
        SBOField
    }

    public enum EnumCrudOperation
    {
        Create,
        Retrieve,
        Update,
        Delete
    }
}

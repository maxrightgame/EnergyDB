
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace EnergyCompany.Models
{

using System;
    using System.Collections.Generic;
    
public partial class bill
{

    public int id_client { get; set; }

    public int id { get; set; }

    public Nullable<decimal> payment { get; set; }

    public Nullable<int> id_collector { get; set; }



    public virtual client client { get; set; }

    public virtual collector collector { get; set; }

}

}

using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostServiceWebApplication.Models;

public partial class Location
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Адреса")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public long TypeId { get; set; }

    public virtual ICollection<ParcelHistory> ParcelHistories { get; } = new List<ParcelHistory>();

    [Display(Name = "Тип відділення")]
    public virtual LocationType Type { get; set; } = null!;
}

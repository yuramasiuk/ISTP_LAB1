using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace PostServiceWebApplication.Models;

public partial class ParcelHistory
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public long LocationId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public long ParcelId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Дата прибуття")]
    [BindProperty, DataType(DataType.Date)]
    public DateTime ArrivalDate { get; set; }

    [Display(Name = "Відділення")]
    public virtual Location Location { get; set; } = null!;

    [Display(Name = "Посилка")]
    public virtual Parcel Parcel { get; set; } = null!;
}

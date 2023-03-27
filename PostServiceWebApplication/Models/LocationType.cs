using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostServiceWebApplication.Models;

public partial class LocationType
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Час відкриття")]
    public TimeSpan OpenTime { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Час закриття")]
    public TimeSpan CloseTime { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Місткість")]
    public long Capacity { get; set; }

    public virtual ICollection<Location> Locations { get; } = new List<Location>();
}

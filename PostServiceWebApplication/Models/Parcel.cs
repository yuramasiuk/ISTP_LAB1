using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostServiceWebApplication.Models;

public partial class Parcel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public long ClientFromId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public long ClientToId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public long TypeId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public long StatusId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Хрупкість")]
    public bool IsFragile { get; set; }

    public bool IsDeleted { get; set; } = false;

    [Display(Name = "Відправник")]
    public virtual Client ClientFrom { get; set; } = null!;


    [Display(Name = "Отримувач")]
    public virtual Client ClientTo { get; set; } = null!;

    public virtual ICollection<ParcelHistory> ParcelHistories { get; } = new List<ParcelHistory>();

    [Display(Name = "Статус")]
    public virtual Status Status { get; set; } = null!;

    [Display(Name = "Тип")]
    public virtual ParcelType Type { get; set; } = null!;
}

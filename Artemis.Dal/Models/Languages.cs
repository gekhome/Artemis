﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Dal.Models;

public partial class Languages
{
    [Key]
    public int LanguageId { get; set; }

    [StringLength(50)]
    public string Language { get; set; } = null!;

    [StringLength(2)]
    public string Code { get; set; } = null!;
}
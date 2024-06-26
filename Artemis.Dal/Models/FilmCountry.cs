﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Dal.Models;

public partial class FilmCountry
{
    [Key]
    public int Id { get; set; }

    public int FilmId { get; set; }

    public int CountryId { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("FilmCountry")]
    public virtual Country Country { get; set; } = null!;

    [ForeignKey("FilmId")]
    [InverseProperty("FilmCountry")]
    public virtual Film Film { get; set; } = null!;
}
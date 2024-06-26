﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Dal.Models;

public partial class FilmActor
{
    [Key]
    public int Id { get; set; }

    public int FilmId { get; set; }

    public int ActorId { get; set; }

    [StringLength(50)]
    public string? Role { get; set; }

    [StringLength(255)]
    public string? Notes { get; set; }

    [ForeignKey("ActorId")]
    [InverseProperty("FilmActor")]
    public virtual Actor Actor { get; set; } = null!;

    [ForeignKey("FilmId")]
    [InverseProperty("FilmActor")]
    public virtual Film Film { get; set; } = null!;
}
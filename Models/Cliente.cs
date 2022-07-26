﻿using System.ComponentModel.DataAnnotations;

namespace WebApiClientes.Models
{
    public class Cliente
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string Direccion { get; set; }
    }
}

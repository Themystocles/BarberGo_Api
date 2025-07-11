﻿using BarberGo.Interfaces;

namespace BarberGo.Entities
{
    public class SystemCustomization : IEntity
    {
        public int Id { get; set; }
        public string LogoUrl { get; set; }
        public string NomeSistema { get; set; }
        public string CorPrimaria { get; set; }
        public string CorSecundaria { get; set; }
        public string BackgroundUrl { get; set; }
        public string BackgroundColor { get; set; }
        public string Descricao { get; set; }
        
    }
}

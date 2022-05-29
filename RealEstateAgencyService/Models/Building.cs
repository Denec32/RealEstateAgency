using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgencyService.Models
{
    public class Building
    {
        [Key]
        public int Id { get; set; }

        public string? Region { get; set; } // Область
        public string? City { get; set; } // Город
        public string? Street { get; set; } // Улица
        public string? BuildingNumber { get; set; } // Номер дома

        public int YearBuilt { get; set; } // Год постройки
        public string? WallMaterial { get; set; } // Материал стен
        public string? HouseSeries { get; set; } // Серия дома
        public int FloorNumber { get; set; } // Количество этажей
        public int FlatNumber { get; set; } // Количество квартир
        public bool GarbageDisposal { get; set; } //Мусоропровод
        public bool PlayGround { get; set; } //Детская площадка	
        public bool SportsArea { get; set; } //Спортивная площадка
        public int CommissioningYear { get; set; } //Год ввода в эксплуатацию
        public int EntranceNumber { get; set; } //Количество подъездов
        public string? ColdWaterSupply { get; set; } //Холодное водоснабжение
        public string? HotWaterSupply { get; set; } //Горячее водоснабжение
        public string? WaterDump { get; set; } //Водоотведение
        public string? HeatSupply { get; set; } //Теплоснабжение
        public string? EnergySupply { get; set; } //Энергоснабжение
        public string? Ventilation { get; set; } //Вентиляция
        public string? CoveringType { get; set; } //Тип перекрытий
        public string? FoundationType { get; set; } //Тип фундамента
        public string? EnergyEfficiencyClass { get; set; } //Класс энергоэффективности

        [JsonIgnore]
        public List<Listing>? Listings { get; set; }

        [JsonIgnore]
        public List<BuildingPhoto>? BuildingPhotos { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace RPG_MV_Trans_API
{
    public class Uploading
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// Тип данных
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Текстовые данные.
        /// </summary>
        public string UploadingData { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public Uploading() { }

        /// <summary>
        /// Конструктор добавления.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="data">Данные</param>
        public Uploading(string id, string data) 
        {
            Id = id;
            UploadingData = data;
        }
    }
}

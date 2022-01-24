using System.ComponentModel;

namespace Dotes.BE.Enums
{
    public enum TagType
    {
        [Description("Строка")]
        String = 0,

        [Description("Таблица")]
        Table = 1,

        [Description("Изображение")]
        Image = 2
    }
}

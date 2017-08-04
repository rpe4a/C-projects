using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    /// <summary>
    /// Класс логирования
    /// </summary>
    public class LogAnalyzer
    {
        private Ilogger logger;
        private IWebService service;

        /// <summary>
        /// Минимальная необходимая длина <see cref="LogAnalyzer"/>
        /// </summary>
        /// <remarks>Ascent is the maximum height above the baseline reached
        /// by glyphs in this font, excluding the height of glyphs for
        /// accented characters.</remarks>
        public int MinNameLength { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger">Зависимость 1</param>
        /// <param name="service">Зависимость 2</param>
        public LogAnalyzer(Ilogger logger, IWebService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Метод анализа файла
        /// </summary>
        /// <param name="fileName">Путь к файлу</param>
        public void Analyze(string fileName)
        {
            if (fileName.Length < MinNameLength)
            {
                try
                {
                    logger.LogError(string.Format("Слишком короткое имя файла: {0}", fileName));
                }
                catch (Exception e)
                {
                    service.Write("Ошибка регитсратора: " + e);
                }
            }
        }

        /// <summary>
        /// Получить данные 
        /// </summary>
        /// <returns>Строка с данными</returns>
        public string GetData()
        {
            return service.Get();
        }
    }
}
